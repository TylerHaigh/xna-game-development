using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Components;
using Packt.Mono.Framework.Entities;
using System.Collections.Generic;

namespace Packt.Mono.Framework.Collision
{
    public class CollisionEngine
    {
        private List<CollisionComponent> _colliders = new List<CollisionComponent>();
        private List<CollisionComponent> _collidersToRemove = new List<CollisionComponent>();

        public void AddEntity(CollisionComponent e) => _colliders.Add(e);
        public void RemoveEntity(CollisionComponent e) => _collidersToRemove.Add(e);
        public void RemoveEntity(GameEntity e)
        {
            IEnumerable<Component> comp = e.GetComponent<CollisionComponent>();
            foreach (var c in comp)
            {
                _collidersToRemove.Add((CollisionComponent)c);
            }

        }

        public void Update(GameTime gameTime)
        {
            _collidersToRemove.ForEach(e => _colliders.Remove(e));
            _collidersToRemove.Clear();

            for (int i = 0; i < _colliders.Count; i++)
            {
                CollisionComponent a = _colliders[i];

                // collisions for a a->b might trigger b to be destroyed (e.g. Shot vs Asteroid)
                // when b->c is evaluated, b might be destroyed and this can't possibly collide with c

                if (!a.IsActive || a.Entity == null || a.Entity.IsDestroyed) continue; // next iteration of I loop

                for (int j = i + 1; j < _colliders.Count; j++)
                {
                    
                    CollisionComponent b = _colliders[j];

                    if (!a.IsActive || a.Entity == null || a.Entity.IsDestroyed)
                        break;
                    else if (!b.IsActive || b.Entity == null || b.Entity.IsDestroyed)
                        continue; // next iteration of J loop

                    // Check for collision
                    if (Collides(a.Geometry, b.Geometry))
                    {
                        // todo: handle game specific logic??
                        
                        a.RaiseCollision(b);
                    }
                }
            }
        }

        

        private bool Collides(CollisionGeometry a, CollisionGeometry b)
        {
            if (a is CollisionCircle) return CollisionCircleCollides((CollisionCircle)a, b);
            if (a is CollisionBoundingBox) return BoundingBoxCollides((CollisionBoundingBox)a, b);

            return false;
        }

        private bool CollisionCircleCollides(CollisionCircle a, CollisionGeometry b)
        {
            if (b is CollisionCircle) return a.Intersects((CollisionCircle)b);
            return false;
        }

        private bool BoundingBoxCollides(CollisionBoundingBox a, CollisionGeometry b)
        {
            if (b is CollisionBoundingBox) return a.Intersects((CollisionBoundingBox)b);
            return false;
        }

        public static bool Intersects(CollisionCircle a, CollisionCircle b) => a.Intersects(b);
        public static bool Intersects(CollisionCircle a, Vector2 b) => a.Intersects(b);
        public static bool Intersects(CollisionCircle a, Point b) => a.Intersects(b);

        public static bool Intersects(CollisionBoundingBox a, CollisionBoundingBox b) => a.Intersects(b);
        public static bool Intersects(CollisionBoundingBox a, Rectangle b) => a.Intersects(b);
    }
}

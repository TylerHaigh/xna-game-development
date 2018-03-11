using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Entities;
using System.Collections.Generic;

namespace Packt.Mono.Framework.Collision
{
    public class CollisionEngine
    {
        private List<CollisionComponent> _entities = new List<CollisionComponent>();
        private List<CollisionComponent> _entitiesToRemove = new List<CollisionComponent>();

        public void AddEntity(CollisionComponent e) => _entities.Add(e);
        public void RemoveEntity(CollisionComponent e) => _entitiesToRemove.Add(e);

        public void Update(GameTime gameTime)
        {
            _entitiesToRemove.ForEach(e => _entities.Remove(e));

            for (int i = 0; i < _entities.Count; i++)
            {
                for (int j = i + 1; j < _entities.Count; j++)
                {
                    CollisionComponent a = _entities[i];
                    CollisionComponent b = _entities[j];

                    // Check for collision
                    if (Collides(a.Geometry, b.Geometry))
                        a.RaiseCollision(b);
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

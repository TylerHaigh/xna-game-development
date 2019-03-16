using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Components;
using Packt.Mono.Framework.Entities;

namespace Packt.Mono.Framework.Collision
{
    public abstract class CollisionComponent : Component
    {
        public CollisionGeometry Geometry;

        public CollisionComponent(GameEntity entity) : base(entity) { }

        public event EventHandler<CollisionEventArgs> CollisionDetected;
        public static event EventHandler<CreatedCollisionComponentArgs> CreatedCollisionComponent;

        public void RaiseCollision(CollisionComponent other)
        {
            CollisionEventArgs args = new CollisionEventArgs
            {
                ThisComponent = this,
                OtherComponent = other,
                CollisionResolved = false
            };
            this.CollisionDetected?.Invoke(this, args);
            other.CollisionDetected?.Invoke(other, args); // raise event for other entity
        }

        protected void RaiseCreated()
        {
            CreatedCollisionComponent?.Invoke(this, new CreatedCollisionComponentArgs(this));
        }

    }


    public class BoundingBoxComponent : CollisionComponent
    {
        private CollisionBoundingBox _box;

        public BoundingBoxComponent(GameEntity entity, Vector2 location, Rectangle baseRect) : base(entity)
        {
            //_box = (CollisionBoundingBox)CollisionGeometryFactory.CreateGeometry(CollisionType.BoundingBox);
            _box = new CollisionBoundingBox(location, baseRect);

            Geometry = _box;

            RaiseCreated();
        }

        public BoundingBoxComponent(GameEntity entity, CollisionBoundingBox box) : base(entity)
        {
            _box = box;
            Geometry = box;
            RaiseCreated();
        }

        public override void Update(GameTime gameTime)
        {
            if(this.IsActive)
                _box.Location = Entity.Location;
        }
    }

    public class CollisionCircleComponent : CollisionComponent
    {
        private CollisionCircle _circle;

        public CollisionCircleComponent(GameEntity entity, Vector2 center, float collisionRadius) : base(entity)
        {
            //_circle = (CollisionCircle)CollisionGeometryFactory.CreateGeometry(CollisionType.Circle);
            _circle = new CollisionCircle(center, collisionRadius);

            this.Geometry = _circle;
            RaiseCreated();

        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsActive)
                _circle.Center = Entity.Sprite.Center;
        }
    }

}

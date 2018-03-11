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
        protected CollisionGeometry Geometry;

        public CollisionComponent(GameEntity entity) : base(entity) { }

        public abstract CollisionType CollisionType { get; }
    }

    public class BoundingBoxComponent : CollisionComponent
    {
        public override CollisionType CollisionType => CollisionType.BoundingBox;

        public BoundingBoxComponent(GameEntity entity) : base(entity)
        {
            Geometry = CollisionGeometryFactory.CreateGeometry(CollisionType.BoundingBox);
        }


        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

    public class CollisionCircleComponent : CollisionComponent
    {
        public override CollisionType CollisionType => CollisionType.Circle;

        public CollisionCircleComponent(Vector2 center, float collisionRadius, GameEntity entity) : base(entity)
        {
            var collisionCircle = (CollisionCircle)CollisionGeometryFactory.CreateGeometry(CollisionType.Circle);
            collisionCircle.Center = center;
            collisionCircle.Radius = collisionRadius;

            this.Geometry = collisionCircle;
        }

        public override void Update(GameTime gameTime)
        {
            GameEntity e = (GameEntity)Entity;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Packt.Mono.Framework.Collision
{
    public abstract class CollisionComponent : IComponent
    {
        private IGameEntity _entity;
        protected CollisionGeometry Geometry;
        public abstract CollisionType CollisionType { get; }

        public bool IsActive { get; set; }

        public void AttachToEntity(IGameEntity entity) => _entity = entity;
        public void DetachFromEntity() => _entity = null;

        public abstract void Update(GameTime gameTime);
    }

    public class BoundingBoxComponent : CollisionComponent
    {
        public override CollisionType CollisionType => CollisionType.BoundingBox;

        public BoundingBoxComponent()
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

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

}

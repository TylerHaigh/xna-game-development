using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using RobotRampage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Collision
{
    public class WorldBoundingBoxComponent : CollisionComponent
    {
        private CollisionBoundingBox _box;

        public WorldBoundingBoxComponent(WorldEntity entity, Vector2 location, Rectangle baseRect) : base(entity)
        {
            //_box = (CollisionBoundingBox)CollisionGeometryFactory.CreateGeometry(CollisionType.BoundingBox);
            _box = new CollisionBoundingBox(location, baseRect);

            Geometry = _box;

            RaiseCreated();
        }

        public WorldBoundingBoxComponent(WorldEntity entity, CollisionBoundingBox box) : base(entity)
        {
            _box = box;
            Geometry = box;
            RaiseCreated();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsActive)
                _box.Location = ((WorldEntity)Entity).WorldSprite.WorldLocation;
        }
    }
}

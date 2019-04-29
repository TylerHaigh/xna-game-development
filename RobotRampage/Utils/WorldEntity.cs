using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Utils
{
    public class WorldEntity : GameEntity
    {
        public Vector2 LastKnownWorldLocation { get; set; } = Vector2.Zero;
        public WorldSprite WorldSprite { get; set; }

        protected Camera Cam { get; set; }

        private Vector2 _worldLocation = Vector2.Zero;
        public Vector2 WorldLocation
        {
            get { return _worldLocation; }
            set
            {
                _worldLocation = value;
                if (this.WorldSprite != null)
                    this.WorldSprite.WorldLocation = value;
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            WorldSprite.Draw(gameTime, spriteBatch);
        }

        public WorldEntity(Camera cam) { this.Cam = cam; }
        public WorldEntity(Camera cam, WorldSprite sprite) : base(sprite)
        {
            this.Cam = cam;
            this.WorldSprite = sprite;
        }

        public override void Update(GameTime gameTime)
        {
            // has to come before component update because components will
            // reference Location derived variabled (e.g. center) in their update routine
            LastKnownWorldLocation = WorldLocation;
            WorldLocation += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var c in Components) { c.Update(gameTime); }
            foreach (var c in ChildEntities) { c.Update(gameTime); }

            if (WorldSprite != null)
                WorldSprite.Update(gameTime);
        }
    }
}

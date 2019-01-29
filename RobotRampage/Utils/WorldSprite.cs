using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Utils
{
    class WorldSprite : Sprite
    {

        public Vector2 WorldLocation { get => Location; set => Location = value; }
        public Vector2 ScreenLocation => _cam.Transform(WorldLocation);

        public Rectangle WorldRectangle => new Rectangle((int)WorldLocation.X, (int)WorldLocation.Y, FrameWidth, FrameHeight);
        public Rectangle ScreenRectangle => _cam.Transform(WorldRectangle);

        public Vector2 WorldCenter => WorldLocation + RelativeCenter;
        public Vector2 ScreenCenter => _cam.Transform(WorldCenter);

        private Camera _cam;

        public WorldSprite(Texture2D texture, Rectangle initialFrame, Camera cam) : this(texture, initialFrame, cam, Vector2.Zero) { }

        public WorldSprite(Texture2D texture, Rectangle initialFrame, Camera cam, Vector2 worldLocation) : base(texture, initialFrame)
        {
            _cam = cam;
            WorldLocation = worldLocation;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_cam.ObjectIsVisible(WorldRectangle))
                spriteBatch.Draw(Texture, ScreenCenter, Source, TintColor, Rotation, RelativeCenter, Scale, SpriteEffects.None, 0);
        }
    }
}

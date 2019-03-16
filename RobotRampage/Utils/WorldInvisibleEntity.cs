using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotRampage.Utils;

namespace RobotRampage.Utils
{
    public class WorldInvisibleEntity : WorldEntity
    {
        public WorldInvisibleEntity() : base(null) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Nothing to draw
        }
    }
}



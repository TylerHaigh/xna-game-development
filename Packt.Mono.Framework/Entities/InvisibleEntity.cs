using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Entities
{
    public class InvisibleEntity : GameEntity
    {
        public InvisibleEntity() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Nothing to draw
        }
    }
}

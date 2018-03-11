using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Star
{
    class Star : GameEntity
    {
        private Color[] _colors = { Color.White, Color.Yellow, Color.Wheat, Color.WhiteSmoke, Color.SlateGray, Color.CornflowerBlue, Color.Orange };
        private static Random _rand = new Random();

        public const int AnimationFrames = 1;
        public const int TextureWidth = 2;
        public const int TextureHeight = 2;
        

        public Star(Sprite sprite) {
            this.Sprite = sprite;

            Color starColor = _colors[_rand.Next(0, _colors.Length)];
            starColor *= (_rand.Next(30, 80) / 100.0f);
            Sprite.TintColor = starColor;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch);
        }
    }
}

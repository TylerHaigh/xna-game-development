using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models
{
    class Star : IMovableGameEntity
    {
        private Sprite _sprite;
        private Color[] _colors = { Color.White, Color.Yellow, Color.Wheat, Color.WhiteSmoke, Color.SlateGray, Color.CornflowerBlue, Color.Orange };
        private static Random _rand = new Random();

        public const int AnimationFrames = 1;

        public Vector2 Location { get => _sprite.Location; set => _sprite.Location = value; }
        public Vector2 Velocity { get => _sprite.Velocity; set => _sprite.Velocity = value; }

        public Star(Sprite sprite) {
            this._sprite = sprite;

            Color starColor = _colors[_rand.Next(0, _colors.Length)];
            starColor *= (_rand.Next(30, 80) / 100.0f);
            _sprite.TintColor = starColor;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
        }
    }
}

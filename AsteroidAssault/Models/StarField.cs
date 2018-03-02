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
    class StarField : IGameEntity
    {

        private List<Sprite> _stars = new List<Sprite>();

        private const int DefaultScreenWidth = 800;
        private const int DefaultScreenHeight = 600;

        private Random _rand = new Random();
        private Color[] _colors = { Color.White, Color.Yellow, Color.Wheat, Color.WhiteSmoke, Color.SlateGray };

        private int _screenWidth = DefaultScreenHeight;
        private int _screenHeight = DefaultScreenHeight;

        public StarField(int screenWidth, int screenHeight, int starCount, Vector2 starVelocity, Texture2D texture, Rectangle frameRectangle)
        {
            this._screenWidth = screenWidth;
            this._screenHeight = screenHeight;

            GenerateStars(starCount, starVelocity, texture, frameRectangle);
        }

        private void GenerateStars(int starCount, Vector2 starVelocity, Texture2D texture, Rectangle frameRectangle)
        {
            for (int i = 0; i < starCount; i++)
            {
                Vector2 loc = new Vector2(_rand.Next(0, _screenWidth), _rand.Next(0, _screenHeight));
                Sprite star = new Sprite(loc, texture, frameRectangle, starVelocity);

                Color starColor = _colors[_rand.Next(0, _colors.Length)];
                starColor *= (_rand.Next(30, 80) / 100.0f);
                star.TintColor = starColor;

                _stars.Add(star);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var star in _stars)
            {
                star.Update(gameTime);
                if (star.Location.Y > _screenHeight)
                    star.Location = new Vector2(_rand.Next(0, _screenWidth), 0);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var star in _stars)
                star.Draw(gameTime, spriteBatch);
        }
    }
}

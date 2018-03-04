using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Star
{
    class StarField : IGameEntity
    {

        private List<Star> _stars = new List<Star>();

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        private Random _rand = new Random();

        public StarField(int starCount, Vector2 starVelocity, TileSheet tileSheet, Rectangle screenBounds)
        {
            this._screenBounds = screenBounds;
            this._tileSheet = tileSheet;

            GenerateStars(starCount, starVelocity);
        }

        private void GenerateStars(int starCount, Vector2 starVelocity)
        {
            for (int i = 0; i < starCount; i++)
            {
                Vector2 loc = new Vector2(_rand.Next(0, _screenBounds.Width), _rand.Next(0, _screenBounds.Height));
                Star star = new Star(_tileSheet.SpriteAnimation());

                star.Velocity = starVelocity * (_rand.Next(30, 100) / 100.0f);
                star.Location = loc;

                _stars.Add(star);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var star in _stars)
            {
                star.Update(gameTime);
                if (star.Location.Y > _screenBounds.Height)
                    star.Location = new Vector2(_rand.Next(0, _screenBounds.Width), 0);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var star in _stars)
                star.Draw(gameTime, spriteBatch);
        }
    }
}

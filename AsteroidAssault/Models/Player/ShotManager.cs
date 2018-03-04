using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class ShotManager : IGameEntity
    {
        private List<Shot> _shots = new List<Shot>();

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        public ShotManager(TileSheet tileSheet, Rectangle screenBounds)
        {
            this._tileSheet = tileSheet;
            this._screenBounds = screenBounds;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var s in _shots)
            {
                s.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            // iterate backwards to avoid having to decrement i
            for (int i = _shots.Count - 1; i >= 0; i--)
            {
                Shot s = _shots[i];
                s.Update(gameTime);

                if (!s.IsOnScreen(_screenBounds))
                    _shots.RemoveAt(i);
            }
        }

        public void CreateShot(ShotFiredEventArgs args)
        {
            Sprite s = _tileSheet.SpriteAnimation();
            Shot shot = new Shot(s);

            shot.Location = args.Location;
            shot.Velocity = args.Velocity * args.ShotSpeed;

            _shots.Add(shot);

        }
    }
}

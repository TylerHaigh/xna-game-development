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

namespace AsteroidAssault.Models.Player
{
    class ShotManager : IEntityManager
    {
        private List<Shot> _shots = new List<Shot>();

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        public event EventHandler OnShotDestroy;

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
                {
                    s.DestroyEntity();
                    _shots.RemoveAt(i);
                }
            }
        }

        public void CreateShot(ShotFiredEventArgs args)
        {
            Sprite s = _tileSheet.SpriteAnimation();
            Shot shot = new Shot(s);

            shot.Location = args.Location;
            shot.Velocity = args.Velocity * args.ShotSpeed;

            shot.OnDestroy += HandleOnShotDestroy;

            _shots.Add(shot);
        }

        private void HandleOnShotDestroy(object sender, EventArgs e)
        {
            OnShotDestroy?.Invoke(sender, e);
        }

        public void Clear()
        {
            _shots.ForEach(s => s.DestroyEntity());
            _shots.Clear();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Models
{
    class ShotManager : IEntityManager
    {
        private List<Shot> _shots = new List<Shot>();

        private ShotFactory _shotFactory;

        public ShotManager(ShotFactory shotFactory)
        {
            _shotFactory = shotFactory;
        }

        public void AddShot(ShotFiredEventArgs args)
        {
            AddShot(args.Location, args.Velocity, args.ShotType);
        }

        public void AddShot(Vector2 location, Vector2 velocity, ShotType shotType)
        {
            Shot shot = _shotFactory.CreateShot(location, velocity, shotType);
            AddShot(shot);
        }

        public void AddShot(Shot shot)
        {
            _shots.Add(shot);
        }

        public void Clear()
        {
            _shots.ForEach(s => s.DestroyEntity());
            _shots.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var shot in _shots)
            {
                shot.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            _shots.RemoveAll(s => !s.IsActive || s.IsDestroyed);

            for (int i = _shots.Count - 1; i >= 0; i--)
            {
                Shot shot = _shots[i];
                shot.Update(gameTime);
                if (!shot.IsActive)
                    _shots.RemoveAt(i);
            }
        }
    }
}

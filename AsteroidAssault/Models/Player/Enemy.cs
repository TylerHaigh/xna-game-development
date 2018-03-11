using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Extensions;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class Enemy : GameEntity
    {
  
        public bool Destroyed { get; private set; }

        private Sprite _sprite;
        private Vector2 _gunOffset = new Vector2(25, 25);

        private Queue<Vector2> _wayPoints = new Queue<Vector2>();
        private Vector2 _currentWayPoint = Vector2.Zero;
        private Vector2 _previousLocation = Vector2.Zero;

        private const float Speed = 120;
        private const int CollisionRadius = 15;
        public const int AnimationFrames = 6;
        public const int TextureWidth = 50;
        public const int TextureHeight = 50;
        private const float ShotSpeed = 150f;

        public event EventHandler<ShotFiredEventArgs> ShotFired;

        public Enemy(Sprite s, Vector2 location)
        {
            this._sprite = s;
            //_sprite.CollisionRadius = CollisionRadius;

            _currentWayPoint = location;
            _previousLocation = location;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(IsActive())
                _sprite.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {

            if(IsActive())
            {
                Velocity = CalculateHeading();

                _previousLocation = Location;

                _sprite.Rotation = (float)Math.Atan2(
                    Location.Y - _previousLocation.Y,
                    Location.X - _previousLocation.X
                );

                base.Update(gameTime);
                _sprite.Location = this.Location;
                _sprite.Update(gameTime);

                if (ReachedWayPoint)
                    GetNextWaypoint();
            }

        }

        private void GetNextWaypoint()
        {
            if (_wayPoints.Count > 0)
                _currentWayPoint = _wayPoints.Dequeue();
        }

        private Vector2 CalculateHeading()
        {
            Vector2 heading = _currentWayPoint - Location;
            if (heading != Vector2.Zero) heading.Normalize(); // unit vector
            heading *= Speed; // magnitude scale
            return heading;
        }

        public void AddWayPoint(Vector2 wayPoint) => _wayPoints.Enqueue(wayPoint);
        public void AddPath(EnemyPath path) => _wayPoints.EnqueueRange(path.WayPoints);

        public bool ReachedWayPoint => Vector2.Distance(Location, _currentWayPoint) < _sprite.Source.Width / 2;
        
        public bool IsActive()
        {
            /// Used to check if should be updated and drawn
            if (Destroyed) return false;
            if (_wayPoints.Count > 0) return true;
            if (ReachedWayPoint) return false; // reached final way point
            return true;
        }

        public void FireShot()
        {
            ShotFiredEventArgs args = new ShotFiredEventArgs
            {
                Location = Location + _gunOffset,
                ShotSpeed = ShotSpeed
                // Don't know directional velocity because we don't know where the player is
            };
            ShotFired?.Invoke(this, args);
        }
    }
}

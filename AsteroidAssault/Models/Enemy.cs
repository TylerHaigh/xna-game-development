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
    class Enemy : IMovableGameEntity
    {
        public Vector2 Location { get => _sprite.Location; set => _sprite.Location = value; }
        public Vector2 Velocity { get => _sprite.Velocity; set => _sprite.Velocity = value; }
        public bool Destroyed { get; private set; }

        private Sprite _sprite;
        private Vector2 _gunOffset = new Vector2(25, 25);

        private Queue<Vector2> _waypoints = new Queue<Vector2>();
        private Vector2 _currentWayPoint = Vector2.Zero;
        private Vector2 _previousLocation = Vector2.Zero;

        private const float Speed = 120;
        private const int CollisionRadius = 15;
        public const int AnimationFrames = 1;
        public const int TextureWidth = 1;
        public const int TextureHeight = 1;

        public Enemy(Sprite s, Vector2 location)
        {
            this._sprite = s;
            _sprite.CollisionRadius = CollisionRadius;

            _currentWayPoint = location;
            _previousLocation = location;
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

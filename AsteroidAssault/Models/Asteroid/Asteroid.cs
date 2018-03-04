using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Graphics;
using System;

namespace AsteroidAssault.Models.Asteroid
{
    class Asteroid : IMovableGameEntity
    {

        private const int MinSpeed = 60;
        private const int MaxSpeed = 120;

        private const int ScreenPadding = 10;

        public Vector2 Location { get { return _sprite.Location; } set { _sprite.Location = value; } }
        public Vector2 Velocity { get { return _sprite.Velocity; } set { _sprite.Velocity = value; } }

        public const int SpriteWidth = 50;
        public const int SpriteHeight = 50;
        public const int AsteroidFrames = 20;

        public const int CollisionRadius = 15;
        public const int CollisionBoxPadding = 0;


        private Random _rand = new Random();

        private Sprite _sprite;
        public Vector2 Center => _sprite.Center;

        private CollisionCircle _collisionCircle => new CollisionCircle(Center, CollisionRadius);
        public CollisionBoundingBox BoundingBoxRectangle => new CollisionBoundingBox(Location, _sprite.Source);

        public bool IsCircleColliding(CollisionCircle otherCircle) => CollisionEngine.Intersects(_collisionCircle, otherCircle);
        public bool IsBoxColliding(Rectangle otherRect) => CollisionEngine.Intersects(BoundingBoxRectangle, otherRect);

        public Asteroid(Sprite sprite)
        {
            this._sprite = sprite;
            //this._sprite.CollisionRadius = CollisionRadius;
        }


        private Vector2 GenerateRandomVelocity()
        {
            Vector2 velocity = new Vector2(
                _rand.Next(-50, 51),
                _rand.Next(-50, 51)
            ); // Get direction

            velocity.Normalize(); // Unit Vector
            velocity *= _rand.Next(MinSpeed, MaxSpeed); // Scale to random magnitude
            return velocity;
        }

        public void RandomiseVelocity()
        {
            this.Velocity = GenerateRandomVelocity();
        }


        public bool IsOnScreen(Rectangle screenBounds)
        {
            Rectangle screenRectWithPadding = new Rectangle(-ScreenPadding, -ScreenPadding, screenBounds.Width + ScreenPadding, screenBounds.Height + ScreenPadding);
            return this._sprite.Destination.Intersects(screenRectWithPadding);
        }

        public void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch);
        }

        public void BounceAsteroids(Asteroid other)
        {
            Vector2 centerOfMass = (this.Velocity + other.Velocity) / 2;

            Vector2 thisNormal = other._sprite.Center - this._sprite.Center;
            Vector2 otherNormal = this._sprite.Center - other._sprite.Center;

            thisNormal.Normalize();
            otherNormal.Normalize();

            this.Velocity -= centerOfMass;
            this.Velocity = Vector2.Reflect(this.Velocity, thisNormal);
            this.Velocity += centerOfMass;


            other.Velocity -= centerOfMass;
            other.Velocity = Vector2.Reflect(other.Velocity, otherNormal);
            other.Velocity += centerOfMass;
        }
    }
}

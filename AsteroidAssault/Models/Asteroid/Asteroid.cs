using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;

namespace AsteroidAssault.Models.Asteroid
{
    class Asteroid : GameEntity
    {

        private const int MinSpeed = 60;
        private const int MaxSpeed = 120;

        private const int ScreenPadding = 10;
        

        public const int SpriteWidth = 50;
        public const int SpriteHeight = 50;
        public const int AsteroidFrames = 20;

        public const int CollisionRadius = 15;
        public const int CollisionBoxPadding = 0;


        private Random _rand = new Random();

        //private CollisionCircle _collisionCircle => new CollisionCircle(Sprite.Center, CollisionRadius);
        public CollisionBoundingBox BoundingBoxRectangle => new CollisionBoundingBox(Location, Sprite.Source);

        //public bool IsCircleColliding(CollisionCircle otherCircle) => CollisionEngine.Intersects(_collisionCircle, otherCircle);
        public bool IsBoxColliding(Rectangle otherRect) => CollisionEngine.Intersects(BoundingBoxRectangle, otherRect);

        public Asteroid(Sprite sprite)
        {
            this.Sprite = sprite;


            // Register Components
            CollisionComponent circleCollider = new CollisionCircleComponent(this, Sprite.Center, CollisionRadius);
            CollisionComponent boxCollider = new BoundingBoxComponent(this, Location, Sprite.Source);
            Components.Add(circleCollider);
            Components.Add(boxCollider);

            circleCollider.CollisionDetected += HandleCollision;
        }

        private void HandleCollision(object sender, CollisionEventArgs e)
        {
            GameEntity otherEntity = e.OtherComponent.Entity;

            if (otherEntity is Asteroid) {
                BounceAsteroids((Asteroid)otherEntity);
                e.CollisionResolved = true;
            }
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
            return this.Sprite.Destination.Intersects(screenRectWithPadding);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch);
        }

        public void BounceAsteroids(Asteroid other)
        {
            Vector2 centerOfMass = (this.Velocity + other.Velocity) / 2;

            Vector2 thisNormal = other.Sprite.Center - this.Sprite.Center;
            Vector2 otherNormal = this.Sprite.Center - other.Sprite.Center;

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

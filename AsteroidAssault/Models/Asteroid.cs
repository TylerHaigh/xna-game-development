using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using System;

namespace AsteroidAssault.Models
{
    class Asteroid : IGameEntity
    {

        private const int MinSpeed = 60;
        private const int MaxSpeed = 120;

        private const int ScreenPadding = 10;

        public Vector2 Location { get { return Sprite.Location; } set { Sprite.Location = value; } }
        public Vector2 Velocity { get { return Sprite.Velocity; } set { Sprite.Velocity = value; } }

        public const int SpriteWidth = 50;
        public const int SpriteHeight = 50;
        public const int AsteroidFrames = 20;

        private Random _rand = new Random();

        public readonly Sprite Sprite;

        public Asteroid(Sprite sprite)
        {
            this.Sprite = sprite;
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

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch);
        }
    }
}

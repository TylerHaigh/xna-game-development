using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using System;

namespace AsteroidAssault.Models
{
    class Shot : IGameEntity, IParticle
    {

        public float ShotSpeed { get; set; }
        public int CollisionRadius { get; set; }
        public Sprite Sprite { get; set; }

        public Vector2 Location { get { return Sprite.Location; } set { Sprite.Location = value; } }
        public Vector2 Velocity { get { return Sprite.Velocity; } set { Sprite.Velocity = value; } }

        public Shot(Sprite s) { this.Sprite = s; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public bool IsOnScreen(Rectangle screenBounds)
        {
            return this.Sprite.Destination.Intersects(screenBounds);
        }
    }

    class ShotFiredEventArgs : EventArgs
    {
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
        public float ShotSpeed { get; set; }
        public int CollisionRadius { get; set; }
    }

   
}
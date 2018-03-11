using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;

namespace AsteroidAssault.Models.Player
{
    class Shot : GameEntity
    {

        public const int AnimationFrames = 4;
        public const int TextureWidth = 5;
        public const int TextureHeight = 5;

        private const int CollisionRadius = 2; // might be able to move to Shot class
        

        private Sprite _sprite { get; set; }

        public Shot(Sprite s) {
            this._sprite = s;
            //_sprite.CollisionRadius = CollisionRadius;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _sprite.Location = this.Location;
            _sprite.Update(gameTime);
        }

        public bool IsOnScreen(Rectangle screenBounds)
        {
            return this._sprite.Destination.Intersects(screenBounds);
        }
    }

    class ShotFiredEventArgs : EventArgs
    {
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
        public float ShotSpeed { get; set; }
    }

   
}
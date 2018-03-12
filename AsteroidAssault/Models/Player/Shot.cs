using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;

namespace AsteroidAssault.Models.Player
{

    public enum WhoFired { Player, Enemy };

    class Shot : GameEntity
    {

        public const int AnimationFrames = 4;
        public const int TextureWidth = 5;
        public const int TextureHeight = 5;

        private const int CollisionRadius = 2;

        private Vector2 _shotToAsteroidImpact = new Vector2(0, -20); // impart upward velocity on impact

        private readonly WhoFired FiredBy;

        public Shot(Sprite s, WhoFired firedBy) {
            this.Sprite = s;
            FiredBy = firedBy;

            var circle = new CollisionCircleComponent(this, this.Sprite.Center, CollisionRadius);
            circle.CollisionDetected += CollisionDetected;
            Components.Add(circle);
        }

        private void CollisionDetected(object sender, CollisionEventArgs e)
        {
            //DestroyEntity();

            if (e.CollisionResolved) return;

            GameEntity otherEntity = e.OtherComponent.Entity;
            otherEntity = (otherEntity == this) ? e.ThisComponent.Entity : otherEntity;

            if (otherEntity is Asteroid.Asteroid)
            {
                // handle collision with asteroid

                e.CollisionResolved = true;
                this.DestroyEntity();
            }
            if (otherEntity is Enemy && FiredBy != WhoFired.Enemy)
            {
                otherEntity.DestroyEntity();
                this.DestroyEntity();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!IsDestroyed)
                Sprite.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsDestroyed)
                base.Update(gameTime);
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
        public WhoFired FiredBy { get; set; }
    }

   
}
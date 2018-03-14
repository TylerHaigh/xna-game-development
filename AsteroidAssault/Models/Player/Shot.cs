using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;

namespace AsteroidAssault.Models.Player
{

    public enum FiredBy { Player, Enemy };

    class Shot : GameEntity
    {

        public const int AnimationFrames = 4;
        public const int TextureWidth = 5;
        public const int TextureHeight = 5;

        private const int CollisionRadius = 2;

        private Vector2 _shotToAsteroidImpact = new Vector2(0, -20); // impart upward velocity on impact

        private readonly FiredBy _firedBy;
        private int _damage = 0; // allows flexibility to allow player to slightly damage enemy

        public Shot(Sprite s, FiredBy firedBy, int damage) {
            this.Sprite = s;
            _firedBy = firedBy;
            _damage = damage;

            var circle = new CollisionCircleComponent(this, this.Sprite.Center, CollisionRadius);
            circle.CollisionDetected += CollisionDetected;
            Components.Add(circle);
        }

        private void CollisionDetected(object sender, CollisionEventArgs e)
        {
            //DestroyEntity();

            if (e.CollisionResolved) return;

            GameEntity otherEntity = e.OtherEntity(this);

            if (otherEntity is Asteroid.Asteroid)
            {
                // handle collision with asteroid
                otherEntity.Velocity += _shotToAsteroidImpact;
                this.DestroyEntity();
                e.CollisionResolved = true;
            }
            if (otherEntity is Enemy && _firedBy != FiredBy.Enemy)
            {
                otherEntity.DestroyEntity();
                this.DestroyEntity();
                e.CollisionResolved = true;
            }
            if (otherEntity is Player && _firedBy != FiredBy.Player)
            {
                ((Player)otherEntity).Hit(_damage);
                this.DestroyEntity();
                e.CollisionResolved = true;
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
        public FiredBy FiredBy { get; set; }
        public int Damage { get; set; }
    }

   
}
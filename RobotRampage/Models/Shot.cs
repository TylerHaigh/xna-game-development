﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Models
{
    public enum ShotType
    {
        Bullet = 0,
        Missile = 1,
        Triple = 2, // Use same sprite frame as bullet
    }

    

    public class ShotCollisionEventArgs
    {
        public Shot Shot { get; set; }
        public GameEntity OtherEntity { get; set; }
    }

    public class Shot : Particle
    {
        private const float MaxSpeed = 400f;
        private const int Duration = 120;
        private static readonly Color InitialColor = Color.White;
        private static readonly Color FinalColor = Color.White;

        private const int ShotCollisionRadius = 14;

        public ShotType ShotType { get; private set; }

        public event EventHandler<ShotCollisionEventArgs> OnShotCollision;

        public Shot(Sprite s, Vector2 accelleration, ShotType shotType)
            : base(s, accelleration, MaxSpeed, Duration, InitialColor, FinalColor)
        {
            this.ShotType = shotType;
            AddCollisionCircle();
        }

        private void AddCollisionCircle()
        {
            CollisionCircleComponent component = new CollisionCircleComponent(this, this.Sprite.Center, ShotCollisionRadius);
            component.CollisionDetected += ShotCollision;

            this.Components.Add(component);
        }

        private void ShotCollision(object sender, CollisionEventArgs e)
        {
            if (e.OtherEntity(this) is Player) return;
            if (e.OtherEntity(this) is Shot) return;

            ShotCollisionEventArgs args = new ShotCollisionEventArgs
            {
                Shot = this,
                OtherEntity = e.OtherEntity(this)
            };

            OnShotCollision?.Invoke(this, args);
            this.DestroyEntity();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!IsDestroyed && IsActive)
                base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if(!IsDestroyed && IsActive)
                base.Update(gameTime);
        }
    }

    public class ShotFactory
    {
        private TileSheet _shotTileSheet;

        public ShotFactory(TileSheet shotTileSheet)
        {
            _shotTileSheet = shotTileSheet;
        }

        public Shot CreateShot(Vector2 location, Vector2 velocity, ShotType shotType)
        {
            Sprite s = _shotTileSheet.SpriteAnimation();
            s.Animate = false;
            s.Frame = (int)shotType;
            s.RotateTo(velocity);

            Shot shot = new Shot(s, Vector2.Zero, shotType)
            {
                Location = location,
                Velocity = velocity,
            };

            return shot;
        }
    }
}

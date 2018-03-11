﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidAssault.Models.Player
{
    class Player : GameEntity
    {
        public event EventHandler<ShotFiredEventArgs> ShotFired;
        public const int PlayerAnimationFrames = 3;
        public const int PlayerSpriteWidth = 50;
        public const int PlayerSpriteHeight = 50;

        private const float PlayerSpeed = 160;
        private const int MaxLives = 3;
        private const int CollisionRadius = 15;
        private const float GunTimeout = 0.2f;

        private const float ShotSpeed = 250f;

        private Rectangle _areaBounds;

        private GameTimer _shotTimer = new GameTimer(TimeSpan.FromSeconds(GunTimeout));
        private Vector2 _gunOffset = new Vector2(25, 10);

        public int RemainingLives { get; private set; } = MaxLives;
        public int PlayerScore { get; set; } = 0;
        public bool Destroyed { get; private set; } = false;
        
        public Player(Sprite s, Rectangle screenBounds)
        {
            this.Sprite = s;
            //_sprite.CollisionRadius = CollisionRadius;
            _areaBounds = new Rectangle(screenBounds.X, screenBounds.Height / 2, screenBounds.Width, screenBounds.Height / 2);

            Location = new Vector2 { X = _areaBounds.Center.X, Y = _areaBounds.Center.Y };
        }

        public void FireShot()
        {
            if (_shotTimer.Completed)
            {
                ShotFiredEventArgs args = new ShotFiredEventArgs
                {
                    Location = Location + _gunOffset,
                    Velocity = new Vector2(0, -1), // player can only fire on one direction
                    ShotSpeed = ShotSpeed
                    // ShotBy = ShotBy.Player
                };

                ShotFired?.Invoke(this, args);
                _shotTimer.Reset();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(!Destroyed)
            {
                Velocity = Vector2.Zero;
                _shotTimer.Update(gameTime);

                HandleKeybordInput(Keyboard.GetState());
                HandleGamePadInput(GamePad.GetState(PlayerIndex.One));

                Velocity.Normalize();
                Velocity *= PlayerSpeed;

                base.Update(gameTime);

                ImposeMovementLimits(); // must come after sprite updates position from velocity
            }
        }

        private void HandleKeybordInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Up))    Velocity += new Vector2( 0, -1);
            if (keyState.IsKeyDown(Keys.Down))  Velocity += new Vector2( 0,  1);
            if (keyState.IsKeyDown(Keys.Left))  Velocity += new Vector2(-1,  0);
            if (keyState.IsKeyDown(Keys.Right)) Velocity += new Vector2( 1,  0);

            if (keyState.IsKeyDown(Keys.Space)) FireShot();
        }

        private void HandleGamePadInput(GamePadState gamePadState)
        {
            Velocity += new Vector2(
                gamePadState.ThumbSticks.Left.X,
                -gamePadState.ThumbSticks.Left.Y
            );

            if (gamePadState.Buttons.A == ButtonState.Pressed) FireShot();
        }

        private void ImposeMovementLimits()
        {
            Vector2 location = this.Location;

            location.X = MathHelper.Clamp(location.X, _areaBounds.X, _areaBounds.Right  - Sprite.Source.Width);
            location.Y = MathHelper.Clamp(location.Y, _areaBounds.Y, _areaBounds.Bottom - Sprite.Source.Height);

            this.Location = location;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Destroyed)
                Sprite.Draw(gameTime, spriteBatch);
        }
    }
}

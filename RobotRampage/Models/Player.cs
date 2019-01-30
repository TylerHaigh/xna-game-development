using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using RobotRampage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Models
{

    class Player : GameEntity
    {
        // TODO: See if we can attach base and turret as child entities

        private WorldSprite _baseSprite;
        private WorldSprite _turretSprite;

        public Player(
                Texture2D spriteTexture,
                Rectangle baseInitialFrame,
                int baseFrameCount,
                Rectangle turretInitialFrame,
                int turretFrameCount,
                Vector2 worldLocation,
                Camera cam)
        {
            BuildPlayerBase(spriteTexture, baseInitialFrame, baseFrameCount, worldLocation, cam);
            BuildPlayerTurret(spriteTexture, turretInitialFrame, turretFrameCount, worldLocation, cam);

            Sprite = _baseSprite;

            // TODO: Create bounding box with 4 pixel barrier
        }


        private void BuildPlayerBase(
                Texture2D spriteTexture,
                Rectangle baseInitialFrame,
                int baseFrameCount,
                Vector2 worldLocation,
                Camera cam)
        {

            // Build Base sprite
            _baseSprite = new WorldSprite(spriteTexture, baseInitialFrame, cam, worldLocation);

            // Add Sprite Animation
            _baseSprite.AddAnimation(baseInitialFrame, baseFrameCount);
        }

        private void BuildPlayerTurret(
                Texture2D spriteTexture,
                Rectangle turretInitialFrame,
                int turretFrameCount,
                Vector2 worldLocation,
                Camera cam)
        {
            // Build base sprite
            _turretSprite = new WorldSprite(spriteTexture, turretInitialFrame, cam, worldLocation);

            // Add animation
            _turretSprite.AddAnimation(turretInitialFrame, turretFrameCount);
            _turretSprite.Animate = false;
        }



        public Player(WorldSprite baseSprite, WorldSprite turretSprite)
        {
            this._baseSprite = baseSprite;
            this._turretSprite = turretSprite;

            Sprite = _baseSprite;

            // TODO: Create bounding box with 4 pixel barrier

        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            
            // Update the player's location
            base.Update(gameTime);

            _turretSprite.WorldLocation = this.Location;
            _turretSprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw base first, then turret
            Sprite.Draw(gameTime, spriteBatch);

            _turretSprite.Draw(gameTime, spriteBatch);
        }


        // Handling Input

        private const float PlayerSpeed = 90f;

        private Vector2 HandleKeyboardMovement(KeyboardState keyState)
        {

            // This is a fairly generic method for getting movement vector.
            // It could be refactored into a generic utility class
            // TODO: Refactor Movement handling into dedicated class
            Vector2 keyMovemenmt = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.A)) keyMovemenmt.X--;
            if (keyState.IsKeyDown(Keys.D)) keyMovemenmt.X++;

            if (keyState.IsKeyDown(Keys.W)) keyMovemenmt.Y--;
            if (keyState.IsKeyDown(Keys.S)) keyMovemenmt.Y++;

            return keyMovemenmt;
        }

        private Vector2 HandleGamePadMovement(GamePadState gamePadState)
        {
            return new Vector2(
                gamePadState.ThumbSticks.Left.X,
                -gamePadState.ThumbSticks.Left.Y
            );
        }

        private Vector2 HandleKeyboardShots(KeyboardState keyState)
        {
            Vector2 shotAngle = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.NumPad1)) shotAngle += new Vector2(-1, 1);
            if (keyState.IsKeyDown(Keys.NumPad2)) shotAngle += new Vector2( 0, 1);
            if (keyState.IsKeyDown(Keys.NumPad3)) shotAngle += new Vector2( 1, 1);

            if (keyState.IsKeyDown(Keys.NumPad4)) shotAngle += new Vector2(-1, 0);
            if (keyState.IsKeyDown(Keys.NumPad6)) shotAngle += new Vector2( 1, 0);

            if (keyState.IsKeyDown(Keys.NumPad7)) shotAngle += new Vector2(-1, -1);
            if (keyState.IsKeyDown(Keys.NumPad8)) shotAngle += new Vector2( 0, -1);
            if (keyState.IsKeyDown(Keys.NumPad9)) shotAngle += new Vector2( 1, -1);

            return shotAngle;
        }

        private Vector2 HandleGamePadShots(GamePadState gamePadState)
        {
            return new Vector2(
                gamePadState.ThumbSticks.Right.X,
                -gamePadState.ThumbSticks.Right.Y
            );
        }

        private void HandleInput(GameTime gameTime)
        {

            // TODO: Warning - If two devices are connected, the angles could be affected??
            Vector2 moveAngle = Vector2.Zero;
            moveAngle += HandleKeyboardMovement(Keyboard.GetState());
            moveAngle += HandleGamePadMovement(GamePad.GetState(PlayerIndex.One));

            Vector2 fireAngle = Vector2.Zero;
            fireAngle += HandleKeyboardShots(Keyboard.GetState());
            fireAngle += HandleGamePadShots(GamePad.GetState(PlayerIndex.One));


            if (moveAngle != Vector2.Zero)
            {
                moveAngle.Normalize();
                _baseSprite.RotateTo(moveAngle);
                this.Velocity = moveAngle * PlayerSpeed;
            }

            if (fireAngle != Vector2.Zero)
            {
                fireAngle.Normalize();
                _turretSprite.RotateTo(fireAngle);
            }
        }

    }
}

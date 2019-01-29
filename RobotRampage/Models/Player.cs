using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    }
}

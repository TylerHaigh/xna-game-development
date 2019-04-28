using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Utilities;
using RobotRampage.Collision;
using RobotRampage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Models
{

    class Player : WorldEntity
    {
        private const float PlayerSpeed = 90f;
        private static Rectangle ScrollArea = new Rectangle(150, 100, 500, 400); // TODO: Hardcoded based on world

        private WorldSprite _baseSprite;
        private WorldSprite _turretSprite;

        private Vector2 _moveAngle = Vector2.Zero;

        private const float WeaponSpeed = 600f;
        private const float ShotMinTimer = 0.15f;
        private GameTimer _shotTimer = new GameTimer(TimeSpan.FromSeconds(ShotMinTimer));
        public bool CanFireWeapon => _shotTimer.Completed;



        public Player(WorldSprite baseSprite, WorldSprite turretSprite, Camera cam) : base(cam, baseSprite)
        {
            // Base sprite will be the WorldSprite
            this._baseSprite = baseSprite;

            // Will need to update turretSprite manually
            this._turretSprite = turretSprite;

            CreateCollisionBoundingBox();
        }

        private void CreateCollisionBoundingBox()
        {
            // Create bounding box with 4 pixel barrier
            CollisionBoundingBox box = new CollisionBoundingBox(WorldLocation, _baseSprite.WorldRectangle, 4, 4);
            CollisionComponent comp = new WorldBoundingBoxComponent(this, box);

            comp.CollisionDetected += CollisionDetected;
            Components.Add(comp);
        }

        private void CollisionDetected(object sender, CollisionEventArgs e)
        {
            if (e.CollisionResolved) return;

            Console.WriteLine("Collision Detected");
            WorldLocation = LastKnownWorldLocation;
            _baseSprite.WorldLocation = LastKnownWorldLocation;
            _turretSprite.WorldLocation = LastKnownWorldLocation;
            e.CollisionResolved = true;
        }

        public override void Update(GameTime gameTime)
        {
            _shotTimer.Update(gameTime);

            HandleInput(gameTime);
            ClampToWorld();
            
            // Update the player's location
            base.Update(gameTime);

            _turretSprite.WorldLocation = this.WorldLocation;
            _turretSprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw base first, then turret
            _baseSprite.Draw(gameTime, spriteBatch);

            _turretSprite.Draw(gameTime, spriteBatch);
        }


        #region InputHandling

        private void HandleInput(GameTime gameTime)
        {
            // TODO: Warning - If two devices are connected, the angles could be affected??
            Vector2 moveAngle = Vector2.Zero;
            moveAngle += KeyboardStateConverter.VectorFromWASDDirections(Keyboard.GetState());
            moveAngle += GamePadStateConverter.VectorFromLeftThumbStick(GamePad.GetState(PlayerIndex.One));

            Vector2 fireAngle = Vector2.Zero;
            fireAngle += KeyboardStateConverter.VectorFromNumpadDirections(Keyboard.GetState());
            fireAngle += GamePadStateConverter.VectorFromRightThumbStick(GamePad.GetState(PlayerIndex.One));


            if (moveAngle != Vector2.Zero)
            {
                moveAngle.Normalize();
                _baseSprite.RotateTo(moveAngle);
            }

            this.Velocity = moveAngle * PlayerSpeed;
            this._moveAngle = moveAngle;

            if (fireAngle != Vector2.Zero)
            {
                fireAngle.Normalize();
                _turretSprite.RotateTo(fireAngle);

                // Firing a shot will only work when the player presses the key
                // Should this be changed to a space bar?
                FireWeapon(fireAngle);
            }
        }

        #endregion


        #region MovementLimitation

        private void ClampToWorld()
        {
            float x = MathHelper.Clamp(
                WorldLocation.X,
                0,
                Cam.WorldRectangle.Right - _baseSprite.FrameWidth
            );

            float y = MathHelper.Clamp(
                WorldLocation.Y,
                0,
                Cam.WorldRectangle.Bottom - _baseSprite.FrameHeight
            );

            WorldLocation = new Vector2(x, y);
        }


        private bool PlayerMovingLeft  => _moveAngle.X < 0;
        private bool PlayerMovingRight => _moveAngle.X > 0;
        private bool PlayerMovingUp    => _moveAngle.Y < 0;
        private bool PlayerMovingDown  => _moveAngle.Y > 0;


        public Vector2 GetCameraRepositionLocation(GameTime gameTime)
        {
            Vector2 totalMovementOffset = Vector2.Zero;

            float moveScale = PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_baseSprite.ScreenRectangle.X < ScrollArea.X && PlayerMovingLeft)
                totalMovementOffset += new Vector2(_moveAngle.X, 0) * moveScale;

            if (_baseSprite.ScreenRectangle.Right > ScrollArea.Right && PlayerMovingRight)
                totalMovementOffset += new Vector2(_moveAngle.X, 0) * moveScale;

            if (_baseSprite.ScreenRectangle.Y < ScrollArea.Y && PlayerMovingUp)
                totalMovementOffset += new Vector2(0, _moveAngle.Y) * moveScale;

            if (_baseSprite.ScreenRectangle.Bottom > ScrollArea.Bottom && PlayerMovingDown)
                totalMovementOffset += new Vector2(0, _moveAngle.Y) * moveScale;

            return totalMovementOffset;
        }

        #endregion


        private void FireWeapon(Vector2 fireAngle)
        {
            if (CanFireWeapon)
            {

                Console.WriteLine("Shot Fired");
                ShotFiredEventArgs args = new ShotFiredEventArgs
                {
                    Location = _turretSprite.WorldLocation,
                    Velocity = fireAngle * WeaponSpeed
                };

                ShotFired?.Invoke(this, args);

                _shotTimer.Reset();
            }
        }

        public event EventHandler<ShotFiredEventArgs> ShotFired;

    }

    public class ShotFiredEventArgs
    {
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
    }
}

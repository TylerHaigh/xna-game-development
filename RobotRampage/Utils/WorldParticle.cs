using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using RobotRampage.Utils;

namespace RobotRampage.Utils
{
    public class WorldParticle : WorldEntity
    {

        private Vector2 _accelleration; // applied accelleration to base sprite's velocity
        private float _maxSpeed; // Limits magnitude of velocity vector
        private int _initialDuration; // should be a timespan, not number of frames
        private int _remainingDuration; // should be a Timer
        private Color _initialColor; // could package into ColorRange class
        private Color _finalColor;

        public int ElapsedDuration => _initialDuration - _remainingDuration;
        public float DurationProgress => _remainingDuration / (float)_initialDuration;
        public bool IsActive => _remainingDuration > 0;

        public WorldParticle(
            WorldSprite s, Camera cam,
            Vector2 accelleration, float maxSpeed, int duration, Color initialColor, Color finalColor)
            : base(cam, s)
        {
            _accelleration = accelleration;
            _maxSpeed = maxSpeed;
            _initialDuration = duration;
            _remainingDuration = duration;
            _initialColor = initialColor;
            _finalColor = finalColor;
        }

        public override void Update(GameTime gameTime)
        {
            if(IsActive)
            {
                Velocity += _accelleration;
                if(Velocity.Length() > _maxSpeed)
                {
                    Vector2 vel = Velocity;
                    vel.Normalize();
                    Velocity = vel * _maxSpeed;
                }

                WorldSprite.TintColor = Color.Lerp(_initialColor, _finalColor, DurationProgress);
                _remainingDuration--;

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
                WorldSprite.Draw(gameTime, spriteBatch);
        }
    }
}

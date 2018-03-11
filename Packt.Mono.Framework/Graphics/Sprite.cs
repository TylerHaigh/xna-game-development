using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packt.Mono.Framework.Graphics
{
    public class Sprite
    {

        public Texture2D Texture { get; set; }

        public Color TintColor { get; set; } = Color.White;
        public Vector2 Location { get; set; } = Vector2.Zero;
        //public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float Scale { get; set; } = 1;

        private float _rotation = 0;
        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % (float)(Math.PI * 2); }
        }

        // Animating Sprite details
        protected List<Rectangle> _frames = new List<Rectangle>();
        private Rectangle _initialFrame;
        private int _currentFrame;
        private GameTimer _timeForCurrentFrame;

        public int Frame
        {
            get { return _currentFrame; }
            set { _currentFrame = MathHelper.Clamp(value, 0, _frames.Count - 1); }
        }

        private float _frameDisplayTime = 0.1f;
        public float FrameDisplayTime
        {
            get { return _frameDisplayTime; }
            set {
                _frameDisplayTime = Math.Max(0, value);
                _timeForCurrentFrame = new GameTimer(_frameDisplayTime);
            }
        }

        public void AddFrame(Rectangle frameRect) => _frames.Add(frameRect);
        public void AddFrames(IEnumerable<Rectangle> frameRects) => _frames.AddRange(frameRects);

        public Rectangle Source => _frames[_currentFrame];
        public Rectangle Destination => new Rectangle((int)Location.X, (int)Location.Y, _initialFrame.Width, _initialFrame.Height);
        private Vector2 CenterOfFrame => new Vector2(_initialFrame.Width / 2.0f, _initialFrame.Height / 2.0f);
        public Vector2 Center => Location + CenterOfFrame;



        // Collision Detection
        //public int CollisionRadius { get; set; } = 0; // Bounding Circle Collision
        //public int BoundingXPadding { get; set; } = 0; // Boudning Box Collision
        //public int BoundingYPadding { get; set; } = 0;
        //public CollisionCircle CollisionCircle => new CollisionCircle (Center, CollisionRadius);
        //public CollisionBoundingBox BoundingBoxRectangle => new CollisionBoundingBox(Location, _initialFrame, BoundingXPadding, BoundingYPadding);

        public Sprite(Texture2D texture, Rectangle initialFrame) : this(texture, initialFrame, Vector2.Zero, Vector2.Zero) { }

        public Sprite(Texture2D texture, Rectangle initialFrame, Vector2 location, Vector2 velocity)
        {
            this.Location = location;
            this.Texture = texture;
            //this.Velocity = velocity;

            _frames.Add(initialFrame);
            _initialFrame = initialFrame;

            _timeForCurrentFrame =  new GameTimer(_frameDisplayTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateCurrentFrameTimer(gameTime);

            //Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void UpdateCurrentFrameTimer(GameTime gameTime)
        {
            _timeForCurrentFrame.Update(gameTime);
            if (_timeForCurrentFrame.Completed)
            {
                _currentFrame = (_currentFrame + 1) % _frames.Count;
                _timeForCurrentFrame.Reset();
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, Source, TintColor, Rotation, CenterOfFrame, Scale, SpriteEffects.None, 0);
        }

    }
}

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
        public Vector2 RelativeCenter => new Vector2(FrameWidth / 2, FrameHeight / 2);
        public Vector2 Center => Location + RelativeCenter;

        public int FrameWidth => _initialFrame.Width;
        public int FrameHeight => _initialFrame.Height;

        public bool Animate { get; set; }

        public Sprite(Texture2D texture, Rectangle initialFrame) : this(texture, initialFrame, Vector2.Zero) { }


        public Sprite(Texture2D texture, Rectangle initialFrame, Vector2 location)
        {
            this.Location = location;
            this.Texture = texture;

            _frames.Add(initialFrame);
            _initialFrame = initialFrame;

            _timeForCurrentFrame =  new GameTimer(_frameDisplayTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            if(Animate)
                UpdateCurrentFrameTimer(gameTime);
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
            spriteBatch.Draw(Texture, Center, Source, TintColor, Rotation, RelativeCenter, Scale, SpriteEffects.None, 0);
        }

        public void RotateTo(Vector2 direction)
        {
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public void ClearFrames()
        {
            _frames.Clear();
        }

        public void AddAnimation(int framesX, int framesY = 1, int paddingX = 0, int paddingY = 0)
        {
            AddAnimation(_initialFrame, framesX, framesY, paddingX, paddingY);
        }

        public void AddAnimation(Rectangle initialFrame, int framesX, int framesY = 1, int paddingX = 0, int paddingY = 0)
        {
            ClearFrames();

            for (int x = 0; x < framesX; x++)
            {
                for (int y = 0; y < framesY; y++)
                {
                    Rectangle rect = new Rectangle(
                        initialFrame.X + ((initialFrame.Width + paddingX) * x),
                        initialFrame.Y + ((initialFrame.Height + paddingY) * y),
                        initialFrame.Width,
                        initialFrame.Height
                    );
                    _frames.Add(rect);
                }
            }

            Animate = true;
        }

    }
}

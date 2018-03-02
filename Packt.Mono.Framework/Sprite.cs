using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    public class Sprite : IGameEntity
    {

        public Texture2D Texture { get; set; }

        public Color TintColor { get; set; } = Color.White;
        public Vector2 Location { get; set; } = Vector2.Zero;
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float Scale { get; set; } = 1;

        private float _rotation = 0;
        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % (float)(Math.PI * 2); }
        }

        // Animating Sprite details
        protected List<Rectangle> _frames = new List<Rectangle>();
        private int _frameWidth = 0;
        private int _frameHeight = 0;
        private int _currentFrame;
        private Timer _timeForCurrentFrame;

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
                _timeForCurrentFrame = new Timer(_frameDisplayTime);
            }
        }

        public void AddFrame(Rectangle frameRect) => _frames.Add(frameRect);

        public Rectangle Source => _frames[_currentFrame];
        public Rectangle Destination => new Rectangle((int)Location.X, (int)Location.Y, _frameWidth, _frameHeight);
        private Vector2 CenterOfFrame => new Vector2(_frameWidth / 2.0f, _frameHeight / 2.0f);
        public Vector2 Center => Location + CenterOfFrame;



        // Collision Detection
        public int CollisionRadius { get; set; } = 0; // Bounding Circle Collision
        public int BoundingXPadding { get; set; } = 0; // Boudning Box Collision
        public int BoundingYPadding { get; set; } = 0;
        private CollisionCircle CollisionCircle => new CollisionCircle (Center, CollisionRadius);

        public Rectangle BoundingBoxRectangle => new Rectangle((int)Location.X + BoundingXPadding, (int)Location.Y + BoundingYPadding, _frameWidth - (BoundingXPadding * 2), _frameHeight - (BoundingYPadding * 2));
        public bool IsBoxColliding(Rectangle otherBox) => BoundingBoxRectangle.Intersects(otherBox);
        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius) => this.IsCircleColliding(new CollisionCircle(otherCenter, otherRadius));
        public bool IsCircleColliding(CollisionCircle otherCircle) => this.CollisionCircle.Intersects(otherCircle);


        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.Location = location;
            this.Texture = texture;
            this.Velocity = velocity;

            _frames.Add(initialFrame);
            _frameWidth = initialFrame.Width;
            _frameHeight = initialFrame.Height;

            _timeForCurrentFrame =  new Timer(_frameDisplayTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateCurrentFrameTimer(gameTime);

            Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
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

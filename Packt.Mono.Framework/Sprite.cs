﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    class Sprite
    {

        public Texture2D Texture { get; set; }

        public Color TintColor { get; set; } = Color.White;
        public Vector2 Location { get; set; } = Vector2.Zero;
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        private float _rotation = 0;
        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % (float)(Math.PI * 2); }
        }

        // Animating Sprite details
        private List<Rectangle> _frames = new List<Rectangle>();
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
            set { _frameDisplayTime = Math.Max(0, value); }
        }

        public Rectangle Source => _frames[_currentFrame];
        public Rectangle Destination => new Rectangle((int)Location.X, (int)Location.Y, _frameWidth, _frameHeight);
        private Vector2 CenterOfFrame => new Vector2(_frameWidth / 2.0f, _frameHeight / 2.0f);
        public Vector2 Center => Location + CenterOfFrame;



        // Collision Detection
        public int CollisionRadius { get; set; } = 0; // Bounding Circle Collision
        public int BoundingXPadding { get; set; } = 0; // Boudning Box Collision
        public int BoundingYPadding { get; set; } = 0;

        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.Location = location;
            this.Texture = texture;
            this.Velocity = velocity;

            _frames.Add(initialFrame);
            _frameWidth = initialFrame.Width;
            _frameHeight = initialFrame.Height;

            _timeForCurrentFrame =  new Timer(TimeSpan.FromSeconds(_frameDisplayTime));
        }


    }
}

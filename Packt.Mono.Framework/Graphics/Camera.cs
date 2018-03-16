using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Graphics
{
    public class Camera
    {

        private Vector2 _position = Vector2.Zero;
        private Vector2 _viewPortSize = Vector2.Zero;
        public Rectangle WorldRectangle { get; set; } = new Rectangle(0, 0, 0, 0);

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = new Vector2(
                    MathHelper.Clamp(value.X, WorldRectangle.X, WorldRectangle.Width - ViewPortWidth),
                    MathHelper.Clamp(value.Y, WorldRectangle.Y, WorldRectangle.Height - ViewPortHeight)
                );
            }
        }

        public int ViewPortWidth
        {
            get => (int)_viewPortSize.X;
            set => _viewPortSize.X = value;
        }

        public int ViewPortHeight
        {
            get => (int)_viewPortSize.Y;
            set => _viewPortSize.Y = value;
        }

        public Rectangle ViewPort => new Rectangle((int)Position.X, (int)Position.Y, ViewPortWidth, ViewPortHeight);

        public void Move(Vector2 offset) => Position += offset;
        public bool ObjectIsVisible(Rectangle bounds) => ViewPort.Intersects(bounds);
        public Vector2 Transform(Vector2 point) => point - Position;
        public Rectangle Transform(Rectangle rect) => new Rectangle(rect.Left - (int)Position.X, rect.Top - (int)Position.Y, rect.Width, rect.Height);

        public Camera(Rectangle worldRect, int viewPortWidth, int viewPortHeight)
        {
            WorldRectangle = worldRect;
            ViewPortWidth = viewPortWidth;
            ViewPortHeight = viewPortHeight;
        }


    }
}

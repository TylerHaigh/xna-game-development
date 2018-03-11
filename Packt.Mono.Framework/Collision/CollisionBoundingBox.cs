using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Collision
{
    public class CollisionBoundingBox : CollisionGeometry
    {
        public Vector2 Location { get; set; }
        public Rectangle BaseRectangle { get; set; }
        public int XPadding { get; set; } // Padding inside Base Rect
        public int YPadding { get; set; }

        public Rectangle BoundingRect => new Rectangle(
            (int) Location.X + XPadding,
            (int) Location.Y + YPadding,
            BaseRectangle.Width  - (XPadding * 2),
            BaseRectangle.Height - (YPadding * 2)
        );

        public CollisionBoundingBox() : this(Vector2.Zero, new Rectangle(0, 0, 0, 0)) { }

        public CollisionBoundingBox(Vector2 location, Rectangle baseRectangle, int xPadding = 0, int yPadding = 0)
        {
            this.Location = location;
            this.BaseRectangle = baseRectangle;
            this.XPadding = xPadding;
            this.YPadding = yPadding;
        }

        public bool Intersects(CollisionBoundingBox other)
        {
            return this.BoundingRect.Intersects(other.BoundingRect);
        }

        public bool Intersects(Rectangle other)
        {
            return this.BoundingRect.Intersects(other);
        }

        public bool Intersects(Vector2 other)
        {
            return Intersects(new Rectangle((int)other.X, (int)other.Y, 1, 1));
        }

        public bool Intersects(Point other)
        {
            return Intersects(new Vector2(other.X, other.Y));
        }

    }
}

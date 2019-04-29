using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Collision
{
    public class CollisionCircle : CollisionGeometry
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public CollisionCircle() : this(Vector2.Zero, 0) { }

        public CollisionCircle(Vector2 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public bool Intersects(CollisionCircle other)
        {
            return Vector2.Distance(this.Center, other.Center) < (this.Radius + other.Radius);
        }

        public bool Intersects(Vector2 other, float otherRadius)
        {
            return Intersects(new CollisionCircle(other, otherRadius));
        }

        public bool Intersects(Vector2 other)
        {
            return Intersects(other, 0);
        }

        public bool Intersects(Point other)
        {
            return Intersects(new Vector2 { X = other.X, Y = other.Y });
        }

        public bool Intersects(CollisionBoundingBox other)
        {
            // See https://stackoverflow.com/a/402019/2442468 for how to do collision
            // Option 1: Circle center is within boundingbox
            if (other.Intersects(this.Center)) return true;

            // Option 2, BoundingBox has a point inside the circle
            if (this.Intersects(other.BoundingRect.TopLeft())) return true;
            if (this.Intersects(other.BoundingRect.TopRight())) return true;
            if (this.Intersects(other.BoundingRect.BottomLeft())) return true;
            if (this.Intersects(other.BoundingRect.BottomRight())) return true;

            // No collision detected
            return false;
        }

    }
}

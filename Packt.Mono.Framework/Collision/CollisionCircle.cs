using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Collision
{
    public class CollisionCircle : ICollisionGeometry<CollisionCircle>
    {
        public readonly Vector2 Center;
        public readonly float Radius;

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

    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    public class CollisionCircle
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

    }
}

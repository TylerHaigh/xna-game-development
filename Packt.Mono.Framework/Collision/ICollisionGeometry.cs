using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Collision
{

    public interface ICollisionGeometry<T>
    {
        bool Intersects(T other);
    }

    public static class CollisionEngine
    {
        public static bool Intersects(CollisionCircle a, CollisionCircle b) => a.Intersects(b);
        public static bool Intersects(CollisionCircle a, Vector2 b) => a.Intersects(b);
        public static bool Intersects(CollisionCircle a, Point b) => a.Intersects(b);

        public static bool Intersects(CollisionBoundingBox a, CollisionBoundingBox b) => a.Intersects(b);
        public static bool Intersects(CollisionBoundingBox a, Rectangle b) => a.Intersects(b);
    }
}

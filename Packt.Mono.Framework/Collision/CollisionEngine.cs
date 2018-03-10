using Microsoft.Xna.Framework;

namespace Packt.Mono.Framework.Collision
{
    public static class CollisionEngine
    {
        public static bool Intersects(CollisionCircle a, CollisionCircle b) => a.Intersects(b);
        public static bool Intersects(CollisionCircle a, Vector2 b) => a.Intersects(b);
        public static bool Intersects(CollisionCircle a, Point b) => a.Intersects(b);

        public static bool Intersects(CollisionBoundingBox a, CollisionBoundingBox b) => a.Intersects(b);
        public static bool Intersects(CollisionBoundingBox a, Rectangle b) => a.Intersects(b);
    }
}

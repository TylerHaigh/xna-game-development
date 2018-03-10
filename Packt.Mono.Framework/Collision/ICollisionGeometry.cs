using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Collision
{

    public abstract class CollisionGeometry { }


    public enum CollisionType
    {
        BoundingBox,
        Circle
    }


    public class CollisionGeometryFactory
    {

        public static event EventHandler<CollisionArgs> CreatedCollisionGeometry;

        public static CollisionGeometry CreateGeometry(CollisionType key)
        {
            switch (key)
            {
                case CollisionType.BoundingBox:
                    {
                        var box = new CollisionBoundingBox();
                        CreatedCollisionGeometry?.Invoke(null, new CollisionArgs(box));
                        return box;
                    }
                case CollisionType.Circle:
                    {
                        var c = new CollisionCircle();
                        CreatedCollisionGeometry?.Invoke(null, new CollisionArgs(c));
                        return c;
                    }
                default: throw new ArgumentException("Collision Geometry not defined for geometry type");
            }
        }
    }

    public class CollisionArgs
    {
        public readonly CollisionGeometry Geometry;
        public CollisionArgs(CollisionGeometry geom) { this.Geometry = geom; }
    }
}

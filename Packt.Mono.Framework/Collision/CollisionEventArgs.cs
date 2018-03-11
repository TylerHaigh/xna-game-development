using Packt.Mono.Framework.Entities;

namespace Packt.Mono.Framework.Collision
{
    public class CollisionEventArgs
    {
        public CollisionComponent ThisComponent { get; set; }
        public CollisionComponent OtherComponent { get; set; }
        public bool CollisionResolved { get; set; }
    }
}

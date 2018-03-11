using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Collision
{

    public abstract class CollisionGeometry
    {
    }

    public class CreatedCollisionComponentArgs
    {
        public readonly CollisionComponent Component;

        public CreatedCollisionComponentArgs(CollisionComponent component) {
            this.Component = component;
        }
    }
}

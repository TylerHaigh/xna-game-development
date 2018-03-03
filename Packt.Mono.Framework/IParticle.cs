using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    public interface IParticle
    {
        Vector2 Location { get; set; }
        Vector2 Velocity { get; set; }
    }
}

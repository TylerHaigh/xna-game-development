using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Utils
{
    public class Size2D
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Size2D() : this(0, 0) { }
        public Size2D(float w, float h) { this.Width = w; this.Height = h; }
    }

    public class Size3D
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public Size3D() : this(0, 0, 0) { }
        public Size3D(float w, float h, float d) { this.Width = w; this.Height = h; this.Depth = d; }
    }
}

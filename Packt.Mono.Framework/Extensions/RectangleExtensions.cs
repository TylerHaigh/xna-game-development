using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Extensions
{
    public static class RectangleExtensions
    {
        public static Vector2 TopLeft(this Rectangle rect)
        {
            return new Vector2
            {
                X = rect.Left,
                Y = rect.Top
            };
        }

        public static Vector2 TopRight(this Rectangle rect)
        {
            return new Vector2
            {
                X = rect.Right,
                Y = rect.Top
            };
        }

        public static Vector2 BottomLeft(this Rectangle rect)
        {
            return new Vector2
            {
                X = rect.Left,
                Y = rect.Bottom
            };
        }

        public static Vector2 BottomRight(this Rectangle rect)
        {
            return new Vector2
            {
                X = rect.Right,
                Y = rect.Bottom
            };
        }
    }
}

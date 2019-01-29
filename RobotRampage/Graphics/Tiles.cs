using Microsoft.Xna.Framework;
using RobotRampage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Graphics
{
    public enum Tiles
    {
        GreyFloor = 0,
        OrangeFloor,
        PurpleFloor,
        PinkFloor,

        GreyWall,
        OrageWall,
        PurpleWall,
        PinkWall
    }

    public static class EnumExtensions
    {
        public static int ToInt(this Enum e) => Convert.ToInt32(e);
    }

    public static class RandomExtensions
    {
        public static T Next<T> (this Random rand, T start, T end) where T : Enum
        {
            int next = rand.Next(start.ToInt(), end.ToInt());
            return (T)Enum.ToObject(typeof(T), next);
        }
    }
}

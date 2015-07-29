using System;
using System.Collections.Generic;
using System.Text;

namespace EgyptLazerGame.Classes
{
    public struct Point
    {
        public int X, Y;
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
    }
}

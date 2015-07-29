using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EgyptLazerGame.Classes.XNA
{
    class PointConversion
    {
        public static Vector2 toVector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Point toPoint(Vector2 v)
        {
            return new Point(v.ToPoint().X, v.ToPoint().Y);
        }
    }
}

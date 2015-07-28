using System;
using System.Collections.Generic;
using System.Text;

namespace EgyptLazerGame.Classes
{
    class CellObject
    {
        internal enum Direction { Up = 0x01, Right = 0x02, Down = 0x04, Left = 0x08 }
        protected Point position;
        protected Direction direction;
        public CellObject(Point pos, Direction dir)
        {
            position = pos;
            direction = dir;
        }

        public Point Position { get { return position; } private set { } }
        public Direction MoveDirection { get { return direction; } private set { } }

        public void Rotate(bool isClockwise)
        {
            //возможно нужен рефакторинг
            if (isClockwise)
                direction = (Direction)((((int)direction) << 1) % 15);
            else
                direction = direction == Direction.Up ?
                    Direction.Left :
                    (Direction)((int)direction / 2);
        }

        public static Direction MirrorDirection(Direction dir)
        {
            Direction newDir = new Direction();
            if (dir.HasFlag(Direction.Down))
                newDir |= Direction.Up;
            if (dir.HasFlag(Direction.Up))
                newDir |= Direction.Down;
            if (dir.HasFlag(Direction.Left))
                newDir |= Direction.Right;
            if (dir.HasFlag(Direction.Right))
                newDir |= Direction.Left;
            return newDir;
        }

    }
}

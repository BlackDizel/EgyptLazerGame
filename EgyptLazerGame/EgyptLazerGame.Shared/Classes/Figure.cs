using System;
using System.Collections.Generic;
using System.Text;

namespace EgyptLazerGame.Classes
{
    class Figure:CellObject
    {
        public enum Type {sphinx, pharaoh, pyramid, scarab, anubis}
        public enum LightCollision { GameOver, None, Death, clockwise, counterclockwise }
        

        private Type type;
        private int playerID;

        public int PlayerID { get { return playerID; } private set { } }

        public Type FigureType { get { return type; } private set { } }

        public Figure(Type t, Point pos, Direction dir, int playerID):base(pos,dir)
        {
            if ((pos.X < 0) || (pos.X < 0) || (pos.X > 9) || (pos.Y > 7)) throw new ArgumentOutOfRangeException();

            type = t;
            this.playerID = playerID;
        }

        public static void DirectionCorrectTest(Direction dir)
        {
            if ((dir.HasFlag(Direction.Up)) && (dir.HasFlag(Direction.Down)) ||
                (dir.HasFlag(Direction.Left)) && (dir.HasFlag(Direction.Right))) throw new ArgumentException();

        }

        public static Point CalcNewPoint(Point p, Direction dir)
        {
            Point pt = new Point(p.X,p.Y);

            if (dir.HasFlag(Direction.Down))
                pt.Y += 1;
            if (dir.HasFlag(Direction.Up))
                pt.Y -= 1;
            if (dir.HasFlag(Direction.Left))
                pt.X -= 1;
            if (dir.HasFlag(Direction.Right))
                pt.X += 1;
            return pt;
 
        }

        public static Direction CalcDirection(Point pos, Point other)
        {
            if (pos == other) throw new InvalidOperationException("невозможно расчитать направление на ту же точку");

            Direction result;

            if (other.X > pos.X)
                result = Direction.Right;
            else
                if (other.X < pos.X)
                    result = Direction.Left;
                else //X1==X2
                    if (other.Y > pos.Y)
                        result = Direction.Down;
                    else
                        if (other.Y < pos.Y)
                            result = Direction.Up;
                        else throw new InvalidOperationException("невозможно расчитать направление на ту же точку");                

            if (other.Y>pos.Y)
                result |= Direction.Down;
            else
            if (other.Y < pos.Y)
                result |= Direction.Up;

            return result;
        }

        public void Move(Direction dir)
        {
            DirectionCorrectTest(dir);
            if (type==Type.sphinx) throw new TypeLoadException();
            position = CalcNewPoint(position, dir);
        }

        
        
        /// <summary>
        /// коллизия со светом
        /// </summary>
        /// <param name="dir">направление от света к фигуре</param>
        /// <returns></returns>
        public LightCollision OnLight(Direction dir)
        {
            switch (type)
            {
                case Type.anubis:
                    if ((direction ==Direction.Up)&&(dir==Direction.Down)||
                         (direction ==Direction.Right)&&(dir==Direction.Left)||
                         (direction ==Direction.Down)&&(dir==Direction.Up)||
                         (direction ==Direction.Left)&&(dir==Direction.Right))
                        return LightCollision.None;
                    else return LightCollision.Death;                    
                case Type.pharaoh:
                    return LightCollision.GameOver;                    
                case Type.pyramid:
                    //пирамида смотрит "вверх", если зеркало по побочной диагонали матрицы "\" и направлено вверх-вправо
                    switch (dir)
                    {
                        case Direction.Down:
                            switch (direction)
                            {
                                case Direction.Up:
                                    return LightCollision.counterclockwise;      
                                case Direction.Right:  
                                case Direction.Down:
                                    return LightCollision.Death;
                                case Direction.Left:
                                    return LightCollision.clockwise;                                                              
                            }
                            break;
                        case Direction.Left:
                            switch (direction)
                            {
                                case Direction.Up:
                                    return LightCollision.clockwise;
                                case Direction.Right:
                                    return LightCollision.counterclockwise;      
                                case Direction.Down:
                                case Direction.Left:
                                    return LightCollision.Death;
                                                          
                            }
                            break;
                        case Direction.Up:
                            switch (direction)
                            {
                                case Direction.Up:
                                case Direction.Left:
                                    return LightCollision.Death;
                                case Direction.Right:
                                    return LightCollision.clockwise;
                                case Direction.Down:
                                    return LightCollision.counterclockwise;      
                            }
                            break;
                        case Direction.Right:
                            switch (direction)
                            {
                                case Direction.Up:
                                case Direction.Right:
                                    return LightCollision.Death;
                                case Direction.Down:
                                    return LightCollision.clockwise;
                                case Direction.Left:
                                    return LightCollision.counterclockwise;      
                            }
                            break;
                    }
                    

                    break;
                case Type.scarab:
                    //скарабей смотрит "вверх", если его голова смотрит вверх-направо — главная диагональ матрицы "/"
                    if ((direction == Direction.Up) || (direction == Direction.Down)) 
                    {
                        if ((dir ==Direction.Down)||(dir ==Direction.Up))
                            return LightCollision.clockwise;
                        else return LightCollision.counterclockwise;
                    }
                    else
                    {
                        if ((dir == Direction.Down) || (dir == Direction.Up))
                            return LightCollision.counterclockwise;
                        else return LightCollision.clockwise;
                    }
                case Type.sphinx:
                    return LightCollision.None;
                    
            }
            return LightCollision.None;

        }

        

    }
}

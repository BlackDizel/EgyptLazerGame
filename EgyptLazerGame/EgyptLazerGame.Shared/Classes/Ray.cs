using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EgyptLazerGame.Classes
{
    class Ray
    {
        List<CellObject> lights;
        CellObject last;
        bool isGameOver;
        public bool IsGameOver { get { return isGameOver; } private set { } }
        public Ray(Point startPos, CellObject.Direction dir)
        {
            isGameOver = false;
            lights = new List<CellObject>();
            last = new CellObject(startPos, dir);
            lights.Add(last);
 
        }


        public List<CellObject> Lights { get { return lights; } private set { } }
        public void Move()
        {
            CellObject c = Step();

            while ((c != null) && (lightExist(c.Position) == null))
            {
                lights.Add(last);
                c = Step();
            }

        }

        CellObject lightExist(Point pos)
        {
            return (from c in lights where c.Position == pos select c).FirstOrDefault();            
        }
        CellObject Step()
        {
            if ((last.MoveDirection == CellObject.Direction.Up) && (last.Position.Y == 0) ||
                (last.MoveDirection == CellObject.Direction.Right) && (last.Position.X == 9)||
                (last.MoveDirection == CellObject.Direction.Down) && (last.Position.Y == 7)||
                (last.MoveDirection == CellObject.Direction.Left) && (last.Position.X == 0)
                )
                return null;

            Point newPos = last.Position;
            switch (last.MoveDirection)
            {
                case CellObject.Direction.Up:
                    newPos.Y-=1;
                    break;
                case CellObject.Direction.Right:
                    newPos.X+=1;
                    break;
                case CellObject.Direction.Down:
                    newPos.Y+=1;
                    break;
                case CellObject.Direction.Left:
                    newPos.X-=1;
                    break;
            }
            
            last = new CellObject(newPos,last.MoveDirection);
            
            Figure f = (from c in Field.figures where c.Position==newPos select c).FirstOrDefault();
            if (f!=null)
            {
                Figure.LightCollision lc= f.OnLight(last.MoveDirection);
                switch (lc)
                {
                    case Figure.LightCollision.None: return null;
                    case Figure.LightCollision.GameOver:
                        isGameOver = true;
                        return null; 
                    case Figure.LightCollision.Death: 
                        Field.figures.Remove(f);
                        return null;
                    case Figure.LightCollision.clockwise:
                        last.Rotate(true);
                        break;
                    case Figure.LightCollision.counterclockwise:
                        last.Rotate(false);
                        break;
                }
            }
            
            return last;
        }

    }
}

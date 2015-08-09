using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EgyptLazerGame.Classes
{
    public class RayLight : CellObject
    {
        public enum State { Forward, Rotate, close }
        
        public RayLight(Point pos, Direction dir) : base(pos, dir) { }
        public State state;
        public bool clockwise;
    }
    public class Ray
    {

        public int LooserId { get { return loserId; } private set { } }
        List<RayLight> lights;
        RayLight last;
        bool isGameOver;
        private int loserId;
        public bool IsGameOver { get { return isGameOver; } private set { } }
        public Ray(Point startPos, CellObject.Direction dir)
        {
            isGameOver = false;
            loserId=-1;
            lights = new List<RayLight>();
            last = new RayLight(startPos, dir);
            last.state = EgyptLazerGame.Classes.RayLight.State.close;
            lights.Add(last);
 
        }


        public List<RayLight> Lights { get { return lights; } private set { } }
        public void Move()
        {
            RayLight c = Step();

            while ((c != null) && (lightExist(c.Position) == null))
            {
                lights.Add(last);
                c = Step();
            }

        }

        RayLight lightExist(Point pos)
        {
            return (from c in lights where c.Position == pos select c).FirstOrDefault();            
        }
        RayLight Step()
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

            last = new RayLight(newPos, last.MoveDirection);
            
            Figure f = (from c in Field.figures where c.Position==newPos select c).FirstOrDefault();
            if (f!=null)
            {
                Figure.LightCollision lc= f.OnLight(last.MoveDirection);
                switch (lc)
                {
                    case Figure.LightCollision.None: return null;
                    case Figure.LightCollision.GameOver:
                        isGameOver = true;
                        loserId = f.PlayerID;
                        return null; 
                    case Figure.LightCollision.Death:
                        Field.figures.Remove(f);
                        return null;
                    case Figure.LightCollision.clockwise:
                        last.state = EgyptLazerGame.Classes.RayLight.State.Rotate;
                        last.Rotate(true);
                        last.clockwise = true;
                        break;
                    case Figure.LightCollision.counterclockwise:
                        last.state = EgyptLazerGame.Classes.RayLight.State.Rotate;
                        last.Rotate(false);
                        last.clockwise = false;
                        break;
                }
            }
            
            return last;
        }

    }
}

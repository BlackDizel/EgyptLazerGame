using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EgyptLazerGame.Classes.XNA
{
    class UI
    {
        Texture2D tBt;
        List<Vector2> lBtnDirection;
        Vector2 btnRotateLeft, btnRotateRight, btnTurn;
        Vector2[] field;
         

        public enum Action {    Move,
                                Rotate,
                                Turn,
                                SelectFigure                                
                            }
        
        public Action UIAction;

        public bool IsClockwise;
        public CellObject.Direction direction;

        public UI()
        {
            lBtnDirection = new List<Vector2>
            {
                new Vector2(11*GameClass.CellSize,0),
                new Vector2(12*GameClass.CellSize,0),
                new Vector2(13*GameClass.CellSize,0),                

                new Vector2(11*GameClass.CellSize,GameClass.CellSize),
                new Vector2(13*GameClass.CellSize,GameClass.CellSize),

                new Vector2(11*GameClass.CellSize,2*GameClass.CellSize),
                new Vector2(12*GameClass.CellSize,2*GameClass.CellSize),
                new Vector2(13*GameClass.CellSize,2*GameClass.CellSize)
            };

            btnRotateLeft = new Vector2(11*GameClass.CellSize,4*GameClass.CellSize);
            btnRotateRight = new Vector2(13*GameClass.CellSize,4*GameClass.CellSize);
            btnTurn = new Vector2(12 * GameClass.CellSize, 7 * GameClass.CellSize);

            field = new Vector2[80];
            for (int j = 0; j < 8; ++j)
                for (int i = 0; i < 10; ++i)
                    field[j * 10 + i] = new Vector2(i, j)*GameClass.CellSize;
        }


        public void Update(GameTime gameTime)
        {
            
        }

        public void SetControlMovePos(Vector2 pos)
        {
            lBtnDirection.Clear();
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Up));
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Down));
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Left));
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Right));
                         
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Up | CellObject.Direction.Left));
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Up | CellObject.Direction.Right));
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Down | CellObject.Direction.Left));
            lBtnDirection.Add(setControlMovePos(pos, CellObject.Direction.Down | CellObject.Direction.Right));
        }

        public void SetControlMovePos(Vector2 pos,List<CellObject.Direction> list)
        {
            if (list == null) return;
            lBtnDirection.Clear();
            foreach (var el in list)
                lBtnDirection.Add(setControlMovePos(pos, el));
 
        }

        private Vector2 setControlMovePos(Vector2 pos, CellObject.Direction dir)
        {
            Figure.DirectionCorrectTest(dir);
            Vector2 newPos = pos;
            if (dir.HasFlag(CellObject.Direction.Up))
                newPos += new Vector2(0, -GameClass.CellSize);
            if (dir.HasFlag(CellObject.Direction.Down))
                newPos += new Vector2(0, GameClass.CellSize);
            if (dir.HasFlag(CellObject.Direction.Right))
                newPos += new Vector2(GameClass.CellSize,0);
            if (dir.HasFlag(CellObject.Direction.Left))
                newPos += new Vector2(-GameClass.CellSize,0);
            return newPos;
        }

        public void Input(Vector2? SelectedFigurePos, Vector2 msPosition)
        {

            UIAction = UI.Action.SelectFigure;
            if (SelectedFigurePos != null)
            {
                var p = (msPosition / GameClass.CellSize).ToPoint().ToVector2() * GameClass.CellSize;
                Vector2 v = (from c in lBtnDirection where c.Equals(p) select c).FirstOrDefault();
                if (v != default(Vector2))
                {
                    direction = Figure.CalcDirection((SelectedFigurePos.Value*GameClass.CellSize).ToPoint(), v.ToPoint());
                    UIAction = UI.Action.Move;
                }
                else if (p.Equals(btnRotateLeft))
                {
                    IsClockwise = false;
                    UIAction = UI.Action.Rotate;
                }
                else if (p.Equals(btnRotateRight))
                {
                    IsClockwise = true;
                    UIAction = UI.Action.Rotate;
                }
                else if (p.Equals(btnTurn))
                {
                    UIAction = UI.Action.Turn;
                }
            }
        }
        

        public void Load(ContentManager Content)
        {
            tBt = Content.Load<Texture2D>("cell.png");
           
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var el in field)
                sb.Draw(tBt, el, Color.White);

            
            foreach (var el in lBtnDirection)
                sb.Draw(tBt, el, Color.White);
            
            sb.Draw(tBt,btnRotateLeft,Color.White); 
            sb.Draw(tBt, btnRotateRight, Color.White);
            sb.Draw(tBt, btnTurn, Color.White);
        }
    }
}

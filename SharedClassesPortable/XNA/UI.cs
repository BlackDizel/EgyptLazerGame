using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharedClassesPortable.XNA;

namespace EgyptLazerGame.Classes.XNA
{
    class UI
    {
        Texture2D tBt, tField, tRotateLeft, tRotateRight, tEnd, tDirRight, tDirSE;
        SpriteFont sf;
        List<DrawableObject> lBtnDirection;
        Vector2 btnRotateLeft, btnRotateRight, btnTurn;
        Vector2[] field;
        Vector2 vText;

        public enum Action
        {
            Move,
            Rotate,
            Turn,
            SelectFigure
        }

        public Action UIAction;

        public bool IsClockwise;
        public CellObject.Direction direction;

        public UI()
        {

        }


        int oldcell;
        public void Update(GameTime gameTime)
        {
            if (oldcell != GameClass.CellSize)
            {
                oldcell = GameClass.CellSize;
                if (lBtnDirection == null)
                    lBtnDirection = new List<DrawableObject>();

                btnRotateLeft = new Vector2(11 * GameClass.CellSize, 4 * GameClass.CellSize);
                btnRotateRight = new Vector2(13 * GameClass.CellSize, 4 * GameClass.CellSize);
                btnTurn = new Vector2(11 * GameClass.CellSize, 6 * GameClass.CellSize);

                field = new Vector2[80];
                for (int j = 0; j < 8; ++j)
                    for (int i = 0; i < 10; ++i)
                        field[j * 10 + i] = new Vector2(i, j) * GameClass.CellSize;

                vText = new Vector2(11 * GameClass.CellSize, 2 * GameClass.CellSize);
            }

        }

        public void clearDirections()
        {
            lBtnDirection.Clear();
        }

        public void SetControlMovePos(Vector2 pos, List<CellObject.Direction> list)
        {
            if (list == null) return;
            lBtnDirection.Clear();
            foreach (var el in list)
                lBtnDirection.Add(setControlMovePos(pos, el));

        }

        private DrawableObject setControlMovePos(Vector2 pos, CellObject.Direction dir)
        {
            Figure.DirectionCorrectTest(dir);
            DrawableObject obj = new DrawableObject();
            Vector2 newPos = pos;
            Texture2D texture = null;
            float rotate = 0f;

            if (dir.HasFlag(CellObject.Direction.Up))
            {
                newPos += new Vector2(0, -GameClass.CellSize);
                texture = tDirRight;
                rotate = -90f;
            }
            if (dir.HasFlag(CellObject.Direction.Down))
            {
                newPos += new Vector2(0, GameClass.CellSize);
                texture = tDirRight;
                rotate = 90f;
            }
            if (dir.HasFlag(CellObject.Direction.Right))
            {
                newPos += new Vector2(GameClass.CellSize, 0);
                if (texture == null) texture = tDirRight;
                else if (rotate == -90f)
                {
                    texture = tDirSE;
                }
                else if (rotate == 90f)
                {
                    texture = tDirSE;
                    rotate = 0f;
                }
            }

            if (dir.HasFlag(CellObject.Direction.Left))
            {
                newPos += new Vector2(-GameClass.CellSize, 0);
                if (texture == null)
                {
                    texture = tDirRight;
                    rotate = 180f;
                }
                else if (rotate == -90f)
                {
                    texture = tDirSE;
                    rotate = 180f;

                }
                else if (rotate == 90f)
                {
                    texture = tDirSE;
                }
            }

            obj.Position = newPos;
            obj.texture = texture;
            obj.rotate = rotate;

            return obj;
        }

        public void Input(Vector2? SelectedFigurePos, Vector2 msPosition)
        {

            UIAction = UI.Action.SelectFigure;
            if (SelectedFigurePos != null)
            {
                var p = (msPosition / GameClass.CellSize).ToPoint().ToVector2() * GameClass.CellSize;
                DrawableObject v = (from c in lBtnDirection where c.Position.Equals(p) select c).FirstOrDefault();
                if (v != null)
                {
                    direction = Figure.CalcDirection(PointConversion.toPoint(SelectedFigurePos.Value * GameClass.CellSize), PointConversion.toPoint(v.Position));
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
                else if (p.Equals(btnTurn) || p.Equals(btnTurn + new Vector2(GameClass.CellSize, 0)) || p.Equals(btnTurn + new Vector2(2 * GameClass.CellSize, 0)))
                {
                    UIAction = UI.Action.Turn;
                }
            }
        }


        public void Load(ContentManager Content)
        {
            tBt = Content.Load<Texture2D>("cell");

            tField = Content.Load<Texture2D>("map");
            tRotateLeft = Content.Load<Texture2D>("button_left");
            tRotateRight = Content.Load<Texture2D>("button_right");
            tEnd = Content.Load<Texture2D>("end");
            tDirRight = Content.Load<Texture2D>("direction/right");
            tDirSE = Content.Load<Texture2D>("direction/se");

            sf = Content.Load<SpriteFont>("font");

        }

        public void Draw(SpriteBatch sb)
        {
            if (field != null)
                foreach (var el in field)
                    sb.Draw(
                        texture: tField
                        , destinationRectangle: new Rectangle((int)el.X, (int)el.Y, GameClass.CellSize, GameClass.CellSize)
                        , color: Color.White);


            if (lBtnDirection != null)
                foreach (var el in lBtnDirection)
                    sb.Draw(
                        texture: el.texture
                        , destinationRectangle: new Rectangle((int)el.Position.X + GameClass.CellSize / 2, (int)el.Position.Y + GameClass.CellSize / 2, GameClass.CellSize, GameClass.CellSize)
                        , rotation: MathHelper.ToRadians(el.rotate)
                        , origin: new Vector2(GameClass.CellSize / 2, GameClass.CellSize / 2)
                        , color: Color.White);

            sb.Draw(
                texture: tRotateLeft
                , destinationRectangle: new Rectangle((int)btnRotateLeft.X, (int)btnRotateLeft.Y, GameClass.CellSize, GameClass.CellSize)
                , color: Color.White);

            sb.Draw(
                texture: tRotateRight
                , destinationRectangle: new Rectangle((int)btnRotateRight.X, (int)btnRotateRight.Y, GameClass.CellSize, GameClass.CellSize)
                , color: Color.White);

            sb.Draw(
                texture: tEnd
                , destinationRectangle: new Rectangle((int)btnTurn.X, (int)btnTurn.Y, GameClass.CellSize * 3, GameClass.CellSize)
                , color: Color.White);

            sb.DrawString(sf, "some text", vText, Color.White);
        }
    }
}

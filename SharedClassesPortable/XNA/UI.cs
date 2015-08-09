using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharedClassesPortable.XNA;
using System.Diagnostics;

namespace EgyptLazerGame.Classes.XNA
{
    class UI
    {
        Texture2D tBt, tField, tRotateLeft, tRotateRight, tEnd, tDirRight, tDirSE, tForbOne, tForbTwo, tFade;
        SpriteFont sf, sfText;
        List<DrawableObject> lBtnDirection;
        Vector2 btnRotateLeft, btnRotateRight, btnTurn;
        Vector2[] field;
        Vector2 vText, vTextFigure, vTextGameOver;

        Vector2[] vOneForbidden = 
        {
            new Vector2(9,0),
            new Vector2(9,1),
            new Vector2(9,2),
            new Vector2(9,3),
            new Vector2(9,4),
            new Vector2(9,5),
            new Vector2(9,6),
            new Vector2(9,7),

            new Vector2(1,0),
            new Vector2(1,7),
        };

        Vector2[] vTwoForbidden = 
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(0,2),
            new Vector2(0,3),
            new Vector2(0,4),
            new Vector2(0,5),
            new Vector2(0,6),
            new Vector2(0,7),

            new Vector2(8,0),
            new Vector2(8,7),
        };

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
            if (lBtnDirection == null)
                lBtnDirection = new List<DrawableObject>();

            btnRotateLeft = new Vector2(11, 4);
            btnRotateRight = new Vector2(13, 4);
            btnTurn = new Vector2(11, 6);

            field = new Vector2[80];
            for (int j = 0; j < 8; ++j)
                for (int i = 0; i < 10; ++i)
                    field[j * 10 + i] = new Vector2(i, j);

            vText = new Vector2(11, 1);
            vTextFigure = new Vector2(10.5f, 2);
            vTextGameOver = new Vector2(2.5f, 3.8f);
        }

        public void Update(GameTime gameTime)
        { }

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
                newPos += new Vector2(0, -1);
                texture = tDirRight;
                rotate = -90f;
            }
            if (dir.HasFlag(CellObject.Direction.Down))
            {
                newPos += new Vector2(0, 1);
                texture = tDirRight;
                rotate = 90f;
            }
            if (dir.HasFlag(CellObject.Direction.Right))
            {
                newPos += new Vector2(1, 0);
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
                newPos += new Vector2(-1, 0);
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
                var p = (msPosition / GameClass.CellSize).ToPoint().ToVector2();
                DrawableObject v = (from c in lBtnDirection where c.Position.Equals(p) select c).FirstOrDefault();
                if (v != null)
                {
                    direction = Figure.CalcDirection(PointConversion.toPoint(SelectedFigurePos.Value), PointConversion.toPoint(v.Position));
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
                else if (p.Equals(btnTurn) || p.Equals(btnTurn + new Vector2(1, 0)) || p.Equals(btnTurn + new Vector2(2, 0)))
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

            tForbOne = Content.Load<Texture2D>("no_red");
            tForbTwo = Content.Load<Texture2D>("no_blue");
            tFade = Content.Load<Texture2D>("white");


            sf = Content.Load<SpriteFont>("font");
            sfText = Content.Load<SpriteFont>("fontText");
        }

        public void Draw(SpriteBatch sb, int player, String selectedFigureText)
        {
            if (field != null)
                foreach (var el in field)
                    sb.Draw(
                        texture: tField
                        , destinationRectangle: new Rectangle((int)el.X * GameClass.CellSize, (int)el.Y * GameClass.CellSize, GameClass.CellSize, GameClass.CellSize)
                        , color: Color.White);

            foreach (var el in vOneForbidden)
                sb.Draw(tForbOne, new Rectangle((int)el.X * GameClass.CellSize, (int)el.Y * GameClass.CellSize, GameClass.CellSize, GameClass.CellSize), Color.White);

            foreach (var el in vTwoForbidden)
                sb.Draw(tForbTwo, new Rectangle((int)el.X * GameClass.CellSize, (int)el.Y * GameClass.CellSize, GameClass.CellSize, GameClass.CellSize), Color.White);



            if (lBtnDirection != null)
                foreach (var el in lBtnDirection)
                    sb.Draw(
                        texture: el.texture
                        , destinationRectangle: new Rectangle((int)el.Position.X * GameClass.CellSize + GameClass.CellSize / 2, (int)el.Position.Y * GameClass.CellSize + GameClass.CellSize / 2, GameClass.CellSize, GameClass.CellSize)
                        , rotation: MathHelper.ToRadians(el.rotate)
                        , origin: new Vector2(GameClass.CellSize / 2, GameClass.CellSize / 2)
                        , color: Color.White);

            sb.Draw(
                texture: tRotateLeft
                , destinationRectangle: new Rectangle((int)btnRotateLeft.X * GameClass.CellSize, (int)btnRotateLeft.Y * GameClass.CellSize, GameClass.CellSize, GameClass.CellSize)
                , color: Color.White);

            sb.Draw(
                texture: tRotateRight
                , destinationRectangle: new Rectangle((int)btnRotateRight.X * GameClass.CellSize, (int)btnRotateRight.Y * GameClass.CellSize, GameClass.CellSize, GameClass.CellSize)
                , color: Color.White);

            sb.Draw(
                texture: tEnd
                , destinationRectangle: new Rectangle((int)btnTurn.X * GameClass.CellSize, (int)btnTurn.Y * GameClass.CellSize, GameClass.CellSize * 3, GameClass.CellSize)
                , color: Color.White);

            sb.DrawString(sf, "ХОД " + (player == 0 ? "КРАСНОГО" : "СИНЕГО") + " ИГРОКА", vText * GameClass.CellSize, Color.White);

            sb.DrawString(sfText, selectedFigureText, vTextFigure * GameClass.CellSize, Color.White);

        }

        public void DrawGameOver(SpriteBatch sb, int winner)
        {
            sb.Draw(tFade, new Rectangle(0, 0, 14 * GameClass.CellSize, 8 * GameClass.CellSize), new Color(0,0,0,0.5f));
            sb.DrawString(sf, "ИГРА ОКОНЧЕНА.\n\rФАРАОН ПОВЕРЖЕН. ПОБЕДА " + (winner == 0 ? "КРАСНОГО" : "СИНЕГО") + " ИГРОКА", vTextGameOver * GameClass.CellSize, Color.White);
        }
    }
}

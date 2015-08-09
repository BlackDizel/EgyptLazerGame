using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace EgyptLazerGame.Classes.XNA
{
    public class GameClass : DrawableGameComponent
    {
        UI ui;
        SpriteBatch sb;
        Field field;
        Texture2D[] tFigs;
        Texture2D[] tRay;
        Texture2D tSelected;
        public MouseState oldMS;
        KeyboardState oldKS;
        bool isGameOver = false;
        int winnerId=-1;

        public static int CellSize = 100;

        public GameClass(Game game)
            : base(game)
        {
            ui = new UI();
            field = new Field();

            oldMS = Mouse.GetState();
            oldKS = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            ui.Load(this.Game.Content);
            sb = new SpriteBatch(GraphicsDevice);

            tFigs = new Texture2D[10]
            {
                Game.Content.Load<Texture2D>("red/sphinx"),
                Game.Content.Load<Texture2D>("red/pharaon"),
                Game.Content.Load<Texture2D>("red/pyramid"),
                Game.Content.Load<Texture2D>("red/scarab"),
                Game.Content.Load<Texture2D>("red/anubis"),
                Game.Content.Load<Texture2D>("blue/sphinx"),
                Game.Content.Load<Texture2D>("blue/pharaon"),
                Game.Content.Load<Texture2D>("blue/pyramid"),
                Game.Content.Load<Texture2D>("blue/scarab"),
                Game.Content.Load<Texture2D>("blue/anubis")

            };
            tRay = new Texture2D[3]
            {
                Game.Content.Load<Texture2D>("ray/lazer_end"),
                Game.Content.Load<Texture2D>("ray/forward"),
                Game.Content.Load<Texture2D>("ray/rotate")
            };

            tSelected = Game.Content.Load<Texture2D>("cell");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ui.Update(gameTime);
            Input();
        }

        void Input()
        {
            KeyboardState state = Keyboard.GetState();

            if (oldKS != null && oldKS.IsKeyDown(Keys.Escape))
                if (state.IsKeyUp(Keys.Escape))
                {
                    Game.Components.Add(new MenuClass(Game));
                    Game.Components.Remove(this);
                }
            oldKS = state;

            MouseState ms = Mouse.GetState();
            if ((ms.LeftButton == ButtonState.Released) && (oldMS.LeftButton == ButtonState.Pressed))
            {
                if (!isGameOver)
                {
                    if (field.IsFigureSelected())
                        ui.Input(PointConversion.toVector2(field.SelectedFigurePosition()), ms.Position.ToVector2());
                    else
                        ui.Input(null, ms.Position.ToVector2());
                    switch (ui.UIAction)
                    {
                        case UI.Action.Turn:
                            if (field.IsFigureSelected() && field.StepType != Field.FigureStepType.None)
                            {
                                isGameOver = field.Turn(out winnerId);
                                winnerId = (winnerId + 1) % 2;
                                ui.clearDirections();
                            }
                            break;
                        case UI.Action.SelectFigure:
                            field.SetSelectedFigure(PointConversion.toPoint(ms.Position.ToVector2() / CellSize));
                            if (field.IsFigureSelected())
                            {
                                var dirs = field.IsDirectionsAvailableForSelectedFigure();
                                ui.SetControlMovePos(PointConversion.toVector2(field.SelectedFigurePosition()), dirs);
                            }
                            break;
                        case UI.Action.Rotate:
                            field.StepType = Field.FigureStepType.Rotate;
                            field.IsClockwiseRotation = ui.IsClockwise;
                            break;
                        case UI.Action.Move:
                            field.StepType = Field.FigureStepType.Move;
                            field.SetDirection(ui.direction);
                            break;
                    }
                }
            }

            oldMS = ms;
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            ui.Draw(sb, field.CurrentPlayer, field.selectedText);

            //figures
            foreach (var fg in Field.figures)
            {
                Texture2D t = null;
                switch (fg.FigureType)
                {
                    case Figure.Type.sphinx: t = fg.PlayerID == 0 ? tFigs[0] : tFigs[5]; break;
                    case Figure.Type.pharaoh: t = fg.PlayerID == 0 ? tFigs[1] : tFigs[6]; break;
                    case Figure.Type.pyramid: t = fg.PlayerID == 0 ? tFigs[2] : tFigs[7]; break;
                    case Figure.Type.scarab: t = fg.PlayerID == 0 ? tFigs[3] : tFigs[8]; break;
                    case Figure.Type.anubis: t = fg.PlayerID == 0 ? tFigs[4] : tFigs[9]; break;

                }

                float r = 0;
                switch (fg.MoveDirection)
                {
                    case CellObject.Direction.Up: r = 0; break;
                    case CellObject.Direction.Right: r = (float)(Math.PI / 2.0); break;
                    case CellObject.Direction.Down: r = (float)Math.PI; break;
                    case CellObject.Direction.Left: r = -(float)(Math.PI / 2.0); break;
                }
                sb.Draw(
                    t
                    , origin: new Vector2((float)t.Width / 2, (float)t.Width / 2)
                    , color: Color.White
                    , rotation: r
                    , destinationRectangle: new Rectangle(fg.Position.X * CellSize + CellSize / 2, fg.Position.Y * CellSize + CellSize / 2, CellSize, CellSize)
                    );
            }


            //ray
            if (field.RayLight != null)
                foreach (var el in field.RayLight.Lights)
                {
                    Texture2D t = tRay[0];
                    if (el.state == RayLight.State.Forward)
                        t = tRay[1];
                    else if (el.state == RayLight.State.Rotate)
                        t = tRay[2];

                    float rotation = 0f;

                    if (t != tRay[2])
                        switch (el.MoveDirection)
                        {
                            case CellObject.Direction.Down:
                                rotation = (float)Math.PI;
                                break;
                            case CellObject.Direction.Left:
                                rotation = (float)-Math.PI / 2;
                                break;
                            case CellObject.Direction.Right:
                                rotation = (float)Math.PI / 2;
                                break;
                            case CellObject.Direction.Up:
                                rotation = 0f;
                                break;
                        }
                    else
                    {
                        if (el.MoveDirection.HasFlag(CellObject.Direction.Up) && !el.clockwise ||
                            el.MoveDirection.HasFlag(CellObject.Direction.Left) && el.clockwise)
                            rotation = 0f;
                        else
                            if (el.MoveDirection.HasFlag(CellObject.Direction.Left) && !el.clockwise ||
                            el.MoveDirection.HasFlag(CellObject.Direction.Down) && el.clockwise)
                                rotation = (float)-Math.PI / 2;
                            else
                                if (el.MoveDirection.HasFlag(CellObject.Direction.Right) && !el.clockwise ||
                                el.MoveDirection.HasFlag(CellObject.Direction.Up) && el.clockwise)
                                    rotation = (float)Math.PI / 2;
                                else
                                    if (el.MoveDirection.HasFlag(CellObject.Direction.Down) && !el.clockwise ||
                                    el.MoveDirection.HasFlag(CellObject.Direction.Right) && el.clockwise)
                                        rotation = (float)Math.PI;
                    }

                    sb.Draw(texture: t
                            , destinationRectangle: new Rectangle(el.Position.X * CellSize + CellSize / 2, el.Position.Y * CellSize + CellSize / 2, CellSize, CellSize)
                            , color: Color.White
                            , rotation: rotation
                            , origin: new Vector2(CellSize / 2, CellSize / 2));
                }
            //selected figure
            if (field.IsFigureSelected())
                sb.Draw(
                    texture: tSelected
                    , destinationRectangle: new Rectangle(field.SelectedFigurePosition().X * CellSize, field.SelectedFigurePosition().Y * CellSize, CellSize, CellSize)
                    , color: Color.Yellow);

            if (isGameOver)
                ui.DrawGameOver(sb, winnerId);

            sb.End();
            base.Draw(gameTime);
        }

    }
}

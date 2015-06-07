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
    class GameClass:DrawableGameComponent
    {
        UI ui;
        SpriteBatch sb;
        Field field;
        Texture2D[] tFigs;
        Texture2D[] tRay;
        Texture2D tSelected;
        public static MouseState oldMS;
       

        public static int CellSize = 100; //минимум между 1/14 ширины и 1/8 высоты

        public GameClass(Game game):base(game)
        {
            ui = new UI();
            field = new Field();

            oldMS = Mouse.GetState();
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
                Game.Content.Load<Texture2D>("rayOver"),
                Game.Content.Load<Texture2D>("ray"),
                Game.Content.Load<Texture2D>("rayTurn")
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
            MouseState ms = Mouse.GetState();
            if ((ms.LeftButton == ButtonState.Released) && (oldMS.LeftButton == ButtonState.Pressed))
            {
                if (field.IsFigureSelected())
                    ui.Input(field.SelectedFigurePosition(), ms.Position.ToVector2());
                else
                    ui.Input(null, ms.Position.ToVector2());
                switch (ui.UIAction)
                {
                    case UI.Action.Turn:
                        if (field.IsFigureSelected())
                        {
                            field.Turn();
                            ui.SetControlMovePos(new Vector2(-CellSize * 4, 0));
                        }
                        break;
                    case UI.Action.SelectFigure:
                        var pos = (ms.Position.ToVector2()/CellSize).ToPoint();                                      
                        field.SetSelectedFigure(pos);
                        if (field.IsFigureSelected())
                        {
                            var dirs = field.IsDirectionsAvailableForSelectedFigure();
                            ui.SetControlMovePos(pos.ToVector2() * CellSize, dirs);
                            
                        }
                        break;
                    case UI.Action.Rotate:
                        field.StepType=Field.FigureStepType.Rotate;
                        field.IsClockwiseRotation  = ui.IsClockwise;
                        break;
                    case UI.Action.Move: 
                        field.StepType=Field.FigureStepType.Move;
                        field.SetDirection(ui.direction);
                        break;
                    
                }
            }

            oldMS = ms;
        }
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            ui.Draw(sb);

            //figures
            foreach (var fg in Field.figures)
            {
                Texture2D t=null;
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
                    case CellObject.Direction.Up:       r = 0; break;
                    case CellObject.Direction.Right:    r = (float) (Math.PI/2.0); break;
                    case CellObject.Direction.Down:     r = (float) Math.PI; break;
                    case CellObject.Direction.Left:     r = -(float) (Math.PI/2.0); break;
                }
                sb.Draw(
                    t
                    ,position:fg.Position.ToVector2() * CellSize+new Vector2(50,50)
                    ,origin:new Vector2(50,50)
                    ,color:Color.White
                    ,rotation:r
                    );                
            }


            //ray
            if (field.RayLight != null)
                foreach (var el in field.RayLight.Lights)
                {
                    if (field.RayLight.Lights.IndexOf(el) == 0)
                    {
                        sb.Draw(tRay[0], el.Position.ToVector2() * CellSize, Color.White);
                    }
                    else
                    {
                        if ((el.MoveDirection.HasFlag(Figure.Direction.Right)) ||
                            (el.MoveDirection.HasFlag(Figure.Direction.Left)))
                            sb.Draw(texture: tRay[1],
                                    position: el.Position.ToVector2() * CellSize,
                                    color: Color.White,
                                    rotation: (float)(Math.PI / 2.0f),
                                    origin: new Vector2(CellSize / 2, CellSize / 2));
                        else
                            sb.Draw(texture: tRay[1],
                                    position: el.Position.ToVector2() * CellSize,
                                    color: Color.White);
                    }
                }
            //selected figure
            if (field.IsFigureSelected())
                sb.Draw(tSelected,field.SelectedFigurePosition()* CellSize, Color.Yellow);


            sb.End();
            base.Draw(gameTime);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Byters.GameComponents;

namespace EgyptLazerGame.Classes.XNA
{
    class MenuClass : DrawableGameComponent
    {
        SpriteBatch sb;
        MouseState oldMS;
        Texture2D tBg;
        Rectangle rec;
        ParticleController dustController;
        double lastEngineTime;
        double delayEngine = 750;

        public MenuClass(Game game)
            : base(game)
        { }


        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            tBg = Game.Content.Load<Texture2D>("main_menu");

            dustController = new ParticleController();
            dustController.LoadContent(Game.Content.Load<Texture2D>("dust"));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (tBg == null || tBg.Width != GameClass.CellSize)
                rec = new Rectangle(0, 0, GameClass.CellSize * 14, GameClass.CellSize * 8);

            if (lastEngineTime < gameTime.TotalGameTime.TotalMilliseconds)
            {
                dustController.EngineRocket(
                    new Vector2(7 * GameClass.CellSize, 5 * GameClass.CellSize),
                    new Vector2(4 * GameClass.CellSize, 2 * GameClass.CellSize), 0, -2, 40, 0.05f, velocityRandom: new Vector2(0, 2));

                lastEngineTime = gameTime.TotalGameTime.TotalMilliseconds + delayEngine;
            }

            dustController.Update(gameTime);

            Input();
        }

        void Input()
        {
            MouseState ms = Mouse.GetState();

            if (oldMS.LeftButton.Equals(ButtonState.Pressed) && ms.LeftButton.Equals(ButtonState.Released))
            {
                Game.Components.Add(new GameClass(Game));
                Game.Components.Remove(this);
            }

            oldMS = ms;
        }
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(tBg, rec, Color.White);
            dustController.Draw(sb);

            sb.End();
            base.Draw(gameTime);
        }

    }
}

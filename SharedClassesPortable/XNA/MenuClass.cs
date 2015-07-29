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
    class MenuClass : DrawableGameComponent
    {
        SpriteBatch sb;
        MouseState oldMS;

        public MenuClass(Game game)
            : base(game)
        { }

        protected override void LoadContent()
        {
            
            sb = new SpriteBatch(GraphicsDevice);
            /*
            tSelected = Game.Content.Load<Texture2D>("cell");
            */
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Input();
        }

        void Input()
        {
            MouseState ms = Mouse.GetState();

            if (oldMS.LeftButton.Equals(ButtonState.Pressed) && ms.LeftButton.Equals(ButtonState.Released)) {
                Game.Components.Add(new GameClass(Game));
                Game.Components.Remove(this);                
            }

            oldMS = ms;
        }
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            sb.End();
            base.Draw(gameTime);
        }

    }
}

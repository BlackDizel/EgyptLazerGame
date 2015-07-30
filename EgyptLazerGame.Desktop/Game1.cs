using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using EgyptLazerGame.Classes;
using EgyptLazerGame.Classes.XNA;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
namespace EgyptLazerGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : EgyptLazerGame.Classes.Game1
    {
        public Game1()
            : base()
        {
            Window.AllowUserResizing = true;
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GameClass.CellSize = Math.Min(Window.ClientBounds.Width / 14, Window.ClientBounds.Height / 8);
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.ApplyChanges();

        }
    }
}

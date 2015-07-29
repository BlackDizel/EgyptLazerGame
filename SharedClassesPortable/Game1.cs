using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using EgyptLazerGame.Classes;
using EgyptLazerGame.Classes.XNA;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
namespace EgyptLazerGame.Classes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            Components.Add(new MenuClass(this));


            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
//            GameClass.CellSize = Math.Min(GraphicsDevice.PresentationParameters.BackBufferWidth / 14, GraphicsDevice.PresentationParameters.BackBufferHeight / 8);



        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}

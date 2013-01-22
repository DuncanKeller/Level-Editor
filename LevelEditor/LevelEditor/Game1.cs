using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            CreateFolders();
            TextureManager.Init(Content);
            LineBatch.Init(graphics.GraphicsDevice);
            MenuSystem.Init(graphics.GraphicsDevice);

            
            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            MenuSystem.textureBank.SaveTextures();
            base.OnExiting(sender, args);
        }

        public void CreateFolders()
        {
            if (!Directory.Exists("blueprints"))
            {
                Directory.CreateDirectory("blueprints");
            }
            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }
            if (!Directory.Exists("scripts"))
            {
                Directory.CreateDirectory("scripts");
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Load();

            Editor.Init();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            Editor.Update();
            MenuSystem.Update(gameTime.ElapsedGameTime.Seconds);

            Input.LateUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Editor.Draw(spriteBatch);
            MenuSystem.Draw(spriteBatch);
            spriteBatch.Draw(TextureManager.TexMap["blank"], new Rectangle(Input.X - 1, Input.Y - 1, 3, 3), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

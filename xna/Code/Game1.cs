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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Board_Game.Logic;
using Board_Game.UI;
using Board_Game.Input;

namespace Board_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AI mAI;
        Screen mScreen;
        GameState mGameState;

        Texture2D mBackground;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mAI = new AI();
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
            InputManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D mBomberTexture = Content.Load<Texture2D>("textures/units/bomber");
            Texture2D mFighterTexture = Content.Load<Texture2D>("textures/units/fighter");
            Texture2D mSoldierTexture = Content.Load<Texture2D>("textures/units/soldier");
            Texture2D mDeminerTexture = Content.Load<Texture2D>("textures/units/deminer");
            Texture2D mGrenadierTexture = Content.Load<Texture2D>("textures/units/grenadier");
            mBackground = Content.Load<Texture2D>("textures/backgrounds/battlefield");

            FontManager.Initialize(Content);

            mGameState = new GameState(
                mAI,
                Content.Load<Texture2D>("textures/tiles/single"),
                Content.Load<Texture2D>("textures/tiles/mine"),
                Content.Load<Texture2D>("textures/UI/selector"));

            mGameState.Initialize(
                mBomberTexture,
                mFighterTexture,
                mSoldierTexture,
                mDeminerTexture,
                mGrenadierTexture
            );

            // TODO: use this.Content to load your game content here
            mAI.Initialize(mGameState);

            mScreen = new Screen(GraphicsDevice,
                mBomberTexture,
                mFighterTexture,
                mSoldierTexture,
                mDeminerTexture,
                mGrenadierTexture
            );

            mAI.mScreen = mScreen;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputManager.Get().Update();
            mGameState.Update(gameTime);
            mScreen.Update(mGameState);

            // TODO: Add your update logic here

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

            spriteBatch.Draw(mBackground, new Rectangle(0, 0, 800, 600), Color.White);
            mGameState.Render(spriteBatch, mGameState.mGrid.position);
            mScreen.Render(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

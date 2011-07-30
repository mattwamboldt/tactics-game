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
using Board_Game.Rendering;
using BoardGameContent.DB;
using Board_Game.DB;
using GameEditor;
using Board_Game.Code.Util;
#if EDITOR
using Board_Game.Code.Editing;
#endif

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
        Cursor mCursor;
        StorageManager mStorage;

#if EDITOR
        Editor mEditorForm;
        UnitEditor mUnitEditor;
#endif

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));
            mStorage = new StorageManager();

            mAI = new AI();
#if EDITOR
            mEditorForm = new Editor();
            mEditorForm.Show();
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            DatabaseManager.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            InputManager.Initialize(Content.Load<InputMap>("xml/InputMap"));

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Initialize(Content, GraphicsDevice);
            FontManager.Initialize(Content);
            
            mBackground = Content.Load<Texture2D>("textures/backgrounds/battlefield");
            mCursor = new Cursor(TextureManager.Get().Find("textures/UI/cursor"));
            
            DatabaseManager.Get().Load(Content);

            mGameState = new GameState(
                mAI,
                Content.Load<Sprite>("xml/Selector"),
                Content.Load<GameGrid>("map/test")
            );
            mGameState.Initialize(Content);
            mAI.Initialize(mGameState);
            mScreen = new Screen(GraphicsDevice, Content);
            mAI.mScreen = mScreen;

#if EDITOR
            mEditorForm.DisplayStartData();
            mEditorForm.PopulateTree(mScreen.Root);

            mUnitEditor = new UnitEditor(mEditorForm, mGameState, Content, mStorage);
            mUnitEditor.SetCallbacks();

            mGameState.Selector.SetEditorHandle(mUnitEditor);
#endif
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
            InputManager.Get().Update();
            mCursor.Update();
            
            // Allows the game to exit
            if (InputManager.Get().isTriggered(Button.Home))
            {
                this.Exit();
            }

            if (InputManager.Get().isTriggered(Button.Triangle))
            {
                graphics.ToggleFullScreen();
            }

#if EDITOR
            if (Keyboard.GetState().GetPressedKeys().Contains(Keys.Delete))
            {
                mUnitEditor.DeleteCreature();
            }
#endif

            mStorage.Update(gameTime);

            mGameState.Selector.HandleInput(mCursor);
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

            spriteBatch.Draw(mBackground, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            mGameState.Render(spriteBatch, mGameState.mGrid.Position);
            mScreen.Render(spriteBatch);
            mCursor.Render(spriteBatch);
#if EDITOR
            mUnitEditor.Render(spriteBatch, mGameState.mGrid.Position);
#endif
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

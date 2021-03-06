﻿using AsteroidAssault.Audio;
using AsteroidAssault.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Screen;
using System.Collections.Generic;

namespace AsteroidAssault
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private GameScreenState<GameState> _gameScreenState;
        private GameScreen _currentScreen => _gameScreenState.CurrentScreen;

        private TextureManager _textureManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            RegisterGameScreens();
        }

        private void RegisterGameScreens()
        {
            TitleScreen ts = new TitleScreen(this);
            ts.StartGame += (s, e) => _gameScreenState.SetState(GameState.Playing);

            PlayingScreen ps = new PlayingScreen(this);
            ps.GameOverWaitFinished += (s, e) => _gameScreenState.SetState(GameState.TitleScreen);

            GameScreenMap<GameState> map = new GameScreenMap<GameState>()
                .AddState(GameState.TitleScreen, ts)
                .AddState(GameState.Playing, ps);

            _gameScreenState = new GameScreenState<GameState>(map, GameState.TitleScreen);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Add your initialization logic here

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

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 512;

            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.IsFullScreen = true;

            graphics.ApplyChanges();


            SoundManager.Initialise(Content);

            _textureManager = new TextureManager(Content);
            foreach (var gs in _gameScreenState.AllScreens())
                gs.LoadContent(_textureManager);

            // use this.Content to load your game content here
            //_spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Add your update logic here

            _currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Add your drawing code here

            spriteBatch.Begin();
            {
                _currentScreen.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

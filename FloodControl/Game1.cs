using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using FloodControl.Models;
using FloodControl.Utils;
using FloodControl.Models.Pieces;
using FloodControl.Screens;

namespace FloodControl
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TextureManager _textureManager;

        GameState _gameState;
        private Dictionary<GameState, IGameScreen> _gameScreens = new Dictionary<GameState, IGameScreen>();
        private IGameScreen _currentScreen => _gameScreens[_gameState];


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            RegisterGameScreens();
        }

        private void RegisterGameScreens()
        {
            var _titleScreen = new TitleScreen(this);
            var _playingScreen = new PlayingScreen(this);
            var _gameOverScreen = new GameOverScreen(this);

            _titleScreen.StartNewGame += StartNewGame;
            _playingScreen.GameOver += GameOver;
            _gameOverScreen.GameOverDelayComplete += GameOverDelayComplete;

            _gameScreens[GameState.TitleScreen] = _titleScreen;
            _gameScreens[GameState.PlayingScreen] = _playingScreen;
            _gameScreens[GameState.GameOver] = _gameOverScreen;
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

            base.Initialize();

            this.IsMouseVisible = true;

            // Set screen resolution
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            _gameState = GameState.TitleScreen;
        }

        private void GameOverDelayComplete(object sender, EventArgs e)
        {
            _gameState = GameState.TitleScreen;
        }

        private void GameOver(object sender, EventArgs e)
        {
            ((GameOverScreen)_gameScreens[GameState.GameOver]).ResetTimer();
            _gameState = GameState.GameOver;
        }

        private void StartNewGame(object sender, EventArgs e)
        {
            ((PlayingScreen)_gameScreens[GameState.PlayingScreen]).StartNewGame();
            _gameState = GameState.PlayingScreen;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            _textureManager = new TextureManager(this.Content);

            foreach (var gs in _gameScreens.Values)
                gs.LoadContent(_textureManager);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            _currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            {
                _currentScreen.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}

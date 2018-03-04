using AsteroidAssault.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework;
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


        private GameState _gameState = GameState.Playing;
        private Dictionary<GameState, IGameScreen> _gameScreens = new Dictionary<GameState, IGameScreen>();
        IGameScreen _currentScreen => _gameScreens[_gameState];

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
            PlayingScreen ps = new PlayingScreen(this);

            _gameScreens[GameState.TitleScreen] = ts;
            _gameScreens[GameState.Playing] = ps;
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
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            _textureManager = new TextureManager(Content);
            foreach (var gs in _gameScreens.Values)
                gs.LoadContent(_textureManager);

            // TODO: use this.Content to load your game content here
            //_spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
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
            GraphicsDevice.Clear(Color.Black);

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

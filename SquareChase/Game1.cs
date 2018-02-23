using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SquareChase
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        

        private int _playerScore = 0; // game world state variables
        private float _remainingTime = 0;
        private Square _square;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; // refactor to global constants
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

            // do things like set screen resolution, toggle full screen mode, etc.

            base.Initialize();

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _square = new Square(Content.Load<Texture2D>(@"Square"));
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


            if (_remainingTime <= 0)
            {
                _square.WarpSquare(this.Window);
                _remainingTime = Square.TimePerSquare;
            }

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && _square.Bounds.Contains(mouse.X, mouse.Y))
            {
                _playerScore++;
                _remainingTime = 0;
                _square.OnClick();
            }

            float timeSinceLastCallToUpdateMethod = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _remainingTime = Math.Max(0, _remainingTime - timeSinceLastCallToUpdateMethod);

            this.Window.Title = string.Format("Score: {0}", _playerScore);

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
            _spriteBatch.Begin();
            {
                _spriteBatch.Draw(_square.Texture, _square.Bounds, _square.ColourToShow);
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}

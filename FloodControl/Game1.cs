using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FloodControl
{
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D _playingPieces;
        private Texture2D _backgroundScreen;
        private Texture2D _titleScreen;

        private GameBoard _gameBoard;
        private Vector2 _gameBoardOrigin = new Vector2(70, 89); // What???

        private int _playerScore = 0;

        private enum GameState {  TitleScreen, PlayingScreen };
        private GameState _gameState = GameState.TitleScreen;

        private Rectangle _emptyPieceTileSheetReferenceSource = new Rectangle(1, 247, 40, 40); // points to empty piece texture in tile sheet for ease of reference

        private const float MinTimeSinceLastInput = 0.25f;
        private float _timeSinceLastInput = 0;

        private SpriteFont _pericles36Font;
        private Vector2 _scorePosition = new Vector2(605, 215);
        private Queue<ScoreZoom> _scoreZooms = new Queue<ScoreZoom>();

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

            base.Initialize();

            this.IsMouseVisible = true;

            // Set screen resolution
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            _gameBoard = new GameBoard();

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

            _playingPieces    = Content.Load<Texture2D>(@"Textures/Tile_Sheet");
            _backgroundScreen = Content.Load<Texture2D>(@"Textures/Background");
            _titleScreen      = Content.Load<Texture2D>(@"Textures/TitleScreen");

            _pericles36Font = Content.Load<SpriteFont>(@"Fonts/Pericles36"); // Download font from here before compiling: http://xbox.create.msdn.com/en-US/education/catalog/utility/font_pack
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

            switch(_gameState)
            {
                case GameState.TitleScreen:
                    {
                        if(Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            _gameBoard.GenerateNewGameBoard(false);
                            _playerScore = 0;
                            _gameState = GameState.PlayingScreen;
                        }
                        break;
                    }
                case GameState.PlayingScreen:
                    {
                        _timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (_gameBoard.PiecesAreAnimating)
                        {
                            _gameBoard.UpdateAnimatedPieces();
                        }
                        else
                        {

                            // Calculate score
                            _gameBoard.ResetWater();
                            for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                            {
                                CheckScoringChain(_gameBoard.GetWaterChain(y));
                            }

                            _gameBoard.GeneratePieces(true);

                            if (_timeSinceLastInput >= MinTimeSinceLastInput)
                                HandleMouseInput(Mouse.GetState());
                        }

                        UpdateScoreZooms();

                        break;
                    }
            }



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

            Rectangle screenBounds = new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            spriteBatch.Begin();
            {
                switch (_gameState)
                {
                    case GameState.TitleScreen:
                        {
                            spriteBatch.Draw(_titleScreen, screenBounds, Color.White);
                            break;
                        }
                    case GameState.PlayingScreen:
                        {
                            spriteBatch.Draw(_backgroundScreen, screenBounds, Color.White);

                            for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                                for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                                {
                                    int pixelX = (int)_gameBoardOrigin.X + (x * GamePiece.PieceWidth);
                                    int pixelY = (int)_gameBoardOrigin.Y + (y * GamePiece.PieceHeight);

                                    DrawEmptyPiece(pixelX, pixelY);

                                    bool pieceDrawn = false;
                                    string positionName = string.Format("{0}_{1}", x, y);

                                    if(_gameBoard.HasRotatingPiece(positionName))
                                    {
                                        pieceDrawn = true;
                                        DrawRotatingPiece(pixelX, pixelY, positionName);
                                    }
                                    if (_gameBoard.HasFallingPiece(positionName))
                                    {
                                        pieceDrawn = true;
                                        DrawFallingPiece(pixelX, pixelY, positionName);
                                    }
                                    if (_gameBoard.HasFadingPiece(positionName))
                                    {
                                        pieceDrawn = true;
                                        DrawFadingPiece(pixelX, pixelY, positionName);
                                    }

                                    if (!pieceDrawn)
                                        DrawStandardPiece(x, y, pixelX, pixelY);


                                }

                            //this.Window.Title = string.Format("Score: {0}", _playerScore);

                            foreach (ScoreZoom zoom in _scoreZooms)
                            {
                                Vector2 windowCenter = new Vector2(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2);
                                Vector2 stringLength = _pericles36Font.MeasureString(zoom.Text);
                                Vector2 textCenter = new Vector2(stringLength.X / 2, stringLength.Y / 2);

                                spriteBatch.DrawString(_pericles36Font, zoom.Text, windowCenter, zoom.DrawColor, 0.0f, textCenter, zoom.Scale, SpriteEffects.None, 0);
                            }

                            spriteBatch.DrawString(_pericles36Font, _playerScore.ToString(), _scorePosition, Color.Black);
                            break;
                        }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawEmptyPiece(int pixelX, int pixelY)
        {
            // Render background empty piece
            Rectangle rect = GamePiece.CalculateScreenRenderDestinationRectangle(pixelX, pixelY);
            spriteBatch.Draw(_playingPieces, rect, _emptyPieceTileSheetReferenceSource, Color.White);
        }

        private void DrawStandardPiece(int boardX, int boardY, int pixelX, int pixelY)
        {
            // Render game piece on top of background
            Rectangle rect = GamePiece.CalculateScreenRenderDestinationRectangle(pixelX, pixelY);
            spriteBatch.Draw(_playingPieces, rect, _gameBoard.GetPieceRectangle(boardX, boardY), Color.White);
        }

        private void DrawRotatingPiece(int pixelX, int pixelY, string pieceName)
        {
            RotatingPiece p = _gameBoard.GetRotatingPiece(pieceName);

            Rectangle rect = new Rectangle(
                pixelX + (GamePiece.PieceWidth / 2),
                pixelY + (GamePiece.PieceHeight / 2),
                GamePiece.PieceWidth, GamePiece.PieceHeight
            );

            Vector2 tileCenter = new Vector2(GamePiece.PieceWidth / 2, GamePiece.PieceHeight / 2);

            spriteBatch.Draw(_playingPieces, rect, p.GetSoruceRect(), Color.White, p.RotationAmount, tileCenter, SpriteEffects.None, 0);
        }

        private void DrawFallingPiece(int pixelX, int pixelY, string pieceName)
        {
            FallingPiece p = _gameBoard.GetFallingPiece(pieceName);
            Rectangle rect = new Rectangle(pixelX, pixelY - p.VerticalOffset, GamePiece.PieceWidth, GamePiece.PieceHeight);
            spriteBatch.Draw(_playingPieces, rect, p.GetSoruceRect(), Color.White);
        }

        private void DrawFadingPiece(int pixelX, int pixelY, string pieceName)
        {
            FadingPiece p = _gameBoard.GetFadingPiece(pieceName);
            Rectangle rect = GamePiece.CalculateScreenRenderDestinationRectangle(pixelX, pixelY);
            spriteBatch.Draw(_playingPieces, rect, p.GetSoruceRect(), Color.White * p.AlphaLevel);
        }


        private int DetermineScore(int squareCount)
        {
            double res = (Math.Pow(squareCount / 5, 2) + squareCount) * 10;
            return (int)res;
        }

        private void CheckScoringChain(List<Vector2> waterChain)
        {
            if (waterChain.Count > 0)
            {
                Vector2 lastPipe = waterChain[waterChain.Count - 1];
                if (lastPipe.X == GameBoard.GameBoardWidth - 1) // Must end on RHS of game board
                {
                    if (_gameBoard.PieceHasConector((int)lastPipe.X, (int)lastPipe.Y, "Right")) // Must be connecting to RHS of board
                    {
                        _playerScore = DetermineScore(waterChain.Count);
                        _scoreZooms.Enqueue(new ScoreZoom("+" + _playerScore, new Color(1, 0, 0, 0.4f))); // red
                        // improve: could make score increase each time we "win"

                        // Clear tiles filled with water
                        // will be refilled by calling GenerateNewPieces function
                        foreach (Vector2 tile in waterChain)
                        {
                            _gameBoard.AddFadingPiece((int)tile.X, (int)tile.Y, _gameBoard.GetPieceType((int)tile.X, (int)tile.Y));
                            _gameBoard.SetPieceType((int)tile.X, (int)tile.Y, GamePiece.EmptyPieceType);
                        }
                    }
                }
            }
        }

        private void HandleMouseInput(MouseState mouseState)
        {
            int x = (mouseState.X - (int)_gameBoardOrigin.X) / GamePiece.PieceWidth;
            int y = (mouseState.Y - (int)_gameBoardOrigin.Y) / GamePiece.PieceHeight;

            if(_gameBoard.PieceIsWithinGameBounds(x, y))
            {
                if(mouseState.LeftButton == ButtonState.Pressed)
                {
                    _gameBoard.AddRotatingPiece(x, y, _gameBoard.GetPieceType(x, y), false);
                    _gameBoard.RotatePiece(x, y, false);
                    _timeSinceLastInput = 0;
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    _gameBoard.AddRotatingPiece(x, y, _gameBoard.GetPieceType(x, y), true);
                    _gameBoard.RotatePiece(x, y, true);
                    _timeSinceLastInput = 0;
                }
            }
        }

        private void UpdateScoreZooms()
        {
            int dequeueCounter = 0;
            foreach (ScoreZoom zoom in _scoreZooms)
            {
                zoom.Update();
                if (zoom.IsCompleted)
                    dequeueCounter++;
            }

            for (int d = 0; d < dequeueCounter; d++)
                _scoreZooms.Dequeue();
        }



    }
}

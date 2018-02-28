using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloodControl.Models;
using FloodControl.Models.Pieces;
using FloodControl.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FloodControl.Screens
{
    class PlayingScreen : GameScreen
    {

        private const float MinTimeSinceLastInput = 0.25f;
        private const float MaxFloodCount = 100;
        private const float TimeBetweenFloodIncreases = 1.0f;
        private const float FloodAccellerationPerLevel = 1.0f;
        private const int MaxWaterHeight = 244;
        private const int WaterWidth = 297;

        private Texture2D _playingPieces;
        private Texture2D _backgroundScreen;
        private SpriteFont _pericles36Font;

        // Fixed coordinates on screen where to render specific elements
        private Vector2 _scorePosition = new Vector2(605, 215);
        private Vector2 _gameBoardOrigin = new Vector2(70, 89);
        private Vector2 _levelTextPosition = new Vector2(512, 215);
        private Vector2 _waterOverlayStart = new Vector2(85, 245);
        private Vector2 _waterPosition = new Vector2(478, 338);
        private Rectangle _emptyPieceTileSheetReferenceSource = new Rectangle(1, 247, 40, 40); // points to empty piece texture in tile sheet for ease of reference

        private Queue<ScoreZoom> _scoreZooms = new Queue<ScoreZoom>();

        private GameBoard _gameBoard;

        private float _floodIncreaseAmount = 0.5f;
        private float _currentFloodCount = 0;
        private int _currentLevel = 0;
        private int _linesCompletedThisLevel = 0;
        private int _playerScore = 0;


        private Timer _lastFloodIncreaseTimer = new Timer(TimeSpan.FromSeconds(TimeBetweenFloodIncreases));
        private Timer _inputTimer = new Timer(TimeSpan.FromSeconds(MinTimeSinceLastInput));


        public event EventHandler GameOver;


        public PlayingScreen(Game game) : base(game)
        {
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _playingPieces = textureManager.OptionalLoadContent<Texture2D>(@"Textures/Tile_Sheet");
            _backgroundScreen = textureManager.OptionalLoadContent<Texture2D>(@"Textures/Background");

            _pericles36Font = textureManager.OptionalLoadContent<SpriteFont>(@"Fonts/Pericles36");// Download font from here before compiling: http://xbox.create.msdn.com/en-US/education/catalog/utility/font_pack
        }

        public override void Update(GameTime gameTime)
        {
            float timeSinceLastUpdate = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _lastFloodIncreaseTimer.Update(gameTime);
            _inputTimer.Update(gameTime);

            if (_lastFloodIncreaseTimer.Completed)
            {
                _currentFloodCount += _floodIncreaseAmount;
                _lastFloodIncreaseTimer.Reset();

                if (_currentFloodCount > MaxFloodCount)
                    GameOver?.Invoke(this, null);
            }


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

                if (_inputTimer.Completed)
                    HandleMouseInput(Mouse.GetState());
            }

            UpdateScoreZooms();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_backgroundScreen, ScreenBounds, Color.White);

            for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                {
                    int pixelX = (int)_gameBoardOrigin.X + (x * GamePiece.PieceWidth);
                    int pixelY = (int)_gameBoardOrigin.Y + (y * GamePiece.PieceHeight);

                    DrawEmptyPiece(pixelX, pixelY, spriteBatch);

                    bool pieceDrawn = false;
                    string positionName = string.Format("{0}_{1}", x, y);

                    if (_gameBoard.HasRotatingPiece(positionName))
                    {
                        pieceDrawn = true;
                        DrawRotatingPiece(pixelX, pixelY, positionName, spriteBatch);
                    }
                    if (_gameBoard.HasFallingPiece(positionName))
                    {
                        pieceDrawn = true;
                        DrawFallingPiece(pixelX, pixelY, positionName, spriteBatch);
                    }
                    if (_gameBoard.HasFadingPiece(positionName))
                    {
                        pieceDrawn = true;
                        DrawFadingPiece(pixelX, pixelY, positionName, spriteBatch);
                    }

                    if (!pieceDrawn)
                        DrawStandardPiece(x, y, pixelX, pixelY, spriteBatch);


                }

            //this.Window.Title = string.Format("Score: {0}", _playerScore);

            foreach (ScoreZoom zoom in _scoreZooms)
            {
                Vector2 windowCenter = new Vector2(ClientBounds.Width / 2, ClientBounds.Height / 2);
                Vector2 stringLength = _pericles36Font.MeasureString(zoom.Text);
                Vector2 textCenter = new Vector2(stringLength.X / 2, stringLength.Y / 2);

                spriteBatch.DrawString(_pericles36Font, zoom.Text, windowCenter, zoom.DrawColor, 0.0f, textCenter, zoom.Scale, SpriteEffects.None, 0);
            }

            spriteBatch.DrawString(_pericles36Font, _playerScore.ToString(), _scorePosition, Color.Black);

            spriteBatch.DrawString(_pericles36Font, _currentLevel.ToString(), _levelTextPosition, Color.Black);

            int waterHeight = (int)(MaxWaterHeight * (_currentFloodCount / 100));
            Rectangle waterScreenRect = new Rectangle((int)_waterPosition.X, (int)_waterPosition.Y + (MaxWaterHeight - waterHeight), WaterWidth, waterHeight);
            Rectangle waterSourceRect = new Rectangle((int)_waterOverlayStart.X, (int)_waterOverlayStart.Y + (MaxWaterHeight - waterHeight), WaterWidth, waterHeight);
            spriteBatch.Draw(_backgroundScreen, waterScreenRect, waterSourceRect, new Color(255, 255, 255, 180)); // half white
        }


        private void DrawEmptyPiece(int pixelX, int pixelY, SpriteBatch spriteBatch)
        {
            // Render background empty piece
            Rectangle rect = GamePiece.CalculateScreenRenderDestinationRectangle(pixelX, pixelY);
            spriteBatch.Draw(_playingPieces, rect, _emptyPieceTileSheetReferenceSource, Color.White);
        }

        private void DrawStandardPiece(int boardX, int boardY, int pixelX, int pixelY, SpriteBatch spriteBatch)
        {
            // Render game piece on top of background
            Rectangle rect = GamePiece.CalculateScreenRenderDestinationRectangle(pixelX, pixelY);
            spriteBatch.Draw(_playingPieces, rect, _gameBoard.GetPieceRectangle(boardX, boardY), Color.White);
        }

        private void DrawRotatingPiece(int pixelX, int pixelY, string pieceName, SpriteBatch spriteBatch)
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

        private void DrawFallingPiece(int pixelX, int pixelY, string pieceName, SpriteBatch spriteBatch)
        {
            FallingPiece p = _gameBoard.GetFallingPiece(pieceName);
            Rectangle rect = new Rectangle(pixelX, pixelY - p.VerticalOffset, GamePiece.PieceWidth, GamePiece.PieceHeight);
            spriteBatch.Draw(_playingPieces, rect, p.GetSoruceRect(), Color.White);
        }

        private void DrawFadingPiece(int pixelX, int pixelY, string pieceName, SpriteBatch spriteBatch)
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
                        int score = DetermineScore(waterChain.Count);
                        _playerScore += score;
                        _scoreZooms.Enqueue(new ScoreZoom("+" + score, new Color(1, 0, 0, 0.4f))); // red
                        _currentFloodCount = MathHelper.Clamp(_currentFloodCount - (score / 10), 0, 100);
                        _linesCompletedThisLevel++;


                        // Clear tiles filled with water
                        // will be refilled by calling GenerateNewPieces function
                        foreach (Vector2 tile in waterChain)
                        {
                            _gameBoard.AddFadingPiece((int)tile.X, (int)tile.Y, _gameBoard.GetPieceType((int)tile.X, (int)tile.Y));
                            _gameBoard.SetPieceType((int)tile.X, (int)tile.Y, GamePiece.EmptyPieceType);
                        }

                        if (_linesCompletedThisLevel >= 10)
                            StartNewLevel();
                    }
                }
            }
        }

        private void HandleMouseInput(MouseState mouseState)
        {
            int x = (mouseState.X - (int)_gameBoardOrigin.X) / GamePiece.PieceWidth;
            int y = (mouseState.Y - (int)_gameBoardOrigin.Y) / GamePiece.PieceHeight;

            if (_gameBoard.PieceIsWithinGameBounds(x, y))
            {
                bool isAClickInput = mouseState.RightButton == ButtonState.Pressed || mouseState.LeftButton == ButtonState.Pressed;
                if (isAClickInput)
                {
                    bool rotateClockwise = mouseState.RightButton == ButtonState.Pressed;
                    _gameBoard.AddRotatingPiece(x, y, _gameBoard.GetPieceType(x, y), rotateClockwise);
                    _gameBoard.RotatePiece(x, y, rotateClockwise);
                    _inputTimer.Reset();
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

        private void StartNewLevel()
        {
            _currentLevel++;
            _currentFloodCount = 0;
            _linesCompletedThisLevel = 0;
            _floodIncreaseAmount += FloodAccellerationPerLevel;
            _gameBoard.GenerateNewGameBoard(false);
        }


        public void StartNewGame()
        {
            _playerScore = 0;
            _currentLevel = 0;
            _floodIncreaseAmount = 0;

            _gameBoard = new GameBoard();
            StartNewLevel();
        }



    }
}

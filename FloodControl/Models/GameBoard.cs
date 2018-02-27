using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloodControl.Models.Pieces;
using Microsoft.Xna.Framework;

namespace FloodControl.Models
{
    /// <summary>
    /// Represents the game world on which water filled tiles are placed
    /// </summary>
    class GameBoard
    {
        /*
         * Responsibilities:
         *  - Store GamePiece for each tile on the board
         *  - Provide an API to update individual pieces on the board
         *  - Randomly generate a board on initialisation
         *  - Set and clear "Filled" flags on Game Pieces
         *  - Generate score based on water chains
         */

        // Number of pieces on board = 80
        public const int GameBoardWidth = 8;
        public const int GameBoardHeight = 10;

        private Random _rand = new Random();
        private GamePiece[,] _boardSquares = new GamePiece[GameBoardWidth, GameBoardHeight];
        private List<Vector2> _waterTracker = new List<Vector2>();


        // Use of dictionaries to track animating pieces is a bad idea...
        // Use a list/dictionary of generic IAnimatingPiece and create api to treat all pieces the same to reduce code copy
        private Dictionary<string, RotatingPiece> _rotatingPieces = new Dictionary<string, RotatingPiece>();
        private Dictionary<string, FallingPiece>  _fallingPieces  = new Dictionary<string, FallingPiece>();
        private Dictionary<string, FadingPiece>   _fadingPieces   = new Dictionary<string, FadingPiece>();

        public void AddRotatingPiece(int x, int y, string pieceName, bool clockwise)     { _rotatingPieces[string.Format("{0}_{1}", x, y)] = new RotatingPiece(pieceName, clockwise); }
        public void  AddFallingPiece(int x, int y, string pieceName, int verticleOffset) {  _fallingPieces[string.Format("{0}_{1}", x, y)] = new FallingPiece(pieceName, verticleOffset); }
        public void   AddFadingPiece(int x, int y, string pieceName)                     {   _fadingPieces[string.Format("{0}_{1}", x, y)] = new FadingPiece(pieceName, GamePiece.WaterSuffixString); }

        public bool PiecesAreAnimating => _rotatingPieces.Count > 0 || _fallingPieces.Count > 0 || _fadingPieces.Count > 0;

        public RotatingPiece GetRotatingPiece(string pieceName) => _rotatingPieces[pieceName];
        public  FallingPiece  GetFallingPiece(string pieceName) =>  _fallingPieces[pieceName];
        public   FadingPiece   GetFadingPiece(string pieceName) =>   _fadingPieces[pieceName];

        public bool HasRotatingPiece(string pieceName) => _rotatingPieces.ContainsKey(pieceName);
        public bool  HasFallingPiece(string pieceName) =>  _fallingPieces.ContainsKey(pieceName);
        public bool   HasFadingPiece(string pieceName) =>   _fadingPieces.ContainsKey(pieceName);


        public GameBoard() { ClearBoard(); }

        /// <summary>
        /// Resets the game board and assigns pieces to all tiles on the board to be empty
        /// </summary>
        public void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = 0; y < GameBoardHeight; y++)
                    _boardSquares[x, y] = new GamePiece(GamePiece.EmptyPieceType);
        }

        public void RotatePiece(int x, int y, bool clockWise) { _boardSquares[x, y].Rotate(clockWise); }
        public Rectangle GetPieceRectangle(int x, int y) { return _boardSquares[x, y].GetSoruceRect(); }
        public string GetPieceType(int x, int y) { return _boardSquares[x, y].PieceType; }
        public void SetPieceType(int x, int y, string pieceType) { _boardSquares[x, y].Set(pieceType); }
        public bool PieceHasConector(int x, int y, string direction) { return _boardSquares[x, y].HasConnector(direction); }
        public bool PieceIsEmpty(int x, int y) { return _boardSquares[x, y].IsEmpty; }
        public void FillPieceWithWater(int x, int y) { _boardSquares[x, y].FillWithWater(); }
        public bool PieceIsFilledWithWater(int x, int y) { return _boardSquares[x, y].IsFilledWithWater; }
        private string[] GetPieceOtherEnds(int x, int y, string startingEnd) { return _boardSquares[x, y].GetOtherEnds(startingEnd); }
        public bool PieceIsWithinGameBounds(int x, int y) { return (y >= 0 && y < GameBoardHeight) && (x >= 0 && x < GameBoardWidth); }

        public void RandomisePieceType(int x, int y) {
            int randIndex = _rand.Next(0, GamePiece.MaxPlayablePieceIndex + 1);
            string pieceType = GamePiece.PieceTypes[randIndex];
            _boardSquares[x, y].Set(pieceType);
        }

        /// <summary>
        /// Pulls a aingle non-empty piece downwards into [x,y] from above
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void PullDownNonEmptyPieceFromAbove(int x, int y)
        {
            int rowLookup = y - 1; // Look upwards
            while(rowLookup >= 0)
            {
                if (DidPullDownNonEmptyGamePiece(x, y, rowLookup))
                    break;

                rowLookup--;
            }
        }

        private bool DidPullDownNonEmptyGamePiece(int x, int y, int rowLookup)
        {
            if (!PieceIsEmpty(x, rowLookup))
            {
                PullDownGamePieceIntoCurrent(x, y, rowLookup, GetPieceType(x, rowLookup));
                AddFallingPiece(x, y, GetPieceType(x, y), GamePiece.PieceHeight * (y - rowLookup));
                return true;
            }
            return false;
        }

        private void PullDownGamePieceIntoCurrent(int x, int y, int rowLookup, string lookupPieceType)
        {
            SetPieceType(x, y, lookupPieceType);
            SetPieceType(x, rowLookup, GamePiece.EmptyPieceType);
        }

        /// <summary>
        /// Creates a whole new game board by clearing and regenerating the board
        /// </summary>
        /// <param name="dropSquares"></param>
        public void GenerateNewGameBoard(bool dropSquares)
        {
            ClearBoard();
            GeneratePieces(dropSquares);
        }

        /// <summary>
        /// Fills in empty pieces either by pulling non-empty game pieces downwards
        /// or by randomizing them
        /// </summary>
        /// <param name="dropSquares"></param>
        public void GeneratePieces(bool dropSquares)
        {
            if(dropSquares)
                FillGameBoardByPullingPiecesDownFromTop();

            // Because FillFromAbove doesn't replace the pieces
            // and will leave empty pieces at the top
            FillEmptyGamePiecesWithRandomValues();
        }

        private void FillGameBoardByPullingPiecesDownFromTop()
        {
            // Work our way up from the bottom and pull pieces downards
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = GameBoardHeight-1; y >= 0; y--)
                    if (PieceIsEmpty(x, y))
                        PullDownNonEmptyPieceFromAbove(x, y);
        }

        private void FillEmptyGamePiecesWithRandomValues()
        {
            // Fill remaining (empty) squares with random pieces
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = 0; y < GameBoardHeight; y++)
                    if (PieceIsEmpty(x, y))
                    {
                        RandomisePieceType(x, y);
                        AddFallingPiece(x, y, GetPieceType(x, y), GamePiece.PieceHeight * GameBoardHeight);
                    }
        }

        public void ResetWater()
        {
            for (int y = 0; y < GameBoardHeight; y++)
                for (int x = 0; x < GameBoardWidth; x++)
                    _boardSquares[x, y].EmptyWater();
        }

        public List<Vector2> GetWaterChain(int startingY)
        {
            _waterTracker.Clear();
            PropagateWater(0, startingY, "Left");
            return _waterTracker;
        }

        private void PropagateWater(int x, int y, string fromDirection)
        {
            if (PieceIsWithinGameBounds(x, y))
            {
                if (PieceHasConector(x, y, fromDirection) && !PieceIsFilledWithWater(x, y))
                {
                    FillPieceWithWater(x, y);
                    _waterTracker.Add(new Vector2(x, y));

                    // Should only be one other end
                    foreach (string end in GetPieceOtherEnds(x, y, fromDirection))
                    {
                        switch (end)
                        {
                            case "Left":   PropagateWater(x - 1, y,     "Right");  break; // refactor to DIRECTION contants or enums
                            case "Right":  PropagateWater(x + 1, y,     "Left");   break;
                            case "Top":    PropagateWater(x,     y - 1, "Bottom"); break;
                            case "Bottom": PropagateWater(x,     y + 1, "Top");    break;
                        }
                    }
                }
            }
        }


        public void UpdateAnimatedPieces()
        {
            if (_fadingPieces.Count > 0)
                UpdateFadingPieces(); // only animate fading. When done, allow other animations to run
            else
            {
                UpdateRotatingPieces();
                UpdateFallingPieces();
            }
        }

        public void UpdateRotatingPieces()
        {
            Queue<string> keysToRemove = new Queue<string>();
            foreach (string key in _rotatingPieces.Keys)
            {
                RotatingPiece p = _rotatingPieces[key];
                p.UpdatePiece();

                if (p.RotationTicksRemaining == 0)
                    keysToRemove.Enqueue(key);
            }

            while (keysToRemove.Count > 0)
                _rotatingPieces.Remove(keysToRemove.Dequeue());
        }

        public void UpdateFallingPieces()
        {
            Queue<string> keysToRemove = new Queue<string>();
            foreach (string key in _fallingPieces.Keys)
            {
                FallingPiece p = _fallingPieces[key];
                p.UpdatePiece();

                if (p.VerticalOffset == 0)
                    keysToRemove.Enqueue(key);
            }

            while (keysToRemove.Count > 0)
                _fallingPieces.Remove(keysToRemove.Dequeue());
        }

        public void UpdateFadingPieces()
        {
            Queue<string> keysToRemove = new Queue<string>();
            foreach (string key in _fadingPieces.Keys)
            {
                FadingPiece p = _fadingPieces[key];
                p.UpdatePiece();

                if (p.AlphaLevel == FadingPiece.FullTransparentAlpha)
                    keysToRemove.Enqueue(key);
            }

            while (keysToRemove.Count > 0)
                _fadingPieces.Remove(keysToRemove.Dequeue());
        }



    }
}

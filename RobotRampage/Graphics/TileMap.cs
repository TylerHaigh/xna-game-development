using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Graphics;
using RobotRampage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Graphics
{
    // Bespoke TileMap used solely for RobotRampage project.
    // General ideas should be refactored into the generic Game Engine Library
    // TODO: After implementing TileMap, try and refactror into generic class

    class TileMap
    {
        private Texture2D _texture;
        private List<Rectangle> _tiles = new List<Rectangle>();
        private Tiles[,] _mapSquares = new Tiles[MapWidth, MapHeight];

        private Random _rand = new Random();

        // RobotRampage Specific Config
        
        private const int MapWidth = 50;
        private const int MapHeight = 50;

        private const int FloorTileCount = 4;
        private const int WallTileCount = 4;

        private const int TileWidth = 32;
        private const int TileHeight = 32;

        public void Intialise(Texture2D tileTexture)
        {
            _texture = tileTexture;

            // Build new Grid
            ReloadTiles();
            ResetSquares();
        }

        private void ReloadTiles()
        {
            _tiles.Clear();

            // Load floor tiles
            for (int i = 0; i < FloorTileCount; i++)
            {
                _tiles.Add(new Rectangle(TileWidth * i, 0, TileWidth, TileHeight));
            }

            // Load wall tiles
            for (int i = 0; i < WallTileCount; i++)
            {
                _tiles.Add(new Rectangle(TileWidth * i, TileHeight, TileWidth, TileHeight));
            }
        }

        private void ResetSquares()
        {
            for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                    _mapSquares[x, y] = Tiles.GreyFloor;
        }

        // We will use "square" to refer to a location within the _mapSquares array, while we will use
        // "tile" to refer to the index number stored in a particular square.

        // GetSquareByPixelX() and GetSquareByPixelY() will allow us to
        // convert world-based pixel coordinates to map square references

        public int GetSquareByPixelX(int pixelX) => pixelX / TileWidth;
        public int GetSquareByPixelY(int pixelY) => pixelY / TileHeight;

        public Vector2 GetSquareAtPixel(Vector2 vec)
        {
            return new Vector2(
                GetSquareByPixelX((int)vec.X),
                GetSquareByPixelX((int)vec.Y)
            );
        }

        public Vector2 GetSquareCenter(int squareX, int squareY)
        {
            return new Vector2(
                (squareX * TileWidth) + (TileWidth / 2),
                (squareY * TileHeight) + (TileHeight / 2));
        }

        public Vector2 GetSquareCenter(Vector2 square)
        {
            return GetSquareCenter((int)square.X, (int)square.Y);
        }

        // SquareWorldRectangle() answers the question "What pixels on the
        // world map does this square occupy?"

        public Rectangle SquareWorldRectangle (int x, int y)
        {
            return new Rectangle(
                x * TileWidth, y * TileHeight,
                TileWidth, TileHeight);
        }

        public Rectangle SquareWorldRectangle(Vector2 square)
        {
            return SquareWorldRectangle((int)square.X, (int)square.Y);
        }

        // SquareScreenRectangle() provides the same information, but in
        // localized screen coordinates. This information will be used in the Draw() method when
        // rendering each square's tile to the display

        public Rectangle SquareScreenRectangle(Camera cam, int x, int y)
        {
            return cam.Transform(SquareWorldRectangle(x, y));
        }

        public Rectangle SquareScreenRectangle(Camera cam, Vector2 square)
        {
            return SquareScreenRectangle(cam, (int)square.X, (int)square.Y);
        }

    }
}

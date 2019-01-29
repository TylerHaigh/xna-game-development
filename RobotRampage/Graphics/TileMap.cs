using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Entities;
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

    public class TileMap
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

        public int GetSquareAtPixelX(int pixelX) => pixelX / TileWidth;
        public int GetSquareAtPixelY(int pixelY) => pixelY / TileHeight;

        public Vector2 GetSquareAtPixel(Vector2 vec)
        {
            return new Vector2(
                GetSquareAtPixelX((int)vec.X),
                GetSquareAtPixelX((int)vec.Y)
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


        // Helper methods related to Tiles enum

        private bool CoordinateWithinBounds(int squareX, int squareY)
        {
            return (
                (squareX >= 0 && squareX < MapWidth) &&
                (squareY >= 0 && squareY < MapHeight)
            );
        }

        private void PreconditionCheckSquareIndex(int squareX, int squareY)
        {
            if (!CoordinateWithinBounds(squareX, squareY))
                throw new ArgumentException("squareX and squareY must be within map bounds");
        }

        public Tiles GetTileAtSquare(int squareX, int squareY)
        {
            PreconditionCheckSquareIndex(squareX, squareY);
            return _mapSquares[squareX, squareY];
        }

        public Tiles GetTileAtSquare(Vector2 square)
        {
            return GetTileAtSquare((int)square.X, (int)square.Y);
        }

        public void SetTileAtSquare(int squareX, int squareY, Tiles tile)
        {
            PreconditionCheckSquareIndex(squareX, squareY);
            _mapSquares[squareX, squareY] = tile;
        }

        public void SetTileAtSquare(Vector2 square, Tiles tile)
        {
            SetTileAtSquare((int)square.X, (int)square.Y, tile);
        }

        public Tiles GetTileAtPixel(int pixelX, int pixelY)
        {
            return GetTileAtSquare(
                GetSquareAtPixelX(pixelX),
                GetSquareAtPixelY(pixelY)
            );
        }

        public Tiles GetTileAtPixel(Vector2 pixel)
        {
            return GetTileAtPixel((int)pixel.X, (int)pixel.Y);
        }

        public bool IsWallTile(int squareX, int squareY)
        {
            Tiles t = GetTileAtSquare(squareX, squareY);
            return t > Tiles.GreyWall; // TODO: Woeful. Fix later
        }

        public bool IsWallTile(Vector2 square)
        {
            return IsWallTile((int)square.X, (int)square.Y);
        }

        public bool IsWallTileAtPixel(int pixelX, int pixelY)
        {
            return IsWallTile(
                GetSquareAtPixelX(pixelX),
                GetSquareAtPixelY(pixelY)
            );
        }

        public bool IsWallTileAtPixel(Vector2 pixel)
        {
            return IsWallTileAtPixel((int)pixel.X, (int)pixel.Y);
        }

        public void Draw(Camera cam, SpriteBatch spriteBatch)
        {
            int startX = GetSquareAtPixelX((int)cam.Position.X);
            int endX = GetSquareAtPixelX((int)cam.Position.X + cam.ViewPortWidth);

            int startY = GetSquareAtPixelY((int)cam.Position.Y);
            int endY = GetSquareAtPixelY((int)cam.Position.Y + cam.ViewPortHeight);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endX; y++)
                {
                    if (CoordinateWithinBounds(x, y))
                        spriteBatch.Draw(
                            _texture,
                            SquareScreenRectangle(cam, x, y),
                            _tiles[(int)GetTileAtSquare(x, y)],
                            Color.White
                        );
                }
            }

        }

    }
}

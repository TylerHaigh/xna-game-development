using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private int[,] _tileMatrix = new int[MapWidth, MapHeight];

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
            ResetTileMap();
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

        private void ResetTileMap()
        {
            for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                    _tileMatrix[x, y] = (int)Tiles.GreyFloor;
        }

    }
}

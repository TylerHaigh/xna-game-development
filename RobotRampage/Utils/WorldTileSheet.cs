using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Utils
{
    public class WorldTileSheet
    {

        private Texture2D _texture;
        public readonly Rectangle InitalFrame; // mutable object...
        private Camera _cam;

        private int _tilesX;
        private int _tilesY;

        private int _paddingX;
        private int _paddingY;

        public WorldTileSheet(Texture2D texture, Rectangle initialFrame, Camera cam, int tilesX = 1, int tilesY = 1, int paddingX = 0, int paddingY = 0)
        {
            this._texture = texture;
            this.InitalFrame = initialFrame;
            this._cam = cam;

            this._tilesX = tilesX;
            this._tilesY = tilesY;
            this._paddingX = paddingX;
            this._paddingY = paddingY;
        }

        public Rectangle TileAt(int x, int y)
        {
            PreconditionCheckCoords(x, y);

            Rectangle rect = new Rectangle(
                InitalFrame.X + ((InitalFrame.Width + _paddingX) * x),
                InitalFrame.Y + ((InitalFrame.Height + _paddingY) * y),
                InitalFrame.Width,
                InitalFrame.Height
            );

            return rect;
        }

        private void PreconditionCheckCoords(int x, int y)
        {
            if ((x < 0 || x > _tilesX) || (y < 0 || y > _tilesY))
                throw new ArgumentException("X,Y coordinates must be contained in tile sheet");

        }

        public Rectangle Tile(int tile)
        {
            int x = tile % _tilesX;
            int y = tile / _tilesX;

            return TileAt(x, y);
        }

        public IEnumerable<Rectangle> AllTiles()
        {
            List<Rectangle> frames = new List<Rectangle>();

            for (int x = 0; x < _tilesX; x++)
                for (int y = 0; y < _tilesY; y++)
                    frames.Add(TileAt(x, y));

            return frames;
        }

        public WorldSprite SpriteTileAt(int x, int y)
        {
            PreconditionCheckCoords(x, y);
            Rectangle frame = TileAt(x, y);
            return new WorldSprite(_texture, frame, _cam);
        }

        public WorldSprite SpriteAnimation()
        {
            WorldSprite s = new WorldSprite(_texture, InitalFrame, _cam);
            s.ClearFrames();
            s.AddFrames(AllTiles());
            return s;
        }


    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    public class TileSheet
    {

        private Texture2D _texture;
        private Rectangle _initalFrame;
        private int _tilesX;
        private int _tilesY;

        private int _paddingX;
        private int _paddingY;

        public TileSheet(Texture2D texture, Rectangle initialFrame, int tilesX, int tilesY, int paddingX = 0, int paddingY = 0)
        {
            this._texture = texture;
            this._initalFrame = initialFrame;
            this._tilesX = tilesX;
            this._tilesY = tilesY;
            this._paddingX = paddingX;
            this._paddingY = paddingY;
        }

        public Rectangle TileAt(int x, int y)
        {
            PreconditionCheckCoords(x, y);

            Rectangle rect = new Rectangle(
                _initalFrame.X + ((_initalFrame.Width + _paddingX) * x),
                _initalFrame.Y + ((_initalFrame.Height + _paddingY) * y),
                _initalFrame.Width,
                _initalFrame.Height
            );

            return rect;
        }

        private void PreconditionCheckCoords(int x, int y)
        {
            if ((x < 0 || x > _tilesX) || (y < 0 || y > _tilesY))
                throw new ArgumentException("X,Y coordinates must be contained in tile sheet");

        }

        public IEnumerable<Rectangle> AllTiles()
        {
            for (int x = 0; x < _tilesX; x++)
                for (int y = 0; y < _tilesY; y++)
                    yield return TileAt(x, y);
        }

        public Sprite SpriteTileAt(int x, int y)
        {
            PreconditionCheckCoords(x, y);
            Rectangle frame = TileAt(x, y);
            return new Sprite(_texture, frame);
        }

        public Sprite SpriteAnimation()
        {
            Sprite s = new Sprite(_texture, _initalFrame);
            s.AddFrames(AllTiles());
            return s;
        }


    }
}

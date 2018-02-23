using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareChase
{
    class Square
    {
        public const float TimePerSquare = 0.75f;
        public const int SquareSize = 25;
        private static readonly Color[] Colours = new Color[] { Color.Red, Color.Blue, Color.Green };

        private Random _rand = new Random();

        public readonly Texture2D Texture;
        public Rectangle Bounds { get; private set; }

        private int _showColorIndex = 0;
        public Color ColourToShow => Colours[_showColorIndex];

        public Square(Texture2D texture)
        {
            Texture = texture;
        }

        public void WarpSquare(GameWindow window)
        {
            Bounds = new Rectangle(
                _rand.Next(0, window.ClientBounds.Width  - SquareSize),
                _rand.Next(0, window.ClientBounds.Height - SquareSize),
                SquareSize,
                SquareSize
            );
        }

        public void OnClick()
        {
            _showColorIndex = (_showColorIndex + 1) % Colours.Length;
        }
    }
}

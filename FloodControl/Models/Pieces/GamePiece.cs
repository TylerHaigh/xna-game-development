using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl.Models.Pieces
{
    /// <summary>
    /// Represents a single tile on the game board
    /// </summary>
    class GamePiece
    {
        /*
         * Requirements:
         *  - Identify sides of tile with pipe connectors
         *  - Differentiate between filled and empty pieces
         *  - Handle rotation
         *  - Provide a rectangle for drawing tile
         */
        
        // Don't use strings. Use enums
        // string indexes are indicative of their position in tile sheet
        public static string[] PieceTypes =
        {
            "Left,Right",
            "Top,Bottom",
            "Left,Top",
            "Top,Right",
            "Right,Bottom",
            "Bottom,Left",
            "Empty"
        };

        public const int PieceHeight = 40;
        public const int PieceWidth = 40;

        // Indexes into PieceTypes array
        public const int MaxPlayablePieceIndex = 5;
        public const int EmptyPieceIndex = 6;

        // Tile Sheet tile offset and padding between tiles
        private const int TextureOffsetX = 1;
        private const int TextureOffsetY = 1;
        private const int TexturePaddingX = 1;
        private const int TexturePaddingY = 1;

        // Refactor??
        public string PieceType { get; private set; } = "";
        public string Suffix { get; private set; } = ""; // String builder or list?

        public bool IsEmpty => PieceType == EmptyPieceType;
        public const string EmptyPieceType = "Empty";

        public GamePiece(string type) : this(type, "") { }
        public GamePiece(string type, string suffix)
        {
            this.PieceType = type;
            this.Suffix = suffix;
        }

        public void Set(string type) { Set(type, ""); }
        public void Set(string type, string suffix)
        {
            this.PieceType = type;
            this.Suffix = suffix;
        }

        public void AddSuffix(string suffix)
        {
            if (!this.Suffix.Contains(suffix))
                this.Suffix += suffix;
        }

        public void RemoveSuffix(string suffix) { this.Suffix = this.Suffix.Replace(suffix, ""); }

        public const string WaterSuffixString = "W";
        public void FillWithWater() { AddSuffix(WaterSuffixString); }
        public void EmptyWater() { RemoveSuffix(WaterSuffixString); }
        public bool IsFilledWithWater => Suffix.Contains(WaterSuffixString);

        public void Rotate(bool clockWise)
        {
            switch (this.PieceType)
            {
                case "Top,Bottom":   this.PieceType = "Left,Right"; break;
                case "Left,Right":   this.PieceType = "Top,Bottom"; break;
                case "Left,Top":     this.PieceType = (clockWise) ? "Top,Right"    : "Bottom,Left";  break;
                case "Top,Right":    this.PieceType = (clockWise) ? "Right,Bottom" : "Left,Top";     break;
                case "Right,Bottom": this.PieceType = (clockWise) ? "Bottom,Left"  : "Top,Right";    break;
                case "Bottom,Left":  this.PieceType = (clockWise) ? "Left,Top"     : "Right,Bottom"; break;
                case "Empty": break;
            }
        }

        public string[] GetOtherEnds(string startingEnd)
        {
            // But this should only return one end... right?
            // unless the starting end isn't part of this piece, then it would return both.
            List<string> opposites = new List<string>();
            foreach (string end in PieceType.Split(','))
            {
                if (end != startingEnd)
                    opposites.Add(end);
            }
            return opposites.ToArray();
        }

        public bool HasConnector(string direction) { return this.PieceType.Contains(direction); }

        public Rectangle GetSoruceRect()
        {
            int x = TextureOffsetX;
            int y = TextureOffsetY;

            if (this.IsFilledWithWater)
                x += PieceWidth + TexturePaddingX;

            y += Array.IndexOf(PieceTypes, PieceType) * (PieceHeight + TexturePaddingY);

            return new Rectangle(x, y, PieceWidth, PieceHeight);
        }


        public static Rectangle CalculateScreenRenderDestinationRectangle(int pixelX, int pixelY)
        {
            Rectangle screenDestinationRect = new Rectangle(pixelX, pixelY, PieceWidth, PieceHeight);
            return screenDestinationRect;
        }

        // TODO: Create empty override method for UpdatePiece();

    }
}

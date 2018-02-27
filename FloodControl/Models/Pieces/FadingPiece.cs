using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl.Models.Pieces
{
    class FadingPiece : GamePiece
    {
        public const float FullOpaqueAlpha = 1.0f;
        public const float FullTransparentAlpha = 0;
        public const float AlphaChangeRate = 0.02f;

        public float AlphaLevel { get; private set; } = FullOpaqueAlpha;

        public FadingPiece(string pieceType, string suffix) : base(pieceType, suffix) { }

        public void UpdatePiece()
        {
            AlphaLevel = Math.Max(FullTransparentAlpha, AlphaLevel - AlphaChangeRate);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl
{
    class FallingPiece : GamePiece
    {
        public const int FallRate = 5;

        public int VerticalOffset { get; private set; }

        public FallingPiece(string pieceType, int verticalOffset) : base(pieceType)
        {
            this.VerticalOffset = verticalOffset;
        }

        public void UpdatePiece()
        {
            VerticalOffset = Math.Max(0, VerticalOffset - FallRate);
        }

    }
}

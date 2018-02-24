using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl
{
    class RotatingPiece : GamePiece
    {

        public const float RotationRate = (float)(Math.PI / 2.0) / AnimationFrames; // 90 Degrees over 10 frames
        public const int AnimationFrames = 10;

        public readonly bool RotateClockwise;
        public int RotationTicksRemaining { get; private set; } = AnimationFrames;

        private float _rotationAmount = 0;
        public float RotationAmount {
            get {
                return (float)( (RotateClockwise) ? _rotationAmount : (Math.PI * 2) - _rotationAmount);
            }
        }

        public RotatingPiece(string pieceType, bool clockwise) : base(pieceType) { this.RotateClockwise = clockwise; }
        
        public void UpdatePiece()
        {
            _rotationAmount += RotationRate;

            RotationTicksRemaining = Math.Max(0, RotationTicksRemaining - 1);
        }


    }
}

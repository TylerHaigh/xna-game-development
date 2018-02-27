using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl.Utils
{
    class ScoreZoom
    {

        private const int MaxDisplayCount = 30;
        private const float ScaleAmount = 0.4f;

        public string Text { get; set; }
        public Color DrawColor { get; set; }

        private int _displayCounter = 0;
        //private float _scale = 0.4f;
        private float _lastScaleAmount = 0;


        public float Scale => ScaleAmount * _displayCounter;
        public bool IsCompleted => _displayCounter > MaxDisplayCount;

        public ScoreZoom(string displayText, Color fontColor)
        {
            Text = displayText;
            DrawColor = fontColor;
            _displayCounter = 0;
        }

        public void Update()
        {
            //_scale += _lastScaleAmount + ScaleAmount;
            _lastScaleAmount += ScaleAmount;
            _displayCounter++;
        }


    }
}

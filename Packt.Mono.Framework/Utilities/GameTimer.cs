using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Utilities
{
    public class GameTimer
    {
        public readonly TimeSpan Duration;

        public bool Completed => _currentTime >= Duration;

        private TimeSpan _currentTime;

        public GameTimer(float f) : this(TimeSpan.FromSeconds(f)) { }
        public GameTimer(double d) : this(TimeSpan.FromSeconds(d)) { }
        public GameTimer(TimeSpan duration) { Duration = duration; _currentTime = new TimeSpan(); }

        public void Update(GameTime gameTime)
        {
            _currentTime = _currentTime.Add(gameTime.ElapsedGameTime);
        }

        public void Reset() { _currentTime = new TimeSpan(); }

    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    public class Timer
    {
        public readonly TimeSpan Duration;

        public bool Completed => _currentTime >= Duration;

        private TimeSpan _currentTime;

        public Timer(TimeSpan duration) { Duration = duration; _currentTime = new TimeSpan(); }

        public void Update(GameTime gameTime)
        {
            _currentTime = _currentTime.Add(gameTime.ElapsedGameTime);
        }

        public void Reset() { _currentTime = new TimeSpan(); }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models
{
    class Player
    {
        public event EventHandler<ShotFiredEventArgs> ShotFired;

        public void FireShot()
        {
            ShotFired?.Invoke(this, null);
        }
    }
}

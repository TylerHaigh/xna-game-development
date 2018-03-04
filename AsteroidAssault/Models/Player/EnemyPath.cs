using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class EnemyPath
    {
        private List<Vector2> _waypoints = new List<Vector2>();

        public int NumberOfEnemiesAvailable { get; set; }
        public readonly int PathNumber;

        public EnemyPath() { }

        public void AddWayPoint(Vector2 waypoint) => _waypoints.Add(waypoint);

        public Vector2 this [int i] { get => _waypoints[i]; }

        public IEnumerable<Vector2> WayPoints => new List<Vector2>(_waypoints);

    }
}

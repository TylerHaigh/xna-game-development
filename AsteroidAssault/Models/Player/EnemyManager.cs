using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class EnemyManager
    {

        public const int MinEnemiesPerWave = 5;
        public const int MaxEnemiesPerWave = 8;

        private const float NextWaveTimeout = 8;
        private const float SpawnWaitTimeout = 0.5f;
        private const float ShipShotChance = 0.2f;

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        private List<Enemy> _enemies = new List<Enemy>();
        private GameTimer _nextWaveTimer = new GameTimer(TimeSpan.FromSeconds(NextWaveTimeout));
        private GameTimer _spawnTimer = new GameTimer(TimeSpan.FromSeconds(SpawnWaitTimeout));
        private Random _rand = new Random();

        private List<Vector2> _pathWayPoints = new List<Vector2>();
        private Dictionary<int, int> _waveSpawns = new Dictionary<int, int>();

        public bool Active { get; private set; }



    }
}

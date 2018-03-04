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

        public int MinEnemiesPerWave { get; set; } = 5;
        public int MaxEnemiesPerWave { get; set; } = 8;

        private const float NextWaveTimeout = 8;
        private const float SpawnWaitTimeout = 0.5f;
        private const float ShipShotChance = 0.2f; // out of 100
        private const float ShotSpeed = 150f;

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        private List<Enemy> _enemies = new List<Enemy>();
        private GameTimer _nextWaveTimer = new GameTimer(TimeSpan.FromSeconds(NextWaveTimeout));
        private GameTimer _spawnTimer = new GameTimer(TimeSpan.FromSeconds(SpawnWaitTimeout));
        private Random _rand = new Random();

        private List<List<Vector2>> _pathWayPoints = new List<List<Vector2>>();
        private Dictionary<int, int> _waveSpawns = new Dictionary<int, int>(); // path number and number of ships

        public bool Active { get; set; }

        // TODO: Needs to know the Player's location to work out where to fire shots at

        public EnemyManager(TileSheet tileSheet, Rectangle screenBounds)
        {
            _tileSheet = tileSheet;
            _screenBounds = screenBounds;

            SetupWayPoints();
        }

        private void SpawnEnemy(int path)
        {
            List<Vector2> pathWayPoints = _pathWayPoints[path]; // TODO: Refactor into Path class
            Enemy e = new Enemy(_tileSheet.SpriteAnimation(), pathWayPoints[0]);
            e.AddWayPoints(pathWayPoints);
            _enemies.Add(e);
        }

        private void SpawnWave(int waveType)
        {
            // add enemies to number of enemies to be spawned on
            // a particular path
            _waveSpawns[waveType] += _rand.Next(MinEnemiesPerWave, MaxEnemiesPerWave + 1);
        }

        private void UpdateWaveSpawns(GameTime gameTime)
        {
            UpdateSpawnTimer(gameTime);
            UpdateNextWaveTimer(gameTime);
        }

        private void UpdateSpawnTimer(GameTime gameTime)
        {
            _spawnTimer.Update(gameTime);

            if (_spawnTimer.Completed)
            {
                for (int i = _waveSpawns.Count; i >= 0; i--)
                {
                    if (_waveSpawns[i] > 0) // number of ships to spawn
                    {
                        SpawnEnemy(i);
                        _waveSpawns[i]--;
                    }
                }

                _spawnTimer.Reset();
            }
        }

        private void UpdateNextWaveTimer(GameTime gameTime)
        {
            _nextWaveTimer.Update(gameTime);

            if(_nextWaveTimer.Completed)
            {
                SpawnWave(_rand.Next(0, _pathWayPoints.Count));
                _nextWaveTimer.Reset();
            }
        }


        private void SetupWayPoints()
        {
            // fucking hard coded bullshit....
            List<Vector2> path0 = new List<Vector2>();
            path0.Add(new Vector2(850, 300));
            path0.Add(new Vector2(-100, 300));
            _pathWayPoints.Add(path0);
            _waveSpawns[0] = 0;

            List<Vector2> path1 = new List<Vector2>();
            path1.Add(new Vector2(-50, 225));
            path1.Add(new Vector2(850, 225));
            _pathWayPoints.Add(path1);
            _waveSpawns[1] = 0;

            List<Vector2> path2 = new List<Vector2>();
            path2.Add(new Vector2(-100, 50));
            path2.Add(new Vector2(150, 50));
            path2.Add(new Vector2(200, 75));
            path2.Add(new Vector2(200, 125));
            path2.Add(new Vector2(150, 150));
            path2.Add(new Vector2(150, 175));
            path2.Add(new Vector2(200, 200));
            path2.Add(new Vector2(600, 200));
            path2.Add(new Vector2(850, 600));
            _pathWayPoints.Add(path2);
            _waveSpawns[2] = 0;

            List<Vector2> path3 = new List<Vector2>();
            path3.Add(new Vector2(600, -100));
            path3.Add(new Vector2(600, 250));
            path3.Add(new Vector2(580, 275));
            path3.Add(new Vector2(500, 250));
            path3.Add(new Vector2(500, 200));
            path3.Add(new Vector2(450, 175));
            path3.Add(new Vector2(400, 150));
            path3.Add(new Vector2(-100, 150));
            _pathWayPoints.Add(path3);
            _waveSpawns[3] = 0;
        }


    }
}

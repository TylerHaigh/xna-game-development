using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class EnemyManager : IEntityManager
    {

        public int MinEnemiesPerWave { get; set; } = 5;
        public int MaxEnemiesPerWave { get; set; } = 8;

        private const float NextWaveTimeout = 8;
        private const float SpawnWaitTimeout = 0.5f;
        private const float ShipShotChance = 0.2f; // out of 100

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        private List<Enemy> _enemies = new List<Enemy>();
        private GameTimer _nextWaveTimer = new GameTimer(TimeSpan.FromSeconds(NextWaveTimeout));
        private GameTimer _spawnTimer = new GameTimer(TimeSpan.FromSeconds(SpawnWaitTimeout));
        private Random _rand = new Random();

        private List<EnemyPath> _paths = new List<EnemyPath>();

        public bool Active { get; set; } = true;

        public event EventHandler<ShotFiredEventArgs> ShotFired;
        public event EventHandler OnEnemyDestroyed;

        public EnemyManager(TileSheet tileSheet, Rectangle screenBounds)
        {
            _tileSheet = tileSheet;
            _screenBounds = screenBounds;

            SetupWayPoints();
        }

        private void SpawnEnemy(EnemyPath path)
        {
            Enemy e = new Enemy(_tileSheet.SpriteAnimation(), path[0]);
            e.AddPath(path);
            _enemies.Add(e);

            e.ShotFired += EnemyShotFired;
            e.OnDestroy += HandleOnEnemyDestroy;
        }

        private void HandleOnEnemyDestroy(object sender, EventArgs e)
        {
            OnEnemyDestroyed?.Invoke(sender, e);
        }

        private void EnemyShotFired(object sender, ShotFiredEventArgs e)
        {
            ShotFired?.Invoke(sender, e);
        }

        private void SpawnWave(EnemyPath path)
        {
            // add enemies to number of enemies to be spawned on
            // a particular path
            path.NumberOfEnemiesAvailable += _rand.Next(MinEnemiesPerWave, MaxEnemiesPerWave + 1);
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
                foreach (var p in _paths)
                {
                    if (p.NumberOfEnemiesAvailable > 0) // number of ships to spawn
                    {
                        SpawnEnemy(p);
                        p.NumberOfEnemiesAvailable--;
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
                int pathIndex = _rand.Next(0, _paths.Count);
                EnemyPath path = _paths[pathIndex];
                SpawnWave(path);
                _nextWaveTimer.Reset();
            }
        }


        private void SetupWayPoints()
        {
            // fucking hard coded bullshit....
            EnemyPath path0 = new EnemyPath();
            path0.AddWayPoint(new Vector2(850, 300));
            path0.AddWayPoint(new Vector2(-100, 300));
            _paths.Add(path0);

            EnemyPath path1 = new EnemyPath();
            path1.AddWayPoint(new Vector2(-50, 225));
            path1.AddWayPoint(new Vector2(850, 225));
            _paths.Add(path1);

            EnemyPath path2 = new EnemyPath();
            path2.AddWayPoint(new Vector2(-100, 50));
            path2.AddWayPoint(new Vector2(150, 50));
            path2.AddWayPoint(new Vector2(200, 75));
            path2.AddWayPoint(new Vector2(200, 125));
            path2.AddWayPoint(new Vector2(150, 150));
            path2.AddWayPoint(new Vector2(150, 175));
            path2.AddWayPoint(new Vector2(200, 200));
            path2.AddWayPoint(new Vector2(600, 200));
            path2.AddWayPoint(new Vector2(850, 600));
            _paths.Add(path2);

            EnemyPath path3 = new EnemyPath();
            path3.AddWayPoint(new Vector2(600, -100));
            path3.AddWayPoint(new Vector2(600, 250));
            path3.AddWayPoint(new Vector2(580, 275));
            path3.AddWayPoint(new Vector2(500, 250));
            path3.AddWayPoint(new Vector2(500, 200));
            path3.AddWayPoint(new Vector2(450, 175));
            path3.AddWayPoint(new Vector2(400, 150));
            path3.AddWayPoint(new Vector2(-100, 150));
            _paths.Add(path3);
        }

        public void Update(GameTime gameTime)
        {
            _enemies.RemoveAll(e => e.Destroyed);

            for (int i = _enemies.Count-1; i >= 0; i--)
            {
                Enemy e = _enemies[i];
                e.Update(gameTime);
                if (!e.IsActive())
                {
                    e.DestroyEntity();
                    _enemies.RemoveAt(i);
                }
                else
                {
                    // todo: move to Enemy class
                    float chance = _rand.Next(0, 100);
                    if(chance <= ShipShotChance)
                    {
                        e.FireShot();
                    }
                }
            }

            if (Active)
                UpdateWaveSpawns(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var e in _enemies)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }

        public void Clear()
        {
            _enemies.ForEach(e => e.DestroyEntity());
            _enemies.Clear();
        }

    }
}

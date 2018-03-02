using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models
{
    class AsteroidManager
    {
        /*
         * Responsibilities:
         *  - manage asteroid initial positioning
         *  - collision management
         *  - reposition when off-screen
         */

        private const int ScreenPadding = 10;
        private const int MinSpeed = 60;
        private const int MaxSpeed = 120;

        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        private List<Sprite> _asteroids = new List<Sprite>();
        private Random _rand = new Random();


        public AsteroidManager(int asteroidCount, TileSheet tileSheet, Rectangle screenBounds)
        {
            this._tileSheet = tileSheet;
            this._screenBounds = screenBounds;

            GenerateAsteroids(asteroidCount);
        }


        public void AddAsteroid()
        {
            Vector2 initPosition = new Vector2(-500, -500); // hack to force the asteroid to reposition itself when Update() is called
            Sprite asteroid = _tileSheet.SpriteAnimation();

            asteroid.Rotation = MathHelper.ToRadians((float)_rand.Next(0, 360));
            asteroid.CollisionRadius = 15;

            _asteroids.Add(asteroid);
        }

        public void Clear() => _asteroids.Clear();

        public void GenerateAsteroids(int asteroidCount)
        {
            Clear();

            for (int i = 0; i < asteroidCount; i++)
                AddAsteroid();
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Asteroid
{
    class AsteroidManager : IEntityManager
    {
        /*
         * Responsibilities:
         *  - manage asteroid initial positioning
         *  - collision management
         *  - reposition when off-screen
         */


        private TileSheet _tileSheet;
        private Rectangle _screenBounds;

        private List<Asteroid> _asteroids = new List<Asteroid>();
        private Random _rand = new Random();

        public event EventHandler OnAsteroidDestroy;

        public AsteroidManager(int asteroidCount, TileSheet tileSheet, Rectangle screenBounds)
        {
            this._tileSheet = tileSheet;
            this._screenBounds = screenBounds;

            GenerateAsteroids(asteroidCount);
        }

        public void Reset()
        {
            foreach (var a in _asteroids)
            {
                a.Location = new Vector2(-500, -500); ; // off screen
            }
        }

        public void AddAsteroid()
        {
            Vector2 initPosition = new Vector2(-500, -500); // hack to force the asteroid to reposition itself when Update() is called
            Sprite asteroidSprite = _tileSheet.SpriteAnimation();

            asteroidSprite.Location = initPosition;
            asteroidSprite.Rotation = MathHelper.ToRadians(_rand.Next(0, 360));

            Asteroid a = new Asteroid(asteroidSprite);
            a.OnDestroy += HandleOnAsteroidDestroy;
            _asteroids.Add(a);
        }

        private void HandleOnAsteroidDestroy(object sender, EventArgs e)
        {
            OnAsteroidDestroy?.Invoke(sender, e);
        }

        public void Clear() {

            _asteroids.ForEach(a => a.DestroyEntity());
            _asteroids.Clear();
        }

        public void GenerateAsteroids(int asteroidCount)
        {
            Clear();

            for (int i = 0; i < asteroidCount; i++)
                AddAsteroid();
        }

        private Vector2 GenerateRandomLocation()
        {
            Vector2 location = Vector2.Zero;
            bool invalidLocation = true;
            int tryCount = 0;
            const int TryCount = 5;

            do
            {
                tryCount++;

                location = CalculateRandomLocationOutsideOfScreen();
                invalidLocation = CheckIfLocationCollidesWithExistingAsteroid(location);

                if (tryCount > TryCount && invalidLocation)
                {
                    // force it to be some random location out of the screen and we will re-attempt again on next call to Update()
                    location = new Vector2(-500, -500);
                    invalidLocation = false;
                }
            } while (invalidLocation);

            return location;
        }

        private Vector2 CalculateRandomLocationOutsideOfScreen()
        {
            Vector2 location = Vector2.Zero;

            switch (_rand.Next(0, 3))
            {
                case 0: // Somewhere on Y axis on the outside of the LHS of screen
                    {
                        location.X = 0 - _tileSheet.InitalFrame.Width;
                        location.Y = _rand.Next(0, _screenBounds.Height);
                        break;
                    }
                case 1: // Somewhere on Y axis on the outside of the RHS of screen
                    {
                        location.X = _screenBounds.Width + 0;
                        location.Y = _rand.Next(0, _screenBounds.Height);
                        break;
                    }
                case 2: // somewhere on the X axis on the outside of the TOP of the screen
                    {
                        location.X = _rand.Next(0, _screenBounds.Width);
                        location.Y = 0 - _tileSheet.InitalFrame.Height;
                        break;
                    }
            }

            return location;
        }

        private bool CheckIfLocationCollidesWithExistingAsteroid(Vector2 location)
        {
            foreach (var a in _asteroids)
            {
                // If there is already an asteroid at the target location, then we can't move the target asteroid to here
                Rectangle other = new Rectangle((int)location.X, (int)location.Y, _tileSheet.InitalFrame.Width, _tileSheet.InitalFrame.Height);
                if (a.IsBoxColliding(other))
                {
                    return true;
                }
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var a in _asteroids)
            {
                a.Update(gameTime);

                if (!a.IsOnScreen(_screenBounds))
                {
                    a.Location = GenerateRandomLocation();
                    a.RandomiseVelocity();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var a in _asteroids)
                a.Draw(gameTime, spriteBatch);
        }

    }
}

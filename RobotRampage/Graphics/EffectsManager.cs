using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Graphics
{
    public class ParticleConfig
    {
        public int MinPointCount { get; set; }
        public int MaxPointCount { get; set; }
        public int MinPieceCount { get; set; }
        public int MaxPieceCount { get; set; }
        public float PieceSpeedScale { get; set; }
        public int Duration { get; set; }
        public Color InitialColor { get; set; }
        public Color FinalColor { get; set; }
    }

    public class EffectsManager : IEntityManager
    {
        // Manages both Sparks and Explosions
        // TODO: Split out SparkParticleSystem and ExplosionParticleSystem

        private const float ExplosionMaxSpeed = 30;

        private List<Particle> _particles = new List<Particle>();
        private Random _rand = new Random();

        private TileSheet _explosionTileSheet;
        private TileSheet _particleTileSheet;

        public EffectsManager(TileSheet explosionTileSheet, TileSheet particleTileSheet)
        {
            _explosionTileSheet = explosionTileSheet;
            _particleTileSheet = particleTileSheet;
        }

        private Vector2 RandomDirection(float scale)
        {
            Vector2 direction = Vector2.Zero;
            while (direction.Length() == 0)
            {
                direction = new Vector2(
                    _rand.Next(-50, 51),
                    _rand.Next(-50, 51)
                );
            }

            direction.Normalize();
            direction *= scale;

            return direction;
        }

        public void AddExplosion(Vector2 location, Vector2 momentum, ParticleConfig config)
        {
            Rectangle rect = _explosionTileSheet.TileAt(0, 0);
            Vector2 pieceLocation = location - new Vector2(rect.Width / 2, rect.Height / 2);

            int piecesToGenerate = _rand.Next(config.MinPieceCount, config.MaxPieceCount + 1);
            GeneratePieces(pieceLocation, momentum, piecesToGenerate, config);

            int pointsToGenerate = _rand.Next(config.MinPointCount, config.MaxPointCount + 1);
            GeneratePoints(pieceLocation, momentum, pointsToGenerate, config);

        }

        private void GeneratePieces(Vector2 pieceLocation, Vector2 momentum, int piecesToGenerate, ParticleConfig config)
        {
            for (int i = 0; i < piecesToGenerate; i++)
            {
                Sprite s = _explosionTileSheet.SpriteAnimation();

                Particle p = new Particle(s, Vector2.Zero, ExplosionMaxSpeed, config.Duration, config.InitialColor, config.FinalColor);
                p.Location = pieceLocation;
                p.Velocity = RandomDirection(config.PieceSpeedScale) + momentum;

                _particles.Add(p);
            }
        }

        private void GeneratePoints(Vector2 pointLocation, Vector2 momentum, int pointsToGenerate, ParticleConfig config)
        {
            int pointSpeedMin = (int)config.PieceSpeedScale * 2;
            int pointSpeedMax = (int)config.PieceSpeedScale * 3;

            for (int i = 0; i < pointsToGenerate; i++)
            {
                Sprite s = _particleTileSheet.SpriteAnimation();

                Particle p = new Particle(s, Vector2.Zero, ExplosionMaxSpeed, config.Duration, config.InitialColor, config.FinalColor);
                p.Location = pointLocation;

                int pointSpeed = _rand.Next(pointSpeedMin, pointSpeedMax);
                p.Velocity = RandomDirection(pointSpeed) + momentum;

                _particles.Add(p);
            }
        }

        public void AddExplosion(Vector2 location, Vector2 momentum)
        {
            ParticleConfig config = new ParticleConfig
            {
                MinPointCount = 15,
                MaxPointCount = 20,
                MinPieceCount = 2,
                MaxPieceCount = 4,
                PieceSpeedScale = 6,
                Duration = 90,
                InitialColor = new Color(1, 3, 0, 0.5f),
                FinalColor = new Color(1, 3, 0, 0)
            };
            AddExplosion(location, momentum, config);
        }

        public void AddLargeExplosion(Vector2 location)
        {
            ParticleConfig config = new ParticleConfig
            {
                MinPointCount = 15,
                MaxPointCount = 20,
                MinPieceCount = 4,
                MaxPieceCount = 6,
                PieceSpeedScale = 30,
                Duration = 90,
                InitialColor = new Color(1, 3, 0, 0.5f),
                FinalColor = new Color(1, 3, 0, 0)
            };
            AddExplosion(location, Vector2.Zero, config);
        }

        public void AddSparksEffect(Vector2 location, Vector2 impactVelocity)
        {
            int particleCount = _rand.Next(10, 20);

            // Divide by 60 because the game runs in 60fps
            // so this will be the location one frame previous
            Vector2 particleSpawnLocation = location - (impactVelocity / 60);
            for(int i = 0; i < particleCount; i++)
            {
                Sprite s = _particleTileSheet.SpriteAnimation();
                Particle p = new Particle(s, Vector2.Zero, 60, 20, Color.Yellow, Color.Orange);
                p.Location = particleSpawnLocation;
                p.Velocity = RandomDirection(_rand.Next(10, 20));

                _particles.Add(p);
            }
        }


        public void Clear()
        {
            _particles.ForEach(p => p.DestroyEntity());
            _particles.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
                p.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                Particle p = _particles[i];
                if (p.IsActive)
                    p.Update(gameTime);
                else
                    _particles.RemoveAt(i);
            }
        }
    }
}

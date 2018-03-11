using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class ExplosionParticleSystem
    {

        public enum ParticleType { Point, Piece }

        private TileSheet _tileSheet;
        public readonly ParticleType Type;

        public const int PieceAnimationFrames = 3;
        public const int PointAnimationFrames = 1;
        public const int PieceWidth = 50;
        public const int PieceHeight = 50;
        public const int PointWidth = 2;
        public const int PointHeight = 2;

        // large particle pieces
        private const int MinPieceCount = 3;
        private const int MaxPieceCount = 6;
        private const float PieceSpeed = 6;

        // single point particles
        private const int MinPointCount = 20;
        private const int MaxPointCount = 30;
        private const int MinPointSpeed = 15;
        private const int MaxPointSpeed = 30;

        private const int DurationCount = 90; // timespan
        private const float ExplosionMaxSpeed = 30;

        private static readonly Color InitialColor = new Color(1, 0.3f, 0) * 0.5f; // orange 50%
        private static readonly Color FinalColor = new Color(0, 0, 0);
        private Random _rand = new Random();

        private List<Particle> _particles = new List<Particle>();


        public ExplosionParticleSystem(TileSheet tilesheet, ParticleType type)
        {
            this._tileSheet = tilesheet;
            Type = type;
        }


        private Vector2 RandomDirection(float scale)
        {
            Vector2 direction;

            do
            {
                direction = new Vector2(
                    _rand.Next(-50, 51),
                    _rand.Next(-50, 51)
                );
            } while (direction.Length() == 0);

            direction.Normalize();
            direction *= scale;

            return direction;
        }

        // Spawns x particles at the location to form an explosion
        // todo: refactor into explosion particle class
        public void AddExplosion(Vector2 location, Vector2 momentum)
        {
            switch (Type)
            {
                case ParticleType.Point:
                    {
                        int pointsToGenerate = _rand.Next(MinPointCount, MaxPointCount + 1);
                        GeneratePoints(location, momentum, pointsToGenerate);
                        break;
                    }
                case ParticleType.Piece:
                    {
                        // location is the center of the sprite where we want to render the particle
                        // so we compensate by calculating the upper left corner which is where we
                        // need to position the particle for rendering
                        Rectangle rect = _tileSheet.TileAt(0, 0);
                        Vector2 pieceLocation = location - new Vector2(rect.Width / 2, rect.Height / 2);

                        int piecesToGenerate = _rand.Next(MinPieceCount, MaxPieceCount + 1);
                        GeneratePieces(pieceLocation, momentum, piecesToGenerate);
                        break;
                    }
                default:
                    break;
            }
        }

        private void GeneratePieces(Vector2 pieceLocation, Vector2 momentum, int piecesToGenerate)
        {
            for (int i = 0; i < piecesToGenerate; i++)
            {
                Sprite s = _tileSheet.SpriteAnimation();
                
                Particle p = new Particle(s, Vector2.Zero, ExplosionMaxSpeed, DurationCount, InitialColor, FinalColor);
                p.Location = pieceLocation;
                p.Velocity = RandomDirection(PieceSpeed) + momentum;

                _particles.Add(p);
            }
        }

        private void GeneratePoints(Vector2 pointLocation, Vector2 momentum, int pointsToGenerate)
        {
            for (int i = 0; i < pointsToGenerate; i++)
            {
                Sprite s = _tileSheet.SpriteAnimation();

                Particle p = new Particle(s, Vector2.Zero, ExplosionMaxSpeed, DurationCount, InitialColor, FinalColor);
                p.Location = pointLocation;

                float pointSpeed = _rand.Next(MinPointSpeed, MaxPointSpeed);
                p.Velocity = RandomDirection(pointSpeed) + momentum;

                _particles.Add(p);
            }
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
                p.Draw(gameTime, spriteBatch);
        }
    }
}

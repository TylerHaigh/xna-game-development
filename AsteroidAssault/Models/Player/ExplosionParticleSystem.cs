using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Models.Player
{
    class ExplosionParticleSystem : IGameEntity
    {

        private TileSheet _tileSheet;
        private Rectangle _pointRectangle;

        private const int MinPieceCount = 3;
        private const int MaxPieceCount = 6;
        private const float PieceSpeedScale = 6;

        private const int MinPointCount = 20;
        private const int MaxPointCount = 30;
        private const int MinPointSpeed = 15;
        private const int MaxPointSpeed = 30;

        private const int DurationCount = 90;
        private const float ExplosionMaxSpeed = 30;

        private static readonly Color InitialColor = new Color(1, 0.3f, 0) * 0.5f;
        private static readonly Color FinalColor = new Color(0, 0, 0);
        private Random _rand = new Random();

        private List<Particle> _particles = new List<Particle>();


        public ExplosionParticleSystem(TileSheet tilesheet, Rectangle pointRectangle)
        {
            this._tileSheet = tilesheet;
            this._pointRectangle = pointRectangle;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}

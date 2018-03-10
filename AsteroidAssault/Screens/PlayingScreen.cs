using AsteroidAssault.Models;
using AsteroidAssault.Models.Asteroid;
using AsteroidAssault.Models.Player;
using AsteroidAssault.Models.Star;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Screens
{
    class PlayingScreen : GameScreen
    {
        private Texture2D _spriteSheet;

        private StarField _starField;
        private const int StarCount = 200;
        private Vector2 _starVelocity = new Vector2(0, 30);
        private Rectangle _starTextureSourceRectangle = new Rectangle(0, 450, Star.TextureWidth, Star.TextureHeight);

        private const int AsteroidCount = 10;
        private Rectangle _initalAsteroidFrame = new Rectangle(0, 0, Asteroid.SpriteWidth, Asteroid.SpriteHeight);
        private AsteroidManager _asteroidManager;

        private Player _player;
        private Rectangle _initalPlayerFrame = new Rectangle(0, 150, Player.PlayerSpriteWidth, Player.PlayerSpriteHeight);

        private ShotManager _shotManager;
        private Rectangle _shotTextureInitialFrame = new Rectangle(0, 300, Shot.TextureWidth, Shot.TextureHeight);

        private EnemyManager _enemyManager;
        private Rectangle _enemyInitialFrame = new Rectangle(0, 200, Enemy.TextureWidth, Enemy.TextureHeight);

        private ExplosionParticleSystem _pieceExplosionManager;
        private ExplosionParticleSystem _pointExplosionManager;
        private Rectangle _explosionPieceSpriteFrame = new Rectangle(0, 100, ExplosionParticleSystem.PieceWidth, ExplosionParticleSystem.PieceHeight);
        private Rectangle _explosionPointSpriteFrame = new Rectangle(0, 450, ExplosionParticleSystem.PointWidth, ExplosionParticleSystem.PointHeight);

        public PlayingScreen(Game game) : base(game)
        {
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _spriteSheet = textureManager.OptionalLoadContent<Texture2D>(@"Textures/SpriteSheet");

            _starField = new StarField(StarCount, _starVelocity, new TileSheet(_spriteSheet, _starTextureSourceRectangle, Star.AnimationFrames), ScreenBounds);
            _asteroidManager = new AsteroidManager(AsteroidCount, new TileSheet(_spriteSheet, _initalAsteroidFrame, Asteroid.AsteroidFrames), ScreenBounds);
            _player = new Player(new TileSheet(_spriteSheet, _initalPlayerFrame, Player.PlayerAnimationFrames).SpriteAnimation(), ScreenBounds);
            _shotManager = new ShotManager(new TileSheet(_spriteSheet, _shotTextureInitialFrame, Shot.AnimationFrames), ScreenBounds);
            _enemyManager = new EnemyManager(new TileSheet(_spriteSheet, _enemyInitialFrame, Enemy.AnimationFrames), ScreenBounds);

            _pieceExplosionManager = new ExplosionParticleSystem(new TileSheet(_spriteSheet, _explosionPieceSpriteFrame, ExplosionParticleSystem.PieceAnimationFrames), ExplosionParticleSystem.ParticleType.Piece);
            _pointExplosionManager = new ExplosionParticleSystem(new TileSheet(_spriteSheet, _explosionPointSpriteFrame, ExplosionParticleSystem.PointAnimationFrames), ExplosionParticleSystem.ParticleType.Point);

            _player.ShotFired += (sender, args) => _shotManager.CreateShot(args);
            _enemyManager.ShotFired += EnemyManagerShotFired;
        }

        private void EnemyManagerShotFired(object sender, ShotFiredEventArgs e)
        {
            // Work out direction of enemy to player
            Vector2 shotDirection = _player.Center - e.Location;
            shotDirection.Normalize();
            e.Velocity = shotDirection;

            _shotManager.CreateShot(e);
        }

        public override void Update(GameTime gameTime)
        {
            _starField.Update(gameTime);
            _asteroidManager.Update(gameTime);
            _player.Update(gameTime);
            _shotManager.Update(gameTime);
            _enemyManager.Update(gameTime);
            _pieceExplosionManager.Update(gameTime);
            _pointExplosionManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _starField.Draw(gameTime, spriteBatch);
            _asteroidManager.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            _shotManager.Draw(gameTime, spriteBatch);
            _enemyManager.Draw(gameTime, spriteBatch);
            _pieceExplosionManager.Draw(gameTime, spriteBatch);
            _pointExplosionManager.Draw(gameTime, spriteBatch);
        }
    }
}

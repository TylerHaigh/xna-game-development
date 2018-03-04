using AsteroidAssault.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
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
        private Rectangle _starTextureSourceRectangle = new Rectangle(0, 450, 2, 2); // todo: create star class

        private const int AsteroidCount = 10;
        private Rectangle _initalAsteroidFrame = new Rectangle(0, 0, Asteroid.SpriteWidth, Asteroid.SpriteHeight);
        private AsteroidManager _asteroidManager;

        private Player _player;
        private Rectangle _initalPlayerFrame = new Rectangle(0, 150, Player.PlayerSpriteWidth, Player.PlayerSpriteHeight);

        private ShotManager _shotManager;
        private Rectangle _shotTexture = new Rectangle(0, 300, Shot.TextureWidth, Shot.TextureHeight);

        public PlayingScreen(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _starField.Draw(gameTime, spriteBatch);
            _asteroidManager.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            _shotManager.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _spriteSheet = textureManager.OptionalLoadContent<Texture2D>(@"Textures/SpriteSheet");

            _starField = new StarField(StarCount, _starVelocity, new TileSheet(_spriteSheet, _starTextureSourceRectangle, Star.AnimationFrames), ScreenBounds);
            _asteroidManager = new AsteroidManager(AsteroidCount, new TileSheet(_spriteSheet, _initalAsteroidFrame, Asteroid.AsteroidFrames), ScreenBounds);
            _player = new Player(new TileSheet(_spriteSheet, _initalPlayerFrame, Player.PlayerAnimationFrames).SpriteAnimation(), ScreenBounds);
            _shotManager = new ShotManager(new TileSheet(_spriteSheet, _shotTexture, 4), ScreenBounds);

            _player.ShotFired += (sender, args) => _shotManager.CreateShot(args);

        }

        public override void Update(GameTime gameTime)
        {
            _starField.Update(gameTime);
            _asteroidManager.Update(gameTime);
            _player.Update(gameTime);
            _shotManager.Update(gameTime);
        }
    }
}

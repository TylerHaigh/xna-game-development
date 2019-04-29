using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework.Collision;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Screen;
using RobotRampage.Graphics;
using RobotRampage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotRampage.Utils
{
    class GameWorld : GameScreen
    {
        private Camera _cam;

        private List<GameEntity> _entities = new List<GameEntity>();

        private TileMap _tileMap;
        private Player _player;
        private CollisionEngine _collisionEngine;
        private EffectsManager _effectsManager;
        private ShotManager _shotManager;

        public GameWorld(Game game, Camera cam) : base(game)
        {
            _cam = cam;
            _tileMap = new TileMap();
            _collisionEngine = new CollisionEngine();

            CollisionComponent.CreatedCollisionComponent += RegisterCollisionBox;
        }

        private void RegisterCollisionBox(object sender, CreatedCollisionComponentArgs args)
        {
            _collisionEngine.AddEntity(args.Component);
        }

        public override void LoadContent(TextureManager textureManager)
        {
            Texture2D spriteSheet = textureManager.OptionalLoadContent<Texture2D>(@"Textures\SpriteSheet");

            _tileMap.Intialise(spriteSheet);

            //_player = new Player(
            //    spriteSheet,
            //    new Rectangle(0, 64, 32, 32), 6,
            //    new Rectangle(0, 96, 32, 32), 1,
            //    new Vector2(300, 300),
            //    _cam
            //);

            Rectangle playerBaseInitialRectangle = new Rectangle(0, 64, 32, 32);
            Rectangle playerTurretInitialRectangle = new Rectangle(0, 96, 32, 32);
            Vector2 playerLocation = new Vector2(300, 300);

            WorldSprite playerBase   = new WorldSprite(spriteSheet, playerBaseInitialRectangle, _cam);
            WorldSprite playerTurret = new WorldSprite(spriteSheet, playerTurretInitialRectangle, _cam);

            playerBase.AddAnimation(6);

            _player = new Player(playerBase, playerTurret, _cam)
            {
                WorldLocation = playerLocation
            };


            Rectangle particleRect = new Rectangle(0, 288, 2, 2);
            Rectangle explosionParticleRect = new Rectangle(0, 256, 32, 32);
            WorldTileSheet explosionTileSheet = new WorldTileSheet(spriteSheet, particleRect, _cam);
            WorldTileSheet particleTileSheet = new WorldTileSheet(spriteSheet, explosionParticleRect, _cam, 3);

            _effectsManager = new EffectsManager(explosionTileSheet, particleTileSheet);

            // Shots
            Rectangle shotRect = new Rectangle(0, 128, 32, 32);
            WorldTileSheet shotTileSheet = new WorldTileSheet(spriteSheet, shotRect, _cam, 2);
            ShotFactory shotFactory = new ShotFactory(shotTileSheet, _cam);
            _shotManager = new ShotManager(shotFactory);
            //_shotManager.OnShotDestroyed += OnShotDestroyed;
            _shotManager.OnShotCollision += OnShotCollision;

            _player.ShotFired += PlayerShotFired;

        }

        private void OnShotCollision(object sender, ShotCollisionEventArgs e)
        {
            if(e.OtherEntity is WallTileEntity)
            {
                if (e.Shot.ShotType == ShotType.Bullet)
                    _effectsManager.AddSparksEffect(e.Shot.Sprite.Location, e.Shot.Velocity);
                else
                    CreateLargeExplosion(e.Shot.Sprite.Location);
            }
        }

        private void CreateLargeExplosion(Vector2 location)
        {
            // TODO: Do we need a method for this?
            _effectsManager.AddLargeExplosion(location + new Vector2(-10, -10));
            _effectsManager.AddLargeExplosion(location + new Vector2(-10, +10));
            _effectsManager.AddLargeExplosion(location + new Vector2(+10, +10));
            _effectsManager.AddLargeExplosion(location + new Vector2(+10, -10));
            _effectsManager.AddLargeExplosion(location);
        }

        private void PlayerShotFired(object sender, ShotFiredEventArgs e)
        {
            _shotManager.AddShot(e);
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _shotManager.Update(gameTime);
            _effectsManager.Update(gameTime);
            _collisionEngine.Update(gameTime);
            _cam.Move(_player.GetCameraRepositionLocation(gameTime));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _tileMap.Draw(_cam, gameTime, spriteBatch);
            _shotManager.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            _effectsManager.Draw(gameTime, spriteBatch);
        }

        
    }
}

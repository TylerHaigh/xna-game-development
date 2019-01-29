using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private TileMap _tileMap = new TileMap();
        private Player _player;

        public GameWorld(Game game, Camera cam) : base(game)
        {
            _cam = cam;
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

            _player = new Player(playerBase, playerTurret)
            {
                Location = playerLocation
            };

        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _tileMap.Draw(_cam, gameTime, spriteBatch);

            _player.Draw(gameTime, spriteBatch);
        }

        
    }
}

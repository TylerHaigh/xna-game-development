using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework.Entities;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Screen;
using RobotRampage.Graphics;
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

        private WorldSprite _sp1;
        private WorldSprite _sp2;

        private TileMap _tileMap = new TileMap();

        public GameWorld(Game game, Camera cam) : base(game)
        {
            _cam = cam;
        }

        public override void LoadContent(TextureManager textureManager)
        {
            Texture2D spriteSheet = textureManager.OptionalLoadContent<Texture2D>(@"Textures\SpriteSheet");
            _sp1 = new WorldSprite(spriteSheet, new Rectangle(0,  64, 32, 32), new Vector2(100, 100), _cam);
            _sp2 = new WorldSprite(spriteSheet, new Rectangle(0, 160, 32, 32), new Vector2(200, 200), _cam);

            _tileMap.Intialise(spriteSheet);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 spriteMove = Vector2.Zero;
            Vector2 cameraMove = Vector2.Zero;

            KeyboardState kdb = Keyboard.GetState();

            if (kdb.IsKeyDown(Keys.A)) spriteMove.X = -1;
            if (kdb.IsKeyDown(Keys.D)) spriteMove.X =  1;
            if (kdb.IsKeyDown(Keys.W)) spriteMove.Y = -1;
            if (kdb.IsKeyDown(Keys.S)) spriteMove.Y =  1;

            if (kdb.IsKeyDown(Keys.Left))  cameraMove.X = -1;
            if (kdb.IsKeyDown(Keys.Right)) cameraMove.X =  1;
            if (kdb.IsKeyDown(Keys.Up))    cameraMove.Y = -1;
            if (kdb.IsKeyDown(Keys.Down))  cameraMove.Y =  1;

            _cam.Move(cameraMove);

            Vector2 velocity = (spriteMove * 60);
            _sp1.WorldLocation += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _sp1.Update(gameTime);
            _sp2.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _tileMap.Draw(_cam, spriteBatch);

            _sp1.Draw(gameTime, spriteBatch);
            _sp2.Draw(gameTime, spriteBatch);
        }

        
    }
}

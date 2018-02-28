using FloodControl.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl.Screens
{
    interface IGameScreen
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void LoadContent(TextureManager textureManager);
    }

    abstract class GameScreen : IGameScreen
    {
        protected Game _game;
        public GameScreen(Game game) { _game = game; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void LoadContent(TextureManager textureManager);
        public abstract void Update(GameTime gameTime);

        public Rectangle ClientBounds => _game.Window.ClientBounds;
        public Rectangle ScreenBounds => new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height);
    }
}

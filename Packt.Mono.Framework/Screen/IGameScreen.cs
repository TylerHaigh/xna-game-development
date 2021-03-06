﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Screen
{
    //public interface IGameScreen : IGameEntity
    //{
    //    void LoadContent(TextureManager textureManager);
    //}

    public abstract class GameScreen
    {
        protected Game _game;
        public GameScreen(Game game) { _game = game; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void LoadContent(TextureManager textureManager);
        public abstract void Update(GameTime gameTime);

        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        public Rectangle ClientBounds => _game.Window.ClientBounds;
        public Rectangle ScreenBounds => new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloodControl.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;

namespace FloodControl.Screens
{
    class TitleScreen : GameScreen
    {

        private const string TitleScreenTextureKey = @"Textures/TitleScreen";

        public event EventHandler StartNewGame;


        private Texture2D _titleScreen;

        public TitleScreen(Game game) : base(game) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_titleScreen, ScreenBounds, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                StartNewGame?.Invoke(this, null);
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _titleScreen = textureManager.OptionalLoadContent<Texture2D>(TitleScreenTextureKey);
        }
    }
}

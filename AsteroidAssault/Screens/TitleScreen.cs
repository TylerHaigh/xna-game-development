using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
using Packt.Mono.Framework.Screen;
using Packt.Mono.Framework.Utilities;
using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Screens
{
    class TitleScreen : GameScreen
    {

        private Texture2D _titleScreen;
        private SpriteFont _perecles14;

        private GameTimer _timer = new GameTimer(TimeSpan.FromSeconds(1));

        public event EventHandler StartGame;


        public TitleScreen(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_titleScreen, new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height), Color.White);
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _titleScreen = textureManager.OptionalLoadContent<Texture2D>(@"Textures\TitleScreen");
            _perecles14 = textureManager.OptionalLoadContent<SpriteFont>(@"Fonts\Perecles14");
        }

        public override void OnEnter()
        {
            _timer.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            _timer.Update(gameTime);

            // if the player is holding down the space bar as soon as the title screen shows
            // after a game over, they won't see the title screen. put in a small delay to prevent that
            if (!_timer.Completed) return;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                StartGame?.Invoke(this, null);
            }

        }

    }
}

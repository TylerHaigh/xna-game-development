using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework;
using Packt.Mono.Framework.Graphics;
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
        }

        public override void Update(GameTime gameTime)
        {
        }

    }
}

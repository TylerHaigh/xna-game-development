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
        private Texture2D _starFieldTexture;

        private StarField _starField;
        private const int StarCount = 200;
        private Vector2 _starVelocity = new Vector2(0, 30);
        private Rectangle _starTextureSourceRectangle = new Rectangle(0, 450, 2, 2);

        public PlayingScreen(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _starField.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _starFieldTexture = textureManager.OptionalLoadContent<Texture2D>(@"Textures/SpriteSheet");

            _starField = new StarField(ClientBounds.Width, ClientBounds.Height, StarCount, _starVelocity, _starFieldTexture, _starTextureSourceRectangle);
        }

        public override void Update(GameTime gameTime)
        {
            _starField.Update(gameTime);
        }
    }
}

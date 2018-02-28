using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloodControl.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FloodControl.Screens
{
    class GameOver : GameScreen
    {


        private Texture2D _titleScreen;
        private SpriteFont _pericles36Font;

        private Vector2 _gameOverLocation = new Vector2(200, 260);
        float gameOverTimer;
        private const int GameOverWaitTime = 4;


        public event EventHandler GameOverDelayComplete;

        public GameOver(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_titleScreen, ScreenBounds, Color.White);
            spriteBatch.DrawString(_pericles36Font, "G A M E   O V E R!", _gameOverLocation, Color.Yellow);
        }

        public override void LoadContent(TextureManager textureManager)
        {
            _titleScreen = textureManager.OptionalLoadContent<Texture2D>(@"Textures/TitleScreen");
            _pericles36Font = textureManager.OptionalLoadContent<SpriteFont>(@"Fonts/Pericles36");
        }

        public override void Update(GameTime gameTime)
        {
            gameOverTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameOverTimer <= 0)
                GameOverDelayComplete?.Invoke(this, null);
        }
    }
}

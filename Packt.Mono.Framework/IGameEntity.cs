using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework
{
    public interface IGameEntity
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface IMovableGameEntity : IGameEntity
    {
        Vector2 Location { get; set; }
        Vector2 Velocity { get; set; }
    }

    public abstract class GameEntity : IGameEntity
    {
        protected List<IComponent> Components = new List<IComponent>();

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }

    public interface IComponent
    {
        void Update(GameTime gameTime);
        void AttachToEntity(IGameEntity entity);
        void DetachFromEntity();
        bool IsActive { get; set; }
    }
}

using Microsoft.Xna.Framework;
using Packt.Mono.Framework.Entities;

namespace Packt.Mono.Framework.Components
{
    public abstract class Component
    {
        protected GameEntity Entity;
        public Component(GameEntity entity) { Entity = entity; }

        public bool IsActive { get; set; }

        public abstract void Update(GameTime gameTime);

        public virtual void DetachFromEntity() => Entity = null;

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Components;
using System;
using System.Collections.Generic;

namespace Packt.Mono.Framework.Entities
{
    public abstract class GameEntity
    {
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public GameEntity ParentEntity { get; set; }

        public event EventHandler OnDestroy;

        public List<Component> Components = new List<Component>();
        public List<GameEntity> ChildEntities = new List<GameEntity>();

        public void DestroyEntity()
        {
            OnDestroy?.Invoke(this, null);

            // remove all components and child entities
            Components.ForEach(c => c.DetachFromEntity());
            ChildEntities.ForEach(c => c.DestroyEntity());

            ParentEntity = null;
        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (var c in Components) { c.Update(gameTime); }
            foreach (var c in ChildEntities) { c.Update(gameTime); }

            Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Components;
using Packt.Mono.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Packt.Mono.Framework.Entities
{
    public interface IEntityManager
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Clear();
    }

    //public class EntityDestroyedEventArgs
    //{
    //    public readonly GameEntity Entity;
    //    public EntityDestroyedEventArgs(GameEntity entity) { Entity = entity; }
    //}

    public abstract class GameEntity
    {
        private Vector2 _location = Vector2.Zero;
        public Vector2 Location { get => _location; set { _location = value; Sprite.Location = value; } }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public GameEntity ParentEntity { get; set; }

        public event EventHandler OnDestroy;

        public List<Component> Components = new List<Component>();
        public List<GameEntity> ChildEntities = new List<GameEntity>();

        public Sprite Sprite { get; set; }

        public void DestroyEntity()
        {
            // Must come before anything else to notify collision engine
            // of which components to remove from collision engine
            OnDestroy?.Invoke(this, null);

            // remove all components and child entities
            Components.ForEach(c => c.DetachFromEntity());
            ChildEntities.ForEach(c => c.DestroyEntity());

            ParentEntity = null;
        }

        public virtual void Update(GameTime gameTime)
        {
            // has to come before component update because components will
            // reference Location derived variabled (e.g. center) in their update routine
            Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var c in Components) { c.Update(gameTime); }
            foreach (var c in ChildEntities) { c.Update(gameTime); }

            Sprite.Update(gameTime);
        }

        public IEnumerable<Component> GetComponent<T>() where T : Component
        {
            return Components.OfType<T>();
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

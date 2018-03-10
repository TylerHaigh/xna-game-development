using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packt.Mono.Framework.Graphics;

namespace Packt.Mono.Framework.Screen
{
    public class GameScreenState<T>
    {
        public class EmptyGameScreen : GameScreen
        {
            public EmptyGameScreen(Game game) : base(game) { }
            public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
            public override void LoadContent(TextureManager textureManager) { }
            public override void Update(GameTime gameTime) { }
        }



        public T CurrentState { get; private set; }
        public GameScreen CurrentScreen { get; private set; }

        private GameScreenMap<T> _map;

        public GameScreenState(GameScreenMap<T> map, T initialState)
        {
            _map = map;

            CurrentScreen = new EmptyGameScreen(null); // Only for initialisation purposes
            SetState(initialState);
        }


        public void SetState(T newState)
        {
            CheckStateRegistered(newState);

            // TODO: Implement State transition check if required

            UpdateState(newState);
            TransitionState(newState);
        }

        private void CheckStateRegistered(T state)
        {
            if (!_map.ContainsKey(state))
                throw new ArgumentException(string.Format("Invalid State '{0}'. State not registered", state), "newState");
        }

        private void UpdateState(T newState)
        {
            CurrentState = newState;
        }

        private void TransitionState(T newState)
        {
            CurrentScreen.OnExit();
            CurrentScreen = _map[newState];
            CurrentScreen.OnEnter();
        }

        public IEnumerable<GameScreen> AllScreens() => _map.AllScreens();

    }

    public class GameScreenMap<T>
    {
        private Dictionary<T, GameScreen> _screenMap = new Dictionary<T, GameScreen>();

        public GameScreenMap<T> AddState(T state, GameScreen screen)
        {
            _screenMap[state] = screen;
            return this;
        }

        public GameScreen this[T key] => _screenMap[key];
        public bool ContainsKey(T key) => _screenMap.ContainsKey(key);

        public IEnumerable<GameScreen> AllScreens() => _screenMap.Values;
    }
}

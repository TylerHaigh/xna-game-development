using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Utilities
{
    public class KeyboardStateConverter
    {
        public KeyboardState State { get; set; }

        public KeyboardStateConverter(KeyboardState state)
        {
            State = state;
        }

        public Vector2 VectorFromWASDDirections() => VectorFromWASDDirections(State);
        public Vector2 VectorFromNumpadDirections() => VectorFromWASDDirections(State);

        public static Vector2 VectorFromWASDDirections(KeyboardState state)
        {
            Vector2 movementVector = Vector2.Zero;

            if (state.IsKeyDown(Keys.A)) movementVector.X--;
            if (state.IsKeyDown(Keys.D)) movementVector.X++;

            if (state.IsKeyDown(Keys.W)) movementVector.Y--;
            if (state.IsKeyDown(Keys.S)) movementVector.Y++;

            return movementVector;
        }

        public static Vector2 VectorFromNumpadDirections(KeyboardState state)
        {
            Vector2 movementVector = Vector2.Zero;

            if (state.IsKeyDown(Keys.NumPad1)) movementVector += new Vector2(-1, 1);
            if (state.IsKeyDown(Keys.NumPad2)) movementVector += new Vector2(0, 1);
            if (state.IsKeyDown(Keys.NumPad3)) movementVector += new Vector2(1, 1);

            if (state.IsKeyDown(Keys.NumPad4)) movementVector += new Vector2(-1, 0);
            if (state.IsKeyDown(Keys.NumPad6)) movementVector += new Vector2(1, 0);

            if (state.IsKeyDown(Keys.NumPad7)) movementVector += new Vector2(-1, -1);
            if (state.IsKeyDown(Keys.NumPad8)) movementVector += new Vector2(0, -1);
            if (state.IsKeyDown(Keys.NumPad9)) movementVector += new Vector2(1, -1);

            return movementVector;
        }
    }



    public class GamePadStateConverter
    {

        public GamePadState State { get; set; }

        public GamePadStateConverter(GamePadState state)
        {
            State = state;
        }

        public Vector2 VectorFromLeftThumbStick() => VectorFromLeftThumbStick(State);
        public Vector2 VectorFromRightThumbStick() => VectorFromRightThumbStick(State);

        public static Vector2 VectorFromLeftThumbStick(GamePadState state)
        {
            return new Vector2(
                state.ThumbSticks.Left.X,
                -state.ThumbSticks.Left.Y
            );
        }

        public static Vector2 VectorFromRightThumbStick(GamePadState state)
        {
            return new Vector2(
                state.ThumbSticks.Right.X,
                -state.ThumbSticks.Right.Y
            );
        }

        
    }
}

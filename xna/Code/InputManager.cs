using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Board_Game.Code
{
    class InputManager
    {
        KeyboardState previousState;
        KeyboardState currentState;

        private static InputManager mInstance;

        public static void Initialize() { mInstance = new InputManager(); }
        public static InputManager Get() { return mInstance; }

        private InputManager()
        {
            currentState = Keyboard.GetState();
        }

        public void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
        }

        public bool isTriggered(Keys key)
        {
            return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }
    }
}

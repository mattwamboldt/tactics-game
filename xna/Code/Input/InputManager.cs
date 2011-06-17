using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Board_Game.Input;

namespace Board_Game.Input
{
    class InputManager
    {
        InputState previousState;
        InputState currentState;
        InputMap mInputMap;

        private static InputManager mInstance;

        public static void Initialize() { mInstance = new InputManager(); }
        public static InputManager Get() { return mInstance; }

        private InputManager()
        {
            mInputMap = new InputMap();
            currentState = mInputMap.GetInGameState(Keyboard.GetState());
        }

        public void Update()
        {
            previousState = currentState;
            currentState = mInputMap.GetInGameState(Keyboard.GetState());
        }

        public bool isTriggered(Button button)
        {
            return previousState.IsButtonUp(button) && currentState.IsButtonDown(button);
        }
    }
}

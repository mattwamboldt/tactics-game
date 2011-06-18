using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Board_Game.Input;
using Microsoft.Xna.Framework;

namespace Board_Game.Input
{
    class InputManager
    {
        InputState previousState;
        InputState currentState;
        InputMap mInputMap;

        private static InputManager mInstance;

        public static void Initialize(InputMap map) { mInstance = new InputManager(map); }
        public static InputManager Get() { return mInstance; }

        private InputManager(InputMap map)
        {
            mInputMap = map;
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            if (padState.IsConnected)
            {
                currentState = mInputMap.GetInGameState(padState);
            }
            else
            {
                currentState = mInputMap.GetInGameState(Keyboard.GetState());
            }
        }

        public void Update()
        {
            previousState = currentState;
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            if (padState.IsConnected)
            {
                currentState = mInputMap.GetInGameState(padState);
            }
            else
            {
                currentState = mInputMap.GetInGameState(Keyboard.GetState());
            }
        }

        public bool isTriggered(Button button)
        {
            return previousState.IsButtonUp(button) && currentState.IsButtonDown(button);
        }
    }
}

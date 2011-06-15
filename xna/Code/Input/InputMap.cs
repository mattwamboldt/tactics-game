using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Board_Game.Code.Input
{
    class InputMap
    {
        private Keys[] mKeyboardMapping;
        private Buttons[] mGamepadMapping;

        public InputMap()
        {
            //eventually these maps will be loaded from the
            //content pipeline, for now put in a manual steup
            mKeyboardMapping = new Keys[(int)Button.NumButtons] {
                Keys.Up,
                Keys.Down,
                Keys.Left,
                Keys.Right,
                Keys.Space,
                Keys.LeftShift,
                Keys.S,
                Keys.A,
                Keys.W,
                Keys.D,
                Keys.Q,
                Keys.Z,
                Keys.T,
                Keys.E,
                Keys.C,
                Keys.Y,
                Keys.Escape
            };

            mGamepadMapping = new Buttons[(int)Button.NumButtons] {
                Buttons.DPadUp,
                Buttons.DPadDown,
                Buttons.DPadLeft,
                Buttons.DPadRight,
                Buttons.Start,
                Buttons.Back,
                Buttons.A,
                Buttons.X,
                Buttons.Y,
                Buttons.B,
                Buttons.LeftShoulder,
                Buttons.LeftTrigger,
                Buttons.LeftStick,
                Buttons.RightShoulder,
                Buttons.RightTrigger,
                Buttons.RightStick,
                Buttons.BigButton
            };
        }

        public InputState GetInGameState(GamePadState pad)
        {
            InputState inGame = new InputState();

            for (int i = 0; i < (int)Button.NumButtons; i++ )
            {
                inGame.States[i] = pad.IsButtonDown(mGamepadMapping[i]);
            }

            return inGame;
        }

        public InputState GetInGameState(KeyboardState keyboard)
        {
            InputState inGame = new InputState();

            for (int i = 0; i < (int)Button.NumButtons; i++)
            {
                inGame.States[i] = keyboard.IsKeyDown(mKeyboardMapping[i]);
            }

            return inGame;
        }
    }
}

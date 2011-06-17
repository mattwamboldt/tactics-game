using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Board_Game.Input
{
    public class InputMap
    {
        private Keys[] mKeyboardMapping;
        public Keys[] KeyboardMap
        {
            get { return mKeyboardMapping; }
            set { mKeyboardMapping = value; }
        }

        private Buttons[] mGamepadMapping;
        public Buttons[] PadMap
        {
            get { return mGamepadMapping; }
            set { mGamepadMapping = value; }
        }

        public InputMap(){}

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

    public class InputMapReader : ContentTypeReader<InputMap>
    {
        protected override InputMap Read(
            ContentReader input,
            InputMap existingInstance)
        {
            InputMap map = new InputMap();
            map.KeyboardMap = input.ReadObject<Keys[]>();
            map.PadMap = input.ReadObject<Buttons[]>();
            return map;
        }
    }
}

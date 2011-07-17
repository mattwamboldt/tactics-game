using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Board_Game.Input
{
    //This is the list of in game supported inputs
    //It matches the PS3 controller for a) funniness
    //and b) I can't remember the face buttons on anything else
    public enum Button
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        Start = 4,
        Select = 5,
        Cross = 6,
        Square = 7,
        Triangle = 8,
        Circle = 9,
        L1 = 10,
        L2 = 11,
        L3 = 12,
        R1 = 13,
        R2 = 14,
        R3 = 15,
        Home = 16,
        NumButtons = 17
    }

    public class InputState 
    {
        private bool[] mStates;

        public bool[] States { get { return mStates; } }

        public InputState()
        {
            mStates = new bool[(int)Button.NumButtons];
        }

        public bool IsButtonUp(Button button)
        {
            return !mStates[(int)button];
        }

        public bool IsButtonDown(Button button)
        {
            return mStates[(int)button];
        }
    }
}

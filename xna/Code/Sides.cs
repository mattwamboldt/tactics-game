using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Board_Game.Code
{
    //This contains functions for changing the allegiance of
    //something, be it a unit or mine. The squares use their own version
    //but same member variable
    class Sides
    {
        public int mColour;

        public Sides()
        {
            mColour = Constants.NEUTRAL;
        }

        public void TurnBlue()
        {
            mColour = Constants.BLUE;
        }

        public void TurnNeutral()
        {
            mColour = Constants.NEUTRAL;
        }

        public void TurnRed()
        {
            mColour = Constants.RED;
        }

        public void ChangeColour(int colour)
        {
            mColour = colour;
        }
    }
}

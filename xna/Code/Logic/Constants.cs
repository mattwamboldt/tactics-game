using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Board_Game.Code
{
    class Constants
    {
        public const int RED = 0;
        public const int BLUE = 1;
        public const int NEUTRAL = 2;

        //NEW TO C# VERSION
        //I have to add this Type enum since there was no "class" for it in flash
        //Eventually this will replace the below constants once I figure stuff out
        public enum UnitType
        {
            Undefined = -1,
            Bomber = 0,
            Fighter = 1,
            Soldier = 2,
            Granadier = 3,
            Miner = 4
        }

        public enum Side
        {
            Red = 0,
            Blue = 1,
            Neutral = 2
        }


        public const int BOMBER = 0;
        public const int FIGHTER = 1;
        public const int SOLDIER = 2;
        public const int GRANADIER = 3;
        public const int MINER = 4;
        public const int NUM_UNIT_TYPES = 5;

        public const int GRID_WIDTH = 12;
        public const int GRID_HEIGHT = 12;
    }
}

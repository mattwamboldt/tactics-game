using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Board_Game.Util
{
    class Rounding
    {
        public static int FloorAtMinimum(int numberToLimit, int minimumAmount)
        {
            if (numberToLimit < minimumAmount)
            {
                return minimumAmount;
            }

            return numberToLimit;
        }

        public static int CapAtMaximum(int numberToLimit, int maximumAmount)
        {
            if (numberToLimit > maximumAmount)
            {
                return maximumAmount;
            }

            return numberToLimit;
        }

        public static int MakeEven(int numberToEvenize)
        {
            if (numberToEvenize % 2 != 0)
            {
                return numberToEvenize - 1;
            }

            return numberToEvenize;
        }
    }
}

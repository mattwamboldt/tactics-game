using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Units
{
    class Bomber : AirUnit
    {
        public Bomber(GameGrid grid, AI AIRef, UnitDescription unitDesc)
            : base(grid, AIRef, unitDesc)
        {
        }

        public bool CanDestroy(int i, int j)
        {
            return grid.mTiles[i, j].side != side
                && (!grid.mTiles[i, j].Occupied
                || !grid.mTiles[i, j].occupiedUnit.mUnitDesc.CanFly);
        }

        public override bool CheckColour(int i, int j)
        {
	        //Bombers destroy a block of units if one of they're own aren't on it and also can't destroy fighters
            return CanDestroy(i, j) && CanDestroy(i + 1, j) && CanDestroy(i, j + 1) && CanDestroy(i + 1, j + 1);
        }
    }
}

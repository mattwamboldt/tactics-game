using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class Bomber : AirCreature
    {
        public Bomber(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public bool CanDestroy(int i, int j)
        {
            return grid.mTiles[i, j].side != side
                && (!grid.mTiles[i, j].Occupied
                || !grid.mTiles[i, j].occupiedCreature.mCreatureDesc.CanFly);
        }

        public override bool CheckColour(int i, int j)
        {
	        //Bombers destroy a block of Creatures if one of they're own aren't on it and also can't destroy fighters
            return CanDestroy(i, j) && CanDestroy(i + 1, j) && CanDestroy(i, j + 1) && CanDestroy(i + 1, j + 1);
        }
    }
}

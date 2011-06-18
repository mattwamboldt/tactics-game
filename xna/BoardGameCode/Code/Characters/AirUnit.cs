using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class AirCreature : Creature
    {
        public AirCreature(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public override void RemoveCreatures(int newLocationI, int newLocationJ)
        {
	        for(var i = 0; i < 2; ++i)
	        {
		        for(var j = 0; j < 2; ++j)
		        {
                    if (grid.mTiles[newLocationI + i, newLocationJ + j].Occupied)
			        {
				        mAIRef.State.RemoveCreature(grid.mTiles[newLocationI + i, newLocationJ + j].occupiedCreature);
			        }
		        }
	        }
        }
    }
}

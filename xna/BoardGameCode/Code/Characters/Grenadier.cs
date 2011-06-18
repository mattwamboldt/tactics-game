using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class Grenadier : GroundCreature
    {
        public Grenadier(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public override bool CheckColour(int i, int j)
        {
	        return grid.mTiles[i, j].side != side;
        }

        public override void RemoveCreatures(int newLocationI, int newLocationJ)
        {
	        var Creature = grid.mTiles[newLocationI, newLocationJ].occupiedCreature;
            int CreatureLocationI = (int)((Creature.position.Y - Creature.position.Y % Creature.ScreenDimensions().Y) / Tile.TILE_SIZE);
            int CreatureLocationJ = (int)((Creature.position.X - Creature.position.X % Creature.ScreenDimensions().X) / Tile.TILE_SIZE);

            if (Creature.mCreatureDesc.CanFly)
	        {
		        for(var i = 0; i < 2; ++i)
		        {
			        for(var j = 0; j < 2; ++j)
			        {
                        grid.mTiles[CreatureLocationI + i, CreatureLocationJ + j].side = Side.Neutral;
                        grid.mTiles[CreatureLocationI + i, CreatureLocationJ + j].occupiedCreature = null;
			        }
		        }
	        }

	        mAIRef.State.RemoveCreature(Creature);
        }
    }
}

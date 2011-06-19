using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class Grenadier : Creature
    {
        public Grenadier(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public override void RemoveCreatures(int newX, int newY)
        {
	        var Creature = grid.mTiles[newX, newY].occupiedCreature;
            int CreatureX = (int)((Creature.position.X - Creature.position.X % Creature.ScreenDimensions().X) / Tile.TILE_SIZE);
            int CreatureY = (int)((Creature.position.Y - Creature.position.Y % Creature.ScreenDimensions().Y) / Tile.TILE_SIZE);
            
            if (Creature.mCreatureDesc.CanFly)
	        {
		        for(var x = 0; x < 2; ++x)
		        {
			        for(var y = 0; y < 2; ++y)
			        {
                        grid.mTiles[CreatureX + x, CreatureY + y].side = Side.Neutral;
                        grid.mTiles[CreatureX + x, CreatureY + y].occupiedCreature = null;
			        }
		        }
	        }

	        mAIRef.State.RemoveCreature(Creature);
        }
    }
}

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

        public override void SetLocation(int newLocationI, int newLocationJ)
        {
	        for(var i = 0; i < 2; ++i)
	        {
		        for(var j = 0; j < 2; ++j)
		        {
                    grid.mTiles[newLocationI + i, newLocationJ + j].side = side;
                    grid.mTiles[newLocationI + i, newLocationJ + j].occupiedCreature = this;
		        }
	        }

            position.X = newLocationJ * Tile.TILE_SIZE;
            position.Y = newLocationI * Tile.TILE_SIZE;
        }

        public override void Move(int newLocationI, int newLocationJ)
        {
	        for(var i = 0; i < 2; ++i)
	        {
		        for(var j = 0; j < 2; ++j)
		        {
			        grid.mTiles[originalI + i, originalJ + j].side = Side.Neutral;
			        grid.mTiles[originalI + i, originalJ + j].occupiedCreature = null;
		        }
	        }
        	
	        SetLocation(newLocationI, newLocationJ);
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

        //flying Creatures ignore mines
        public override bool IsEnemyMine(int i, int j)
        {
            return false;
        }
    }
}

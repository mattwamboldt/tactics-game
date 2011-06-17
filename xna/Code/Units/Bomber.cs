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
        public Bomber(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = UnitType.Bomber;
            attackablePriorities = new UnitType[] {
                UnitType.Granadier,
                UnitType.Miner,
                UnitType.Soldier
            };
        }

        public override bool CanAttack(UnitType unitType)
        {
	        switch(unitType)
	        {
                case UnitType.Soldier:
                case UnitType.Granadier:
                case UnitType.Miner:
			        return true;
		        default:
			        return false;
	        }
        }

        public bool CanDestroy(int i, int j)
        {
            return grid.mTiles[i, j].side != side
                && (!grid.mTiles[i, j].Occupied
                || !grid.mTiles[i, j].occupiedUnit.CanFly);
        }

        public override bool CheckColour(int i, int j)
        {
	        //Bombers destroy a block of units if one of they're own aren't on it and also can't destroy fighters
            return CanDestroy(i, j) && CanDestroy(i + 1, j) && CanDestroy(i, j + 1) && CanDestroy(i + 1, j + 1);
        }
    }
}

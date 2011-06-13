using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Units
{
    class Bomber : AirUnit
    {
        public Bomber(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = Constants.UnitType.Bomber;
            attackablePriorities = new Constants.UnitType[] {
                Constants.UnitType.Granadier,
                Constants.UnitType.Miner,
                Constants.UnitType.Soldier
            };
        }

        public override bool CanAttack(Constants.UnitType unitType)
        {
	        switch(unitType)
	        {
                case Constants.UnitType.Soldier:
                case Constants.UnitType.Granadier:
                case Constants.UnitType.Miner:
			        return true;
		        default:
			        return false;
	        }
        }

        public bool CanDestroy(int i, int j)
        {
            return grid.mTiles[i, j].side.mColour != side.mColour
                && (grid.mTiles[i, j].occupiedUnit == null
                || !grid.mTiles[i, j].occupiedUnit.CanFly);
        }

        public override bool CheckColour(int i, int j)
        {
	        //Bombers destroy a block of units if one of they're own aren't on it and also can't destroy fighters
            return CanDestroy(i, j) && CanDestroy(i + 1, j) && CanDestroy(i, j + 1) && CanDestroy(i + 1, j + 1);
        }
    }
}

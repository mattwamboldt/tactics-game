using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Units
{
    class Fighter : AirUnit
    {
        public Fighter(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = Constants.UnitType.Fighter;
            attackablePriorities = new Constants.UnitType[] {
                Constants.UnitType.Bomber,
                Constants.UnitType.Miner
            };
        }

        public override bool CheckColour(int i, int j)
        {
            return grid.mTiles[i, j].side.mColour != side.mColour
                && grid.mTiles[i, j].occupiedUnit != null
                && grid.mTiles[i, j].occupiedUnit.CanFly;
        }

        public override bool CanAttack(Constants.UnitType unitType)
        {
	        switch(unitType)
	        {
                case Constants.UnitType.Bomber:
                case Constants.UnitType.Fighter:
			        return true;
		        default:
			        return false;
	        }
        }
    }
}

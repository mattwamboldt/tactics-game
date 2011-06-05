using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Units
{
    class Soldier : GroundUnit
    {
        public Soldier(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = Constants.UnitType.Soldier;
            attackablePriorities = new Constants.UnitType[] {
                Constants.UnitType.Granadier,
                Constants.UnitType.Miner,
                Constants.UnitType.Soldier
            };
        }

        public override bool CheckColour(int i, int j)
        {
	        return grid.mTiles[i, j].side.mColour != side.mColour
                && grid.mTiles[i,j].occupiedUnit != null
                && !grid.mTiles[i,j].occupiedUnit.CanFly;
        }

        public override void RemoveUnits(int newLocationI, int newLocationJ)
        {
            mAIRef.RemoveUnit(grid.mTiles[newLocationI, newLocationJ].occupiedUnit);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Units
{
    class Soldier : GroundUnit
    {
        public Soldier(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = UnitType.Soldier;
            attackablePriorities = new UnitType[] {
                UnitType.Granadier,
                UnitType.Miner,
                UnitType.Soldier
            };
        }

        public override bool CheckColour(int i, int j)
        {
	        return grid.mTiles[i, j].side != side
                && grid.mTiles[i, j].Occupied
                && !grid.mTiles[i,j].occupiedUnit.CanFly;
        }

        public override void RemoveUnits(int newLocationI, int newLocationJ)
        {
            mAIRef.State.RemoveUnit(grid.mTiles[newLocationI, newLocationJ].occupiedUnit);
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
    }
}

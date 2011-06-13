using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Units
{
    class Deminer : GroundUnit
    {
        public Deminer(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = Constants.UnitType.Miner;
            attackablePriorities = null;
        }

        public override bool CheckColour(int i, int j)
        {
            return grid.mTiles[i, j].side.mColour != side.mColour && !grid.mTiles[i, j].occupied;
        }

        public override void RemoveUnits(int newLocationI, int newLocationJ)
        {
            mAIRef.RemoveUnit(grid.mTiles[newLocationI, newLocationJ].occupiedUnit);
        }

        public override bool CanAttack(Constants.UnitType unitType)
        {
	        return false;
        }

        //sides don't matter to miners since they convert
        public override bool IsEnemyMine(int i, int j)
        {
            return false;
        }

        public override bool IsFriendlyMine(int i, int j)
        {
            return false;
        }

    }
}

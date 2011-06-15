using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Code.Logic;

namespace Board_Game.Code.Units
{
    class Grenadier : GroundUnit
    {
        public Grenadier(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = UnitType.Granadier;
            attackablePriorities = new UnitType[] {
                UnitType.Bomber,
                UnitType.Granadier,
                UnitType.Fighter,
                UnitType.Miner,
                UnitType.Soldier
            };
        }

        public override bool CheckColour(int i, int j)
        {
	        return grid.mTiles[i, j].side != side;
        }

        public override void RemoveUnits(int newLocationI, int newLocationJ)
        {
	        var unit = grid.mTiles[newLocationI, newLocationJ].occupiedUnit;
            int UnitLocationI = (int)((unit.position.Y - unit.position.Y % unit.ScreenDimensions().Y) / Tile.TILE_SIZE);
            int UnitLocationJ = (int)((unit.position.X - unit.position.X % unit.ScreenDimensions().X) / Tile.TILE_SIZE);
        	
	        if(unit.CanFly)
	        {
		        for(var i = 0; i < 2; ++i)
		        {
			        for(var j = 0; j < 2; ++j)
			        {
                        grid.mTiles[UnitLocationI + i, UnitLocationJ + j].side = Side.Neutral;
                        grid.mTiles[UnitLocationI + i, UnitLocationJ + j].occupiedUnit = null;
			        }
		        }
	        }
	        mAIRef.RemoveUnit(unit);
        }

        public override bool CanAttack(UnitType unitType)
        {
	        switch(unitType)
	        {
		        case UnitType.Soldier:
                case UnitType.Granadier:
                case UnitType.Miner:
                case UnitType.Bomber:
                case UnitType.Fighter:
			        return true;
		        default:
			        return false;
	        }
        }
    }
}

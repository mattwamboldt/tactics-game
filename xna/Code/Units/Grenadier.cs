﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Units
{
    class Grenadier : GroundUnit
    {
        public Grenadier(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            Type = Constants.UnitType.Granadier;
            attackablePriorities = new Constants.UnitType[] {
                Constants.UnitType.Bomber,
                Constants.UnitType.Granadier,
                Constants.UnitType.Fighter,
                Constants.UnitType.Miner,
                Constants.UnitType.Soldier
            };
        }

        public override bool CheckColour(int i, int j)
        {
	        return grid.mTiles[i, j].side.mColour != side.mColour;
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
                        grid.mTiles[UnitLocationI + i, UnitLocationJ + j].occupied = false;
                        grid.mTiles[UnitLocationI + i, UnitLocationJ + j].side.TurnNeutral();
                        grid.mTiles[UnitLocationI + i, UnitLocationJ + j].occupiedUnit = null;
			        }
		        }
	        }
	        mAIRef.RemoveUnit(unit);
        }

        public override bool CanAttack(Constants.UnitType unitType)
        {
	        switch(unitType)
	        {
		        case Constants.UnitType.Soldier:
                case Constants.UnitType.Granadier:
                case Constants.UnitType.Miner:
                case Constants.UnitType.Bomber:
                case Constants.UnitType.Fighter:
			        return true;
		        default:
			        return false;
	        }
        }
    }
}

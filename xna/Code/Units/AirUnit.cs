﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Code.Logic;

namespace Board_Game.Code.Units
{
    class AirUnit : Unit
    {
        public AirUnit(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            CanFly = true;
            height = 2;
            width = 2;
        }

        public override void SetLocation(int newLocationI, int newLocationJ)
        {
	        for(var i = 0; i < 2; ++i)
	        {
		        for(var j = 0; j < 2; ++j)
		        {
			        grid.mTiles[newLocationI + i, newLocationJ + j].occupied = true;
                    grid.mTiles[newLocationI + i, newLocationJ + j].side = side;
                    grid.mTiles[newLocationI + i, newLocationJ + j].occupiedUnit = this;
		        }
	        }

            position.X = newLocationJ * Tile.TILE_SIZE;
            position.Y = newLocationI * Tile.TILE_SIZE;
        }

        public override bool CheckOccupied(int i, int j)
        {
            return (grid.mTiles[i, j].occupied || grid.mTiles[i + 1, j].occupied
                    || grid.mTiles[i, j + 1].occupied || grid.mTiles[i + 1, j + 1].occupied);
        }

        public override void Move(int newLocationI, int newLocationJ, bool changeTurns)
        {
	        if(changeTurns == true)
	        {
		        mAIRef.ChangeTurns();
	        }

	        for(var i = 0; i < 2; ++i)
	        {
		        for(var j = 0; j < 2; ++j)
		        {
			        grid.mTiles[originalI + i, originalJ + j].occupied = false;
			        grid.mTiles[originalI + i, originalJ + j].side = Constants.Side.Neutral;
			        grid.mTiles[originalI + i, originalJ + j].occupiedUnit = null;
		        }
	        }
        	
	        SetLocation(newLocationI, newLocationJ);
        }

        public override void RemoveUnits(int newLocationI, int newLocationJ)
        {
	        for(var i = 0; i < 2; ++i)
	        {
		        for(var j = 0; j < 2; ++j)
		        {
			        if(grid.mTiles[newLocationI + i, newLocationJ + j].occupiedUnit != null)
			        {
				        mAIRef.RemoveUnit(grid.mTiles[newLocationI + i, newLocationJ + j].occupiedUnit);
			        }
		        }
	        }
        }

        public override Vector2 ScreenDimensions()
        {
            return new Vector2(Mine.MINE_SIZE, Mine.MINE_SIZE);
        }

        //flying units ignore mines
        public override bool IsEnemyMine(int i, int j)
        {
            return false;
        }
    }
}

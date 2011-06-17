using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;

namespace Board_Game.Units
{
    class GroundUnit : Unit
    {
        public GroundUnit(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            CanFly = false;
            height = 1;
            width = 1;
        }

        public override void Move(int newLocationI, int newLocationJ)
        {
            grid.mTiles[originalI, originalJ].side = Side.Neutral;
            grid.mTiles[originalI, originalJ].occupiedUnit = null;
            SetLocation(newLocationI, newLocationJ);
        }
        
        public override bool CheckOccupied(int i, int j)
        {
            return grid.mTiles[i, j].Occupied;
        }

        public override void SetLocation(int newLocationI, int newLocationJ)
        {
            grid.mTiles[newLocationI, newLocationJ].side = side;
            grid.mTiles[newLocationI, newLocationJ].occupiedUnit = this;

            position.X = newLocationJ * Tile.TILE_SIZE;
            position.Y = newLocationI * Tile.TILE_SIZE;
        }

        public override Vector2 ScreenDimensions()
        {
            return new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE);
        }
    }
}

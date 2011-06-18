using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Units
{
    class Grenadier : GroundUnit
    {
        public Grenadier(GameGrid grid, AI AIRef, UnitDescription unitDesc)
            : base(grid, AIRef, unitDesc)
        {
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

            if (unit.mUnitDesc.CanFly)
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

	        mAIRef.State.RemoveUnit(unit);
        }
    }
}

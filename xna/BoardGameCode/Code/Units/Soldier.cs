﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Units
{
    class Soldier : GroundUnit
    {
        public Soldier(GameGrid grid, AI AIRef, UnitDescription unitDesc)
            : base(grid, AIRef, unitDesc)
        {
        }

        public override bool CheckColour(int i, int j)
        {
	        return grid.mTiles[i, j].side != side
                && grid.mTiles[i, j].Occupied
                && !grid.mTiles[i, j].occupiedUnit.mUnitDesc.CanFly;
        }

        public override void RemoveUnits(int newLocationI, int newLocationJ)
        {
            mAIRef.State.RemoveUnit(grid.mTiles[newLocationI, newLocationJ].occupiedUnit);
        }
    }
}

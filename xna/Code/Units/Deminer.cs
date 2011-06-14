using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        public override Vector2 GetNearestTarget()
        {
            Vector2 originalPoint = new Vector2(GetJ(), GetI());
            Vector2 nearestMine = new Vector2(-1, -1);

            double distanceToNearest = mAIRef.GetDistanceToCoordinates(originalPoint, 0, 0);

            //The outer loop goes through the mines
            for (var i = 0; i < Constants.GRID_WIDTH / 2; ++i)
            {
                for (var j = 0; j < Constants.GRID_HEIGHT / 2; ++j)
                {
                    //only gets us mines that have a value
                    if (i % 2 == j % 2)
                    {
                        //we want to head for mines of the opposite colour
                        if (grid.mMines[i, j].side.mColour != side.mColour)
                        {
                            //inner loops checks the mine itself
                            for (var t = 0; t < 2; ++t)
                            {
                                for (var u = 0; u < 2; ++u)
                                {
                                    var iCoord = i * 2 + t;
                                    var jCoord = j * 2 + u;
                                    var distanceToMineSquare = mAIRef.GetDistanceToCoordinates(originalPoint, iCoord, jCoord);

                                    if (distanceToMineSquare < distanceToNearest
                                       || nearestMine.Y == -1)
                                    {
                                        nearestMine.Y = iCoord;
                                        nearestMine.X = jCoord;
                                        distanceToNearest = distanceToMineSquare;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return nearestMine;
        }

    }
}

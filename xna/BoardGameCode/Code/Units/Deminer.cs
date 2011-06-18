﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class Deminer : GroundCreature
    {
        public Deminer(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public override bool CheckColour(int i, int j)
        {
            return grid.mTiles[i, j].side != side && !grid.mTiles[i, j].Occupied;
        }

        public override void RemoveCreatures(int newLocationI, int newLocationJ)
        {
            mAIRef.State.RemoveCreature(grid.mTiles[newLocationI, newLocationJ].occupiedCreature);
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
            foreach (Mine mine in grid.mMines)
            {
                //we want to head for mines of the opposite colour
                if (mine.side != side)
                {
                    Vector2 mineCorner = mine.position;

                    //inner loops checks the mine itself
                    for (var t = 0; t < 2; ++t)
                    {
                        for (var u = 0; u < 2; ++u)
                        {
                            var iCoord = mineCorner.Y * 2 + t;
                            var jCoord = mineCorner.X * 2 + u;
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

            return nearestMine;
        }

    }
}

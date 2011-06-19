using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class Deminer : Creature
    {
        public Deminer(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public override Vector2 GetNearestTarget()
        {
            Vector2 originalPoint = new Vector2(GetX(), GetY());
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
                            var x = mineCorner.X * 2 + u;
                            var y = mineCorner.Y * 2 + t;
                            var distanceToMineSquare = mAIRef.GetDistanceToCoordinates(originalPoint, x, y);

                            if (distanceToMineSquare < distanceToNearest
                               || nearestMine.Y == -1)
                            {
                                nearestMine.X = x;
                                nearestMine.Y = y;
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

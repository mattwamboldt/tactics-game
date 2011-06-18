using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Logic;

namespace Board_Game.Creatures
{
    class Fighter : Creature
    {
        public Fighter(GameGrid grid, AI AIRef, CreatureDescription CreatureDesc)
            : base(grid, AIRef, CreatureDesc)
        {
        }

        public override bool CheckColour(int i, int j)
        {
            return grid.mTiles[i, j].side != side
                && grid.mTiles[i, j].Occupied
                && grid.mTiles[i, j].occupiedCreature.mCreatureDesc.CanFly;
        }
    }
}

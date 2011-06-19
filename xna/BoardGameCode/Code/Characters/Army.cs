using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Board_Game.Creatures;
using Board_Game.Logic;
using Microsoft.Xna.Framework;

namespace Board_Game.Code.Characters
{
    //This is used to wrap a list of creatures for a given side
    //Allowing us to have different unit layouts loaded at runtime
    class Army
    {
        private Side mSide;
        public Side Side { get { return mSide; } set { mSide = value; } }

        private List<Creature> mMembers;
        public List<Creature> Members { get { return mMembers; } set { mMembers = value; } }

        //This is a dummy function until the file loading goes in
        public void Build(
            CreatureDescription bomberDesc,
            CreatureDescription fighterDesc,
            CreatureDescription minerDesc,
            CreatureDescription grenadierDesc,
            CreatureDescription soldierDesc
            )
        {
            mMembers = new List<Creature>();

            for (int x = 0; x < 6; ++x)
            {
                Creature bomber = new Creature(bomberDesc);
                Creature fighter = new Creature(fighterDesc);

                bomber.side = mSide;
                fighter.side = mSide;

                if (mSide == Side.Red)
                {
                    bomber.GridLocation = new Point((x * 4) + 2, GameState.GRID_HEIGHT - 2);
                    fighter.GridLocation = new Point(x * 4, GameState.GRID_HEIGHT - 2);
                }
                else
                {
                    bomber.GridLocation = new Point(x * 4, 0);
                    fighter.GridLocation = new Point((x * 4) + 2, 0);
                }

                mMembers.Add(bomber);
                mMembers.Add(fighter);
            }

            for (int x = 0; x < 12; ++x)
            {
                Creature miner = new Creature(minerDesc);
                Creature grenadier = new Creature(grenadierDesc);

                miner.side = mSide;
                grenadier.side = mSide;

                if (mSide == Side.Red)
                {
                    miner.GridLocation = new Point(((x % 2) + 1) + (int)(4 * Math.Floor(x / 2.0f)), GameState.GRID_HEIGHT - 3);
                    grenadier.GridLocation = new Point((int)(Math.Floor((x + 1) / 2.0f) * 4 - x % 2), GameState.GRID_HEIGHT - 3);
                }
                else
                {
                    miner.GridLocation = new Point(((x % 2) + 1) + (4 * (int)(Math.Floor(x / 2.0f))), 2);
                    grenadier.GridLocation = new Point((int)(Math.Floor((x + 1) / 2.0f) * 4 - x % 2), 2);
                }

                mMembers.Add(miner);
                mMembers.Add(grenadier);
            }

            for (var x = 0; x < 24; ++x)
            {
                Creature soldier = new Creature(soldierDesc);
                soldier.side = mSide;

                if (mSide == Side.Red)
                {
                    soldier.GridLocation = new Point(x, GameState.GRID_HEIGHT - 4);
                }
                else
                {
                    soldier.GridLocation = new Point(x, 3);
                }

                mMembers.Add(soldier);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Board_Game.Creatures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Logic
{
    /// <summary>
    /// This defines a player, each player has a set of Creatures and a reference
    /// to the board. This pulls Creature creation away from the AI, and makes it possible
    /// for each side to have a different number and types of Creatures. Even the possibility of
    /// differently tuned AI's to face off for testing.
    /// </summary>
    class Player
    {
        public bool mIsHuman;
        public Side mSide;
        private List<Creature> mCreatures;
        private GameGrid mGrid;
        public AI mAI;

        public List<Creature> Creatures { get { return mCreatures; } }

        public Player(bool isHuman, Side side, AI AIref)
        {
            mIsHuman = isHuman;
            mSide = side;
            mAI = AIref;
            mGrid = AIref.mGrid;
        }

        public void CreateCreatures(
            CreatureDescription bomberDesc,
            CreatureDescription fighterDesc,
            CreatureDescription minerDesc,
            CreatureDescription grenadierDesc,
            CreatureDescription soldierDesc
            )
        {
            mCreatures = new List<Creature>(GameState.GRID_WIDTH / 2 + GameState.GRID_WIDTH * 2);

            for( int i = 0; i < GameState.GRID_WIDTH/4; ++i )
            {
                Bomber bomber = new Bomber(mGrid, mAI, bomberDesc);
                Fighter fighter = new Fighter(mGrid, mAI, fighterDesc);

                bomber.side = mSide;
                fighter.side = mSide;

                if (mSide == Side.Red)
                {
                    bomber.SetLocation((GameState.GRID_HEIGHT - 2), (i * 4) + 2);
                    fighter.SetLocation((GameState.GRID_HEIGHT - 2), i * 4);
                }
                else
                {
                    bomber.SetLocation(0, i * 4);
                    fighter.SetLocation(0, (i * 4) + 2);
                }

                mCreatures.Add(bomber);
                mCreatures.Add(fighter);
            }

            for (int i = 0; i < GameState.GRID_WIDTH / 2; ++i)
            {
                Creatures.Deminer miner = new Deminer(mGrid, mAI, minerDesc);
                Creatures.Grenadier grenadier = new Grenadier(mGrid, mAI, grenadierDesc);

                miner.side = mSide;
                grenadier.side = mSide;

                if (mSide == Side.Red)
                {
                    miner.SetLocation((GameState.GRID_HEIGHT - 3), ((i % 2) + 1) + (int)(4 * Math.Floor(i / 2.0f)));
                    grenadier.SetLocation((GameState.GRID_HEIGHT - 3), (int)(Math.Floor((i + 1) / 2.0f) * 4 - i % 2));
                }
                else
                {
                    miner.SetLocation(2, ((i % 2) + 1) + (4 * (int)(Math.Floor(i / 2.0f))));
                    grenadier.SetLocation(2, (int)(Math.Floor((i + 1) / 2.0f) * 4 - i % 2));
                }

                mCreatures.Add(miner);
                mCreatures.Add(grenadier);
            }

            for( var i = 0; i < GameState.GRID_WIDTH; ++i )
            {
                Creatures.Soldier soldier = new Soldier(mGrid, mAI, soldierDesc);
                soldier.side = mSide;

                if (mSide == Side.Red)
                {
                    soldier.SetLocation((GameState.GRID_HEIGHT - 4), i);
                }
                else
                {
                    soldier.SetLocation(3, i);
                }

                mCreatures.Add(soldier);
            }
        }

        public void RemoveCreature(Creatures.Creature Creature)
        {
            mCreatures.Remove(Creature);
            Creature = null;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentLocation)
        {
            foreach (Creature Creature in mCreatures)
            {
                Creature.Render(spriteBatch, parentLocation);
            }
        }
    }
}

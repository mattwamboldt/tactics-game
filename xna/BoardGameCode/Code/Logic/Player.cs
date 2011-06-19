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

            for( int x = 0; x < GameState.GRID_WIDTH/4; ++x )
            {
                Creature bomber = new Creature(mGrid, mAI, bomberDesc);
                Creature fighter = new Creature(mGrid, mAI, fighterDesc);

                bomber.side = mSide;
                fighter.side = mSide;

                if (mSide == Side.Red)
                {
                    bomber.SetLocation((x * 4) + 2, (GameState.GRID_HEIGHT - 2));
                    fighter.SetLocation(x * 4, (GameState.GRID_HEIGHT - 2));
                }
                else
                {
                    bomber.SetLocation(x * 4, 0);
                    fighter.SetLocation((x * 4) + 2, 0);
                }

                mCreatures.Add(bomber);
                mCreatures.Add(fighter);
            }

            for (int x = 0; x < GameState.GRID_WIDTH / 2; ++x)
            {
                Creature miner = new Creature(mGrid, mAI, minerDesc);
                Creature grenadier = new Creature(mGrid, mAI, grenadierDesc);

                miner.side = mSide;
                grenadier.side = mSide;

                if (mSide == Side.Red)
                {
                    miner.SetLocation(((x % 2) + 1) + (int)(4 * Math.Floor(x / 2.0f)), (GameState.GRID_HEIGHT - 3));
                    grenadier.SetLocation((int)(Math.Floor((x + 1) / 2.0f) * 4 - x % 2), (GameState.GRID_HEIGHT - 3));
                }
                else
                {
                    miner.SetLocation(((x % 2) + 1) + (4 * (int)(Math.Floor(x / 2.0f))), 2);
                    grenadier.SetLocation((int)(Math.Floor((x + 1) / 2.0f) * 4 - x % 2), 2);
                }

                mCreatures.Add(miner);
                mCreatures.Add(grenadier);
            }

            for( var x = 0; x < GameState.GRID_WIDTH; ++x )
            {
                Creature soldier = new Creature(mGrid, mAI, soldierDesc);
                soldier.side = mSide;

                if (mSide == Side.Red)
                {
                    soldier.SetLocation(x, (GameState.GRID_HEIGHT - 4));
                }
                else
                {
                    soldier.SetLocation(x, 3);
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

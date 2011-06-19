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
        private GameState mGame;
        private GameGrid mGrid;

        public List<Creature> Creatures { get { return mCreatures; } }

        public Player(bool isHuman, Side side,GameState game)
        {
            mIsHuman = isHuman;
            mSide = side;
            mGame = game;
            mGrid = mGame.mGrid;
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
                Creature bomber = new Creature(mGrid, bomberDesc);
                Creature fighter = new Creature(mGrid, fighterDesc);

                bomber.side = mSide;
                fighter.side = mSide;

                if (mSide == Side.Red)
                {
                    mGame.SetLocation((x * 4) + 2, (GameState.GRID_HEIGHT - 2), bomber);
                    mGame.SetLocation(x * 4, (GameState.GRID_HEIGHT - 2), fighter);
                }
                else
                {
                    mGame.SetLocation(x * 4, 0, bomber);
                    mGame.SetLocation((x * 4) + 2, 0, fighter);
                }

                mCreatures.Add(bomber);
                mCreatures.Add(fighter);
            }

            for (int x = 0; x < GameState.GRID_WIDTH / 2; ++x)
            {
                Creature miner = new Creature(mGrid, minerDesc);
                Creature grenadier = new Creature(mGrid, grenadierDesc);

                miner.side = mSide;
                grenadier.side = mSide;

                if (mSide == Side.Red)
                {
                    mGame.SetLocation(((x % 2) + 1) + (int)(4 * Math.Floor(x / 2.0f)), (GameState.GRID_HEIGHT - 3), miner);
                    mGame.SetLocation((int)(Math.Floor((x + 1) / 2.0f) * 4 - x % 2), (GameState.GRID_HEIGHT - 3), grenadier);
                }
                else
                {
                    mGame.SetLocation(((x % 2) + 1) + (4 * (int)(Math.Floor(x / 2.0f))), 2, miner);
                    mGame.SetLocation((int)(Math.Floor((x + 1) / 2.0f) * 4 - x % 2), 2, grenadier);
                }

                mCreatures.Add(miner);
                mCreatures.Add(grenadier);
            }

            for( var x = 0; x < GameState.GRID_WIDTH; ++x )
            {
                Creature soldier = new Creature(mGrid, soldierDesc);
                soldier.side = mSide;

                if (mSide == Side.Red)
                {
                    mGame.SetLocation(x, (GameState.GRID_HEIGHT - 4), soldier);
                }
                else
                {
                    mGame.SetLocation(x, 3, soldier);
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

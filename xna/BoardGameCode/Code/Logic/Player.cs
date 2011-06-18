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
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture
            )
        {
            //TODO: Move to content pipeline
            CreatureDescription bomberDesc = new CreatureDescription();
            bomberDesc.CanFly = true;
            bomberDesc.height = 2;
            bomberDesc.width = 2;
            bomberDesc.Type = CreatureType.Bomber;
            bomberDesc.attackablePriorities = new CreatureType[] {
                CreatureType.Granadier,
                CreatureType.Miner,
                CreatureType.Soldier
            };

            bomberDesc.texture = bomberTexture;

            CreatureDescription fighterDesc = new CreatureDescription();
            fighterDesc.CanFly = true;
            fighterDesc.height = 2;
            fighterDesc.width = 2;
            fighterDesc.Type = CreatureType.Fighter;
            fighterDesc.attackablePriorities = new CreatureType[] {
                CreatureType.Bomber
            };

            fighterDesc.texture = fighterTexture;

            CreatureDescription minerDesc = new CreatureDescription();
            minerDesc.CanFly = false;
            minerDesc.height = 1;
            minerDesc.width = 1;
            minerDesc.Type = CreatureType.Miner;
            minerDesc.attackablePriorities = null;

            minerDesc.texture = deminerTexture;

            CreatureDescription grenadierDesc = new CreatureDescription();
            grenadierDesc.CanFly = false;
            grenadierDesc.height = 1;
            grenadierDesc.width = 1;
            grenadierDesc.Type = CreatureType.Granadier;
            grenadierDesc.attackablePriorities = new CreatureType[] {
                CreatureType.Bomber,
                CreatureType.Granadier,
                CreatureType.Fighter,
                CreatureType.Miner,
                CreatureType.Soldier
            };

            grenadierDesc.texture = grenadierTexture;

            CreatureDescription soldierDesc = new CreatureDescription();
            soldierDesc.CanFly = false;
            soldierDesc.height = 1;
            soldierDesc.width = 1;
            soldierDesc.Type = CreatureType.Soldier;
            soldierDesc.attackablePriorities = new CreatureType[] {
                CreatureType.Granadier,
                CreatureType.Miner,
                CreatureType.Soldier
            };

            soldierDesc.texture = soldierTexture;

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

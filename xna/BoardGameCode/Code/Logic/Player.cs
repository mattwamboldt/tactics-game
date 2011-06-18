using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Board_Game.Units;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Logic
{
    /// <summary>
    /// This defines a player, each player has a set of units and a reference
    /// to the board. This pulls unit creation away from the AI, and makes it possible
    /// for each side to have a different number and types of units. Even the possibility of
    /// differently tuned AI's to face off for testing.
    /// </summary>
    class Player
    {
        public bool mIsHuman;
        public Side mSide;
        private List<Unit> mUnits;
        private GameGrid mGrid;
        public AI mAI;

        public List<Unit> Units { get { return mUnits; } }

        public Player(bool isHuman, Side side, AI AIref)
        {
            mIsHuman = isHuman;
            mSide = side;
            mAI = AIref;
            mGrid = AIref.mGrid;
        }

        public void CreateUnits(
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture
            )
        {
            //TODO: Move to content pipeline
            UnitDescription bomberDesc = new UnitDescription();
            bomberDesc.CanFly = true;
            bomberDesc.height = 2;
            bomberDesc.width = 2;
            bomberDesc.Type = UnitType.Bomber;
            bomberDesc.attackablePriorities = new UnitType[] {
                UnitType.Granadier,
                UnitType.Miner,
                UnitType.Soldier
            };

            bomberDesc.texture = bomberTexture;

            UnitDescription fighterDesc = new UnitDescription();
            fighterDesc.CanFly = true;
            fighterDesc.height = 2;
            fighterDesc.width = 2;
            fighterDesc.Type = UnitType.Fighter;
            fighterDesc.attackablePriorities = new UnitType[] {
                UnitType.Bomber
            };

            fighterDesc.texture = fighterTexture;

            UnitDescription minerDesc = new UnitDescription();
            minerDesc.CanFly = false;
            minerDesc.height = 1;
            minerDesc.width = 1;
            minerDesc.Type = UnitType.Miner;
            minerDesc.attackablePriorities = null;

            minerDesc.texture = deminerTexture;

            UnitDescription grenadierDesc = new UnitDescription();
            grenadierDesc.CanFly = false;
            grenadierDesc.height = 1;
            grenadierDesc.width = 1;
            grenadierDesc.Type = UnitType.Granadier;
            grenadierDesc.attackablePriorities = new UnitType[] {
                UnitType.Bomber,
                UnitType.Granadier,
                UnitType.Fighter,
                UnitType.Miner,
                UnitType.Soldier
            };

            grenadierDesc.texture = grenadierTexture;

            UnitDescription soldierDesc = new UnitDescription();
            soldierDesc.CanFly = false;
            soldierDesc.height = 1;
            soldierDesc.width = 1;
            soldierDesc.Type = UnitType.Soldier;
            soldierDesc.attackablePriorities = new UnitType[] {
                UnitType.Granadier,
                UnitType.Miner,
                UnitType.Soldier
            };

            soldierDesc.texture = soldierTexture;

            mUnits = new List<Unit>(GameState.GRID_WIDTH / 2 + GameState.GRID_WIDTH * 2);

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

                mUnits.Add(bomber);
                mUnits.Add(fighter);
            }

            for (int i = 0; i < GameState.GRID_WIDTH / 2; ++i)
            {
                Units.Deminer miner = new Deminer(mGrid, mAI, minerDesc);
                Units.Grenadier grenadier = new Grenadier(mGrid, mAI, grenadierDesc);

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

                mUnits.Add(miner);
                mUnits.Add(grenadier);
            }

            for( var i = 0; i < GameState.GRID_WIDTH; ++i )
            {
                Units.Soldier soldier = new Soldier(mGrid, mAI, soldierDesc);
                soldier.side = mSide;

                if (mSide == Side.Red)
                {
                    soldier.SetLocation((GameState.GRID_HEIGHT - 4), i);
                }
                else
                {
                    soldier.SetLocation(3, i);
                }

                mUnits.Add(soldier);
            }
        }

        public void RemoveUnit(Units.Unit unit)
        {
            mUnits.Remove(unit);
            unit = null;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentLocation)
        {
            foreach (Unit unit in mUnits)
            {
                unit.Render(spriteBatch, parentLocation);
            }
        }
    }
}

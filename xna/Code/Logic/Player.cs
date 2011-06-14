using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Board_Game.Code.Units;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Code.Logic
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
        public Constants.Side mSide;
        private List<Unit> mUnits;
        private GameGrid mGrid;
        private AI mAI;

        public List<Unit> Units { get { return mUnits; } }

        public Player(bool isHuman, Constants.Side side, AI AIref)
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
            mUnits = new List<Unit>(Constants.GRID_WIDTH / 2 + Constants.GRID_WIDTH * 2);

            for( int i = 0; i < Constants.GRID_WIDTH/4; ++i )
            {
                Bomber bomber = new Bomber(mGrid, mAI, bomberTexture);
                Fighter fighter = new Fighter(mGrid, mAI, fighterTexture);

                bomber.side = mSide;
                fighter.side = mSide;

                if (mSide == Constants.Side.Red)
                {
                    bomber.SetLocation((Constants.GRID_HEIGHT - 2), (i * 4) + 2);
                    fighter.SetLocation((Constants.GRID_HEIGHT - 2), i * 4);
                }
                else
                {
                    bomber.SetLocation(0, i * 4);
                    fighter.SetLocation(0, (i * 4) + 2);
                }

                mUnits.Add(bomber);
                mUnits.Add(fighter);
            }

            for (int i = 0; i < Constants.GRID_WIDTH / 2; ++i)
            {
                Units.Deminer miner = new Deminer(mGrid, mAI, deminerTexture);
                Units.Grenadier grenadier = new Grenadier(mGrid, mAI, grenadierTexture);

                miner.side = mSide;
                grenadier.side = mSide;

                if (mSide == Constants.Side.Red)
                {
                    miner.SetLocation((Constants.GRID_HEIGHT - 3), ((i % 2) + 1) + (int)(4 * Math.Floor(i / 2.0f)));
                    grenadier.SetLocation((Constants.GRID_HEIGHT - 3), (int)(Math.Floor((i + 1) / 2.0f) * 4 - i % 2));
                }
                else
                {
                    miner.SetLocation(2, ((i % 2) + 1) + (4 * (int)(Math.Floor(i / 2.0f))));
                    grenadier.SetLocation(2, (int)(Math.Floor((i + 1) / 2.0f) * 4 - i % 2));
                }

                mUnits.Add(miner);
                mUnits.Add(grenadier);
            }

            for( var i = 0; i < Constants.GRID_WIDTH; ++i )
            {
                Units.Soldier soldier = new Soldier(mGrid, mAI, soldierTexture);
                soldier.side = mSide;

                if (mSide == Constants.Side.Red)
                {
                    soldier.SetLocation((Constants.GRID_HEIGHT - 4), i);
                }
                else
                {
                    soldier.SetLocation(3, i);
                }

                mUnits.Add(soldier);
            }
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

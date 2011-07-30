using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Board_Game.Input;
using Board_Game.Rendering;
using Microsoft.Xna.Framework.Content;
using Board_Game.Creatures;
using Board_Game.DB;
using Board_Game.Characters;

namespace Board_Game.Logic
{
    /// <summary>
    /// Will eventually house an entire games logical components, the grid and players.
    /// </summary>
    class GameState
    {
        Player mRed;
        Player mBlue;
        public Player mCurrentPlayer;
        public GameGrid mGrid;
        Selector mSelector;
        public Selector Selector { get { return mSelector; } }
        public Side winner;

        public Player Red { get { return mRed; } }
        public Player Blue { get { return mBlue; } }

        private AI mAI;
        public AI AI { get { return mAI; } }

        public GameState(AI AIref, Sprite selectorSprite, GameGrid grid)
        {
            mGrid = grid;
            mAI = AIref;
            AIref.mGrid = mGrid;
            
            //passing in the same AI for now, but could be different later
            mRed = new Player(true, Side.Red, this);
            mBlue = new Player(false, Side.Blue, this);

            mCurrentPlayer = mRed;

            mSelector = new Selector(selectorSprite, mGrid, this);
            mSelector.Side = Side.Red;

            winner = Side.Neutral;
        }

        public void Initialize(ContentManager content)
        {
            mRed.mArmy = content.Load<Army>("Armies/level1Red");
            mRed.PlaceOnField();

            mBlue.mArmy = content.Load<Army>("Armies/level1Blue");
            mBlue.PlaceOnField();

            mSelector.Initialize(content);
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentLocation)
        {
            mGrid.Render(spriteBatch);
            mSelector.RenderCreatureRadius(spriteBatch, mGrid.Position);
            Blue.Render(spriteBatch, mGrid.Position);
            Red.Render(spriteBatch, mGrid.Position);
            mSelector.Render(spriteBatch, mGrid.Position);
        }

        internal void Update(GameTime gameTime)
        {
            HandleInput();

            if (mCurrentPlayer.mIsHuman == false)
            {
                mAI.Update(gameTime);
            }
            else
            {
                mSelector.HandleInput();
            }
        }

        public void HandleInput()
        {
            if (InputManager.Get().isTriggered(Button.L1))
            {
                mRed.mIsHuman = !mRed.mIsHuman;
            }
            else if (InputManager.Get().isTriggered(Button.R1))
            {
                mBlue.mIsHuman = !mBlue.mIsHuman;
            }
        }

        public void ChangeTurns()
        {
            if (mCurrentPlayer == mRed)
            {
                mCurrentPlayer = mBlue;
            }
            else if (mCurrentPlayer == mBlue)
            {
                mCurrentPlayer = mRed;
            }

            mSelector.Side = mCurrentPlayer.mSide;
        }

        //determines and sets the winner if a side has won by capturing all the mines.
        public bool MineVictory()
        {
            winner = mGrid.Mines[0].side;

            foreach (Mine mine in mGrid.Mines)
            {
                if (mine.side != winner)
                {
                    winner = Side.Neutral;
                    return false;
                }
            }

            return true;
        }

        //This checks to see who, if anyone, hsa won
        public void CheckVictory()
        {
            if (!MineVictory())
            {
                //we need to check for a destruction victory
                if (Red.Creatures.Count == 0)
                {
                    winner = Side.Blue;
                }
                else if (Blue.Creatures.Count == 0)
                {
                    winner = Side.Red;
                }
            }
        }

        internal void EndTurn()
        {
            if (mCurrentPlayer.mSide == Side.Red)
            {
                mAI.CheckMines(Side.Blue);
            }
            else if (mCurrentPlayer.mSide == Side.Blue)
            {
                mAI.CheckMines(Side.Red);
            }

            CheckVictory();
            ChangeTurns();
        }

        public void RemoveCreature(Creatures.Creature Creature)
        {
            if (Creature.side == Side.Blue)
            {
                Blue.RemoveCreature(Creature);
            }
            else
            {
                Red.RemoveCreature(Creature);
            }
        }

        public void ClearArea(int x, int y, int width, int height)
        {
            for (var u = 0; u < width; ++u)
            {
                for (var v = 0; v < height; ++v)
                {
                    mGrid.Occupants[x + u, y + v] = null;
                }
            }
        }

        public void SetLocation(int x, int y, Creature creature)
        {
            for (var u = 0; u < creature.GridWidth; ++u)
            {
                for (var v = 0; v < creature.GridHeight; ++v)
                {
                    mGrid.Occupants[x + u, y + v] = creature;
                }
            }

            creature.SetLocation(x, y);
        }

        public void Move(int newX, int newY, Creature creature)
        {
            ClearArea(creature.GridLocation.X, creature.GridLocation.Y, creature.GridWidth, creature.GridHeight);
            SetLocation(newX, newY, creature);
        }

        // Finds and destoys the creatures in a given area
        public void DestroyCreatures(int newX, int newY, int width, int height)
        {
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    Creature occupant = mGrid.Occupants[newX + x, newY + y];

                    if (occupant != null)
                    {
                        int CreatureX = (int)((occupant.Position.X - occupant.Position.X % occupant.ScreenDimensions().X) / Tile.TILE_SIZE);
                        int CreatureY = (int)((occupant.Position.Y - occupant.Position.Y % occupant.ScreenDimensions().Y) / Tile.TILE_SIZE);

                        ClearArea(CreatureX, CreatureY, occupant.GridWidth, occupant.GridHeight);
                        RemoveCreature(occupant);
                    }
                }
            }
        }

        // chacks if an area contains ay units
        public bool CheckOccupied(int newX, int newY, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mGrid.Occupants[newX + x, newY + y] != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public ClampArea GetClampArea(Creature creature)
        {
            ClampArea returnValue = creature.GetClampArea();

            //Confine the area so that it fits within the board.
            if (returnValue.leftCut < 0)
            {
                returnValue.leftCut = 0;
            }
            if (returnValue.topCut < 0)
            {
                returnValue.topCut = 0;
            }
            if (returnValue.rightCut + creature.ScreenDimensions().X / 2 > mGrid.PixelWidth())
            {
                returnValue.rightCut = (int)creature.Position.X;
            }
            if (returnValue.bottomCut + creature.ScreenDimensions().Y / 2 > mGrid.PixelHeight())
            {
                returnValue.bottomCut = (int)creature.Position.Y;
            }

            return returnValue;
        }
    }
}

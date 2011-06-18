using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Board_Game.Input;
using Board_Game.Code.Rendering;
using Board_Game.Rendering;

namespace Board_Game.Logic
{
    public enum Side
    {
        Red = 0,
        Blue = 1,
        Neutral = 2
    }

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
        public Side winner;

        public const int GRID_WIDTH = 12;
        public const int GRID_HEIGHT = 12;

        public Player Red { get { return mRed; } }
        public Player Blue { get { return mBlue; } }

        public GameState(AI AIref,
                Sprite selectorSprite)
        {
            mGrid = new GameGrid(
                GameState.GRID_WIDTH,
                GameState.GRID_HEIGHT,
                TextureManager.Get().Find("textures/tiles/single"),
                TextureManager.Get().Find("textures/tiles/mine")
            );
            
            AIref.mGrid = mGrid;
            
            //passing in the same AI for now, but could be different later
            mRed = new Player(true, Side.Red, AIref);
            mBlue = new Player(false, Side.Blue, AIref);

            mCurrentPlayer = mRed;

            mSelector = new Selector(selectorSprite, mGrid, this);
            mSelector.Side = Side.Red;

            winner = Side.Neutral;
        }

        public void Initialize()
        {
            mRed.CreateCreatures(
                TextureManager.Get().Find("textures/Creatures/bomber"),
                TextureManager.Get().Find("textures/Creatures/fighter"),
                TextureManager.Get().Find("textures/Creatures/soldier"),
                TextureManager.Get().Find("textures/Creatures/deminer"),
                TextureManager.Get().Find("textures/Creatures/grenadier")
            );

            mBlue.CreateCreatures(
                TextureManager.Get().Find("textures/Creatures/bomber"),
                TextureManager.Get().Find("textures/Creatures/fighter"),
                TextureManager.Get().Find("textures/Creatures/soldier"),
                TextureManager.Get().Find("textures/Creatures/deminer"),
                TextureManager.Get().Find("textures/Creatures/grenadier")
            );
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentLocation)
        {
            mGrid.Render(spriteBatch);
            mSelector.RenderCreatureRadius(spriteBatch, mGrid.position);
            Blue.Render(spriteBatch, mGrid.position);
            Red.Render(spriteBatch, mGrid.position);
            mSelector.Render(spriteBatch, mGrid.position);
        }

        internal void Update(GameTime gameTime)
        {
            HandleInput();

            if (mCurrentPlayer.mIsHuman == false)
            {
                mCurrentPlayer.mAI.Update(gameTime);
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
            winner = mGrid.mTiles[0, 0].mine.side;

            for (var i = 0; i < GameState.GRID_WIDTH / 2; ++i)
            {
                for (var j = 0; j < GameState.GRID_HEIGHT / 2; ++j)
                {
                    if (i % 2 == j % 2 && winner != mGrid.mTiles[i * 2, j * 2].mine.side)
                    {
                        winner = Side.Neutral;
                        return false;
                    }
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
                mCurrentPlayer.mAI.CheckMines(Side.Blue);
            }
            else if (mCurrentPlayer.mSide == Side.Blue)
            {
                mCurrentPlayer.mAI.CheckMines(Side.Red);
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
    }
}

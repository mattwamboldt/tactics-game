using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Code.Logic;
using Board_Game.Code.Rendering;
using Board_Game.Code.Input;

namespace Board_Game.Code
{
    class Selector
    {
        public Side mSide;
        public GameGrid mGridRef;
        public GameState mGameState;

        //for drawing
        public Sprite mSprite;
        public Vector2 position;
        private Units.Unit selectedUnit;
        private Units.ClampArea unitClamp;

        public Selector(
                Texture2D selectorTexture,
                GameGrid grid,
                GameState gameState)
        {
            position = new Vector2();

            mSprite = new Sprite(
                selectorTexture,
                new Vector2(0, 0),
                Color.White,
                new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE)
            );

            mSide = Side.Neutral;
            mGridRef = grid;
            mGameState = gameState;
            selectedUnit = null;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            mSprite.Position = new Vector2(
                position.X * Tile.TILE_SIZE,
                position.Y * Tile.TILE_SIZE
            );

            mSprite.Color = Color.White;

            if (mSide == Side.Red)
            {
                mSprite.Color = Color.Red;
            }
            else if (mSide == Side.Blue)
            {
                mSprite.Color = Color.Blue;
            }

            mSprite.Render(spriteBatch, parentPosition);
        }

        public void HandleInput()
        {
            if (InputManager.Get().isTriggered(Button.Up))
            {
                MoveUp();
            }
            if (InputManager.Get().isTriggered(Button.Down))
            {
                MoveDown();
            }
            if (InputManager.Get().isTriggered(Button.Left))
            {
                MoveLeft();
            }
            if (InputManager.Get().isTriggered(Button.Right))
            {
                MoveRight();
            }
            if (InputManager.Get().isTriggered(Button.Cross))
            {
                if (selectedUnit != null)
                {
                    SelectSquare();
                }
                else
                {
                    SelectUnit();
                }
            }
        }

        private void SelectUnit()
        {
            if (mGridRef.mTiles[(int)position.Y, (int)position.X].Occupied)
            {
                Units.Unit unit = mGridRef.mTiles[(int)position.Y, (int)position.X].occupiedUnit;
                if (unit.side == mSide)
                {
                    selectedUnit = unit;
                    unit.isSelected = true;
                    unitClamp = selectedUnit.GetClampArea();
                }
            }
        }

        private void SelectSquare()
        {
            int j = ((int)position.X - (int)position.X % selectedUnit.width);
            int i = ((int)position.Y - (int)position.Y % selectedUnit.height);

            if (selectedUnit.CheckOccupied(i, j))
            {
                Units.Unit unit = mGridRef.mTiles[i, j].occupiedUnit;
                if (selectedUnit.CheckColour(i, j))
                {
                    selectedUnit.isSelected = false;
                    selectedUnit.RemoveUnits(i, j);
                    selectedUnit.Move(i, j);
                    selectedUnit = null;

                    mGameState.EndTurn();

                    
                }
                else
                {
                    if (selectedUnit == unit)
                    {
                        selectedUnit = null;
                        unit.isSelected = false;
                    }
                    else if (unit != null && unit.side == mSide)
                    {
                        selectedUnit.isSelected = false;
                        selectedUnit = unit;
                        unit.isSelected = true;
                        unitClamp = selectedUnit.GetClampArea();
                    }
                }
            }
            else
            {
                selectedUnit.isSelected = false;
                selectedUnit.Move(i, j);
                selectedUnit = null;

                mGameState.EndTurn();
            }
        }

#region Moving functions
        private void MoveRight()
        {
            if (position.X < GameState.GRID_WIDTH - 1)
            {
                position.X += 1;
            }
        }

        private void MoveLeft()
        {
            if (position.X > 0)
            {
                position.X -= 1;
            }
        }

        private void MoveUp()
        {
            if (position.Y > 0)
            {
                position.Y -= 1;
            }
        }

        private void MoveDown()
        {
            if (position.Y < GameState.GRID_HEIGHT - 1)
            {
                position.Y += 1;
            }
        }
#endregion
    }
}

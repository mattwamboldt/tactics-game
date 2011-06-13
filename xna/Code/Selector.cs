using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Code
{
    class Selector
    {
        public Sides mSide;
        public GameGrid mGridRef;
        public AI mAIRef;

        //for drawing
        public Texture2D texture;
        public Vector2 position;
        private Units.Unit selectedUnit;
        private Units.ClampArea unitClamp;

        public Selector(
                Texture2D selectorTexture,
                GameGrid grid,
                AI AIRef)
        {
            texture = selectorTexture;
            position = new Vector2();

            mSide = new Sides();
            mGridRef = grid;
            mAIRef = AIRef;
            selectedUnit = null;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            float scale = Tile.TILE_SIZE / texture.Width;

            Vector2 renderPosition = new Vector2(
                parentPosition.X + position.X * Tile.TILE_SIZE,
                parentPosition.Y + position.Y * Tile.TILE_SIZE
            );

            Color color = Color.White;

            if (mSide.mColour == Constants.RED)
            {
                color = Color.Red;
            }
            else if (mSide.mColour == Constants.BLUE)
            {
                color = Color.Blue;
            }

            spriteBatch.Draw(
                texture,
                renderPosition,
                null,
                color,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public void HandleInput()
        {
            if (InputManager.Get().isTriggered(Keys.Up))
            {
                MoveUp();
            }
            if (InputManager.Get().isTriggered(Keys.Down))
            {
                MoveDown();
            }
            if (InputManager.Get().isTriggered(Keys.Left))
            {
                MoveLeft();
            }
            if (InputManager.Get().isTriggered(Keys.Right))
            {
                MoveRight();
            }
            if (InputManager.Get().isTriggered(Keys.Enter))
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
            if (mGridRef.mTiles[(int)position.Y, (int)position.X].occupied)
            {
                Units.Unit unit = mGridRef.mTiles[(int)position.Y, (int)position.X].occupiedUnit;
                if (unit.side.mColour == mSide.mColour)
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
                    selectedUnit.Move(i, j, true);
                    selectedUnit = null;

                    if (mSide.mColour == Constants.RED)
                    {
                        mAIRef.CheckMines(Constants.BLUE);
                    }
                    else if (mSide.mColour == Constants.BLUE)
                    {
                        mAIRef.CheckMines(Constants.RED);
                    }
                }
                else
                {
                    if (selectedUnit == unit)
                    {
                        selectedUnit = null;
                        unit.isSelected = false;
                    }
                    else if (unit.side.mColour == mSide.mColour)
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
                selectedUnit.Move(i, j, true);
                selectedUnit = null;

                if (mSide.mColour == Constants.RED)
                {
                    mAIRef.CheckMines(Constants.BLUE);
                }
                else if (mSide.mColour == Constants.BLUE)
                {
                    mAIRef.CheckMines(Constants.RED);
                }
            }
        }

#region Moving functions
        private void MoveRight()
        {
            if (position.X < Constants.GRID_WIDTH - 1)
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
            if (position.Y < Constants.GRID_HEIGHT - 1)
            {
                position.Y += 1;
            }
        }
#endregion
    }
}

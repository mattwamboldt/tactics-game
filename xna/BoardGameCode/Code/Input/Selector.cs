using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Board_Game.Rendering;
using Board_Game.Input;
using Board_Game.Units;
using Board_Game.Code.Rendering;

namespace Board_Game.Logic
{
    class Selector
    {
        private Side mSide;
        public Side Side 
        {
            get { return mSide; }
            set
            {
                if (mSide != value)
                {
                    mSide = value;
                    Deselect();
                }
            }
        }

        public GameGrid mGridRef;
        public GameState mGameState;

        //for drawing
        public Sprite mSprite;
        public Vector2 position;
        private Units.Unit selectedUnit;
        private Units.ClampArea unitClamp;

        public Selector(
                Sprite selectorSprite,
                GameGrid grid,
                GameState gameState)
        {
            position = new Vector2();

            mSprite = selectorSprite;

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

        private void Deselect()
        {
            if (selectedUnit != null)
            {
                selectedUnit.isSelected = false;
                selectedUnit = null;
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

        private bool isInUnitClampArea()
        {
            ClampArea unitArea = selectedUnit.GetClampArea();
            return position.X * Tile.TILE_SIZE >= unitArea.leftCut
                && position.Y * Tile.TILE_SIZE >= unitArea.topCut
                && position.X * Tile.TILE_SIZE <= unitArea.rightCut
                && position.Y * Tile.TILE_SIZE <= unitArea.bottomCut;
        }

        private void SelectSquare()
        {
            int j = ((int)position.X - (int)position.X % selectedUnit.width);
            int i = ((int)position.Y - (int)position.Y % selectedUnit.height);

            if (selectedUnit.CheckOccupied(i, j))
            {
                Units.Unit unit = mGridRef.mTiles[i, j].occupiedUnit;

                //toggle selection of the current unit
                if (selectedUnit == unit)
                {
                    Deselect();
                }
                //destroy units in your move radius
                else if (isInUnitClampArea() && selectedUnit.CheckColour(i, j))
                {
                    selectedUnit.isSelected = false;
                    selectedUnit.RemoveUnits(i, j);
                    selectedUnit.Move(i, j);
                    selectedUnit = null;

                    mGameState.EndTurn(); 
                }
                //switch to friendly units
                else if (unit != null && unit.side == mSide)
                {
                    selectedUnit.isSelected = false;
                    selectedUnit = unit;
                    unit.isSelected = true;
                    unitClamp = selectedUnit.GetClampArea();
                }
            }
            else if(isInUnitClampArea())
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

        internal void RenderUnitRadius(SpriteBatch spriteBatch, Vector2 parentRactangle)
        {
            if (selectedUnit != null)
            {
                //draw a rectangle under that unit in it's clamp area
                ClampArea unitArea = selectedUnit.GetClampArea();
                Rectangle areaMovable = new Rectangle(
                    (int)(unitArea.leftCut + parentRactangle.X),
                    (int)(unitArea.topCut + parentRactangle.Y),
                    (int)((unitArea.rightCut - unitArea.leftCut + selectedUnit.ScreenDimensions().X)),
                    (int)((unitArea.bottomCut - unitArea.topCut + selectedUnit.ScreenDimensions().Y))
                );

                Texture2D texture = TextureManager.Get().Find("RAW");

                spriteBatch.Draw(texture, areaMovable, new Color(0,255,0,130));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Board_Game.Rendering;
using Board_Game.Input;
using Board_Game.Creatures;
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
        private Creatures.Creature selectedCreature;
        private Creatures.ClampArea CreatureClamp;

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
            selectedCreature = null;
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
                if (selectedCreature != null)
                {
                    SelectSquare();
                }
                else
                {
                    SelectCreature();
                }
            }
        }

        private void Deselect()
        {
            if (selectedCreature != null)
            {
                selectedCreature.isSelected = false;
                selectedCreature = null;
            }
        }

        private void SelectCreature()
        {
            if (mGridRef.mTiles[(int)position.Y, (int)position.X].Occupied)
            {
                Creatures.Creature Creature = mGridRef.mTiles[(int)position.Y, (int)position.X].occupiedCreature;
                if (Creature.side == mSide)
                {
                    selectedCreature = Creature;
                    Creature.isSelected = true;
                    CreatureClamp = selectedCreature.GetClampArea();
                }
            }
        }

        private bool isInCreatureClampArea()
        {
            ClampArea CreatureArea = selectedCreature.GetClampArea();
            return position.X * Tile.TILE_SIZE >= CreatureArea.leftCut
                && position.Y * Tile.TILE_SIZE >= CreatureArea.topCut
                && position.X * Tile.TILE_SIZE <= CreatureArea.rightCut
                && position.Y * Tile.TILE_SIZE <= CreatureArea.bottomCut;
        }

        private void SelectSquare()
        {
            int j = ((int)position.X - (int)position.X % selectedCreature.mCreatureDesc.SizeInSpaces.X);
            int i = ((int)position.Y - (int)position.Y % selectedCreature.mCreatureDesc.SizeInSpaces.Y);

            if (selectedCreature.CheckOccupied(i, j))
            {
                Creatures.Creature Creature = mGridRef.mTiles[i, j].occupiedCreature;

                //toggle selection of the current Creature
                if (selectedCreature == Creature)
                {
                    Deselect();
                }
                //destroy Creatures in your move radius
                else if (isInCreatureClampArea() && selectedCreature.CanDestroyAllUnits(i, j))
                {
                    selectedCreature.isSelected = false;
                    selectedCreature.RemoveCreatures(i, j);
                    selectedCreature.Move(i, j);
                    selectedCreature = null;

                    mGameState.EndTurn();
                }
                //switch to friendly Creatures
                else
                {
                    Creature = mGridRef.mTiles[(int)position.Y, (int)position.X].occupiedCreature;

                    if (Creature != null && Creature.side == mSide)
                    {
                        selectedCreature.isSelected = false;
                        selectedCreature = Creature;
                        Creature.isSelected = true;
                        CreatureClamp = selectedCreature.GetClampArea();
                    }
                }
            }
            else if(isInCreatureClampArea())
            {
                selectedCreature.isSelected = false;
                selectedCreature.Move(i, j);
                selectedCreature = null;

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

        internal void RenderCreatureRadius(SpriteBatch spriteBatch, Vector2 parentRactangle)
        {
            if (selectedCreature != null)
            {
                //draw a rectangle under that Creature in it's clamp area
                ClampArea CreatureArea = selectedCreature.GetClampArea();
                Rectangle areaMovable = new Rectangle(
                    (int)(CreatureArea.leftCut + parentRactangle.X),
                    (int)(CreatureArea.topCut + parentRactangle.Y),
                    (int)((CreatureArea.rightCut - CreatureArea.leftCut + selectedCreature.ScreenDimensions().X)),
                    (int)((CreatureArea.bottomCut - CreatureArea.topCut + selectedCreature.ScreenDimensions().Y))
                );

                Texture2D texture = TextureManager.Get().Find("RAW");

                spriteBatch.Draw(texture, areaMovable, new Color(0,255,0,130));
            }
        }
    }
}

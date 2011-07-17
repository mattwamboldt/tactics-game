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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
#if EDITOR
using Board_Game.Code.Editing;
#endif

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
        private Creature selectedCreature;
        private ClampArea CreatureClamp;

        //Audio for moving the selector around
        private SoundEffect mMoveSound;
        private SoundEffect mSelectSound;
        private SoundEffect mDeSelectSound;
        private SoundEffect mPlaceSound;

#if EDITOR
        private UnitEditor mUnitEditor;
#endif

        public Selector(
                Sprite selectorSprite,
                GameGrid grid,
                GameState gameState
                )
        {
            position = new Vector2();

            mSprite = selectorSprite;

            mSide = Side.Neutral;
            mGridRef = grid;
            mGameState = gameState;
            selectedCreature = null;
        }

        public void Initialize(ContentManager Content)
        {
            mMoveSound = Content.Load<SoundEffect>("audio/sfx/Move");
            mSelectSound = Content.Load<SoundEffect>("audio/sfx/Select");
            mDeSelectSound = Content.Load<SoundEffect>("audio/sfx/DeSelect");
            mPlaceSound = Content.Load<SoundEffect>("audio/sfx/Place");
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
                mMoveSound.Play();
            }
            if (InputManager.Get().isTriggered(Button.Down))
            {
                MoveDown();
                mMoveSound.Play();
            }
            if (InputManager.Get().isTriggered(Button.Left))
            {
                MoveLeft();
                mMoveSound.Play();
            }
            if (InputManager.Get().isTriggered(Button.Right))
            {
                MoveRight();
                mMoveSound.Play();
            }
            if (InputManager.Get().isTriggered(Button.Cross))
            {
                Select();
            }
        }

        private void Select()
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

        private void Deselect()
        {
            if (selectedCreature != null)
            {
                selectedCreature.isSelected = false;
                selectedCreature = null;
                mDeSelectSound.Play();
            }
        }

        private void SelectCreature()
        {
            if (mGridRef.mTiles[(int)position.X, (int)position.Y].Occupied)
            {
                Creatures.Creature Creature = mGridRef.mTiles[(int)position.X, (int)position.Y].occupiedCreature;
                if (Creature.side == mSide)
                {
                    selectedCreature = Creature;
                    Creature.isSelected = true;
                    CreatureClamp = mGameState.GetClampArea(selectedCreature);
                    mSelectSound.Play();
                }
            }
        }

        private bool isInCreatureClampArea()
        {
            int x = ((int)position.X - (int)position.X % selectedCreature.GridWidth);
            int y = ((int)position.Y - (int)position.Y % selectedCreature.GridHeight);

            ClampArea CreatureArea = mGameState.GetClampArea(selectedCreature);
            return x * Tile.TILE_SIZE >= CreatureArea.leftCut
                && y * Tile.TILE_SIZE >= CreatureArea.topCut
                && x * Tile.TILE_SIZE <= CreatureArea.rightCut
                && y * Tile.TILE_SIZE <= CreatureArea.bottomCut;
        }

        private void SelectSquare()
        {
            int x = ((int)position.X - (int)position.X % selectedCreature.GridWidth);
            int y = ((int)position.Y - (int)position.Y % selectedCreature.GridHeight);

            if (mGameState.CheckOccupied(x, y, selectedCreature.GridWidth, selectedCreature.GridHeight))
            {
                Creature Creature = mGridRef.mTiles[x, y].occupiedCreature;

                //toggle selection of the current Creature
                if (selectedCreature == Creature)
                {
                    Deselect();
                }
                //destroy Creatures in your move radius
                else if (isInCreatureClampArea() && mGameState.AI.CanDestroyAllUnits(x, y, selectedCreature))
                {
                    selectedCreature.isSelected = false;
                    mGameState.DestroyCreatures(x, y, selectedCreature.GridWidth, selectedCreature.GridHeight);
                    mGameState.Move(x, y, selectedCreature);
                    selectedCreature = null;

                    mGameState.EndTurn();
                }
                //switch to friendly Creatures
                else
                {
                    Creature = mGridRef.mTiles[(int)position.X, (int)position.Y].occupiedCreature;

                    if (Creature != null && Creature.side == mSide)
                    {
                        selectedCreature.isSelected = false;
                        selectedCreature = Creature;
                        Creature.isSelected = true;
                        CreatureClamp = mGameState.GetClampArea(selectedCreature);
                        mSelectSound.Play();
                    }
                }
            }
            else if(isInCreatureClampArea())
            {
                selectedCreature.isSelected = false;
                mGameState.Move(x, y, selectedCreature);
                selectedCreature = null;

                mGameState.EndTurn();
                mPlaceSound.Play();
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
                ClampArea CreatureArea = mGameState.GetClampArea(selectedCreature);
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

        public void HandleInput(Cursor cursor)
        {
            Point cursorPosition = cursor.GetPosition();
            Rectangle bounds = new Rectangle(
                (int)mGridRef.position.X,
                (int)mGridRef.position.Y,
                (int)mGridRef.Width(),
                (int)mGridRef.Height()
            );

            if (bounds.Contains(cursorPosition))
            {
                Vector2 newPoint = new Vector2(
                    (int)((cursorPosition.X - mGridRef.position.X) / Tile.TILE_SIZE),
                    (int)((cursorPosition.Y - mGridRef.position.Y) / Tile.TILE_SIZE)
                );

                if (newPoint.Equals(position) == false)
                {
                    position = newPoint;
                    mMoveSound.Play();
                }

                if (cursor.IsLeftClick())
                {
                    Select();
                }
                else if (cursor.IsRightClick())
                {
#if EDITOR
                    Creature Creature = mGridRef.mTiles[(int)position.X, (int)position.Y].occupiedCreature;

                    if (Creature != null)
                    {
                        mUnitEditor.SelectCreature(Creature);
                    }
                    else
                    {
                        mUnitEditor.SelectSquare((int)position.X, (int)position.Y);
                    }

                    //TODO: feed this to the editor to trigger changes to the game world
                    Console.WriteLine("Should edit something!");
#endif
                }
            }
        }
#if EDITOR
        internal void SetEditorHandle(UnitEditor unitEditor)
        {
            mUnitEditor = unitEditor;
        }
#endif
    }
}

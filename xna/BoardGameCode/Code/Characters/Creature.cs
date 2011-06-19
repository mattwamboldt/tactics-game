using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;
using Board_Game.Rendering;

namespace Board_Game.Creatures
{
    struct ClampArea
    {
        public int leftCut;
        public int rightCut;
        public int topCut;
        public int bottomCut;
    };

    //Contains all the code for a Creature.
    class Creature
    {
        //These allow the Creature to remember its previous location and
        //return there in the event of an invalid move
        public int originalX;
        public int originalY;

        //this allows us to set the colour
        public Side side;

        //This gives the Creature an awareness of the other Creatures on the playing field
        //so it can destroy itself or better than that, enemy Creatures
        protected GameGrid grid;

        public CreatureDescription mCreatureDesc;

        public Vector2 position;

        public bool isSelected;

        private Sprite mSprite;

        public Creature(GameGrid gridRef, CreatureDescription CreatureDesc)
        {
            mCreatureDesc = CreatureDesc;
            originalX = 0;
            originalY = 0;
            grid = gridRef;
            side = Side.Neutral;
            position = new Vector2(0, 0);
            isSelected = false;

            mSprite = new Sprite(mCreatureDesc.Texture, position, Color.White, new Vector2(GridWidth * Tile.TILE_SIZE, GridHeight * Tile.TILE_SIZE));
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            mSprite.Position = position;
            mSprite.Color = Color.White;

            if (side == Side.Red)
            {
                mSprite.Color = Color.Red;
            }
            else if (side == Side.Blue)
            {
                mSprite.Color = Color.Blue;
            }

            if (isSelected)
            {
                mSprite.Color = Color.Yellow;
            }

            mSprite.Render(spriteBatch, parentPosition);
        }

        public int GridWidth
        {
            get { return mCreatureDesc.SizeInSpaces.X; }
        }

        public int GridHeight
        {
            get { return mCreatureDesc.SizeInSpaces.Y; }
        }

        public bool CheckOccupied(int newX, int newY)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    if (grid.mTiles[newX + x, newY + y].Occupied)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetLocation(int newX, int newY)
        {
            originalX = newX;
            originalY = newY;
            position.X = newX * Tile.TILE_SIZE;
            position.Y = newY * Tile.TILE_SIZE;
        }

        public bool CanDestroyAllUnits(int newX, int newY)
        {
            for (var x = 0; x < GridWidth; ++x)
            {
                for (var y = 0; y < GridHeight; ++y)
                {
                    Tile tile = grid.mTiles[newX + x, newY + y];

                    if ( tile.side == side //Cant attack friendlies
                      || ( tile.Occupied && mCreatureDesc.CanAttack(tile.occupiedCreature.mCreatureDesc.Type) == false))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public ClampArea GetClampArea()
        {
            ClampArea returnValue;
            returnValue.leftCut = (int)(position.X - ScreenDimensions().X);
            returnValue.rightCut = (int)(position.X + ScreenDimensions().X);
            returnValue.topCut = (int)(position.Y - ScreenDimensions().Y);
            returnValue.bottomCut = (int)(position.Y + ScreenDimensions().Y);
            originalX = (int)((position.X - position.X % ScreenDimensions().X) / Tile.TILE_SIZE);
            originalY = (int)((position.Y - position.Y % ScreenDimensions().Y) / Tile.TILE_SIZE);

            //Clamp the area so that it fits within the board.
            if (returnValue.leftCut < 0)
            {
                returnValue.leftCut = 0;
            }
            if (returnValue.topCut < 0)
            {
                returnValue.topCut = 0;
            }
            if (returnValue.rightCut + ScreenDimensions().X / 2 > grid.Width())
            {
                returnValue.rightCut = (int)position.X;
            }
            if (returnValue.bottomCut + ScreenDimensions().Y / 2 > grid.Height())
            {
                returnValue.bottomCut = (int)position.Y;
            }

            return returnValue;
        }
        
        public int GetX()
        {
            return (int)(position.X / Tile.TILE_SIZE);
        }

        public int GetY()
        {
            return (int)(position.Y / Tile.TILE_SIZE);
        }

        public Vector2 ScreenDimensions()
        {
            return mSprite.Dimensions;
        }

        /*
            This tells us if a square is actually an enemy mine location.
        */
        public bool IsEnemyMine(int x, int y)
        {
            if (mCreatureDesc.CanFly || mCreatureDesc.Type == CreatureType.Miner)
            {
                return false;
            }

            return (Math.Floor((double)(x / 2)) % 2 == Math.Floor((double)(y / 2)) % 2)
                && (grid.mTiles[x - x % 2, y - y % 2].mine.side != side);
        }
    }
}

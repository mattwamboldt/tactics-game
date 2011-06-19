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

        public AI mAIRef;
        public bool isSelected;

        private Sprite mSprite;

        public Creature(GameGrid gridRef, AI AIRef, CreatureDescription CreatureDesc)
        {
            mCreatureDesc = CreatureDesc;
            originalX = 0;
            originalY = 0;
            grid = gridRef;
            mAIRef = AIRef;
            side = Side.Neutral;
            position = new Vector2(0, 0);
            isSelected = false;

            mSprite = new Sprite(mCreatureDesc.Texture, position, Color.White, ScreenDimensions());
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

        public bool CheckOccupied(int newX, int newY)
        {
            for (int x = 0; x < mCreatureDesc.SizeInSpaces.X; x++)
            {
                for (int y = 0; y < mCreatureDesc.SizeInSpaces.Y; y++)
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
            for (var x = 0; x < mCreatureDesc.SizeInSpaces.X; ++x)
            {
                for (var y = 0; y < mCreatureDesc.SizeInSpaces.Y; ++y)
                {
                    grid.mTiles[newX + x, newY + y].side = side;
                    grid.mTiles[newX + x, newY + y].occupiedCreature = this;
                }
            }

            originalX = newX;
            originalY = newY;
            position.X = newX * Tile.TILE_SIZE;
            position.Y = newY * Tile.TILE_SIZE;
        }

        public virtual void RemoveCreatures(int newX, int newY)
        {
            for (var x = 0; x < mCreatureDesc.SizeInSpaces.X; ++x)
            {
                for (var y = 0; y < mCreatureDesc.SizeInSpaces.Y; ++y)
                {
                    if (grid.mTiles[newX + x, newY + y].Occupied)
                    {
                        mAIRef.State.RemoveCreature(grid.mTiles[newX + x, newY + y].occupiedCreature);
                    }
                }
            }
        }

        public void Move(int newX, int newY)
        {
            for (var x = 0; x < mCreatureDesc.SizeInSpaces.X; ++x)
            {
                for (var y = 0; y < mCreatureDesc.SizeInSpaces.Y; ++y)
                {
                    grid.mTiles[originalX + x, originalY + y].side = Side.Neutral;
                    grid.mTiles[originalX + x, originalY + y].occupiedCreature = null;
                }
            }
            SetLocation(newX, newY);
        }

        public bool CanDestroyAllUnits(int newX, int newY)
        {
            for (var x = 0; x < mCreatureDesc.SizeInSpaces.X; ++x)
            {
                for (var y = 0; y < mCreatureDesc.SizeInSpaces.Y; ++y)
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
            return new Vector2(mCreatureDesc.SizeInSpaces.X * Tile.TILE_SIZE, mCreatureDesc.SizeInSpaces.Y * Tile.TILE_SIZE);
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

        /*
            Finds the space that the Creature most wants to move into
         * //TODO: recalculates evey Creature to every other Creature, every AI update. why am I not caching the distances?
        */
        public virtual Vector2 GetNearestTarget()
        {
            List<Creature> opposingCreatures;

            if (side == Side.Red)
            {
                opposingCreatures = mAIRef.State.Blue.Creatures;
            }
            else
            {
                opposingCreatures = mAIRef.State.Red.Creatures;
            }

            Vector2 originalPoint = new Vector2(GetX(), GetY());
            Vector2 nearestTarget = new Vector2(-1, -1);

            double distanceToNearest = mAIRef.GetDistanceToCoordinates(originalPoint, 0, 0);

            foreach (Creature Creature in opposingCreatures)
            {
                if (mCreatureDesc.CanAttack(Creature.mCreatureDesc.Type))
                {
                    var x = Creature.GetX();
                    var y = Creature.GetY();
                    double distanceToCreature = mAIRef.GetDistanceToCoordinates(originalPoint, x, y);

                    //if an opponent is on their mine they're safe so don't bother with them.
                    if ((distanceToCreature < distanceToNearest && IsEnemyMine(x, y) == false)
                       || nearestTarget.X == -1)
                    {
                        nearestTarget.X = x;
                        nearestTarget.Y = y;
                        distanceToNearest = distanceToCreature;
                    }
                }
            }

            return nearestTarget;
        }
    }
}

﻿using System;
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
        public int originalI;
        public int originalJ;

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
            originalI = 0;
            originalJ = 0;
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

        public bool CheckOccupied(int i, int j)
        {
            for (int y = i; y < i + mCreatureDesc.SizeInSpaces.Y; y++)
            {
                for (int x = j; x < j + mCreatureDesc.SizeInSpaces.X; x++)
                {
                    if (grid.mTiles[y, x].Occupied)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetLocation(int newLocationI, int newLocationJ)
        {
            for (var i = 0; i < mCreatureDesc.SizeInSpaces.Y; ++i)
            {
                for (var j = 0; j < mCreatureDesc.SizeInSpaces.X; ++j)
                {
                    grid.mTiles[newLocationI + i, newLocationJ + j].side = side;
                    grid.mTiles[newLocationI + i, newLocationJ + j].occupiedCreature = this;
                }
            }

            position.X = newLocationJ * Tile.TILE_SIZE;
            position.Y = newLocationI * Tile.TILE_SIZE;
        }

        public virtual void RemoveCreatures(int p, int p_2)
        {
            throw new NotImplementedException();
        }

        public void Move(int newLocationI, int newLocationJ)
        {
            for (var i = 0; i < mCreatureDesc.SizeInSpaces.Y; ++i)
            {
                for (var j = 0; j < mCreatureDesc.SizeInSpaces.X; ++j)
                {
                    grid.mTiles[originalI + i, originalJ + j].side = Side.Neutral;
                    grid.mTiles[originalI + i, originalJ + j].occupiedCreature = null;
                }
            }

            SetLocation(newLocationI, newLocationJ);
        }

        public virtual bool CheckColour(int i, int j)
        {
            throw new NotImplementedException();
        }

        public ClampArea GetClampArea()
        {
            ClampArea returnValue;
            returnValue.leftCut = (int)(position.X - ScreenDimensions().X);
            returnValue.rightCut = (int)(position.X + ScreenDimensions().X);
            returnValue.topCut = (int)(position.Y - ScreenDimensions().Y);
            returnValue.bottomCut = (int)(position.Y + ScreenDimensions().Y);
            originalJ = (int)((position.X - position.X % ScreenDimensions().X) / Tile.TILE_SIZE);
            originalI = (int)((position.Y - position.Y % ScreenDimensions().Y) / Tile.TILE_SIZE);

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
        
        public int GetI()
        {
            return (int)(position.Y / Tile.TILE_SIZE);
        }

        public int GetJ()
        {
            return (int)(position.X / Tile.TILE_SIZE);
        }

        public Vector2 ScreenDimensions()
        {
            return new Vector2(mCreatureDesc.SizeInSpaces.X * Tile.TILE_SIZE, mCreatureDesc.SizeInSpaces.Y * Tile.TILE_SIZE);
        }

        /*
            This tells us if a square is actually an enemy mine location.
        */
        public bool IsEnemyMine(int i, int j)
        {
            if (mCreatureDesc.CanFly || mCreatureDesc.Type == CreatureType.Miner)
            {
                return false;
            }

            return (Math.Floor((double)(i / 2)) % 2 == Math.Floor((double)(j / 2)) % 2)
                && (grid.mTiles[i - i % 2, j - j % 2].mine.side != side);
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

            Vector2 originalPoint = new Vector2(GetJ(), GetI());
            Vector2 nearestTarget = new Vector2(-1, -1);

            double distanceToNearest = mAIRef.GetDistanceToCoordinates(originalPoint, 0, 0);

            foreach (Creature Creature in opposingCreatures)
            {
                if (mCreatureDesc.CanAttack(Creature.mCreatureDesc.Type))
                {
                    var iCoord = Creature.GetI();
                    var jCoord = Creature.GetJ();
                    double distanceToCreature = mAIRef.GetDistanceToCoordinates(originalPoint, iCoord, jCoord);

                    //if an opponent is on their mine they're safe so don't bother with them.
                    if ((distanceToCreature < distanceToNearest && IsEnemyMine(iCoord, jCoord) == false)
                       || nearestTarget.Y == -1)
                    {
                        nearestTarget.Y = iCoord;
                        nearestTarget.X = jCoord;
                        distanceToNearest = distanceToCreature;
                    }
                }
            }

            return nearestTarget;
        }
    }
}

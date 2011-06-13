using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Code.Units
{
    struct ClampArea
    {
        public int leftCut;
        public int rightCut;
        public int topCut;
        public int bottomCut;
    };

    //Contains all the code for a unit.
    class Unit
    {
        //These allow the unit to remember its previous location and
        //return there in the event of an invalid move
        public int originalI;
        public int originalJ;

        //this allows us to set the colour
        public Sides side;

        //This gives the unit an awareness of the other units on the playing field
        //so it can destroy itself or better than that, enemy units
        protected GameGrid grid;

        public Constants.UnitType Type;

        public bool CanFly;

        public Texture2D texture;
        public Vector2 position;
        public int height;
        public int width;
        public AI mAIRef;
        public bool isSelected;

        //used to determine which enemies this unit
        //type should attack first
        public Constants.UnitType[] attackablePriorities;

        public Unit(GameGrid gridRef, AI AIRef, Texture2D inTexture)
        {
            originalI = 0;
            originalJ = 0;
            grid = gridRef;
            mAIRef = AIRef;
            side = new Sides();
            Type = Constants.UnitType.Undefined;
            CanFly = false;
            position = new Vector2(0, 0);
            texture = inTexture;
            isSelected = false;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            float scale = ScreenDimensions().X / texture.Width;

            Vector2 renderPosition = new Vector2(
                parentPosition.X + position.X,
                parentPosition.Y + position.Y
            );

            Color color = Color.White;

            if (side.mColour == Constants.RED)
            {
                color = Color.Red;
            }
            else if (side.mColour == Constants.BLUE)
            {
                color = Color.Blue;
            }

            if (isSelected)
            {
                color = Color.Yellow;
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

        public virtual bool CheckOccupied(int i, int j)
        {
            throw new NotImplementedException();
        }

        public virtual void SetLocation(int newLocationI, int newLocationJ)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveUnits(int p, int p_2)
        {
            throw new NotImplementedException();
        }

        public virtual void Move(int p, int p_2, bool p_3)
        {
            throw new NotImplementedException();
        }

        public virtual bool CheckColour(int i, int j)
        {
            throw new NotImplementedException();
        }

        public virtual bool CanAttack(Constants.UnitType unitType)
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
                returnValue.leftCut = (int)(ScreenDimensions().X / 2);
            }
            if (returnValue.topCut < 0)
            {
                returnValue.topCut = (int)(ScreenDimensions().Y / 2);
            }
            if (returnValue.rightCut + ScreenDimensions().X / 2 > mAIRef.Width())
            {
                returnValue.rightCut = (int)position.X;
            }
            if (returnValue.bottomCut + ScreenDimensions().Y / 2 > mAIRef.Height())
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

        public virtual Vector2 ScreenDimensions()
        {
            throw new NotImplementedException();
        }

        /*
            This tells us if a square is actually an enemy mine location. If the
            unit in question is a deminer it returns false since they can move accross
            those just fine.
        */
        public virtual bool IsEnemyMine(int i, int j)
        {
            return (Math.Floor((double)(i / 2)) % 2 == Math.Floor((double)(j / 2)) % 2)
                && (grid.mMines[i / 2, j / 2].side.mColour != side.mColour);
        }

        /*
            This tells us if a square is a friendly mine location.
        */
        public virtual bool IsFriendlyMine(int i, int j)
        {
            return (Math.Floor((double)(i / 2)) % 2 == Math.Floor((double)(j / 2)) % 2)
                && (grid.mMines[i / 2, j / 2].side.mColour == side.mColour);
        }

        /*
            Finds the space that the unit most wants to move into
        */
        public virtual Vector2 GetNearestTarget()
        {
            int opposingSide = Constants.RED;

            if (side.mColour == Constants.RED)
            {
                opposingSide = Constants.BLUE;
            }

            Vector2 originalPoint = new Vector2(GetJ(), GetI());
            Vector2 nearestTarget = new Vector2(-1, -1);

            double distanceToNearest = mAIRef.GetDistanceToCoordinates(originalPoint, 0, 0);

            for (var i = 0; i < attackablePriorities.Length; i++)
            {
                Constants.UnitType typeToAttack = attackablePriorities[i];

                for (var t = 0; t < mAIRef.Units[opposingSide, (int)typeToAttack].Length; ++t)
                {
                    var iCoord = mAIRef.Units[opposingSide, (int)typeToAttack][t].GetI();
                    var jCoord = mAIRef.Units[opposingSide, (int)typeToAttack][t].GetJ();
                    double distanceToUnit = mAIRef.GetDistanceToCoordinates(originalPoint, iCoord, jCoord);

                    //if an opponent is on their mine they're safe so don't bother with them.
                    if ((distanceToUnit < distanceToNearest && IsEnemyMine(iCoord, jCoord) == false)
                       || nearestTarget.Y == -1)
                    {
                        nearestTarget.Y = iCoord;
                        nearestTarget.X = jCoord;
                        distanceToNearest = distanceToUnit;
                    }
                }
            }

            return nearestTarget;
        }
    }
}

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

        public void Render(SpriteBatch spriteBatch)
        {
            float scale = ScreenDimensions().X / texture.Width;

            Color color = Color.White;

            if (side.mColour == Constants.RED)
            {
                color = Color.Red;
            }
            else if (side.mColour == Constants.BLUE)
            {
                color = Color.Blue;
            }

            spriteBatch.Draw(
                texture,
                position,
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



        /*
         * Uncomment this as features are implemented
         
        //When user clicks on this unit...
        this.onPress = function()
        {
	        //We check to see if it's the units turn
	        if(this._parent.currentTurn == mColour)
	        {
		        //set the movement area of the piece.
		        var clampArea = GetClampArea();
		        startDrag(this, true, clampArea.leftCut, clampArea.topCut, clampArea.rightCut, clampArea.bottomCut);
	        }
        }

        this.onRelease = function()
        {
	        if(this._parent.currentTurn == mColour)
	        {
		        var j = (this._x - this._x % this._width) / grid[originalI][originalJ]._width;
		        var i = (this._y - this._y % this._height) / grid[originalI][originalJ]._height;
		        if(CheckOccupied(i, j))
		        {
			        if(CheckColour(i,j) == true)
			        {
				        RemoveUnits(i, j);
				        Move(i, j, true);
				        if(mColour == RED)
				        {
					        this._parent.CheckMines(BLUE);
				        }
				        else if(mColour == BLUE)
				        {
					        this._parent.CheckMines(RED);
				        }
			        }
			        else
			        {
				        Move(originalI, originalJ, false);
			        }
		        }
		        else
		        {
			        Move(i, j, true);
			        if(mColour == RED)
			        {
				        this._parent.CheckMines(BLUE);
			        }
			        else if(mColour == BLUE)
			        {
				        this._parent.CheckMines(RED);
			        }
		        }
		        stopDrag();
	        }
        }

        

        this.onReleaseOutside = this.onRelease;
        */
        
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
    }
}

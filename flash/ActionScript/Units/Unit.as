//Contains all the code for a unit. This calls functions in the code that include
//it, so it enables a somewhat hacked form of polymorphism.


//These allow the unit to remember its previous location and
//return there in the event of an invalid move
var originalI = 0;
var originalJ = 0;

//This gives the unit an awareness of the other units on the playing field
//so it can destroy itself or better than that, enemy units
var grid = this._parent.mcGrid.mGrid;

//this allows us to set the colour and use only one library movie clip per unit instead of two
#include "../Sides.as"

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

this.GetI = function()
{
	return ((this._y - this._y % this._height) / grid[0][0]._height);
}

this.GetJ = function()
{
	return ((this._x - this._x % this._width) / grid[0][0]._width);
}

this.Height = function():Number
{
	return (this._height / grid[0][0]._height);
}

this.Width = function():Number
{
	return (this._width / grid[0][0]._width);
}

this.onReleaseOutside = this.onRelease;
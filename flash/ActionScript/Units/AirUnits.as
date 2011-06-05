#include "Unit.as"

var GroundUnit = false;
var AirUnit = true;

this.CheckOccupied = function(i, j):Boolean
{
	return (grid[i][j].occupied || grid[i + 1][j].occupied
			|| grid[i][j + 1].occupied || grid[i + 1][j + 1].occupied);
}

this.Move = function(newLocationI, newLocationJ, changeTurns)
{
	if(changeTurns == true)
	{
		this._parent.currentTurn = (this._parent.currentTurn + 1) % 2;
	}
	for(var i = 0; i < 2; ++i)
	{
		for(var j = 0; j < 2; ++j)
		{
			grid[originalI + i][originalJ + j].occupied = false;
			grid[originalI + i][originalJ + j].TurnNeutral();
			grid[originalI + i][originalJ + j].occupiedUnit = null;
		}
	}
	
	this.SetLocation(newLocationI, newLocationJ);
}

this.RemoveUnits = function(newLocationI, newLocationJ)
{
	for(var i = 0; i < 2; ++i)
	{
		for(var j = 0; j < 2; ++j)
		{
			if(grid[newLocationI + i][newLocationJ + j].occupiedUnit != null)
			{
				this._parent.RemoveUnit(grid[newLocationI + i][newLocationJ + j].occupiedUnit);
			}
		}
	}
}

this.SetLocation = function(newLocationI, newLocationJ)
{
	for(var i = 0; i < 2; ++i)
	{
		for(var j = 0; j < 2; ++j)
		{
			grid[newLocationI + i][newLocationJ + j].occupied = true;
			grid[newLocationI + i][newLocationJ + j].mColour = mColour;
			grid[newLocationI + i][newLocationJ + j].occupiedUnit = this;
		}
	}
	this._x = newLocationJ * grid[0][0]._width + (this._width / 2);
	this._y = newLocationI * grid[0][0]._height + (this._height / 2);
}
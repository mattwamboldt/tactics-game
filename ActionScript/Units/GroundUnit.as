#include "Unit.as"

var GroundUnit = true;
this.CheckOccupied = function(i, j):Boolean
{
	return grid[i][j].occupied;
}

this.SetLocation = function(newLocationI, newLocationJ)
{
	grid[newLocationI][newLocationJ].occupied = true;
	grid[newLocationI][newLocationJ].mColour = mColour;
	grid[newLocationI][newLocationJ].occupiedUnit = this;
	this._x = newLocationJ * grid[0][0]._width + (this._width / 2);
	this._y = newLocationI * grid[0][0]._height + (this._height / 2);
}
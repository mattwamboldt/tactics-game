#include "GroundUnit.as"
var Type:Number = SOLDIER;

//used to determine which enemies this unit
//type should attack first
var attackablePriorities:Array = [GRANADIER, MINER, SOLDIER];

this.CheckColour = function(i, j):Boolean
{
	return grid[i][j].mColour != mColour && grid[i][j].occupiedUnit.GroundUnit;
}

this.GetClampArea = function():Object
{
	var returnValue = new Object();
	returnValue.leftCut = this._x - this._width;
	returnValue.rightCut = this._x + this._width; 
	returnValue.topCut = this._y - this._height;
	returnValue.bottomCut = this._y + this._height;
	originalJ = (this._x - this._x % this._width) / grid[originalI][originalJ]._width;
	originalI = (this._y - this._y % this._height) / grid[originalI][originalJ]._height;
	
	//Clamp the area so that it fits within the board.
	if( returnValue.leftCut < 0  )
	{
		returnValue.leftCut = this._width / 2;
	}
	if( returnValue.topCut < 0  )
	{
		returnValue.topCut = this._height / 2;
	}
	if( returnValue.rightCut + this._width / 2 > this._parent._width )
	{
		returnValue.rightCut = this._x;
	}
	if( returnValue.bottomCut + this._height / 2 > this._parent._height )
	{
		returnValue.bottomCut = this._y;
	}
	
	return returnValue;
}

this.Move = function(newLocationI, newLocationJ, changeTurns)
{
	if(changeTurns == true)
	{
		this._parent.currentTurn = (this._parent.currentTurn + 1) % 2;
	}
	grid[originalI][originalJ].occupied = false;
	grid[originalI][originalJ].TurnNeutral();
	grid[originalI][originalJ].occupiedUnit = null;
	this.SetLocation(newLocationI, newLocationJ);
}

this.RemoveUnits = function(newLocationI, newLocationJ)
{
	this._parent.RemoveUnit(grid[newLocationI][newLocationJ].occupiedUnit);
}

this.CanAttack = function(unitType:Number):Boolean
{
	switch(unitType)
	{
		case SOLDIER:
		case GRANADIER:
		case MINER:
			return true;
		default:
			return false;
	}
}
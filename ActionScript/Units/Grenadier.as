#include "GroundUnit.as"
var Type:Number = GRANADIER;

//used to determine which enemies this unit
//type should attack first
var attackablePriorities:Array = [BOMBER, GRANADIER, FIGHTER, MINER, SOLDIER];

this.CheckColour = function(i, j):Boolean
{
	return grid[i][j].mColour != mColour;
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
	grid[originalI][originalJ].occupied = false;
	grid[originalI][originalJ].TurnNeutral();
	grid[originalI][originalJ].occupiedUnit = null;
	this.SetLocation(newLocationI, newLocationJ);
	if(changeTurns == true)
	{
		this._parent.currentTurn = (this._parent.currentTurn + 1) % 2;
	}
}

this.RemoveUnits = function(newLocationI, newLocationJ)
{
	var unit = grid[newLocationI][newLocationJ].occupiedUnit;
	var UnitLocationI = (unit._y - unit._y % unit._height) / grid[originalI][originalJ]._height;
	var UnitLocationJ = (unit._x - unit._x % unit._width) / grid[originalI][originalJ]._width;
	
	if(unit.AirUnit)
	{
		for(var i = 0; i < 2; ++i)
		{
			for(var j = 0; j < 2; ++j)
			{
				grid[UnitLocationI + i][UnitLocationJ + j].occupied = false;
				grid[UnitLocationI + i][UnitLocationJ + j].TurnNeutral();
				grid[UnitLocationI + i][UnitLocationJ + j].occupiedUnit = null;
			}
		}
	}
	this._parent.RemoveUnit(unit);
}

this.CanAttack = function(unitType:Number):Boolean
{
	switch(unitType)
	{
		case SOLDIER:
		case GRANADIER:
		case MINER:
		case BOMBER:
		case FIGHTER:
			return true;
		default:
			return false;
	}
}
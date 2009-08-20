#include "AirUnits.as"
var Type:Number = BOMBER;

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

this.CheckColour = function(i, j):Boolean
{
	//Bombers destroy a block of units if one of they're own aren't on it and also can't destroy fighters
	return ((grid[i][j].mColour != mColour && !grid[i][j].occupiedUnit.AirUnit)
			&& (grid[i + 1][j].mColour != mColour && !grid[i + 1][j].occupiedUnit.AirUnit)
			&& (grid[i][j + 1].mColour != mColour && !grid[i][j + 1].occupiedUnit.AirUnit)
			&& (grid[i + 1][j + 1].mColour != mColour && !grid[i + 1][j + 1].occupiedUnit.AirUnit));
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
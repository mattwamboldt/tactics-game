//This contains functions for changing the allegiance of
//something, be it a unit or mine. The squares use their own version
//but same member variable

#include "ActionScript/Constants.as"

var mColour:Number = NEUTRAL;

this.TurnBlue = function()
{
	mColour = BLUE;
	redSide._visible = false;
	blueSide._visible = true;
	neutralSide._visible = false;
}

this.TurnNeutral = function()
{
	mColour = NEUTRAL;
	redSide._visible = false;
	blueSide._visible = false;
	neutral._visible = true;
}

this.TurnRed = function()
{
	mColour = RED;
	redSide._visible = true;
	blueSide._visible = false;
	neutralSide._visible = false;
}

this.ChangeColour = function(colour)
{
	mColour = colour;
	
	if(mColour == BLUE)
	{
		redSide._visible = false;
		blueSide._visible = true;
		neutralSide._visible = false;
	}
	else if(mColour == RED)
	{
		redSide._visible = true;
		blueSide._visible = false;
		neutralSide._visible = false;
	}
	else
	{
		redSide._visible = false;
		blueSide._visible = false;
		neutral._visible = true;
	}
}
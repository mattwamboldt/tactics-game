//This creates all the on screen objects
mine._visible = false;
var currentTurn = 0;
var gameInterval = null;
var bIsAIPlaying:Boolean = false;
var redIsHuman:Boolean = true;
var blueIsHuman:Boolean = false;

var mGrid:Array = new Array(); //this holds the individual squares that make up the game board
#include "ActionScript/Constants.as"

//Create and place the faoundation layer of small squares
for( var i = 0; i < GRID_WIDTH; ++i)
{
	mGrid[i] = new Array();
	for( var j = 0; j < GRID_HEIGHT; ++j)
	{
		duplicateMovieClip("square", "square" + i + "i" + j +"j", this.getNextHighestDepth());
		mGrid[i][j] = this["square" + i + "i" + j +"j"];
		mGrid[i][j]._x = j * 25;
		mGrid[i][j]._y = i * 25;
	}
}

var mMineGrid:Array = new Array(); //this holds all the mines

//Create and place the mine squares
for( var i = 0; i < 8; ++i )
{
	mMineGrid[i] = new Array();
	for( var j = 0; j < 8; ++j )
	{
		//this check is what generates the checker pattern of the mines
		if(i % 2 == j % 2)
		{
			duplicateMovieClip("mine", "mine" + i + "i" + j +"j", this.getNextHighestDepth());
			mMineGrid[i][j] = this["mine" + i + "i" + j +"j"];
			mMineGrid[i][j]._x = j * 50;
			mMineGrid[i][j]._y = i * 50;
		}
		else
		{
			mMineGrid[i][j] = null;
		}
	}
}

//create the root array
var mUnits:Array = new Array();

//create the sides
mUnits[RED] = new Array();
mUnits[BLUE] = new Array();

//create the unit types for each side
mUnits[RED][BOMBER] = new Array();
mUnits[RED][FIGHTER] = new Array();
mUnits[RED][SOLDIER] = new Array();
mUnits[RED][GRANADIER] = new Array();
mUnits[RED][MINER] = new Array();
mUnits[BLUE][BOMBER] = new Array();
mUnits[BLUE][FIGHTER] = new Array();
mUnits[BLUE][SOLDIER] = new Array();
mUnits[BLUE][GRANADIER] = new Array();
mUnits[BLUE][MINER] = new Array();

for( var i = 0; i < 4; ++i )
{
	this.attachMovie( "bomberUnit", "blueBomber" + i, this.getNextHighestDepth() );
	mUnits[BLUE][BOMBER][i] = this["blueBomber" + i];
	this.attachMovie( "bomberUnit", "redBomber" + i, this.getNextHighestDepth() );
	mUnits[RED][BOMBER][i] = this["redBomber" + i];
	
	this.attachMovie( "fighterUnit", "blueFighter" + i, this.getNextHighestDepth() );
	mUnits[BLUE][FIGHTER][i] = this["blueFighter" + i];
	this.attachMovie( "fighterUnit", "redFighter" + i, this.getNextHighestDepth() );
	mUnits[RED][FIGHTER][i] = this["redFighter" + i];
}

for( var i = 0; i < 8; ++i )
{
	this.attachMovie( "deminerUnit", "blueDeminer" + i, this.getNextHighestDepth() );
	mUnits[BLUE][MINER][i] = this["blueDeminer" + i];
	this.attachMovie( "deminerUnit", "redDeminer" + i, this.getNextHighestDepth() );
	mUnits[RED][MINER][i] = this["redDeminer" + i];
	
	this.attachMovie( "grenadierUnit", "blueGranadier" + i, this.getNextHighestDepth() );
	mUnits[BLUE][GRANADIER][i] = this["blueGranadier" + i];
	this.attachMovie( "grenadierUnit", "redGranadier" + i, this.getNextHighestDepth() );
	mUnits[RED][GRANADIER][i] = this["redGranadier" + i];
}

for( var i = 0; i < 16; ++i )
{
	this.attachMovie( "soldierUnit", "blueSoldier" + i, this.getNextHighestDepth() );
	mUnits[BLUE][SOLDIER][i] = this["blueSoldier" + i];
	this.attachMovie( "soldierUnit", "redSoldier" + i, this.getNextHighestDepth() );
	mUnits[RED][SOLDIER][i] = this["redSoldier" + i];
}

//This function Checks to see if mines need to be changed to teh given colour
//It also checks for any units that are on mines, that shoudl be deleted
this.CheckMines = function(colour)
{
	//The outer loop goes through the mines
	for( var i = 0; i < 8; ++i )
	{
		for( var j = 0; j < 8; ++j )
		{
			//only gets us mines that have a value
			if(i % 2 == j % 2)
			{
				//inner loops checks the mine itself
				for(var t = 0; t < 2; ++t)
				{
					for(var u = 0; u < 2; ++u)
					{
						var square = mGrid[i*2 + t][j*2 + u];
						if(square.occupiedUnit.Type == MINER
							&& square.mColour == colour)
						{
							mMineGrid[i][j].ChangeColour(colour);
						}
						else if(square.occupiedUnit.mColour != mMineGrid[i][j].mColour
								&& square.occupiedUnit.Type != MINER
								&& square.occupiedUnit.GroundUnit)
						{
							RemoveUnit(square.occupiedUnit);
							square.occupied = false;
							square.TurnNeutral();
						}
					}
				}
			}
		}
	}

	//this is the human vs ai situation
	if(!IsAIvsAI() && !IsHumanVHuman())
	{
		if(CheckVictory() == false
			&& (redIsHuman && colour == BLUE
				|| blueIsHuman && colour == RED))
		{
			AIPass(colour);
		}
	}
}

this.NextAIMove = function()
{
	if(CheckVictory() == false)
	{
		AIPass(currentTurn);
	}
	else
	{
		clearInterval(gameInterval);
		gameInterval = null;
	}
}

this.CheckAILoop = function()
{
	//here we need to start the loop version of the ai
	if(IsAIvsAI())
	{
		if(gameInterval == null)
		{
			//150 apears to be the lowest we can go before
			//causing weird double move and turn skipping problems
			gameInterval = setInterval(NextAIMove, 150);
		}
	}
	else if(gameInterval != null)
	{
		//stop the ai loop
		clearInterval(gameInterval);
		gameInterval = null;
	}
}

this._parent.redBtn.onRelease = function()
{
	//first we toggle to the opposite control scheme
	redIsHuman = !redIsHuman;
	
	if(redIsHuman)
	{
		this.txtController.text = "Human";
	}
	else
	{
		this.txtController.text = "AI";
	}
	
	CheckAILoop();
}

this._parent.blueBtn.onRelease = function()
{
	//first we toggle to the opposite control scheme
	blueIsHuman = !blueIsHuman;
	
	if(blueIsHuman)
	{
		this.txtController.text = "Human";
	}
	else
	{
		this.txtController.text = "AI";
	}

	CheckAILoop();
}

//lets us know the selected multiplayer modes
this.IsHumanVHuman = function():Boolean
{
	return redIsHuman && blueIsHuman;
}

this.IsAIvsAI = function():Boolean
{
	return !redIsHuman && !blueIsHuman;
}

//This checks to see who, if anyone, hsa won, and displays the appropriate message
this.CheckVictory = function():Boolean
{
	var winningColour:Number = mMineGrid[0][0].mColour
	var hasWon:Boolean = true;
	
	for( var i = 0; i < 8; ++i )
	{
		for( var j = 0; j < 8; ++j )
		{
			if(i % 2 == j % 2 && winningColour != mMineGrid[i][j].mColour)
			{
				hasWon = false;
			}
		}
	}
	
	if(hasWon == false)
	{
		//we need to check for a destruction victory for blue
		if(mUnits[RED][SOLDIER].length == 0
			&& mUnits[RED][FIGHTER].length == 0
			&& mUnits[RED][SOLDIER].length == 0
			&& mUnits[RED][GRANADIER].length == 0
			&& mUnits[RED][MINER].length == 0 )
		{
			winningColour = BLUE;
			hasWon = true;
		}
		else if(mUnits[BLUE][BOMBER].length == 0
			&& mUnits[BLUE][FIGHTER].length == 0
			&& mUnits[BLUE][SOLDIER].length == 0
			&& mUnits[BLUE][GRANADIER].length == 0
			&& mUnits[BLUE][MINER].length == 0 )
		{
			winningColour = RED;
			hasWon = true;
		}
	}
	
	if(hasWon == true)
	{
		if(winningColour == BLUE)
		{
			this._parent.txtWinLose.text = "Blue has won!!!";
		}
		else
		{
			this._parent.txtWinLose.text = "Red has won!!!";
		}
		
		this._parent.txtWinLose._visible = true;
	}
	return hasWon;
}

//This will search Through the arrays and eliminate the given unit
this.RemoveUnit = function(Unit)
{
	//find the units array
	var UnitsArray:Array = mUnits[Unit.mColour][Unit.Type];
	
	//delete the unit from its array
	var arrayLength = UnitsArray.length
	for( var i = 0; i < arrayLength; ++i)
	{
		if(UnitsArray[i] == Unit)
		{
			UnitsArray.splice(i, 1);
		}
	}
	
	//we delete the movie clip from the stage
	Unit.removeMovieClip();
}

this._parent.txtWinLose.text = "";
this._parent.txtWinLose.selectable = false;

#include "ActionScript/AI.as"
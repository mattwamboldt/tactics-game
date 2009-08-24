/*
	AI History: MSW
	REV 1(COMPLETED)
	We're gonna make this horribly stupid at first
	It's gonna randomly pick a unit and move it
	to a random location, and it'll attempt to do so
	until it actually makes a move.
	
	REV 2(COMPLETED)
	Adding in a function to check for units that are
	in attack range, and taking them out.
	
	REV 3(COMPLETED)
	Now we need to stop the AI from committing suicide.
	Adding a function to determine if destination is
	an opponent or nuetral mine, and if so avoid it at
	all costs.
	
	REV 4(COMPLETED)
	Time to add some aggression to our passive AI. We
	need to make them actively move toward the user.
	So the first step to this would be to find a random
	target and move in their direction. To do this but
	keep some randomness, we'll change the pickrandom
	function to pick an enemy unit and move in their direction.
	Basic pathfinding here we come.
	
	REV 5(Completed)
	Given that we have multiple modes of victory we need
	to make the AI attempt them. That means moving de-miners
	on occassion.
	
	REV 6(Completed)
	So now we want to improve the AI's aiming, by going through
	all the units that our ranomly chosen unit can attack, and finding
	the one that has the closest target to attack.
	
	REV 7(Completed)
	The deminers are very stupid with no goals. they should be moving
	towards the nearest mines just as other units move towards their prey.
	
	REV 8(Completed)
	Reworking to use a global array of arrays to store units so we
	can iterate over them more effectively. Also changing to numbers
	from strings to allow easier implementation of multiplayer.
	
	REV 9(Completed)
	Add Multiple play modes. player vs player, player vs ai, and ai vs ai.
	
	REV 10(Completed)
	Create an array of priorities for each unit type to differentiate
	what they go after.
	
	REV 11(Completed)
	Change the getMoveList Function to return an array of objects representing
	the i,j of each move, as well as a score which at this point will be 100/the distance.
	Then change the attackrandom to use this score to determine the selected move,
	instead of distance. This is the start of weight based ai.
	
	REV 12(Completed)
	Remove attack attackables function. Add a function to return the value of each unit destroyed
	by making a move based on the unit priorities and add them together. Then add this to
	the previous score. Tune and enjoy sometimes units not getting destroyed when they are adjacent.
	
	REV 13
	Add function to check if a unit in a location is vulnerable to attack. Use this to negatively
	impact the score of moving to that location. Tune and enjoy units no longer commiting suicide.
	
	REV 14
	Add loops to find the unit in the entire army with the best scoring move, and move that unit.
	Tune and this means the opposition will start to act like they are more than just one random
	unit on the feild. We may have to go with random from the top five to keep it from being too hard.
	
	REV 15
	Add shortest path function, and use that to get the distance. Also use that to determine if a unit 
	is still trapped and negatively effect score. This may prove too crazy. If done it will be the last
	large ai change until someone pro comes along.
*/

//This function performs All AI functions
this.AIPass = function(colourToRun:Number)
{
	var randomness = Math.round(Math.random() * 100);
	//40% chance to try to take mines
	if(randomness >= 60)
	{
		if(TakeMines(colourToRun) == false)
		{
			AttackRandom(colourToRun);
		}
	}
	//60% chance to look for other units to destroy
	else
	{
		AttackRandom(colourToRun);
	}
}

/*
	This function will check all the miner units, pick one and move
	them.
*/
this.TakeMines = function(colourToRun:Number):Boolean
{
	var movableDeminers:Array = new Array();
	for(var t = 0; t < mUnits[colourToRun][MINER].length; ++t)
	{
		if(CanUnitMove(mUnits[colourToRun][MINER][t]) == true)
		{
			movableDeminers.push(mUnits[colourToRun][MINER][t]);
		}
	}
	
	if(movableDeminers.length == 0)
	{
		return false;
	}
	
	var possibleMoves:Array;
	var randomMiner = movableDeminers[Math.round(Math.random() * (movableDeminers.length - 1))];
	var closestMineLocation = GetNearestMine(randomMiner);
	possibleMoves = GetMoveList(randomMiner, closestMineLocation);
	
	var bestMove = possibleMoves[0];
	
	for(var t = 0; t < possibleMoves.length; ++t)
	{
		if(bestMove.score < possibleMoves[t].score)
		{
			bestMove = possibleMoves[t];
		}
	}

	randomMiner.Move(bestMove.i, bestMove.j, true);
	
	if(colourToRun == BLUE)
	{
		CheckMines(RED);
	}
	else
	{
		CheckMines(BLUE);
	}
	
	CheckVictory();
	
	return true;
}

/*
	This tells us if a square is actually an enemy mine location. If the
	unit in question is a deminer it returns false since they can move accross
	those just fine.
*/
this.IsEnemyMine = function(i, j, Unit):Boolean
{
	return Unit.Type != MINER && (Math.floor(i / 2) % 2 == Math.floor(j / 2) % 2) && (mMineGrid[Math.floor(i / 2)][Math.floor(j / 2)].mColour != Unit.mColour);
}

/*
	This tells us if a square is adjacent to units that will destroy the passed in units.
*/
this.IsDeathTrap = function(i, j, Unit):Boolean
{
	return false;
}

/*
	This function will pick a random unit that can move
	and a random unit to target, then try to navigate to
	that target. Wish me luck.
*/
this.AttackRandom = function(colourToRun:Number)
{
	var unit = null;
	while(CanUnitMove(unit) == false)
	{
		unit = PickRandomUnit(colourToRun);
	}
	
	var target = GetNearestUnit(unit);
	var possibleMoves:Array = GetMoveList(unit, target);
	var bestMove = possibleMoves[0];
	
	for(var t = 0; t < possibleMoves.length; ++t)
	{
		if(bestMove.score < possibleMoves[t].score)
		{
			bestMove = possibleMoves[t];
		}
	}
	
	if(unit.CheckOccupied(bestMove.i, bestMove.j))
	{
		unit.RemoveUnits(bestMove.i, bestMove.j);
	}
	
	unit.Move(bestMove.i, bestMove.j, true);
	
	if(colourToRun == BLUE)
	{
		CheckMines(RED);
	}
	else
	{
		CheckMines(BLUE);
	}
}

/*
	Returns the unit that is closest to the given unit
*/
this.GetNearestUnit = function(sourceUnit)
{	
	var opposingSide:Number = RED;
	
	if(sourceUnit.mColour == RED)
	{
		opposingSide = BLUE;
	}
	
	var originalPoint = new Object();
	originalPoint.i = sourceUnit.GetI();
	originalPoint.j = sourceUnit.GetJ();
	
	var nearestTarget = new Object();
	nearestTarget.i = -1;
	nearestTarget.j = -1;
	
	var distanceToNearest = GetDistanceToCoordinates(originalPoint, 0, 0);
	
	for(var i = 0; i < sourceUnit.attackablePriorities.length; i++)
	{
		var typeToAttack = sourceUnit.attackablePriorities[i];
		
		for(var t = 0; t < mUnits[opposingSide][typeToAttack].length; ++t)
		{
			var iCoord = mUnits[opposingSide][typeToAttack][t].GetI();
			var jCoord = mUnits[opposingSide][typeToAttack][t].GetJ();
			var distanceToUnit = GetDistanceToCoordinates(originalPoint, iCoord, jCoord);
			
			if(distanceToUnit < distanceToNearest
			   || nearestTarget.i == -1)
			{
				nearestTarget.i = iCoord;
				nearestTarget.j = jCoord;
				distanceToNearest = distanceToUnit;
			}
		}
	}
	
	return nearestTarget;
}

/*
	Returns the mine that is closest to the given unit
*/
this.GetNearestMine = function(sourceUnit)
{
	var originalPoint = new Object();
	originalPoint.i = sourceUnit.GetI();
	originalPoint.j = sourceUnit.GetJ();
	
	var nearestMine = new Object();
	nearestMine.i = -1;
	nearestMine.j = -1;
	
	var distanceToNearest = GetDistanceToCoordinates(originalPoint, 0, 0);
	
	//The outer loop goes through the mines
	for( var i = 0; i < 8; ++i )
	{
		for( var j = 0; j < 8; ++j )
		{
			//only gets us mines that have a value
			if(i % 2 == j % 2)
			{
				//we want to head for mines of the opposite colour
				if(mMineGrid[i][j].mColour != sourceUnit.mColour)
				{
					//inner loops checks the mine itself
					for(var t = 0; t < 2; ++t)
					{
						for(var u = 0; u < 2; ++u)
						{
							var iCoord = i*2 + t;
							var jCoord = j*2 + u;
							var distanceToMineSquare = GetDistanceToCoordinates(originalPoint, iCoord, jCoord);

							if(distanceToMineSquare < distanceToNearest
							   || nearestMine.i == -1)
							{
								nearestMine.i = iCoord;
								nearestMine.j = jCoord;
								distanceToNearest = distanceToMineSquare;
							}
						}
					}
				}
			}
		}
	}
	
	return nearestMine;
}

/*
	Return the number of squares for the shortest path between the
	point and unit, avoiding mines, possibly adding extra for potentially
	dangerous moves to discourage movement in that direction.

this.GetDistanceOfShortestPath = function(point, unit):Number
{
}
*/

/*
	Returns the distance between two points
*/
this.GetDistanceToCoordinates = function(startPoint, endPointI, endPointJ):Number
{
	var jDifference = startPoint.j - endPointJ;
	var iDifference = startPoint.i - endPointI;
	return Math.sqrt( (jDifference * jDifference) + (iDifference * iDifference) );
}

/*
	Returns the distance between the point given and the unit
*/
this.GetDistance = function(point, unit):Number
{
	return GetDistanceToCoordinates(point, unit.GetI(), unit.GetJ());
}

//this rates the
this.GetDestructionScore = function(sourceUnit, targetUnit):Number
{
	for(var i = 0; i < sourceUnit.attackablePriorities.length; i++)
	{
		if(sourceUnit.attackablePriorities[i] == targetUnit.Type)
		{
			return (NUM_UNIT_TYPES - i) * 200;
		}
	}
	
	return 0;
}

/*
	This retreives all the locations the unit can move to.  
*/
this.GetMoveList = function(unit, target):Array
{
	var moveList:Array = new Array();
	var clamp = unit.GetClampArea();
	var UnitLeftBound = (clamp.leftCut - clamp.leftCut % unit._width) / mGrid[0][0]._width;
	var UnitRightBound = (clamp.rightCut - clamp.rightCut % unit._width) / mGrid[0][0]._width;
	var UnitTopBound = (clamp.topCut - clamp.topCut % unit._height) / mGrid[0][0]._height;
	var UnitBottomBound = (clamp.bottomCut - clamp.bottomCut % unit._height) / mGrid[0][0]._height;
	
	for(var i = UnitTopBound; i <= UnitBottomBound; i += (unit._width / mGrid[0][0]._width))
	{
		for( var j = UnitLeftBound; j <= UnitRightBound; j += (unit._height / mGrid[0][0]._height))
		{
			//staying place is the only invalid move right now
			if(i != unit.GetI() || j != unit.GetJ())
			{
				var newMove = new Object();
				newMove.i = i;
				newMove.j = j;
				//set score to use distance
				trace("adding move to " + i + " " + j);
				trace("distance to " + target.i + ", " + target.j + " is " + GetDistanceToCoordinates(newMove, target.i, target.j));
				newMove.score = 100 / (GetDistanceToCoordinates(newMove, target.i, target.j) + 1); //add 1 to prevent infinity
				trace("score is now " + newMove.score);
				
				//factor in if the move will take another unit
				if(unit.CheckOccupied(i, j))
				{
					if(unit.CheckColour(i,j))
					{
						newMove.score += GetDestructionScore(unit, mGrid[i][j].occupiedUnit);
						trace("we found an oponnent, kill IT! " + newMove.score);
					}
					else
					{
						trace("can't kill my allies");
						//cannot move onto friendly squares
						continue;
					}
				}
				else if(IsEnemyMine(i, j, unit))
				{
					trace("it's usually not a good idea to commit intentional suicide.");
					//avoid unoccupied enemy mines
					continue;
				}
				
				//factor in if the move will get you killed
				if(IsEnemyMine(i, j, unit) || IsDeathTrap(i, j, unit))
				{
					newMove.score -= 900;
					trace("This may kill me, but maybe its good for the group " + newMove.score);
				}
				
				moveList.push(newMove);
			}
		}
	}
	
	return moveList;
}

/*
	This is a helper function that will take the units current
	location, look at all possible locations around it, and return
	whether it finds one it can move to.
*/
this.CanUnitMove = function(unit):Boolean
{
	if(unit.Type == null)
	{
		return false;
	}
	
	var clamp = unit.GetClampArea();
	var UnitLeftBound = (clamp.leftCut - clamp.leftCut % unit._width) / mGrid[0][0]._width;
	var UnitRightBound = (clamp.rightCut - clamp.rightCut % unit._width) / mGrid[0][0]._width;
	var UnitTopBound = (clamp.topCut - clamp.topCut % unit._height) / mGrid[0][0]._height;
	var UnitBottomBound = (clamp.bottomCut - clamp.bottomCut % unit._height) / mGrid[0][0]._height;
	
	for(var i = UnitTopBound; i <= UnitBottomBound; i += (unit._width / mGrid[0][0]._width))
	{
		for( var j = UnitLeftBound; j <= UnitRightBound; j += (unit._height / mGrid[0][0]._height))
		{
			if((unit.CheckOccupied(i, j) == false || unit.CheckColour(i,j) == true) && IsEnemyMine(i, j, unit) == false)
			{
				return true;
			}
		}
	}
	
	return false;
}

/*
	Another helper function that picks a random unit from the chosen side
*/
this.PickRandomUnit = function(side:Number):Object
{
	var unit = null;
	var arrayToUse = null;
	do
	{
		unit = null;
		switch(Math.round(Math.random() * 4))
		{
			case 0:
				if(mUnits[side][BOMBER].length > 0)
				{
					arrayToUse = mUnits[side][BOMBER];
				}
				else if(mUnits[side][FIGHTER].length > 0)
				{
					arrayToUse = mUnits[side][SOLDIER];
				}
				else if(mUnits[side][GRANADIER].length > 0)
				{
					arrayToUse = mUnits[side][GRANADIER];
				}
				else if(mUnits[side][SOLDIER].length > 0)
				{
					arrayToUse = mUnits[side][FIGHTER];
				}
				else if(mUnits[side][MINER].length > 0)
				{
					arrayToUse = mUnits[side][MINER];
				}
				else
				{
					trace("This shouldn't be getting called, the ai lost and user should know!!");
					return;
				}
				break;
				
			case 1:
				if(mUnits[side][FIGHTER].length > 0)
				{
					arrayToUse = mUnits[side][FIGHTER];
				}
				else if(mUnits[side][GRANADIER].length > 0)
				{
					arrayToUse = mUnits[side][GRANADIER];
				}
				else if(mUnits[side][BOMBER].length > 0)
				{
					arrayToUse = mUnits[side][BOMBER];
				}
				else if(mUnits[side][SOLDIER].length > 0)
				{
					arrayToUse = mUnits[side][SOLDIER];
				}
				else if(mUnits[side][MINER].length > 0)
				{
					arrayToUse = mUnits[side][MINER];
				}
				else
				{
					trace("This shouldn't be getting called, the ai lost and user should know!!");
					return;
				}
				break;
				
			case 2:
				if(mUnits[side][SOLDIER].length > 0)
				{
					arrayToUse = mUnits[side][SOLDIER];
				}
				else if(mUnits[side][GRANADIER].length > 0)
				{
					arrayToUse = mUnits[side][GRANADIER];
				}
				else if(mUnits[side][MINER].length > 0)
				{
					arrayToUse = mUnits[side][MINER];
				}
				else if(mUnits[side][BOMBER].length > 0)
				{
					arrayToUse = mUnits[side][BOMBER];
				}
				else if(mUnits[side][FIGHTER].length > 0)
				{
					arrayToUse = mUnits[side][FIGHTER];
				}
				else
				{
					trace("This shouldn't be getting called, the ai lost and user should know!!");
					return;
				}
				break;
				
			case 3:
				if(mUnits[side][GRANADIER].length > 0)
				{
					arrayToUse = mUnits[side][GRANADIER];
				}
				else if(mUnits[side][SOLDIER].length > 0)
				{
					arrayToUse = mUnits[side][SOLDIER];
				}
				else if(mUnits[side][BOMBER].length > 0)
				{
					arrayToUse = mUnits[side][BOMBER];
				}
				else if(mUnits[side][FIGHTER].length > 0)
				{
					arrayToUse = mUnits[side][FIGHTER];
				}
				else if(mUnits[side][MINER].length > 0)
				{
					arrayToUse = mUnits[side][MINER];
				}
				else
				{
					trace("This shouldn't be getting called, the ai lost and user should know!!");
					return;
				}
				break;
				
			case 4:
				if(mUnits[side][MINER].length > 0)
				{
					arrayToUse = mUnits[side][MINER];
				}
				else if(mUnits[side][SOLDIER].length > 0)
				{
					arrayToUse = mUnits[side][SOLDIER];
				}
				else if(mUnits[side][FIGHTER].length > 0)
				{
					arrayToUse = mUnits[side][FIGHTER];
				}
				else if(mUnits[side][BOMBER].length > 0)
				{
					arrayToUse = mUnits[side][BOMBER];
				}
				else if(mUnits[side][GRANADIER].length > 0)
				{
					arrayToUse = mUnits[side][GRANADIER];
				}
				else
				{
					trace("This shouldn't be getting called, the ai lost and user should know!!");
					return;
				}
				break;
				
			default:
				trace("THIS SHOULD NEVER HAPPEN, IT MEANS RANDOM IS BROKEN");
				return;
		}
		unit = arrayToUse[Math.round(Math.random() * arrayToUse.length - 1)];
	}while(unit.Type == null);
	
	return unit;
}

/*
	This function will pick a random unit, and a random
	location for it to move to. This is version 1 of
	the AI logic, and the default if the others fail.
FUNCTION NO LONGER USED
this.PickRandom = function()
{
	var unit = null;
	var notMoved = true;
	do
	{
		while(CanUnitMove(unit) == false)
		{
			unit = PickRandomUnit("Blue");
		}
		var clamp = unit.GetClampArea();
		var UnitLeftBound = (clamp.leftCut - clamp.leftCut % unit._width) / mGrid[0][0]._width;
		var UnitRightBound = (clamp.rightCut - clamp.rightCut % unit._width) / mGrid[0][0]._width;
		var UnitTopBound = (clamp.topCut - clamp.topCut % unit._height) / mGrid[0][0]._height;
		var UnitBottomBound = (clamp.bottomCut - clamp.bottomCut % unit._height) / mGrid[0][0]._height;
		
		var j = unit.GetJ() + (unit._width / mGrid[0][0]._width) * Math.round(Math.random() * 2) - (unit._width / mGrid[0][0]._width);
		if(j < UnitLeftBound)
		{
			j = UnitLeftBound;
		}
		else if(j > UnitRightBound)
		{
			j = UnitRightBound;
		}
		
		var i = unit.GetI() + (unit._width / mGrid[0][0]._width) * Math.round(Math.random() * 2) - (unit._width / mGrid[0][0]._width);
		if(i < UnitTopBound)
		{
			i = UnitTopBound;
		}
		else if(i > UnitBottomBound)
		{
			i = UnitBottomBound;
		}
		
		if(unit.CheckOccupied(i, j))
		{
			if(unit.CheckColour(i,j) == true && IsEnemyMine(i, j, unit) == false)
			{
				unit.RemoveUnits(i, j);
				unit.Move(i, j, true);
				CheckMines("Red");
				CheckVictory();
				notMoved = false;
			}
		}
		else if(IsEnemyMine(i, j, unit) == false)
		{
			unit.Move(i, j, true);
			CheckMines("Red");
			CheckVictory();
			notMoved = false;
		}
	}while(notMoved);
}*/
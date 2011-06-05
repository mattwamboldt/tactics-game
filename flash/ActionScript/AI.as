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
	
	REV 13(Completed)
	Add function to check if a unit in a location is vulnerable to attack. Use this to negatively
	impact the score of moving to that location. Tune and enjoy units no longer commiting suicide.
	
	REV 14
	Add loops to find the unit in the entire army with the best scoring move, and move that unit.
	Tune and this means the opposition will start to act like they are more than just one random
	unit on the feild. We may have to go with random from the top five to keep it from being too predictable.
	
	REV 15
	Add shortest path function, and use that to get the distance. Also use that to determine if a unit 
	is still trapped and negatively effect score. This may prove too crazy. If done it will be the last
	large ai change until someone pro comes along.
*/

//This function performs All AI functions
this.AIPass = function(colourToRun:Number)
{
	var bestMove = null;
	var unitToMove = null;
	
	for(var unitType = 0; unitType < NUM_UNIT_TYPES; unitType++)
	{
		var array = mUnits[colourToRun][unitType];
		for(var unitIndex = 0; unitIndex < array.length; ++unitIndex)
		{
			var target = null;
			var unit = array[unitIndex];
			if(unit.Type == MINER)
			{
				target = GetNearestMine(unit);
			}
			else
			{
				target = GetNearestUnit(unit);
			}
			
			var possibleMoves:Array = GetMoveList(unit, target);
			
			for(var i = 0; i < possibleMoves.length; ++i)
			{
				if(bestMove == null || bestMove.score < possibleMoves[i].score)
				{
					bestMove = possibleMoves[i];
					unitToMove = unit;
				}
			}
		}
	}
	
	trace("colourToRun = " + colourToRun);
	trace("bestMove.score = " + bestMove.score);
	
	if(unitToMove.CheckOccupied(bestMove.i, bestMove.j))
	{
		unitToMove.RemoveUnits(bestMove.i, bestMove.j);
	}
	
	unitToMove.Move(bestMove.i, bestMove.j, true);
	
	if(colourToRun == BLUE)
	{
		CheckMines(RED);
	}
	else
	{
		CheckMines(BLUE);
	}
	
	CheckVictory();
}

/*
	This tells us if a square is actually an enemy mine location. If the
	unit in question is a deminer it returns false since they can move accross
	those just fine.
*/
this.IsEnemyMine = function(i:Number, j:Number, Unit):Boolean
{
	return Unit.Type != MINER && Unit.GroundUnit == true && (Math.floor(i / 2) % 2 == Math.floor(j / 2) % 2) && (mMineGrid[Math.floor(i / 2)][Math.floor(j / 2)].mColour != Unit.mColour);
}

/*
	This tells us if a square is a friendly mine location.
*/
this.IsFriendlyMine = function(i:Number, j:Number, Unit):Boolean
{
	return Unit.Type != MINER && Unit.GroundUnit == true && (Math.floor(i / 2) % 2 == Math.floor(j / 2) % 2) && (mMineGrid[Math.floor(i / 2)][Math.floor(j / 2)].mColour == Unit.mColour);
}

this.FloorAtMinimum = function(numberToLimit:Number, minimumAmount:Number):Number
{
	if(numberToLimit < minimumAmount)
	{
		return minimumAmount;
	}
	
	return numberToLimit;
}

this.CapAtMaximum = function(numberToLimit:Number, maximumAmount:Number):Number
{
	if(numberToLimit > maximumAmount)
	{
		return maximumAmount;
	}
	
	return numberToLimit;
}

this.MakeEven = function(numberToEvenize:Number):Number
{
	if(numberToEvenize % 2 != 0)
	{
		return numberToEvenize - 1;
	}
	
	return numberToEvenize;
}

/*
	This tells us if a square is adjacent to units that will destroy the passed in units.
*/
this.IsDeathTrap = function(i:Number, j:Number, Unit):Boolean
{
	var unitAtIJ = mGrid[i][j].occupiedUnit
	//first loop for adjacent ground units
	var lowerI = FloorAtMinimum(i - 1, 0);
	var lowerJ = FloorAtMinimum(j - 1, 0);
	
	var upperI = CapAtMaximum(i + Unit.Width(), GRID_WIDTH - 1);
	var upperJ = CapAtMaximum(j + Unit.Height(), GRID_HEIGHT - 1);
	
	for(var t = lowerI; t <= upperI; ++t)
	{
		for(var v = lowerJ; v <= upperJ; ++v)
		{
			var adjacentUnit = mGrid[t][v].occupiedUnit;
			if(adjacentUnit != null
			   && adjacentUnit.GroundUnit == true
			   && adjacentUnit.mColour != Unit.mColour
			   && adjacentUnit != unitAtIJ)
			{
				//check if the unit in the adjacent square has this as an attackable unit
				for(var priority = 0; priority < adjacentUnit.attackablePriorities.length; priority++)
				{
					if(adjacentUnit.attackablePriorities[priority] == Unit.Type)
					{
						return true;
					}
				}
			}
		}
	}
	
	//then loop for surrounding air units
	lowerI = MakeEven(FloorAtMinimum(i - 2, 0));
	upperI = MakeEven(CapAtMaximum(i + 2, GRID_WIDTH - 2));
	
	lowerJ = MakeEven(FloorAtMinimum(j - 2, 0));
	upperJ = MakeEven(CapAtMaximum(j + 2, GRID_HEIGHT - 2));
	
	for(var t = lowerI; t <= upperI; t += 2)
	{
		for(var v = lowerJ; v <= upperJ; v += 2)
		{
			var adjacentUnit = mGrid[t][v].occupiedUnit;
			if(adjacentUnit != null
			   && adjacentUnit.AirUnit == true
			   && adjacentUnit.mColour != Unit.mColour
			   && adjacentUnit != unitAtIJ)
			{
				//check if the unit in the adjacent square has this as an attackable unit
				for(var priority = 0; priority < adjacentUnit.attackablePriorities.length; priority++)
				{
					if(adjacentUnit.attackablePriorities[priority] == Unit.Type)
					{
						return true;
					}
				}
			}
		}
	}
	
	return false;
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
			
			//if an opponent is on their mine they're safe so don't bother with them.
			if((distanceToUnit < distanceToNearest && IsEnemyMine(iCoord, jCoord, sourceUnit) == false)
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
	for( var i = 0; i < GRID_WIDTH/2; ++i )
	{
		for( var j = 0; j < GRID_HEIGHT/2; ++j )
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
	This rates the destruction of the targetUnit given the sourceUnit's priorities
*/
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
	This rates the how valuable a unit is out of all the units
*/
this.GetUnitValue = function(unit):Number
{
	return unitWorths[unit.Type] * 200;
}

/*
	This retreives all the locations the unit can move to.  
*/
this.GetMoveList = function(unit, target):Array
{
	var randomUnitBonus = random(5);
	
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
				newMove.score = 100 / (GetDistanceToCoordinates(newMove, target.i, target.j) + 1); //add 1 to prevent infinity
				
				//factor in if the move will take another unit
				if(unit.CheckOccupied(i, j))
				{
					if(unit.CheckColour(i,j))
					{
						var damageScore = GetDestructionScore(unit, mGrid[i][j].occupiedUnit);
						if(damageScore == 0)
						{
							//cant move into the space of an unattackable unit
							continue;
						}
						else
						{
							newMove.score += damageScore;
						}
					}
					else
					{
						//cannot move onto friendly squares
						continue;
					}
				}
				else if(IsEnemyMine(i, j, unit))
				{
					//avoid unoccupied enemy mines
					continue;
				}
				
				//factor in if the move will get you killed
				if(IsEnemyMine(i, j, unit) || IsDeathTrap(i, j, unit))
				{
					newMove.score -= GetUnitValue(unit);
				}
				
				//incentivize entering friendly mines for safety reasons 
				if(IsFriendlyMine(i, j, unit)
					&& IsFriendlyMine(unit.GetI(), unit.GetJ(), unit) == false)
				{
					newMove.score += 75;
				}
				
				//penalize leaveing friendly mines for safety reasons 
				if(IsFriendlyMine(unit.GetI(), unit.GetJ(), unit)
					&& IsFriendlyMine(i, j, unit) == false)
				{
					newMove.score -= 40;
				}
				
				//newMove.score =+ randomUnitBonus;
				
				moveList.push(newMove);
			}
		}
	}
	
	return moveList;
}
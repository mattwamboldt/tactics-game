stop();

//this performs the rest of the setup and initialization
//so that the movieclips that were created in the first frame have the right data
mine._visible = false;

//here we set the occupancy and allegiances of the small squares to neutral
for( var i = 0; i < GRID_WIDTH; ++i)
{
	for( var j = 0; j < GRID_HEIGHT; ++j)
	{
		mGrid[i][j].occupied = false;
		mGrid[i][j].TurnNeutral();
	}
}

//Here we set the occupancy and allegiances of the mines
for( var i = 0; i < GRID_WIDTH/2; ++i )
{
	for( var j = 0; j < GRID_HEIGHT/2; ++j )
	{
		if(mMineGrid[i][j] != null)
		{
			if(i < 2)
			{
				mMineGrid[i][j].occupied = true;
				mMineGrid[i][j].TurnBlue();
			}
			else if(i >= (GRID_HEIGHT/2) - 2)
			{
				mMineGrid[i][j].occupied = true;
				mMineGrid[i][j].TurnRed();
			}
			else
			{
				mMineGrid[i][j].occupied = false;
				mMineGrid[i][j].TurnNeutral();
			}
		}
		else
		{
			mMineGrid[i][j] = null;
		}
	}
}

//Here we assign a reference to the main grid for
//each unit, and set their allegience. This is the only time
//this will be set until the game is over
//we also need to set the references for where the units initially stand
for( var i = 0; i < GRID_WIDTH/4; ++i )
{
	mUnits[BLUE][BOMBER][i].grid = mGrid;
	mUnits[BLUE][BOMBER][i].TurnBlue();
	mUnits[BLUE][BOMBER][i].SetLocation(0, i * 4);
	
	mUnits[BLUE][FIGHTER][i].grid = mGrid;
	mUnits[BLUE][FIGHTER][i].TurnBlue();
	mUnits[BLUE][FIGHTER][i].SetLocation(0, (i * 4) + 2);
	
	mUnits[RED][FIGHTER][i].grid = mGrid;
	mUnits[RED][FIGHTER][i].TurnRed();
	mUnits[RED][FIGHTER][i].SetLocation((GRID_HEIGHT - 2), i * 4);
	
	mUnits[RED][BOMBER][i].grid = mGrid;
	mUnits[RED][BOMBER][i].TurnRed();
	mUnits[RED][BOMBER][i].SetLocation((GRID_HEIGHT - 2), (i * 4) + 2);
}

for( var i = 0; i < GRID_WIDTH/2; ++i )
{
	mUnits[BLUE][GRANADIER][i].grid = mGrid;
	mUnits[BLUE][GRANADIER][i].TurnBlue();
	mUnits[BLUE][GRANADIER][i].SetLocation(2, (Math.floor((i + 1) / 2) * 4 - i % 2));
	
	mUnits[BLUE][MINER][i].grid = mGrid;
	mUnits[BLUE][MINER][i].TurnBlue();
	mUnits[BLUE][MINER][i].SetLocation(2, ((i % 2) + 1) + ( 4 *  Math.floor(i / 2)));
	
	mUnits[RED][GRANADIER][i].grid = mGrid;
	mUnits[RED][GRANADIER][i].TurnRed();
	mUnits[RED][GRANADIER][i].SetLocation((GRID_HEIGHT - 3), (Math.floor((i + 1) / 2) * 4 - i % 2));
	
	mUnits[RED][MINER][i].grid = mGrid;
	mUnits[RED][MINER][i].TurnRed();
	mUnits[RED][MINER][i].SetLocation((GRID_HEIGHT - 3), ((i % 2) + 1) + ( 4 *  Math.floor(i / 2)));
}

for( var i = 0; i < GRID_WIDTH; ++i )
{
	mUnits[BLUE][SOLDIER][i].grid = mGrid;
	mUnits[BLUE][SOLDIER][i].TurnBlue();
	mUnits[BLUE][SOLDIER][i].SetLocation(3, i);
	
	mUnits[RED][SOLDIER][i].grid = mGrid;
	mUnits[RED][SOLDIER][i].TurnRed();
	mUnits[RED][SOLDIER][i].SetLocation((GRID_HEIGHT - 4), i);
}
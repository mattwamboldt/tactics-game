using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

namespace Board_Game.Code
{
    struct Move
    {
        public Vector2 position;
        public int score;
    }

    class AI
    {
        int currentTurn = 0;
        //This is a guarunteed amount of time that has to pass
        //between tunrs before teh AI process is allowed to fire.
        //It prevents the game from running too quickly in AI vs AI matches
        const int TURN_TIME = 250;
        int elapsedTime = 0;

        bool redIsHuman = true;
        bool blueIsHuman = false;
        public int[] unitWorths = { 8, 7, 2, 6, 4 };
        private Random random;

        private Units.Unit[,][] mUnits;
        private GameGrid mGrid;

        public void Initialize(
            Texture2D tileTexture,
            Texture2D mineTexture,
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture)
        {
            random = new Random();
            mGrid = new GameGrid(Constants.GRID_WIDTH, Constants.GRID_HEIGHT, tileTexture, mineTexture);

            //Here we assign a reference to the main grid for
            //each unit, and set their allegience. This is the only time
            //this will be set until the game is over
            //we also need to set the references for where the units initially stand

            mUnits = new Units.Unit[Constants.NEUTRAL, Constants.NUM_UNIT_TYPES][];
            mUnits[Constants.RED, Constants.BOMBER] = new Units.Unit[Constants.GRID_WIDTH/4];
            mUnits[Constants.BLUE, Constants.BOMBER] = new Units.Unit[Constants.GRID_WIDTH/4];
            mUnits[Constants.RED, Constants.FIGHTER] = new Units.Unit[Constants.GRID_WIDTH/4];
            mUnits[Constants.BLUE, Constants.FIGHTER] = new Units.Unit[Constants.GRID_WIDTH/4];

            for( int i = 0; i < Constants.GRID_WIDTH/4; ++i )
            {
                mUnits[Constants.RED, Constants.BOMBER][i] = new Units.Bomber(mGrid, this, bomberTexture);
                mUnits[Constants.RED, Constants.BOMBER][i].side.TurnRed();
                mUnits[Constants.RED, Constants.BOMBER][i].SetLocation((Constants.GRID_HEIGHT - 2), (i * 4) + 2);

                mUnits[Constants.BLUE, Constants.BOMBER][i] = new Units.Bomber(mGrid, this, bomberTexture);
                mUnits[Constants.BLUE, Constants.BOMBER][i].side.TurnBlue();
                mUnits[Constants.BLUE, Constants.BOMBER][i].SetLocation(0, i * 4);

                mUnits[Constants.RED, Constants.FIGHTER][i] = new Units.Fighter(mGrid, this, fighterTexture);
                mUnits[Constants.RED, Constants.FIGHTER][i].side.TurnRed();
                mUnits[Constants.RED, Constants.FIGHTER][i].SetLocation((Constants.GRID_HEIGHT - 2), i * 4);

                mUnits[Constants.BLUE, Constants.FIGHTER][i] = new Units.Fighter(mGrid, this, fighterTexture);
                mUnits[Constants.BLUE, Constants.FIGHTER][i].side.TurnBlue();
                mUnits[Constants.BLUE, Constants.FIGHTER][i].SetLocation(0, (i * 4) + 2);
            }

            mUnits[Constants.RED, Constants.MINER] = new Units.Unit[Constants.GRID_WIDTH/2];
            mUnits[Constants.BLUE, Constants.MINER] = new Units.Unit[Constants.GRID_WIDTH/2];
            mUnits[Constants.RED, Constants.GRANADIER] = new Units.Unit[Constants.GRID_WIDTH/2];
            mUnits[Constants.BLUE, Constants.GRANADIER] = new Units.Unit[Constants.GRID_WIDTH/2];

            for( var i = 0; i < Constants.GRID_WIDTH/2; ++i )
            {
                mUnits[Constants.RED, Constants.MINER][i] = new Units.Deminer(mGrid, this, deminerTexture);
                mUnits[Constants.RED, Constants.MINER][i].side.TurnRed();
                mUnits[Constants.RED, Constants.MINER][i].SetLocation((Constants.GRID_HEIGHT - 3), ((i % 2) + 1) + (int)(4 * Math.Floor(i / 2.0f)));

                mUnits[Constants.BLUE, Constants.MINER][i] = new Units.Deminer(mGrid, this, deminerTexture);
                mUnits[Constants.BLUE, Constants.MINER][i].side.TurnBlue();
                mUnits[Constants.BLUE, Constants.MINER][i].SetLocation(2, ((i % 2) + 1) + (4 * (int)(Math.Floor(i / 2.0f))));

                mUnits[Constants.RED, Constants.GRANADIER][i] = new Units.Grenadier(mGrid, this, grenadierTexture);
                mUnits[Constants.RED, Constants.GRANADIER][i].side.TurnRed();
                mUnits[Constants.RED, Constants.GRANADIER][i].SetLocation((Constants.GRID_HEIGHT - 3), (int)(Math.Floor((i + 1) / 2.0f) * 4 - i % 2));

                mUnits[Constants.BLUE, Constants.GRANADIER][i] = new Units.Grenadier(mGrid, this, grenadierTexture);
                mUnits[Constants.BLUE, Constants.GRANADIER][i].side.TurnBlue();
                mUnits[Constants.BLUE, Constants.GRANADIER][i].SetLocation(2, (int)(Math.Floor((i + 1) / 2.0f) * 4 - i % 2));
            }

            mUnits[Constants.RED, Constants.SOLDIER] = new Units.Unit[Constants.GRID_WIDTH];
            mUnits[Constants.BLUE, Constants.SOLDIER] = new Units.Unit[Constants.GRID_WIDTH];
            for( var i = 0; i < Constants.GRID_WIDTH; ++i )
            {
                mUnits[Constants.RED, Constants.SOLDIER][i] = new Units.Soldier(mGrid, this, soldierTexture);
                mUnits[Constants.RED, Constants.SOLDIER][i].side.TurnRed();
                mUnits[Constants.RED, Constants.SOLDIER][i].SetLocation((Constants.GRID_HEIGHT - 4), i);

                mUnits[Constants.BLUE, Constants.SOLDIER][i] = new Units.Soldier(mGrid, this, soldierTexture);
                mUnits[Constants.BLUE, Constants.SOLDIER][i].side.TurnBlue();
                mUnits[Constants.BLUE, Constants.SOLDIER][i].SetLocation(3, i);
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            mGrid.Render(spriteBatch);

            for (int side = 0; side < Constants.NEUTRAL; ++side)
            {
                for (int unitType = 0; unitType < Constants.NUM_UNIT_TYPES; ++unitType)
                {
                    for (int i = 0; i < mUnits[side, unitType].Length; ++i)
                    {
                        mUnits[side, unitType][i].Render(spriteBatch);
                    }
                }
            }
        }

        // Uncomment as features come online
        /// <summary>
        /// Was AIPass in Flash. This decides which unit should move and then
        /// moves that unit.
        /// </summary>
        /// <param name="side">Whether red or blue is going</param>
        public void Update(int colourToRun)
        {
            Move bestMove;
            bestMove.score = -99;
            bestMove.position.Y = -99;
            bestMove.position.X = -99;
            Units.Unit unitToMove = null;

            for (int unitType = 0; unitType < Constants.NUM_UNIT_TYPES; unitType++)
            {
                Units.Unit[] array = mUnits[colourToRun, unitType];

                for (int unitIndex = 0; unitIndex < array.Length; ++unitIndex)
                {
                    Vector2 target;
                    Units.Unit unit = array[unitIndex];
                    if (unit.Type == Constants.UnitType.Miner)
                    {
                        target = GetNearestMine(unit);
                    }
                    else
                    {
                        target = GetNearestUnit(unit);
                    }

                    Stack<Move> possibleMoves = GetMoveList(unit, target);

                    if (possibleMoves.Count == 0)
                    {
                        Console.Out.WriteLine("No moves for " + unitType + " at " + unit.position);
                    }

                    foreach(Move move in possibleMoves)
                    {
                        if (unitToMove == null || bestMove.score < move.score)
                        {
                            bestMove = move;
                            unitToMove = unit;
                        }
                    }
                }
            }

            if (unitToMove.CheckOccupied((int)bestMove.position.Y, (int)bestMove.position.X))
            {
                unitToMove.RemoveUnits((int)bestMove.position.Y, (int)bestMove.position.X);
            }

            unitToMove.Move((int)bestMove.position.Y, (int)bestMove.position.X, true);

            if (colourToRun == Constants.BLUE)
            {
                CheckMines(Constants.RED);
            }
            else
            {
                CheckMines(Constants.BLUE);
            }

            CheckVictory();
        }

        /*
            This tells us if a square is actually an enemy mine location. If the
            unit in question is a deminer it returns false since they can move accross
            those just fine.
        */
        public bool IsEnemyMine(int i, int j, Units.Unit Unit)
        {
            return Unit.Type != Constants.UnitType.Miner
                && Unit.CanFly == false
                && (Math.Floor((double)(i / 2)) % 2 == Math.Floor((double)(j / 2)) % 2)
                && (mGrid.mMines[i / 2, j / 2].side.mColour != Unit.side.mColour);
        }

        /*
            This tells us if a square is a friendly mine location.
        */
        public bool IsFriendlyMine(int i, int j, Units.Unit Unit)
        {
            return Unit.Type != Constants.UnitType.Miner
                && Unit.CanFly == false
                && (Math.Floor((double)(i / 2)) % 2 == Math.Floor((double)(j / 2)) % 2)
                && (mGrid.mMines[i / 2, j / 2].side.mColour == Unit.side.mColour);
        }

        public int FloorAtMinimum(int numberToLimit, int minimumAmount)
        {
            if(numberToLimit < minimumAmount)
            {
                return minimumAmount;
            }

            return numberToLimit;
        }

        public int CapAtMaximum(int numberToLimit, int maximumAmount)
        {
            if(numberToLimit > maximumAmount)
            {
                return maximumAmount;
            }

            return numberToLimit;
        }

        public int MakeEven(int numberToEvenize)
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
        public bool IsDeathTrap(int i, int j, Units.Unit Unit)
        {
            var unitAtIJ = mGrid.mTiles[i, j].occupiedUnit;
            //first loop for adjacent ground units
            var lowerI = FloorAtMinimum(i - 1, 0);
            var lowerJ = FloorAtMinimum(j - 1, 0);

            var upperI = CapAtMaximum(i + Unit.width, Constants.GRID_WIDTH - 1);
            var upperJ = CapAtMaximum(j + Unit.height, Constants.GRID_HEIGHT - 1);

            for (var t = lowerI; t <= upperI; ++t)
            {
                for (var v = lowerJ; v <= upperJ; ++v)
                {
                    var adjacentUnit = mGrid.mTiles[t, v].occupiedUnit;
                    if (adjacentUnit != null
                       && adjacentUnit.CanFly == false
                       && adjacentUnit.Type != Constants.UnitType.Miner
                       && adjacentUnit.side.mColour != Unit.side.mColour
                       && adjacentUnit != unitAtIJ)
                    {
                        //check if the unit in the adjacent square has this as an attackable unit
                        for (var priority = 0; priority < adjacentUnit.attackablePriorities.Length; priority++)
                        {
                            if (adjacentUnit.attackablePriorities[priority] == Unit.Type)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //then loop for surrounding air units
            lowerI = MakeEven(FloorAtMinimum(i - 2, 0));
            upperI = MakeEven(CapAtMaximum(i + 2, Constants.GRID_WIDTH - 2));

            lowerJ = MakeEven(FloorAtMinimum(j - 2, 0));
            upperJ = MakeEven(CapAtMaximum(j + 2, Constants.GRID_HEIGHT - 2));

            for (var t = lowerI; t <= upperI; t += 2)
            {
                for (var v = lowerJ; v <= upperJ; v += 2)
                {
                    var adjacentUnit = mGrid.mTiles[t, v].occupiedUnit;
                    if (adjacentUnit != null
                       && adjacentUnit.CanFly
                       && adjacentUnit.side.mColour != Unit.side.mColour
                       && adjacentUnit != unitAtIJ)
                    {
                        //check if the unit in the adjacent square has this as an attackable unit
                        for (var priority = 0; priority < adjacentUnit.attackablePriorities.Length; priority++)
                        {
                            if (adjacentUnit.attackablePriorities[priority] == Unit.Type)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        //This checks to see who, if anyone, hsa won, and displays the appropriate message
        public bool CheckVictory()
        {
            int winningColour = mGrid.mMines[0, 0].side.mColour;
            bool hasWon = true;

            for( var i = 0; i < Constants.GRID_WIDTH/2; ++i )
            {
                for (var j = 0; j < Constants.GRID_HEIGHT / 2; ++j)
                {
                    if (i % 2 == j % 2 && winningColour != mGrid.mMines[i, j].side.mColour)
                    {
                        hasWon = false;
                    }
                }
            }

            if(hasWon == false)
            {
                //we need to check for a destruction victory for blue
                if (mUnits[Constants.RED, Constants.SOLDIER].Length == 0
                    && mUnits[Constants.RED, Constants.FIGHTER].Length == 0
                    && mUnits[Constants.RED, Constants.SOLDIER].Length == 0
                    && mUnits[Constants.RED, Constants.GRANADIER].Length == 0
                    && mUnits[Constants.RED, Constants.MINER].Length == 0)
                {
                    winningColour = Constants.BLUE;
                    hasWon = true;
                }
                else if (mUnits[Constants.BLUE, Constants.BOMBER].Length == 0
                    && mUnits[Constants.BLUE, Constants.FIGHTER].Length == 0
                    && mUnits[Constants.BLUE, Constants.SOLDIER].Length == 0
                    && mUnits[Constants.BLUE, Constants.GRANADIER].Length == 0
                    && mUnits[Constants.BLUE, Constants.MINER].Length == 0)
                {
                    winningColour = Constants.RED;
                    hasWon = true;
                }
            }

           /* if(hasWon == true)
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
            }*/

            return hasWon;
        }

        //This function Checks to see if mines need to be changed to teh given colour
        //It also checks for any units that are on mines, that shoudl be deleted
        public void CheckMines(int colour)
        {
            //The outer loop goes through the mines
            for (var i = 0; i < Constants.GRID_WIDTH / 2; ++i)
            {
                for (var j = 0; j < Constants.GRID_HEIGHT / 2; ++j)
                {
                    //only gets us mines that have a value
                    if (i % 2 == j % 2)
                    {
                        //inner loops checks the mine itself
                        for (var t = 0; t < 2; ++t)
                        {
                            for (var u = 0; u < 2; ++u)
                            {
                                var square = mGrid.mTiles[i * 2 + t, j * 2 + u];
                                if (square.occupiedUnit != null)
                                {
                                    if (square.occupiedUnit.Type == Constants.UnitType.Miner
                                        && square.side.mColour == colour)
                                    {
                                        mGrid.mMines[i, j].side.ChangeColour(colour);
                                    }
                                    else if (square.occupiedUnit.side.mColour != mGrid.mMines[i, j].side.mColour
                                            && square.occupiedUnit.Type != Constants.UnitType.Miner
                                            && square.occupiedUnit.CanFly == false)
                                    {
                                        RemoveUnit(square.occupiedUnit);
                                        square.occupied = false;
                                        square.side.TurnNeutral();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /*
            Returns the unit that is closest to the given unit
        */
        private Vector2 GetNearestUnit(Units.Unit unit)
        {
            int opposingSide = Constants.RED;

            if (unit.side.mColour == Constants.RED)
            {
                opposingSide = Constants.BLUE;
            }

            Vector2 originalPoint = new Vector2(unit.GetJ(), unit.GetI());
            Vector2 nearestTarget = new Vector2(-1,-1);

            double distanceToNearest = GetDistanceToCoordinates(originalPoint, 0, 0);

            for (var i = 0; i < unit.attackablePriorities.Length; i++)
            {
                Constants.UnitType typeToAttack = unit.attackablePriorities[i];

                for(var t = 0; t < mUnits[opposingSide, (int)typeToAttack].Length; ++t)
                {
                    var iCoord = mUnits[opposingSide, (int)typeToAttack][t].GetI();
                    var jCoord = mUnits[opposingSide, (int)typeToAttack][t].GetJ();
                    double distanceToUnit = GetDistanceToCoordinates(originalPoint, iCoord, jCoord);

                    //if an opponent is on their mine they're safe so don't bother with them.
                    if((distanceToUnit < distanceToNearest && IsEnemyMine(iCoord, jCoord, unit) == false)
                       || nearestTarget.Y == -1)
                    {
                        nearestTarget.Y = iCoord;
                        nearestTarget.X = jCoord;
                        distanceToNearest = distanceToUnit;
                    }
                }
            }

            return nearestTarget;
        }

        /*
            Returns the mine that is closest to the given unit
        */
        private Vector2 GetNearestMine(Units.Unit unit)
        {
            Vector2 originalPoint = new Vector2(unit.GetJ(), unit.GetI());
            Vector2 nearestMine = new Vector2(-1, -1);

            double distanceToNearest = GetDistanceToCoordinates(originalPoint, 0, 0);

            //The outer loop goes through the mines
            for (var i = 0; i < Constants.GRID_WIDTH / 2; ++i)
            {
                for (var j = 0; j < Constants.GRID_HEIGHT / 2; ++j)
                {
                    //only gets us mines that have a value
                    if (i % 2 == j % 2)
                    {
                        //we want to head for mines of the opposite colour
                        if (mGrid.mMines[i, j].side.mColour != unit.side.mColour)
                        {
                            //inner loops checks the mine itself
                            for (var t = 0; t < 2; ++t)
                            {
                                for (var u = 0; u < 2; ++u)
                                {
                                    var iCoord = i * 2 + t;
                                    var jCoord = j * 2 + u;
                                    var distanceToMineSquare = GetDistanceToCoordinates(originalPoint, iCoord, jCoord);

                                    if (distanceToMineSquare < distanceToNearest
                                       || nearestMine.Y == -1)
                                    {
                                        nearestMine.Y = iCoord;
                                        nearestMine.X = jCoord;
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
            This retreives all the locations the unit can move to.  
        */
        private Stack<Move> GetMoveList(Units.Unit unit, Vector2 target)
        {
            int randomUnitBonus = random.Next(0,5);

            Stack<Move> moveList = new Stack<Move>();
            Units.ClampArea clamp = unit.GetClampArea();
            int UnitLeftBound = (int)((clamp.leftCut - clamp.leftCut % unit.ScreenDimensions().X) / Tile.TILE_SIZE);
            int UnitRightBound = (int)((clamp.rightCut - clamp.rightCut % unit.ScreenDimensions().X) / Tile.TILE_SIZE);
            int UnitTopBound = (int)((clamp.topCut - clamp.topCut % unit.ScreenDimensions().Y) / Tile.TILE_SIZE);
            int UnitBottomBound = (int)((clamp.bottomCut - clamp.bottomCut % unit.ScreenDimensions().Y) / Tile.TILE_SIZE);

            var currentDistance = GetDistanceToCoordinates(target, unit.GetI(), unit.GetJ());

            for(int i = UnitTopBound; i <= UnitBottomBound; i += unit.width)
            {
                for( int j = UnitLeftBound; j <= UnitRightBound; j += unit.height)
                {
                    //staying place is the only invalid move right now
                    if(i != unit.GetI() || j != unit.GetJ())
                    {
                        Move newMove;
                        newMove.position.Y = i;
                        newMove.position.X = j;
                        newMove.score = 0;

                        //factor in if the move will take another unit
                        if(unit.CheckOccupied(i, j))
                        {
                            if(unit.CheckColour(i,j))
                            {
                                var damageScore = GetDestructionScore(unit, mGrid.mTiles[i, j].occupiedUnit);
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

                        var moveDistance = GetDistanceToCoordinates(newMove.position, target.Y, target.X);

                        if(moveDistance > currentDistance)
                        {
                            continue;
                        }

                        //set score to use distance
                        if(unit.Type == Constants.UnitType.Miner)
                        {
                            newMove.score += (int)((100 / (moveDistance + 1)) / 2);
                        }
                        else
                        {
                            newMove.score += (int)(100 / (moveDistance + 1));
                        }

                        //factor in if the move will get you killed
                        if (IsEnemyMine((int)newMove.position.Y, (int)newMove.position.X, unit)
                            || IsDeathTrap((int)newMove.position.Y, (int)newMove.position.X, unit))
                        {
                            newMove.score -= GetUnitValue(unit);
                        }

                        moveList.Push(newMove);
                    }
                }
            }

            return moveList;
        }

        /*
            Return the number of squares for the shortest path between the
            point and unit, avoiding mines, possibly adding extra for potentially
            dangerous moves to discourage movement in that direction.
        
        this.GetShortestPathToEnemy = function(unit):Number
        {
            var moveArray = GetMoveList(unit, null);
        }
        */

        /*
            Returns the distance between two points
        */
        public double GetDistanceToCoordinates(Vector2 startPoint, float endPointI, float endPointJ)
        {
            float jDifference = startPoint.X - endPointJ;
            float iDifference = startPoint.Y - endPointI;
            return Math.Sqrt( (jDifference * jDifference) + (iDifference * iDifference) );
        }

        /*
            This rates the destruction of the targetUnit given the sourceUnit's priorities
        */
        public int GetDestructionScore(Units.Unit sourceUnit, Units.Unit targetUnit)
        {
            for(var i = 0; i < sourceUnit.attackablePriorities.Length; i++)
            {
                if(sourceUnit.attackablePriorities[i] == targetUnit.Type)
                {
                    return (Constants.NUM_UNIT_TYPES - i) * 200;
                }
            }

            return 0;
        }

        /*
            This rates the how valuable a unit is out of all the units
        */
        public int GetUnitValue(Units.Unit unit)
        {
            return unitWorths[(int)unit.Type] * 200;
        }

        internal float Width()
        {
            return Constants.GRID_WIDTH * Tile.TILE_SIZE;
        }

        internal float Height()
        {
            return Constants.GRID_HEIGHT * Tile.TILE_SIZE;
        }

        //This will search through the arrays and eliminate the given unit
        internal void RemoveUnit(Units.Unit unit)
        {
            List<Units.Unit> UnitList = new List<Units.Unit>(mUnits[unit.side.mColour, (int)unit.Type]);
            UnitList.Remove(unit);
            mUnits[unit.side.mColour, (int)unit.Type] = UnitList.ToArray();
            unit = null;
        }

        internal void ChangeTurns()
        {
            currentTurn = (currentTurn + 1) % 2;
        }

        internal void Update(GameTime gameTime)
        {
            if((!redIsHuman && currentTurn == Constants.RED)
                || (!blueIsHuman && currentTurn == Constants.BLUE))
            {
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                
                if (elapsedTime >= TURN_TIME)
                {
                    Update(currentTurn);
                    elapsedTime = 0;
                }
            }
        }

        //this._parent.redBtn.onRelease = function()
        //{
        //    //first we toggle to the opposite control scheme
        //    redIsHuman = !redIsHuman;

        //    if(redIsHuman)
        //    {
        //        this.txtController.text = "Human";
        //    }
        //    else
        //    {
        //        this.txtController.text = "AI";
        //    }

        //    CheckAILoop();
        //}

        //this._parent.blueBtn.onRelease = function()
        //{
        //    //first we toggle to the opposite control scheme
        //    blueIsHuman = !blueIsHuman;

        //    if(blueIsHuman)
        //    {
        //        this.txtController.text = "Human";
        //    }
        //    else
        //    {
        //        this.txtController.text = "AI";
        //    }

        //    CheckAILoop();
        //}

        //this._parent.txtWinLose.text = "";
        //this._parent.txtWinLose.selectable = false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Board_Game.UI;
using Board_Game.Util;

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

namespace Board_Game.Logic
{
    struct Move
    {
        public Vector2 position;
        public int score;
    }

    class AI
    {
        //This is a guarunteed amount of time that has to pass
        //between turns before teh AI process is allowed to fire.
        //It prevents the game from running too quickly in AI vs AI matches
        const int TURN_TIME = 250;
        int elapsedTime = 0;

        public int[] unitWorths = { 8, 7, 2, 6, 4 };
        private Random random;

        public GameGrid mGrid;
        private GameState mGameState;

        public GameState State { get { return mGameState; } }
        public Screen mScreen;

        public void Initialize(GameState gameState)
        {
            random = new Random();

            mGameState = gameState;
        }

        //TODO: Move to the Unit
        /*
            This tells us if a square is adjacent to units that will destroy the passed in units.
        */
        public bool IsDeathTrap(int i, int j, Units.Unit Unit)
        {
            var unitAtIJ = mGrid.mTiles[i, j].occupiedUnit;

            //first loop for adjacent ground units
            var lowerI = Rounding.FloorAtMinimum(i - 1, 0);
            var lowerJ = Rounding.FloorAtMinimum(j - 1, 0);

            var upperI = Rounding.CapAtMaximum(i + Unit.mUnitDesc.width, GameState.GRID_WIDTH - 1);
            var upperJ = Rounding.CapAtMaximum(j + Unit.mUnitDesc.height, GameState.GRID_HEIGHT - 1);

            for (var t = lowerI; t <= upperI; ++t)
            {
                for (var v = lowerJ; v <= upperJ; ++v)
                {
                    var adjacentUnit = mGrid.mTiles[t, v].occupiedUnit;
                    if (adjacentUnit != null
                       && adjacentUnit.mUnitDesc.CanFly == false
                       && adjacentUnit.mUnitDesc.Type != Units.UnitType.Miner
                       && adjacentUnit.side != Unit.side
                       && adjacentUnit != unitAtIJ)
                    {
                        //check if the unit in the adjacent square has this as an attackable unit
                        for (var priority = 0; priority < adjacentUnit.mUnitDesc.attackablePriorities.Length; priority++)
                        {
                            if (adjacentUnit.mUnitDesc.attackablePriorities[priority] == Unit.mUnitDesc.Type)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //then loop for surrounding air units
            lowerI = Rounding.MakeEven(Rounding.FloorAtMinimum(i - 2, 0));
            upperI = Rounding.MakeEven(Rounding.CapAtMaximum(i + 2, GameState.GRID_WIDTH - 2));

            lowerJ = Rounding.MakeEven(Rounding.FloorAtMinimum(j - 2, 0));
            upperJ = Rounding.MakeEven(Rounding.CapAtMaximum(j + 2, GameState.GRID_HEIGHT - 2));

            for (var t = lowerI; t <= upperI; t += 2)
            {
                for (var v = lowerJ; v <= upperJ; v += 2)
                {
                    var adjacentUnit = mGrid.mTiles[t, v].occupiedUnit;
                    if (adjacentUnit != null
                       && adjacentUnit.mUnitDesc.CanFly
                       && adjacentUnit.side != Unit.side
                       && adjacentUnit != unitAtIJ)
                    {
                        //check if the unit in the adjacent square has this as an attackable unit
                        for (var priority = 0; priority < adjacentUnit.mUnitDesc.attackablePriorities.Length; priority++)
                        {
                            if (adjacentUnit.mUnitDesc.attackablePriorities[priority] == Unit.mUnitDesc.Type)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        //end

        //TODO: could be moved to a gamestate class
        //This function Checks to see if mines need to be changed to teh given colour
        //It also checks for any units that are on mines, that shoudl be deleted
        public void CheckMines(Side colour)
        {
            foreach(Mine mine in mGrid.mMines)
            {
                //inner loops checks the mine itself
                for (var t = 0; t < 2; ++t)
                {
                    for (var u = 0; u < 2; ++u)
                    {
                        var square = mGrid.mTiles[(int)mine.position.Y * 2 + t, (int)mine.position.X * 2 + u];
                        if (square.Occupied)
                        {
                            if (square.occupiedUnit.mUnitDesc.Type == Units.UnitType.Miner
                                && square.side == colour)
                            {
                                mine.side = colour;
                            }
                            else if (square.occupiedUnit.side != mine.side
                                    && square.occupiedUnit.mUnitDesc.Type != Units.UnitType.Miner
                                    && square.occupiedUnit.mUnitDesc.CanFly == false)
                            {
                                State.RemoveUnit(square.occupiedUnit);
                                square.occupiedUnit = null;
                                square.side = Side.Neutral;
                            }
                        }
                    }
                }
            }
        }

        //TODO: could be moved to the unit class
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

            for (int i = UnitTopBound; i <= UnitBottomBound; i += unit.mUnitDesc.width)
            {
                for (int j = UnitLeftBound; j <= UnitRightBound; j += unit.mUnitDesc.height)
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
                        else if(unit.IsEnemyMine(i, j))
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
                        if (unit.mUnitDesc.Type == Units.UnitType.Miner)
                        {
                            newMove.score += (int)((100 / (moveDistance + 1)) / 2);
                        }
                        else
                        {
                            newMove.score += (int)(100 / (moveDistance + 1));
                        }

                        //factor in if the move will get you killed
                        if (unit.IsEnemyMine((int)newMove.position.Y, (int)newMove.position.X)
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
        //end

        //TODO: Expand to a pathfinding algorithm, to avoid units getting lost
        /*
            Returns the distance between two points
        */
        public double GetDistanceToCoordinates(Vector2 startPoint, float endPointI, float endPointJ)
        {
            float jDifference = startPoint.X - endPointJ;
            float iDifference = startPoint.Y - endPointI;
            return Math.Sqrt( (jDifference * jDifference) + (iDifference * iDifference) );
        }
        //end

        /*
            This rates the destruction of the targetUnit given the sourceUnit's priorities
        */
        public int GetDestructionScore(Units.Unit sourceUnit, Units.Unit targetUnit)
        {
            for (var i = 0; i < sourceUnit.mUnitDesc.attackablePriorities.Length; i++)
            {
                if (sourceUnit.mUnitDesc.attackablePriorities[i] == targetUnit.mUnitDesc.Type)
                {
                    return ((int)Units.UnitType.NumUnitTypes - i) * 200;
                }
            }

            return 0;
        }

        /*
            This rates the how valuable a unit is out of all the units
        */
        public int GetUnitValue(Units.Unit unit)
        {
            return unitWorths[(int)unit.mUnitDesc.Type] * 200;
        }

        internal void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= TURN_TIME)
            {
                if (mGameState.winner == Side.Neutral)
                {
                    Move bestMove;
                    bestMove.score = -99;
                    bestMove.position.Y = -99;
                    bestMove.position.X = -99;
                    Units.Unit unitToMove = null;

                    List<Units.Unit> units;

                    if (mGameState.mCurrentPlayer.mSide == Side.Red)
                    {
                        units = mGameState.Red.Units;
                    }
                    else
                    {
                        units = mGameState.Blue.Units;
                    }

                    foreach (Units.Unit unit in units)
                    {
                        Vector2 target = unit.GetNearestTarget();

                        Stack<Move> possibleMoves = GetMoveList(unit, target);

                        if (possibleMoves.Count == 0)
                        {
                            Console.Out.WriteLine("No moves for " + unit.mUnitDesc.Type + " at " + unit.position);
                        }

                        foreach (Move move in possibleMoves)
                        {
                            if (unitToMove == null || bestMove.score < move.score)
                            {
                                bestMove = move;
                                unitToMove = unit;
                            }
                        }
                    }

                    if (unitToMove.CheckOccupied((int)bestMove.position.Y, (int)bestMove.position.X))
                    {
                        unitToMove.RemoveUnits((int)bestMove.position.Y, (int)bestMove.position.X);
                    }

                    unitToMove.Move((int)bestMove.position.Y, (int)bestMove.position.X);

                    mGameState.EndTurn();
                }

                elapsedTime = 0;
            }
        }
    }
}

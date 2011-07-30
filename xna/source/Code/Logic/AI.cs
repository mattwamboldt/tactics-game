using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Board_Game.UI;
using Board_Game.Util;
using Board_Game.Creatures;

/*
    AI History: MSW
    REV 1(COMPLETED)
    We're gonna make this horribly stupid at first
    It's gonna randomly pick a Creature and move it
    to a random location, and it'll attempt to do so
    until it actually makes a move.

    REV 2(COMPLETED)
    Adding in a function to check for Creatures that are
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
    function to pick an enemy Creature and move in their direction.
    Basic pathfinding here we come.

    REV 5(Completed)
    Given that we have multiple modes of victory we need
    to make the AI attempt them. That means moving de-miners
    on occassion.

    REV 6(Completed)
    So now we want to improve the AI's aiming, by going through
    all the Creatures that our ranomly chosen Creature can attack, and finding
    the one that has the closest target to attack.

    REV 7(Completed)
    The deminers are very stupid with no goals. they should be moving
    towards the nearest mines just as other Creatures move towards their prey.

    REV 8(Completed)
    Reworking to use a global array of arrays to store Creatures so we
    can iterate over them more effectively. Also changing to numbers
    from strings to allow easier implementation of multiplayer.

    REV 9(Completed)
    Add Multiple play modes. player vs player, player vs ai, and ai vs ai.

    REV 10(Completed)
    Create an array of priorities for each Creature type to differentiate
    what they go after.

    REV 11(Completed)
    Change the getMoveList Function to return an array of objects representing
    the i,j of each move, as well as a score which at this point will be 100/the distance.
    Then change the attackrandom to use this score to determine the selected move,
    instead of distance. This is the start of weight based ai.

    REV 12(Completed)
    Remove attack attackables function. Add a function to return the value of each Creature destroyed
    by making a move based on the Creature priorities and add them together. Then add this to
    the previous score. Tune and enjoy sometimes Creatures not getting destroyed when they are adjacent.

    REV 13(Completed)
    Add function to check if a Creature in a location is vulnerable to attack. Use this to negatively
    impact the score of moving to that location. Tune and enjoy Creatures no longer commiting suicide.

    REV 14
    Add loops to find the Creature in the entire army with the best scoring move, and move that Creature.
    Tune and this means the opposition will start to act like they are more than just one random
    Creature on the feild. We may have to go with random from the top five to keep it from being too predictable.

    REV 15
    Add shortest path function, and use that to get the distance. Also use that to determine if a Creature 
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

        public int[] CreatureWorths = { 8, 7, 2, 6, 4 };
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

        //TODO: Move to the Creature
        /*
            This tells us if a square is adjacent to Creatures that will destroy the passed in Creatures.
        */
        public bool IsDeathTrap(int x, int y, Creatures.Creature Creature)
        {
            var CreatureAtIJ = mGrid.Occupants[x, y];

            //first loop for adjacent ground Creatures
            var lowerX = Rounding.FloorAtMinimum(x - 1, 0);
            var lowerY = Rounding.FloorAtMinimum(y - 1, 0);

            var upperX = Rounding.CapAtMaximum(x + Creature.GridWidth, mGrid.Width - 1);
            var upperY = Rounding.CapAtMaximum(y + Creature.GridHeight, mGrid.Height - 1);

            for (var t = lowerX; t <= upperX; ++t)
            {
                for (var v = lowerY; v <= upperY; ++v)
                {
                    var adjacentCreature = mGrid.Occupants[t, v];
                    if (adjacentCreature != null
                       && adjacentCreature.CanFly == false
                       && adjacentCreature.Type != Creatures.CreatureType.Miner
                       && adjacentCreature.side != Creature.side
                       && adjacentCreature != CreatureAtIJ)
                    {
                        //check if the Creature in the adjacent square has this as an attackable Creature
                        for (var priority = 0; priority < adjacentCreature.Class.AttackPriorities.Length; priority++)
                        {
                            if (adjacentCreature.Class.AttackPriorities[priority] == Creature.Type)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //then loop for surrounding air Creatures
            lowerX = Rounding.MakeEven(Rounding.FloorAtMinimum(x - 2, 0));
            upperX = Rounding.MakeEven(Rounding.CapAtMaximum(x + 2, mGrid.Width - 2));

            lowerY = Rounding.MakeEven(Rounding.FloorAtMinimum(y - 2, 0));
            upperY = Rounding.MakeEven(Rounding.CapAtMaximum(y + 2, mGrid.Height - 2));

            for (var t = lowerX; t <= upperX; t += 2)
            {
                for (var v = lowerY; v <= upperY; v += 2)
                {
                    var adjacentCreature = mGrid.Occupants[t, v];
                    if (adjacentCreature != null
                       && adjacentCreature.CanFly
                       && adjacentCreature.side != Creature.side
                       && adjacentCreature != CreatureAtIJ)
                    {
                        //check if the Creature in the adjacent square has this as an attackable Creature
                        for (var priority = 0; priority < adjacentCreature.Class.AttackPriorities.Length; priority++)
                        {
                            if (adjacentCreature.Class.AttackPriorities[priority] == Creature.Type)
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
        //It also checks for any Creatures that are on mines, that shoudl be deleted
        public void CheckMines(Side colour)
        {
            foreach(Mine mine in mGrid.Mines)
            {
                //inner loops checks the mine itself
                for (var t = 0; t < 2; ++t)
                {
                    for (var u = 0; u < 2; ++u)
                    {
                        Creature occupant = mGrid.Occupants[(int)mine.position.X + t, (int)mine.position.Y + u];
                        if (occupant != null)
                        {
                            if (occupant.Type == CreatureType.Miner
                                && occupant.side == colour)
                            {
                                mine.side = colour;
                            }
                            else if (occupant.side != mine.side
                                    && occupant.Type != CreatureType.Miner
                                    && occupant.CanFly == false)
                            {
                                State.RemoveCreature(occupant);
                                mGrid.Occupants[(int)mine.position.X + t, (int)mine.position.Y + u] = null;
                            }
                        }
                    }
                }
            }
        }

        //TODO: could be moved to the Creature class
        /*
            This retreives all the locations the Creature can move to.  
        */
        private Stack<Move> GetMoveList(Creatures.Creature Creature, Vector2 target)
        {
            int randomCreatureBonus = random.Next(0,5);

            Stack<Move> moveList = new Stack<Move>();
            Creatures.ClampArea clamp = mGameState.GetClampArea(Creature);
            int CreatureLeftBound = (int)((clamp.leftCut - clamp.leftCut % Creature.ScreenDimensions().X) / Tile.TILE_SIZE);
            int CreatureRightBound = (int)((clamp.rightCut - clamp.rightCut % Creature.ScreenDimensions().X) / Tile.TILE_SIZE);
            int CreatureTopBound = (int)((clamp.topCut - clamp.topCut % Creature.ScreenDimensions().Y) / Tile.TILE_SIZE);
            int CreatureBottomBound = (int)((clamp.bottomCut - clamp.bottomCut % Creature.ScreenDimensions().Y) / Tile.TILE_SIZE);

            var currentDistance = GetDistanceToCoordinates(target, Creature.GetX(), Creature.GetY());

            for (int x = CreatureLeftBound; x <= CreatureRightBound; x += Creature.GridWidth)
            {
                for (int y = CreatureTopBound; y <= CreatureBottomBound; y += Creature.GridHeight)
                {
                    //staying place is the only invalid move right now
                    if(x != Creature.GetX() || y != Creature.GetY())
                    {
                        Move newMove;
                        newMove.position.X = x;
                        newMove.position.Y = y;
                        newMove.score = 0;

                        //factor in if the move will take another Creature
                        if (State.CheckOccupied(x, y, Creature.GridWidth, Creature.GridHeight))
                        {
                            if (CanDestroyAllUnits(x, y, Creature))
                            {
                                var damageScore = GetDestructionScore(Creature, x, y);
                                if(damageScore == 0)
                                {
                                    //cant move into the space of an unattackable Creature
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
                        else if(IsEnemyMine(x, y, Creature))
                        {
                            //avoid unoccupied enemy mines
                            continue;
                        }

                        var moveDistance = GetDistanceToCoordinates(newMove.position, target.X,target.Y);

                        if(moveDistance > currentDistance)
                        {
                            continue;
                        }

                        //set score to use distance
                        if (Creature.Type == Creatures.CreatureType.Miner)
                        {
                            newMove.score += (int)((100 / (moveDistance + 1)) / 2);
                        }
                        else
                        {
                            newMove.score += (int)(100 / (moveDistance + 1));
                        }

                        //factor in if the move will get you killed
                        if (IsEnemyMine((int)newMove.position.X, (int)newMove.position.Y, Creature)
                            || IsDeathTrap((int)newMove.position.X, (int)newMove.position.Y, Creature))
                        {
                            newMove.score -= GetCreatureValue(Creature);
                        }

                        moveList.Push(newMove);
                    }
                }
            }

            return moveList;
        }
        //end

        //TODO: Expand to a pathfinding algorithm, to avoid Creatures getting lost
        /*
            Returns the distance between two points
        */
        public double GetDistanceToCoordinates(Vector2 startPoint, float endPointX, float endPointY)
        {
            float xDifference = startPoint.X - endPointX;
            float yDifference = startPoint.Y - endPointY;
            return Math.Sqrt( (xDifference * xDifference) + (yDifference * yDifference) );
        }
        //end

        /*
            This rates the destruction of the targetCreature given the sourceCreature's priorities
        */
        public int GetDestructionScore(Creatures.Creature sourceCreature, int desiredX, int desiredY)
        {
            int score = 0;
            //check the sqaures we're moving into, if they contain a unit we can't
            //attack then return zero, otherwise tally up the destruction

            for(int x = 0; x < sourceCreature.GridWidth; x++)
            {
                for (int y = 0; y < sourceCreature.GridHeight; y++ )
                {
                    Creature targetCreature = mGrid.Occupants[x + desiredX, y + desiredY];
                    
                    if (targetCreature != null)
                    {
                        if (sourceCreature.Class.CanAttack(targetCreature.Type))
                        {
                            for (var p = 0; p < sourceCreature.Class.AttackPriorities.Length; p++)
                            {
                                if (sourceCreature.Class.AttackPriorities[p] == targetCreature.Type)
                                {
                                    score += ((int)Creatures.CreatureType.NumCreatureTypes - p) * 200;
                                }
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }

            return score;
        }

        public virtual Vector2 GetNearestTarget(Creature source)
        {
            List<Creature> opposingCreatures;

            if (source.side == Side.Red)
            {
                opposingCreatures = State.Blue.Creatures;
            }
            else
            {
                opposingCreatures = State.Red.Creatures;
            }

            Vector2 originalPoint = new Vector2(source.GetX(), source.GetY());
            Vector2 nearestTarget = new Vector2(-1, -1);

            double distanceToNearest = GetDistanceToCoordinates(originalPoint, 0, 0);

            foreach (Creature Creature in opposingCreatures)
            {
                if (source.Class.CanAttack(Creature.Type))
                {
                    var x = Creature.GetX();
                    var y = Creature.GetY();
                    double distanceToCreature = GetDistanceToCoordinates(originalPoint, x, y);

                    //if an opponent is on their mine they're safe so don't bother with them.
                    if ((distanceToCreature < distanceToNearest && IsEnemyMine(x, y, source) == false)
                       || nearestTarget.X == -1)
                    {
                        nearestTarget.X = x;
                        nearestTarget.Y = y;
                        distanceToNearest = distanceToCreature;
                    }
                }
            }

            return nearestTarget;
        }

        public Vector2 GetNearestMine(Creature source)
        {
            Vector2 originalPoint = new Vector2(source.GetX(), source.GetY());
            Vector2 nearestMine = new Vector2(-1, -1);

            double distanceToNearest = GetDistanceToCoordinates(originalPoint, 0, 0);

            //The outer loop goes through the mines
            foreach (Mine mine in mGrid.Mines)
            {
                //we want to head for mines of the opposite colour
                if (mine.side != source.side)
                {
                    Vector2 mineCorner = mine.position;

                    //inner loops checks the mine itself
                    for (var t = 0; t < 2; ++t)
                    {
                        for (var u = 0; u < 2; ++u)
                        {
                            var x = mineCorner.X * 2 + u;
                            var y = mineCorner.Y * 2 + t;
                            var distanceToMineSquare = GetDistanceToCoordinates(originalPoint, x, y);

                            if (distanceToMineSquare < distanceToNearest
                               || nearestMine.Y == -1)
                            {
                                nearestMine.X = x;
                                nearestMine.Y = y;
                                distanceToNearest = distanceToMineSquare;
                            }
                        }
                    }
                }
            }

            return nearestMine;
        }

        /*
            This rates the how valuable a Creature is out of all the Creatures
        */
        public int GetCreatureValue(Creatures.Creature Creature)
        {
            return CreatureWorths[(int)Creature.Type] * 200;
        }

        /*
            This tells us if a square is actually an enemy mine location.
        */
        public bool IsEnemyMine(int x, int y, Creature creature)
        {
            if (creature.CanFly || creature.Type == CreatureType.Miner)
            {
                return false;
            }

            Mine mine = mGrid.GetMine(x - x % 2, y - y % 2);

            if (mine == null)
            {
                return false;
            }

            return mine.side != creature.side;
        }

        public bool CanDestroyAllUnits(int newX, int newY, Creature creature)
        {
            for (var x = 0; x < creature.GridWidth; ++x)
            {
                for (var y = 0; y < creature.GridHeight; ++y)
                {
                    Creature occupant = mGrid.Occupants[newX + x, newY + y];
                    if (occupant != null)
                    {
                        if (occupant.side == creature.side //Cant attack friendlies
                          || creature.Class.CanAttack(occupant.Type) == false)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        internal void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= TURN_TIME)
            {
                if (mGameState.winner == Side.Neutral)
                {
                    List<Creatures.Creature> Creatures;

                    if (mGameState.mCurrentPlayer.mSide == Side.Red)
                    {
                        Creatures = mGameState.Red.Creatures;
                    }
                    else
                    {
                        Creatures = mGameState.Blue.Creatures;
                    }

                    Move bestMove;
                    bestMove.score = -99;
                    bestMove.position.X = -99;
                    bestMove.position.Y = -99;
                    Creatures.Creature CreatureToMove = null;

                    foreach (Creature Creature in Creatures)
                    {
                        Vector2 target;

                        if (Creature.Type == CreatureType.Miner)
                        {
                            target = GetNearestMine(Creature);
                        }
                        else
                        {
                            target = GetNearestTarget(Creature);
                        }

                        Stack<Move> possibleMoves = GetMoveList(Creature, target);

                        if (possibleMoves.Count == 0)
                        {
                            Console.Out.WriteLine("No moves for " + Creature.Type + " at " + Creature.Position);
                        }

                        foreach (Move move in possibleMoves)
                        {
                            if (CreatureToMove == null || bestMove.score < move.score)
                            {
                                bestMove = move;
                                CreatureToMove = Creature;
                            }
                        }
                    }

                    //BUG: we have run out of units with attackable enemies to move but the games not over
                    //FIX: give up and await our demise
                    if (CreatureToMove != null)
                    {
                        if (State.CheckOccupied((int)bestMove.position.X, (int)bestMove.position.Y, CreatureToMove.GridWidth, CreatureToMove.GridHeight))
                        {
                           mGameState.DestroyCreatures(
                               (int)bestMove.position.X,
                               (int)bestMove.position.Y,
                               CreatureToMove.GridWidth,
                               CreatureToMove.GridHeight
                           );
                        }

                        mGameState.Move((int)bestMove.position.X, (int)bestMove.position.Y, CreatureToMove);
                    }

                    mGameState.EndTurn();
                }

                elapsedTime = 0;
            }
        }
    }
}

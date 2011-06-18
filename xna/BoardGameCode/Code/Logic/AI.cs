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
        public bool IsDeathTrap(int i, int j, Creatures.Creature Creature)
        {
            var CreatureAtIJ = mGrid.mTiles[i, j].occupiedCreature;

            //first loop for adjacent ground Creatures
            var lowerI = Rounding.FloorAtMinimum(i - 1, 0);
            var lowerJ = Rounding.FloorAtMinimum(j - 1, 0);

            var upperI = Rounding.CapAtMaximum(i + Creature.mCreatureDesc.SizeInSpaces.X, GameState.GRID_WIDTH - 1);
            var upperJ = Rounding.CapAtMaximum(j + Creature.mCreatureDesc.SizeInSpaces.Y, GameState.GRID_HEIGHT - 1);

            for (var t = lowerI; t <= upperI; ++t)
            {
                for (var v = lowerJ; v <= upperJ; ++v)
                {
                    var adjacentCreature = mGrid.mTiles[t, v].occupiedCreature;
                    if (adjacentCreature != null
                       && adjacentCreature.mCreatureDesc.CanFly == false
                       && adjacentCreature.mCreatureDesc.Type != Creatures.CreatureType.Miner
                       && adjacentCreature.side != Creature.side
                       && adjacentCreature != CreatureAtIJ)
                    {
                        //check if the Creature in the adjacent square has this as an attackable Creature
                        for (var priority = 0; priority < adjacentCreature.mCreatureDesc.AttackPriorities.Length; priority++)
                        {
                            if (adjacentCreature.mCreatureDesc.AttackPriorities[priority] == Creature.mCreatureDesc.Type)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //then loop for surrounding air Creatures
            lowerI = Rounding.MakeEven(Rounding.FloorAtMinimum(i - 2, 0));
            upperI = Rounding.MakeEven(Rounding.CapAtMaximum(i + 2, GameState.GRID_WIDTH - 2));

            lowerJ = Rounding.MakeEven(Rounding.FloorAtMinimum(j - 2, 0));
            upperJ = Rounding.MakeEven(Rounding.CapAtMaximum(j + 2, GameState.GRID_HEIGHT - 2));

            for (var t = lowerI; t <= upperI; t += 2)
            {
                for (var v = lowerJ; v <= upperJ; v += 2)
                {
                    var adjacentCreature = mGrid.mTiles[t, v].occupiedCreature;
                    if (adjacentCreature != null
                       && adjacentCreature.mCreatureDesc.CanFly
                       && adjacentCreature.side != Creature.side
                       && adjacentCreature != CreatureAtIJ)
                    {
                        //check if the Creature in the adjacent square has this as an attackable Creature
                        for (var priority = 0; priority < adjacentCreature.mCreatureDesc.AttackPriorities.Length; priority++)
                        {
                            if (adjacentCreature.mCreatureDesc.AttackPriorities[priority] == Creature.mCreatureDesc.Type)
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
                            if (square.occupiedCreature.mCreatureDesc.Type == Creatures.CreatureType.Miner
                                && square.side == colour)
                            {
                                mine.side = colour;
                            }
                            else if (square.occupiedCreature.side != mine.side
                                    && square.occupiedCreature.mCreatureDesc.Type != Creatures.CreatureType.Miner
                                    && square.occupiedCreature.mCreatureDesc.CanFly == false)
                            {
                                State.RemoveCreature(square.occupiedCreature);
                                square.occupiedCreature = null;
                                square.side = Side.Neutral;
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
            Creatures.ClampArea clamp = Creature.GetClampArea();
            int CreatureLeftBound = (int)((clamp.leftCut - clamp.leftCut % Creature.ScreenDimensions().X) / Tile.TILE_SIZE);
            int CreatureRightBound = (int)((clamp.rightCut - clamp.rightCut % Creature.ScreenDimensions().X) / Tile.TILE_SIZE);
            int CreatureTopBound = (int)((clamp.topCut - clamp.topCut % Creature.ScreenDimensions().Y) / Tile.TILE_SIZE);
            int CreatureBottomBound = (int)((clamp.bottomCut - clamp.bottomCut % Creature.ScreenDimensions().Y) / Tile.TILE_SIZE);

            var currentDistance = GetDistanceToCoordinates(target, Creature.GetI(), Creature.GetJ());

            for (int i = CreatureTopBound; i <= CreatureBottomBound; i += Creature.mCreatureDesc.SizeInSpaces.X)
            {
                for (int j = CreatureLeftBound; j <= CreatureRightBound; j += Creature.mCreatureDesc.SizeInSpaces.Y)
                {
                    //staying place is the only invalid move right now
                    if(i != Creature.GetI() || j != Creature.GetJ())
                    {
                        Move newMove;
                        newMove.position.Y = i;
                        newMove.position.X = j;
                        newMove.score = 0;

                        //factor in if the move will take another Creature
                        if(Creature.CheckOccupied(i, j))
                        {
                            if (Creature.CanDestroyAllUnits(i, j))
                            {
                                var damageScore = GetDestructionScore(Creature, i, j);
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
                        else if(Creature.IsEnemyMine(i, j))
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
                        if (Creature.mCreatureDesc.Type == Creatures.CreatureType.Miner)
                        {
                            newMove.score += (int)((100 / (moveDistance + 1)) / 2);
                        }
                        else
                        {
                            newMove.score += (int)(100 / (moveDistance + 1));
                        }

                        //factor in if the move will get you killed
                        if (Creature.IsEnemyMine((int)newMove.position.Y, (int)newMove.position.X)
                            || IsDeathTrap((int)newMove.position.Y, (int)newMove.position.X, Creature))
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
        public double GetDistanceToCoordinates(Vector2 startPoint, float endPointI, float endPointJ)
        {
            float jDifference = startPoint.X - endPointJ;
            float iDifference = startPoint.Y - endPointI;
            return Math.Sqrt( (jDifference * jDifference) + (iDifference * iDifference) );
        }
        //end

        /*
            This rates the destruction of the targetCreature given the sourceCreature's priorities
        */
        public int GetDestructionScore(Creatures.Creature sourceCreature, int desiredI, int desiredJ)
        {
            int score = 0;
            //check the sqaures we're moving into, if they contain a unit we can't
            //attack then return zero, otherwise tally up the destruction

            for(int i = 0; i < sourceCreature.mCreatureDesc.SizeInSpaces.Y; i++)
            {
                for (int j = 0; j < sourceCreature.mCreatureDesc.SizeInSpaces.X; j++ )
                {
                    if (mGrid.mTiles[i + desiredI, j + desiredJ].Occupied)
                    {
                        Creature targetCreature = mGrid.mTiles[i + desiredI, j + desiredJ].occupiedCreature;
                        if (targetCreature != null)
                        {
                            if (sourceCreature.mCreatureDesc.CanAttack(targetCreature.mCreatureDesc.Type))
                            {
                                for (var p = 0; p < sourceCreature.mCreatureDesc.AttackPriorities.Length; p++)
                                {
                                    if (sourceCreature.mCreatureDesc.AttackPriorities[p] == targetCreature.mCreatureDesc.Type)
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
            }

            return score;
        }

        /*
            This rates the how valuable a Creature is out of all the Creatures
        */
        public int GetCreatureValue(Creatures.Creature Creature)
        {
            return CreatureWorths[(int)Creature.mCreatureDesc.Type] * 200;
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
                    Creatures.Creature CreatureToMove = null;

                    List<Creatures.Creature> Creatures;

                    if (mGameState.mCurrentPlayer.mSide == Side.Red)
                    {
                        Creatures = mGameState.Red.Creatures;
                    }
                    else
                    {
                        Creatures = mGameState.Blue.Creatures;
                    }

                    foreach (Creatures.Creature Creature in Creatures)
                    {
                        Vector2 target = Creature.GetNearestTarget();

                        Stack<Move> possibleMoves = GetMoveList(Creature, target);

                        if (possibleMoves.Count == 0)
                        {
                            Console.Out.WriteLine("No moves for " + Creature.mCreatureDesc.Type + " at " + Creature.position);
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

                    if (CreatureToMove.CheckOccupied((int)bestMove.position.Y, (int)bestMove.position.X))
                    {
                        CreatureToMove.RemoveCreatures((int)bestMove.position.Y, (int)bestMove.position.X);
                    }

                    CreatureToMove.Move((int)bestMove.position.Y, (int)bestMove.position.X);

                    mGameState.EndTurn();
                }

                elapsedTime = 0;
            }
        }
    }
}

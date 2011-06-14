using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Board_Game.Code.UI;
using Board_Game.Code.Util;
using Board_Game.Code.Logic;

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
        //between turns before teh AI process is allowed to fire.
        //It prevents the game from running too quickly in AI vs AI matches
        const int TURN_TIME = 250;
        int elapsedTime = 0;

        bool redIsHuman = true;
        bool blueIsHuman = false;
        public int[] unitWorths = { 8, 7, 2, 6, 4 };
        private Random random;
        private Constants.Side winner;

        public GameGrid mGrid;
        private GameState mGameState;

        public GameState State { get { return mGameState; } }

        Texture2D mBomberTexture;
        Texture2D mFighterTexture;
        Texture2D mSoldierTexture;
        Texture2D mDeminerTexture;
        Texture2D mGrenadierTexture;
        Selector mSelector;

        SpriteFont mButton;
        SpriteFont mTutorial;
        SpriteFont mUnitName;

        public void Initialize(
            Texture2D tileTexture,
            Texture2D mineTexture,
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture,
            Texture2D selectorTexture,
            SpriteFont button,
            SpriteFont tutorial,
            SpriteFont unitName)
        {
            random = new Random();

            //TODO: separate into a gamestate class
            mGrid = new GameGrid(Constants.GRID_WIDTH, Constants.GRID_HEIGHT, tileTexture, mineTexture);

            mBomberTexture = bomberTexture;
            mFighterTexture = fighterTexture;
            mSoldierTexture = soldierTexture;
            mDeminerTexture = deminerTexture;
            mGrenadierTexture = grenadierTexture;

            mButton = button;
            mTutorial = tutorial;
            mUnitName = unitName;
            mSelector = new Selector(selectorTexture, mGrid, this);
            mSelector.mSide = Constants.Side.Red;
            winner = Constants.Side.Neutral;

            mGameState = new GameState(this);
            mGameState.Initialize(
                bomberTexture,
                fighterTexture,
                soldierTexture,
                deminerTexture,
                grenadierTexture
            );
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            //draw the UI
            //TODO: Move UI into a tool and class to remove all of these consants and draw calls
            //-draw a box under the grid
            spriteBatch.Draw(pixel, new Rectangle(0, 0, Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 30), Color.Black);
            
            //-draw the box under UNITS
            spriteBatch.Draw(pixel, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0, 250, 30), Color.Black);

            //-draw the tutorial text box
            spriteBatch.Draw(pixel, new Rectangle(0, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 20 + 30, Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, 200), Color.Black);

            //-draw the unit sidebar box
            spriteBatch.Draw(pixel, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 50, 250, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 240), Color.Black);

            //-draw the unit backgrounds

            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(pixel, new Rectangle(
                Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5,
                30 + 20 + 5 + (50 + 15) * i,
                (int)Tile.TILE_SIZE * 2 + 10,
                (int)Tile.TILE_SIZE * 2 + 10),
               Color.DarkGray);
            }

            //-draw the units
            spriteBatch.Draw(mBomberTexture, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5, (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            spriteBatch.Draw(mFighterTexture, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + 50 + 15, (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            spriteBatch.Draw(mSoldierTexture, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + (50 + 15) * 2, (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            spriteBatch.Draw(mDeminerTexture, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + (50 + 15) * 3, (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            spriteBatch.Draw(mGrenadierTexture, new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + (50 + 15) * 4, (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);

            //-draw the buttons
            spriteBatch.Draw(pixel, new Rectangle(20 - 1, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 - 1, 100 + 2, 20 + 2), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(20, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40, 100, 20), Color.Red);

            spriteBatch.Draw(pixel, new Rectangle(20 - 1 + 200, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 - 1, 100 + 2, 20 + 2), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(20 + 200, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40, 100, 20), Color.Blue);

            //-draw the text
            //--buttons
            string redString = "HUMAN";
            string blueString = "HUMAN";
            if (!redIsHuman)
            {
                redString = "AI";
            }

            spriteBatch.DrawString(mButton, redString, Layout.CenterAlign(new Rectangle(20, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40, 100, 20), redString, mButton), Color.White);

            if (!blueIsHuman)
            {
                blueString = "AI";
            }

            spriteBatch.DrawString(mButton, blueString, Layout.CenterAlign(new Rectangle(20 + 200, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40, 100, 20), blueString, mButton), Color.White);
            
            //--sidebar header
            spriteBatch.DrawString(mButton, "UNITS", Layout.CenterAlign(new Rectangle(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0, 250, 30), "UNITS", mButton), Color.White);

            //--unit descriptions
            string[] titles = new string[] { "BOMBER", "FIGHTER", "SOLDIER", "DE-MINER", "BAZOOKA" };
            string[] descriptions = new string[] {
                "Movement: 1 large square\nDestroys ground units.",
                "Movement: 1 large square\nDestroys air units.",
                "Movement: 1 square\nDestroys ground units.",
                "Movement: 1 square\nCaptures mines.",
                "Movement: 1 square\nDestroys all units."
            };

            for (int i = 0; i < 5; i++)
            {
                spriteBatch.DrawString(
                    mTutorial, 
                    titles[i],
                    new Vector2(
                        Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5 + (int)Tile.TILE_SIZE * 2 + 10 + 5,
                        30 + 20 + 5 + (50 + 15) * i),
                   Color.White);

                spriteBatch.DrawString(
                    mTutorial,
                    Layout.WrapString(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, mTutorial, descriptions[i]),
                    new Vector2(
                        Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5 + (int)Tile.TILE_SIZE * 2 + 10 + 5,
                        30 + 20 + 5 + (50 + 15) * i + 15),
                   Color.White);
            }

            //--tutorial text
            String tutorialText = "The Objective of the game is to destroy all opponent units, or take control "
                + "of all mines (squares the are around a diamond). The De-Miner units convert a neutral(grey) "
                + "or enemy mine to your colour, at which point you can move your other units safely across."
                + "\n\nFlying units, which are the bomber and fighter, will not be destroyed by mines.";

            spriteBatch.DrawString(
                mTutorial,
                Layout.WrapString(Constants.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, mTutorial, tutorialText),
                new Vector2(5, Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 20 + 30 + 5),
                Color.White
            );

            //draw the grid
            mGrid.Render(spriteBatch);

            //draw the units
            mGameState.Blue.Render(spriteBatch, mGrid.position);
            mGameState.Red.Render(spriteBatch, mGrid.position);

            mSelector.Render(spriteBatch, mGrid.position);

            if (winner != Constants.Side.Neutral)
            {
                //we have a winner

                string victorString = "";
                if (winner == Constants.Side.Red)
                {
                    victorString = "Red has won!";
                }
                else if (winner == Constants.Side.Blue)
                {
                    victorString = "Blue has won";
                }

                //determine the background size based on the text size
                Vector2 stringSize = mButton.MeasureString(victorString);
                Vector2 backgroundSize = new Vector2(stringSize.X + 20, stringSize.Y + 10);

                //draw the background and string centered to the grid
                Vector2 backgroundLocation = Layout.CenterAlign(
                    new Rectangle(
                        (int)mGrid.position.X,
                        (int)mGrid.position.Y,
                        Constants.GRID_WIDTH * (int)Tile.TILE_SIZE,
                        Constants.GRID_HEIGHT * (int)Tile.TILE_SIZE),
                    backgroundSize);

                Rectangle backgroundRect = new Rectangle(
                    (int)backgroundLocation.X,
                    (int)backgroundLocation.Y,
                    (int)backgroundSize.X,
                    (int)backgroundSize.Y);

                spriteBatch.Draw(pixel, new Rectangle(backgroundRect.X - 2, backgroundRect.Y - 2, backgroundRect.Width + 4, backgroundRect.Height + 4), Color.White);
                spriteBatch.Draw(pixel, backgroundRect, Color.Black);

                spriteBatch.DrawString(
                    mButton,
                    victorString,
                    Layout.CenterAlign(backgroundRect, victorString, mButton),
                    Color.White
                );
            }
        }
        
        /// <summary>
        /// Was AIPass in Flash. This decides which unit should move and then
        /// moves that unit.
        /// </summary>
        /// <param name="colourToRun">Whether red or blue is going</param>
        public void Update(int colourToRun)
        {
            if (winner == Constants.Side.Neutral)
            {
                Move bestMove;
                bestMove.score = -99;
                bestMove.position.Y = -99;
                bestMove.position.X = -99;
                Units.Unit unitToMove = null;

                List<Units.Unit> units;

                if(colourToRun == (int)Constants.Side.Red)
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
                        Console.Out.WriteLine("No moves for " + unit.Type + " at " + unit.position);
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

                unitToMove.Move((int)bestMove.position.Y, (int)bestMove.position.X, true);

                //TODO: move into a gamestate class update
                if (colourToRun == (int)Constants.Side.Blue)
                {
                    CheckMines(Constants.Side.Red);
                }
                else
                {
                    CheckMines(Constants.Side.Blue);
                }

                CheckVictory();
            }
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

            var upperI = Rounding.CapAtMaximum(i + Unit.width, Constants.GRID_WIDTH - 1);
            var upperJ = Rounding.CapAtMaximum(j + Unit.height, Constants.GRID_HEIGHT - 1);

            for (var t = lowerI; t <= upperI; ++t)
            {
                for (var v = lowerJ; v <= upperJ; ++v)
                {
                    var adjacentUnit = mGrid.mTiles[t, v].occupiedUnit;
                    if (adjacentUnit != null
                       && adjacentUnit.CanFly == false
                       && adjacentUnit.Type != Constants.UnitType.Miner
                       && adjacentUnit.side != Unit.side
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
            lowerI = Rounding.MakeEven(Rounding.FloorAtMinimum(i - 2, 0));
            upperI = Rounding.MakeEven(Rounding.CapAtMaximum(i + 2, Constants.GRID_WIDTH - 2));

            lowerJ = Rounding.MakeEven(Rounding.FloorAtMinimum(j - 2, 0));
            upperJ = Rounding.MakeEven(Rounding.CapAtMaximum(j + 2, Constants.GRID_HEIGHT - 2));

            for (var t = lowerI; t <= upperI; t += 2)
            {
                for (var v = lowerJ; v <= upperJ; v += 2)
                {
                    var adjacentUnit = mGrid.mTiles[t, v].occupiedUnit;
                    if (adjacentUnit != null
                       && adjacentUnit.CanFly
                       && adjacentUnit.side != Unit.side
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
        //end

        //TODO: could be moved to a gamestate class
        //determines and sets the winner if a side has won by capturing all the mines.
        public bool MineVictory()
        {
            winner = mGrid.mTiles[0, 0].mine.side;

            for (var i = 0; i < Constants.GRID_WIDTH / 2; ++i)
            {
                for (var j = 0; j < Constants.GRID_HEIGHT / 2; ++j)
                {
                    if (i % 2 == j % 2 && winner != mGrid.mTiles[i * 2, j * 2].mine.side)
                    {
                        winner = Constants.Side.Neutral;
                        return false;
                    }
                }
            }

            return true;
        }

        //TODO: could be moved to a gamestate class
        //This checks to see who, if anyone, hsa won
        public void CheckVictory()
        {
            if (!MineVictory())
            {
                //we need to check for a destruction victory
                if (mGameState.Red.Units.Count == 0)
                {
                    winner = Constants.Side.Blue;
                }
                else if (mGameState.Blue.Units.Count == 0)
                {
                    winner = Constants.Side.Red;
                }
            }
        }

        //TODO: could be moved to a gamestate class
        //This function Checks to see if mines need to be changed to teh given colour
        //It also checks for any units that are on mines, that shoudl be deleted
        public void CheckMines(Constants.Side colour)
        {
            foreach(Mine mine in mGrid.mMines)
            {
                //inner loops checks the mine itself
                for (var t = 0; t < 2; ++t)
                {
                    for (var u = 0; u < 2; ++u)
                    {
                        var square = mGrid.mTiles[(int)mine.position.Y * 2 + t, (int)mine.position.X * 2 + u];
                        if (square.occupiedUnit != null)
                        {
                            if (square.occupiedUnit.Type == Constants.UnitType.Miner
                                && square.side == colour)
                            {
                                mine.side = colour;
                            }
                            else if (square.occupiedUnit.side != mine.side
                                    && square.occupiedUnit.Type != Constants.UnitType.Miner
                                    && square.occupiedUnit.CanFly == false)
                            {
                                RemoveUnit(square.occupiedUnit);
                                square.occupied = false;
                                square.side = Constants.Side.Neutral;
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
                        if(unit.Type == Constants.UnitType.Miner)
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
            List<Units.Unit> units;

            if (unit.side == Constants.Side.Blue)
            {
                units = mGameState.Blue.Units;
            }
            else
            {
                units = mGameState.Red.Units;
            }

            units.Remove(unit);
            unit = null;
        }

        internal void ChangeTurns()
        {
            currentTurn = (currentTurn + 1) % 2;
            mSelector.mSide = (Constants.Side)currentTurn;
        }

        internal void Update(GameTime gameTime)
        {
            HandleInput();

            if ((!redIsHuman && currentTurn == Constants.RED)
                || (!blueIsHuman && currentTurn == Constants.BLUE))
            {
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedTime >= TURN_TIME)
                {
                    Update(currentTurn);
                    elapsedTime = 0;
                }
            }
            else
            {
                mSelector.HandleInput();
            }
        }

        public void HandleInput()
        {
            if (InputManager.Get().isTriggered(Keys.R))
            {
                redIsHuman = !redIsHuman;
            }
            else if (InputManager.Get().isTriggered(Keys.B))
            {
                blueIsHuman = !blueIsHuman;
            }
        }
    }
}

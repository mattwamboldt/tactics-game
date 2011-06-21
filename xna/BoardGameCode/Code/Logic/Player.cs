using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Board_Game.Creatures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Characters;

namespace Board_Game.Logic
{
    /// <summary>
    /// This defines a player, each player has a set of Creatures and a reference
    /// to the board. This pulls Creature creation away from the AI, and makes it possible
    /// for each side to have a different number and types of Creatures.
    /// </summary>
    class Player
    {
        public bool mIsHuman;
        public Side mSide;
        public Army mArmy;
        private GameState mGame;

        public List<Creature> Creatures { get { return mArmy.Members; } }

        public Player(bool isHuman, Side side,GameState game)
        {
            mIsHuman = isHuman;
            mSide = side;
            mGame = game;
        }

        public void PlaceOnField()
        {
            foreach (Creature creature in mArmy.Members)
            {
                mGame.SetLocation(creature.GridLocation.X, creature.GridLocation.Y, creature);
            }
        }

        public void RemoveCreature(Creatures.Creature Creature)
        {
            mArmy.Members.Remove(Creature);
            Creature = null;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentLocation)
        {
            foreach (Creature Creature in mArmy.Members)
            {
                Creature.Render(spriteBatch, parentLocation);
            }
        }
    }
}

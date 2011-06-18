using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Creatures
{
    public enum CreatureType
    {
        Undefined = -1,
        Bomber = 0,
        Fighter = 1,
        Soldier = 2,
        Granadier = 3,
        Miner = 4,
        NumCreatureTypes = 5
    }

    /// <summary>
    /// This class is used to seperate the static data about a Creature
    /// from its runtime data, such as location. That way you can
    /// make a Creature have a certain speed, size, etc. without having to change
    /// All the maps and Creatures lists as well
    /// </summary>
    public class CreatureDescription
    {
        public bool CanFly;
        public CreatureType Type;
        public int height;
        public int width;
        public string textureName;
        public Texture2D texture;

        //used to determine which enemies this Creature
        //will attack and in what order
        public CreatureType[] attackablePriorities;

        public bool CanAttack(CreatureType CreatureType)
        {
            return attackablePriorities != null
                && attackablePriorities.Contains(CreatureType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Units
{
    public enum UnitType
    {
        Undefined = -1,
        Bomber = 0,
        Fighter = 1,
        Soldier = 2,
        Granadier = 3,
        Miner = 4,
        NumUnitTypes = 5
    }

    /// <summary>
    /// This class is used to seperate the static data about a unit
    /// from its runtime data, such as location. That way you can
    /// make a unit have a certain speed, size, etc. without having to change
    /// All the maps and units lists as well
    /// </summary>
    class UnitDescription
    {
        public bool CanFly;
        public UnitType Type;
        public int height;
        public int width;
        public string textureName;
        public Texture2D texture;

        //used to determine which enemies this unit
        //will attack and in what order
        public UnitType[] attackablePriorities;

        public bool CanAttack(UnitType unitType)
        {
            return attackablePriorities != null
                && attackablePriorities.Contains(unitType);
        }
    }
}

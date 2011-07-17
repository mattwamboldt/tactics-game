using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Board_Game.Creatures;
using Microsoft.Xna.Framework.Content;

namespace BoardGameContent.DB
{
    //Will house all the game metadata
    public class GameDatabase
    {
        private List<CreatureDescription> mCreatureTable;
        public List<CreatureDescription> CreatureTable
        {
            get { return mCreatureTable; }
            set { mCreatureTable = value; }
        }

        private List<string> mArmyTable;
        public List<string> ArmyTable
        {
            get { return mArmyTable; }
            set { mArmyTable = value; }
        }
    }

    public class GameDatabaseReader : ContentTypeReader<GameDatabase>
    {
        protected override GameDatabase Read(ContentReader input, GameDatabase existingInstance)
        {
            GameDatabase db = new GameDatabase();
            db.CreatureTable = input.ReadObject<List<CreatureDescription>>();
            db.ArmyTable = input.ReadObject<List<string>>();
            return db;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoardGameContent.DB;
using Microsoft.Xna.Framework.Content;
using Board_Game.Creatures;

namespace Board_Game.Code.DB
{
    class DatabaseManager
    {
        private GameDatabase mDatabase;
        private static DatabaseManager mInstance;

        public static void Initialize() { mInstance = new DatabaseManager(); }
        public static DatabaseManager Get() { return mInstance; }

        private DatabaseManager(){}

        public void Load(ContentManager content)
        {
            mDatabase = content.Load<GameDatabase>("DB/Database");
        }

        public List<CreatureDescription> CreatureTable
        {
            get { return mDatabase.CreatureTable; }
        }
    }
}

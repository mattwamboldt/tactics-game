using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Board_Game.Creatures;
using Board_Game.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Board_Game.Characters
{
    //This is used to wrap a list of creatures for a given side
    //Allowing us to have different unit layouts loaded at runtime
    public class Army
    {
        private Side mSide;
        public Side Side { get { return mSide; } set { mSide = value; } }

        private List<Creature> mMembers;
        public List<Creature> Members { get { return mMembers; } set { mMembers = value; } }
    }

    public class ArmyReader : ContentTypeReader<Army>
    {
        protected override Army Read(ContentReader input, Army existingInstance)
        {
            Army output = new Army();
            output.Side = input.ReadObject<Side>();
            output.Members = input.ReadObject<List<Creature>>();

            foreach(Creature creature in output.Members)
            {
                creature.side = output.Side;
            }

            return output;
        }
    }
}

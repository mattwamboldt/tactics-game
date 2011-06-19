using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using BoardGameContent.DB;

namespace BoardGameContentBuilders.DB
{
    [ContentTypeWriter]
    class GameDatabaseWriter : ContentTypeWriter<GameDatabase>
    {
        protected override void Write(ContentWriter output, GameDatabase value)
        {
            output.WriteObject(value.CreatureTable);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(GameDatabaseReader).AssemblyQualifiedName;
        }
    }
}

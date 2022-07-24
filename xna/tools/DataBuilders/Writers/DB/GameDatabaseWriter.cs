using BoardGameContent.DB;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.DB
{
    [ContentTypeWriter]
    class GameDatabaseWriter : ContentTypeWriter<GameDatabase>
    {
        protected override void Write(ContentWriter output, GameDatabase value)
        {
            output.WriteObject(value.CreatureTable);
            output.WriteObject(value.ArmyTable);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(GameDatabaseReader).AssemblyQualifiedName;
        }
    }
}

using Board_Game.Characters;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.Creatures
{
    [ContentTypeWriter]
    class ArmyWriter : ContentTypeWriter<Army>
    {
        protected override void Write(ContentWriter output, Army value)
        {
            output.WriteObject(value.Side);
            output.WriteObject(value.Members);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(ArmyReader).AssemblyQualifiedName;
        }
    }
}
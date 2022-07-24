using Board_Game.Creatures;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.Creatures
{
    [ContentTypeWriter]
    class CreatureDescriptionWriter : ContentTypeWriter<CreatureDescription>
    {
        protected override void Write(ContentWriter output, CreatureDescription value)
        {
            output.Write(value.ID);
            output.Write(value.Name);
            output.Write(value.Description);
            output.Write(value.CanFly);
            output.WriteObject(value.Type);
            output.WriteObject(value.SizeInSpaces);
            output.Write(value.TextureName);
            output.WriteObject(value.AttackPriorities);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(CreatureDescriptionReader).AssemblyQualifiedName;
        }
    }
}

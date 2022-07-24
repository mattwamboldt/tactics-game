using Board_Game.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.Creatures
{
    [ContentTypeWriter]
    class CreatureWriter : ContentTypeWriter<Creature>
    {
        protected override void Write(ContentWriter output, Creature value)
        {
            output.WriteObject<Point>(value.GridLocation);
            output.Write(value.ClassID);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(CreatureReader).AssemblyQualifiedName;
        }
    }
}
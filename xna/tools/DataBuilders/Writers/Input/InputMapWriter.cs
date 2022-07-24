using Board_Game.Input;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.Input
{
    [ContentTypeWriter]
    class InputMapWriter : ContentTypeWriter<InputMap>
    {
        protected override void Write(ContentWriter output, InputMap value)
        {
            output.WriteObject(value.KeyboardMap);
            output.WriteObject(value.PadMap);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(InputMapReader).AssemblyQualifiedName;
        }
    }
}

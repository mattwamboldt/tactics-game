using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.Input;

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

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(InputMapReader).AssemblyQualifiedName;
        }
    }
}

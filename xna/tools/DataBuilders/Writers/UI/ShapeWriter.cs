using Board_Game.UI;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.Collections.Generic;

namespace DataBuilder.UI
{
    [ContentTypeWriter]
    class ShapeWriter : ContentTypeWriter<Shape>
    {
        protected override void Write(ContentWriter output, Shape value)
        {
            output.Write(value.Name);
            output.Write(value.Visible);
            output.WriteObject(value.Animation);
            output.WriteObject(value.Children);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(ShapeReader).AssemblyQualifiedName;
        }
    }
}

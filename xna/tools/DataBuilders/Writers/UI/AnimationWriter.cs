using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using BoardGameContent.UI;

namespace DataBuilder.UI
{
    [ContentTypeWriter]
    class ShapeStateWriter : ContentTypeWriter<ShapeState>
    {
        protected override void Write(ContentWriter output, ShapeState value)
        {
            output.Write(value.Position);
            output.Write(value.Size);
            output.Write(value.Color);
            output.Write(value.Frame);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(ShapeStateReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    class AnimationWriter : ContentTypeWriter<Animation>
    {
        protected override void Write(ContentWriter output, Animation value)
        {
            output.WriteObject<ShapeState[]>(value.KeyFrames);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(AnimationReader).AssemblyQualifiedName;
        }
    }
}

using BoardGameContent.UI;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

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

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(ShapeStateReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    class AnimationWriter : ContentTypeWriter<Animation>
    {
        protected override void Write(ContentWriter output, Animation value)
        {
            output.WriteObject(value.KeyFrames);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationReader).AssemblyQualifiedName;
        }
    }
}

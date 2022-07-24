using Board_Game.Rendering;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.Rendering
{
    [ContentTypeWriter]
    class SpriteWriter : ContentTypeWriter<Sprite>
    {
        protected override void Write(ContentWriter output, Sprite value)
        {
            output.Write(value.TextureName);
            output.Write(value.Position);
            output.Write(value.Color);
            output.Write(value.Dimensions);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteReader).AssemblyQualifiedName;
        }
    }
}

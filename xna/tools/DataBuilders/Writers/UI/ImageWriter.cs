using Board_Game.UI;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.UI
{
    [ContentTypeWriter]
    class ImageWriter : ContentTypeWriter<Image>
    {
        ShapeWriter shapeWriter = null;

        protected override void Initialize(ContentCompiler compiler)
        {
            shapeWriter = compiler.GetTypeWriter(typeof(Shape)) as ShapeWriter;

            base.Initialize(compiler);
        }

        protected override void Write(ContentWriter output, Image value)
        {
            output.WriteRawObject(value as Shape, shapeWriter);
            output.Write(value.TextureName);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(ImageReader).AssemblyQualifiedName;
        }
    }
}

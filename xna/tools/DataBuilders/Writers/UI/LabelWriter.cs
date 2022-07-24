using Board_Game.UI;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.UI
{
    [ContentTypeWriter]
    class LabelWriter : ContentTypeWriter<Label>
    {
        ShapeWriter shapeWriter = null;

        protected override void Initialize(ContentCompiler compiler)
        {
            shapeWriter = compiler.GetTypeWriter(typeof(Shape)) as ShapeWriter;

            base.Initialize(compiler);
        }

        protected override void Write(ContentWriter output, Label value)
        {
            output.WriteRawObject(value as Shape, shapeWriter);

            output.Write(value.Font);
            output.Write(value.Text);
            output.Write(value.WrapWidth);
            output.Write(value.Centered);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(LabelReader).AssemblyQualifiedName;
        }
    }
}

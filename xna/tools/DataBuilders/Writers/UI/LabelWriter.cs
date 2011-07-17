using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.UI;

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
            output.WriteRawObject<Shape>(value as Shape, shapeWriter);

            output.Write(value.Font);
            output.Write(value.Text);
            output.Write(value.WrapWidth);
            output.Write(value.Centered);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(LabelReader).AssemblyQualifiedName;
        }
    }
}

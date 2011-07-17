using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.UI;

namespace BoardGameContentBuilders.UI
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
            output.WriteRawObject<Shape>(value as Shape, shapeWriter);
            output.Write(value.TextureName);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(ImageReader).AssemblyQualifiedName;
        }
    }
}

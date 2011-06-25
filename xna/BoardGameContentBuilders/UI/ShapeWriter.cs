using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.UI;
using System.Collections;

namespace BoardGameContentBuilders.UI
{
    [ContentTypeWriter]
    class ShapeWriter : ContentTypeWriter<Shape>
    {
        protected override void Write(ContentWriter output, Shape value)
        {
            output.Write(value.Name);
            output.Write(value.Visible);
            output.WriteObject(value.Animation);
            output.WriteObject<List<Shape>>(value.Children);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(ShapeReader).AssemblyQualifiedName;
        }
    }
}

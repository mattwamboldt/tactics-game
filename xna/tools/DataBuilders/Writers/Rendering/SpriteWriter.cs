using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.Rendering;

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

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(SpriteReader).AssemblyQualifiedName;
        }
    }
}

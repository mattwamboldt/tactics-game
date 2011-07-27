using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.Logic;

namespace DataBuilder.Logic
{
    [ContentTypeWriter]
    class TileWriter : ContentTypeWriter<Tile>
    {
        protected override void Write(ContentWriter output, Tile value)
        {
            output.WriteObject(value.TextureCoordinates);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(TileReader).AssemblyQualifiedName;
        }
    }
}
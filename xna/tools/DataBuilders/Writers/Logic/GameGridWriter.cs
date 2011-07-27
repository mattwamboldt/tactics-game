using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.Logic;

namespace DataBuilder.Logic
{
    [ContentTypeWriter]
    class GameGridWriter : ContentTypeWriter<GameGrid>
    {
        protected override void Write(ContentWriter output, GameGrid value)
        {
            output.Write(value.TileTextureName);
            output.Write(value.MineTextureName);
            output.Write(value.Position);
            output.Write(value.Width);
            output.Write(value.Height);
            output.WriteObject(value.Tiles);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(GameGridReader).AssemblyQualifiedName;
        }
    }
}
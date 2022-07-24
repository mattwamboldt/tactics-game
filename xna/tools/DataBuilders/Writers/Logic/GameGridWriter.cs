using Board_Game.Logic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

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
            output.WriteObject(value.Mines);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(GameGridReader).AssemblyQualifiedName;
        }
    }
}
using Board_Game.Logic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DataBuilder.Logic
{
    [ContentTypeWriter]
    class TileWriter : ContentTypeWriter<Tile>
    {
        protected override void Write(ContentWriter output, Tile value)
        {
            output.WriteObject(value.TextureCoordinates);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TileReader).AssemblyQualifiedName;
        }
    }
}
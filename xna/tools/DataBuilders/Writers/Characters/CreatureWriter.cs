using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.Creatures;
using Microsoft.Xna.Framework;

namespace DataBuilder.Creatures
{
    [ContentTypeWriter]
    class CreatureWriter : ContentTypeWriter<Creature>
    {
        protected override void Write(ContentWriter output, Creature value)
        {
            output.WriteObject<Point>(value.GridLocation);
            output.Write(value.ClassID);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(CreatureReader).AssemblyQualifiedName;
        }
    }
}
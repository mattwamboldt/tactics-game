using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Board_Game.Code.Rendering;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Board_Game.Creatures;

namespace BoardGameContentBuilders.Creatures
{
    [ContentTypeWriter]
    class CreatureDescriptionWriter : ContentTypeWriter<CreatureDescription>
    {
        protected override void Write(ContentWriter output, CreatureDescription value)
        {
            output.Write(value.Name);
            output.Write(value.Description);
            output.Write(value.CanFly);
            output.WriteObject(value.Type);
            output.WriteObject(value.SizeInSpaces);
            output.Write(value.TextureName);
            output.WriteObject(value.AttackPriorities);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.TargetPlatform targetPlatform)
        {
            return typeof(CreatureDescriptionReader).AssemblyQualifiedName;
        }
    }
}

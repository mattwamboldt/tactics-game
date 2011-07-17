using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = System.Data.DataTable;
using TOutput = System.Collections.Generic.List<Board_Game.Creatures.CreatureDescription>;
using System.Data;
using Board_Game.Creatures;

namespace DataBuilder.Processors.Database
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "ClassTableProcessor")]
    public class ClassTableProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            TOutput output = new List<Board_Game.Creatures.CreatureDescription>();

            foreach (DataRow desc in input.Rows)
            {
                CreatureDescription description = new CreatureDescription();
                description.ID = Convert.ToInt32(desc["ID"]);
                description.Name = (string)desc["Name"];
                description.Description = ((string)desc["Description"]).Replace("\\n", Environment.NewLine);
                description.CanFly = Convert.ToBoolean(desc["CanFly"]);
                description.Type = (CreatureType)description.ID;
                description.SizeInSpaces = new Point(
                    Convert.ToInt32(desc["Width"]),
                    Convert.ToInt32(desc["Height"])
                );

                description.TextureName = (string)desc["Texture"];

                List<CreatureType> creaturesToHit = new List<CreatureType>();
                for(int i = 0; i < 5; ++i)
                {
                    CreatureType type = (CreatureType)Convert.ToInt32(desc["Priority" + i]);
                    if (type != CreatureType.Undefined)
                    {
                        creaturesToHit.Add(type);
                    }
                }

                description.AttackPriorities = creaturesToHit.ToArray();

                output.Add(description);
            }

            return output;
        }
    }
}
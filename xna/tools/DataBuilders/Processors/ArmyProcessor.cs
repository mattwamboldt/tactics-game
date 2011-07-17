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
using TOutput = Board_Game.Characters.Army;
using System.Data;
using Board_Game.Creatures;

namespace DataBuilder.Processors
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
    [ContentProcessor(DisplayName = "Army Processor")]
    public class ArmyProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            TOutput output = new Board_Game.Characters.Army();
            output.Members = new List<Creature>();
            if(input.TableName.Contains("Red"))
            {
                output.Side = Board_Game.Creatures.Side.Red;
            }
            else
            {
                output.Side = Board_Game.Creatures.Side.Blue;
            }

            foreach (DataRow unit in input.Rows)
            {
                Creature creature = new Creature();
                creature.ClassID = Convert.ToInt32(unit["classID"]);
                creature.GridLocation = new Point(
                    Convert.ToInt32(unit["x"]),
                    Convert.ToInt32(unit["y"]));
                output.Members.Add(creature);
            }
            
            return output;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = System.Collections.Generic.List<string>;
using TOutput = Board_Game.Logic.GameGrid;
using System.Data;
using Board_Game.Logic;
using Board_Game.Creatures;

namespace DataBuilder.Processors.Logic
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
    [ContentProcessor(DisplayName = "TileMapProcessor")]
    public class TileMapProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            TOutput output = new GameGrid();
            output.TileTextureName = input[0];
            output.MineTextureName = input[1];
            string[] line = input[2].Split(new char[] { ',' });
            output.Position = new Vector2(
                Convert.ToInt32(line[0]),
                Convert.ToInt32(line[1])
            );

            

            output.Width = Convert.ToInt32(line[2]);
            output.Height = Convert.ToInt32(line[3]);

            int numMines = Convert.ToInt32(line[4]);

            output.Tiles = new List<Tile>(output.Width * output.Height);

            for (int y = 0; y < output.Height; y++)
            {
                line = input[3 + y].Split(new char[] { ',' });
                for (int x = 0; x < output.Width; x++)
                {
                    int imageIndex = Convert.ToInt32(line[x]);
                    Tile tile = new Tile();
                    tile.TextureCoordinates = new Point(
                        imageIndex % 16,
                        imageIndex / 16
                    );

                    output.Tiles.Add(tile);
                }
            }

            output.Mines = new List<Mine>(numMines);
            for (int i = 0; i < numMines; i++)
            {
                line = input[3 + output.Height + i].Split(new char[] { ',' });
                Mine mine = new Mine();
                mine.side = (Side)Convert.ToInt32(line[0]);
                mine.position = new Vector2(
                    Convert.ToInt32(line[1]),
                    Convert.ToInt32(line[2])
                );

                output.Mines.Add(mine);
            }


            return output;
        }
    }
}
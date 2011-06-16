using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Board_Game.Code.Logic;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.UI
{
    class Screen
    {
        List<Label> mLabels;

        public Screen()
        {
            mLabels = new List<Label>(16);

            //eventually this will be in the Content pipeline

            //--sidebar header
            Label sideHeader = new Label(
                    "UNITS",
                    "Button",
                    true,
                    0,
                    new Vector2(0,0),
                    new Rectangle(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0, 250, 30)
                );

            mLabels.Add(sideHeader);

            //--unit descriptions
            string[] titles = new string[] { "BOMBER", "FIGHTER", "SOLDIER", "DE-MINER", "BAZOOKA" };
            string[] descriptions = new string[] {
                "Movement: 1 large square\nDestroys ground units.",
                "Movement: 1 large square\nDestroys air units.",
                "Movement: 1 square\nDestroys ground units.",
                "Movement: 1 square\nCaptures mines.",
                "Movement: 1 square\nDestroys all units."
            };

            for (int i = 0; i < 5; i++)
            {
                Label unitTitle = new Label(
                    titles[i],
                    "Tutorial",
                    false,
                    0,
                    new Vector2(
                        GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5 + (int)Tile.TILE_SIZE * 2 + 10 + 5,
                        30 + 20 + 5 + (50 + 15) * i),
                    Rectangle.Empty
                );

                mLabels.Add(unitTitle);

                Label unitDesc = new Label(
                    descriptions[i],
                    "Tutorial",
                    false,
                    GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40,
                    new Vector2(
                        GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5 + (int)Tile.TILE_SIZE * 2 + 10 + 5,
                        30 + 20 + 5 + (50 + 15) * i + 15),
                    Rectangle.Empty
                );

                mLabels.Add(unitDesc);
            }

            //--tutorial text
            String tutorialText = "The Objective of the game is to destroy all opponent units, or take control "
                + "of all mines (squares the are around a diamond). The De-Miner units convert a neutral(grey) "
                + "or enemy mine to your colour, at which point you can move your other units safely across."
                + "\n\nFlying units, which are the bomber and fighter, will not be destroyed by mines.";

            Label tutorial = new Label(
                    tutorialText,
                    "Tutorial",
                    false,
                    GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40,
                    new Vector2(5, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 20 + 30 + 5),
                    Rectangle.Empty
                );

            mLabels.Add(tutorial);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (Label label in mLabels)
            {
                label.Render(spriteBatch);
            }
        }
    }
}

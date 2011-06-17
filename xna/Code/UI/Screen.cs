using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Board_Game.Logic;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.UI
{
    class Screen
    {
        List<Shape> mShapes;

        public Screen(
            GraphicsDevice device,
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture)
        {
            mShapes = new List<Shape>(32);

            Texture2D pixel = new Texture2D(device, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            //eventually this will be in the Content pipeline
            //draw the UI
            //TODO: Move UI into a tool and class to remove all of these consants and draw calls
            //-draw a box under the grid
            Image gridBackground = new Image(pixel, new Vector2(0, 0), new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 30), Color.Black);
            mShapes.Add(gridBackground);

            //-draw the box under UNITS
            Image titleBackground = new Image(pixel, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0), new Vector2(250, 30), Color.Black);
            mShapes.Add(titleBackground);

            //-draw the tutorial text box
            Image tutorialBackground = new Image(pixel, new Vector2(0, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 20 + 30), new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, 200), Color.Black);
            mShapes.Add(tutorialBackground);

            //-draw the unit sidebar box
            Image sidebarBackground = new Image(pixel, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 50), new Vector2(250, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 240), Color.Black);
            mShapes.Add(sidebarBackground);

            //-draw the unit backgrounds
            for (int i = 0; i < 5; i++)
            {
                Image unitBG = new Image(
                    pixel,
                    new Vector2(
                        GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5,
                        30 + 20 + 5 + (50 + 15) * i),
                    new Vector2(
                        (int)Tile.TILE_SIZE * 2 + 10,
                        (int)Tile.TILE_SIZE * 2 + 10),
                    Color.DarkGray
                );

                mShapes.Add(unitBG);
            }

            //-draw the units
            Image bomber = new Image(bomberTexture, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5), new Vector2( (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            mShapes.Add(bomber);
            Image fighter = new Image(fighterTexture, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + 50 + 15), new Vector2( (int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            mShapes.Add(fighter); 
            Image soldier = new Image(soldierTexture, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + (50 + 15) * 2), new Vector2((int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            mShapes.Add(soldier); 
            Image miner = new Image(deminerTexture, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + (50 + 15) * 3), new Vector2((int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            mShapes.Add(miner); 
            Image grenadier = new Image(grenadierTexture, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20 + 5, 30 + 20 + 5 + (50 + 15) * 4), new Vector2((int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10), Color.Red);
            mShapes.Add(grenadier);

            //-draw the buttons
            Image redBGEdge = new Image(pixel, new Vector2(20 - 1, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 - 1), new Vector2(100 + 2, 20 + 2), Color.White);
            mShapes.Add(redBGEdge); 
            Image redBG = new Image(pixel, new Vector2(20, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40), new Vector2(100, 20), Color.Red);
            mShapes.Add(redBG);

            Image blueBGEdge = new Image(pixel, new Vector2(20 - 1 + 200, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 - 1), new Vector2(100 + 2, 20 + 2), Color.White);
            mShapes.Add(blueBGEdge); 
            Image blueBG = new Image(pixel, new Vector2(20 + 200, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40), new Vector2(100, 20), Color.Blue);
            mShapes.Add(blueBG);

            //--sidebar header
            Label sideHeader = new Label(
                    "UNITS",
                    "Button",
                    true,
                    0,
                    new Vector2(0,0),
                    new Rectangle(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0, 250, 30)
                );

            mShapes.Add(sideHeader);

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

                mShapes.Add(unitTitle);

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

                mShapes.Add(unitDesc);
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

            mShapes.Add(tutorial);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (Shape shape in mShapes)
            {
                shape.Render(spriteBatch);
            }
        }
    }
}

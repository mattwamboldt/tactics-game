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
        Shape mRoot;

        public Shape Root { get { return mRoot; } }

        public Screen(
            GraphicsDevice device,
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture)
        {
            mRoot = new Shape(ShapeType.Clip, "root", Color.White, Vector2.Zero, Vector2.Zero);

            Texture2D pixel = new Texture2D(device, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            //eventually this will be in the Content pipeline
            //draw the UI
            //TODO: Move UI into a tool and class to remove all of these consants and draw calls
            //-draw a box under the grid
            Image gridBackground = new Image("gridBackground", pixel, new Vector2(0, 0), new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 30), Color.Black);
            mRoot.AddChild(gridBackground);

            //-draw the buttons
            Image redBGEdge = new Image("redBGEdge", pixel, new Vector2(20 - 1, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 - 1), new Vector2(100 + 2, 20 + 2), Color.White);
            gridBackground.AddChild(redBGEdge);
            Image redBG = new Image("redBG", pixel, new Vector2(1, 1), new Vector2(100, 20), Color.Red);
            redBGEdge.AddChild(redBG);

            Image blueBGEdge = new Image("blueBGEdge", pixel, new Vector2(20 - 1 + 200, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 - 1), new Vector2(100 + 2, 20 + 2), Color.White);
            gridBackground.AddChild(blueBGEdge);
            Image blueBG = new Image("blueBG", pixel, new Vector2(1, 1), new Vector2(100, 20), Color.Blue);
            blueBGEdge.AddChild(blueBG);



            //-draw title
            Image titleBackground = new Image("titleBackground", pixel, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0), new Vector2(250, 30), Color.Black);
            mRoot.AddChild(titleBackground);

            Label sideHeader = new Label(
                    "sideHeader",
                    "UNITS",
                    "Button",
                    true,
                    0,
                    new Vector2(0, 0),
                    new Rectangle(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 0, 250, 30)
                );

            titleBackground.AddChild(sideHeader);
            //end title


            //-draw tutorial
            Image tutorialBackground = new Image("tutorialBackground", pixel, new Vector2(0, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 40 + 20 + 30), new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40, 200), Color.Black);
            mRoot.AddChild(tutorialBackground);

            //--tutorial text
            String tutorialText = "The Objective of the game is to destroy all opponent units, or take control "
                + "of all mines (squares the are around a diamond). The De-Miner units convert a neutral(grey) "
                + "or enemy mine to your colour, at which point you can move your other units safely across."
                + "\n\nFlying units, which are the bomber and fighter, will not be destroyed by mines.";

            Label tutorial = new Label(
                    "tutorial",
                    tutorialText,
                    "Tutorial",
                    false,
                    GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40,
                    new Vector2(5, 5),
                    Rectangle.Empty
                );

            tutorialBackground.AddChild(tutorial);

            //end tutorial


            //-draw the unit sidebar box
            Image sidebarBackground = new Image("sidebarBackground", pixel, new Vector2(GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40 + 20, 50), new Vector2(250, GameState.GRID_HEIGHT * (int)Tile.TILE_SIZE + 240), Color.Black);
            mRoot.AddChild(sidebarBackground);

            Texture2D[] unitTextures = new Texture2D[] { bomberTexture, fighterTexture, soldierTexture, deminerTexture, grenadierTexture };
            //--unit descriptions
            string[] titles = new string[] { "BOMBER", "FIGHTER", "SOLDIER", "DE-MINER", "BAZOOKA" };
            string[] descriptions = new string[] {
                "Movement: 1 large square\nDestroys ground units.",
                "Movement: 1 large square\nDestroys air units.",
                "Movement: 1 square\nDestroys ground units.",
                "Movement: 1 square\nCaptures mines.",
                "Movement: 1 square\nDestroys all units."
            };

            //-draw the unit backgrounds
            for (int i = 0; i < 5; i++)
            {
                Image unitBG = new Image(
                    "unitBG" + i,
                    pixel,
                    new Vector2(5, 5 + (50 + 15) * i),
                    new Vector2(
                        (int)Tile.TILE_SIZE * 2 + 10,
                        (int)Tile.TILE_SIZE * 2 + 10),
                    Color.DarkGray
                );

                sidebarBackground.AddChild(unitBG);

                Image unit = new Image(
                    "unit" + i,
                    unitTextures[i],
                    Vector2.Zero,
                    new Vector2((int)Tile.TILE_SIZE * 2 + 10, (int)Tile.TILE_SIZE * 2 + 10),
                    Color.Red);

                unitBG.AddChild(unit);

                Label unitTitle = new Label(
                    "unitTitle" + i,
                    titles[i],
                    "Tutorial",
                    false,
                    0,
                    new Vector2((int)Tile.TILE_SIZE * 2 + 10 + 5, 0),
                    Rectangle.Empty
                );

                unitBG.AddChild(unitTitle);

                Label unitDesc = new Label(
                    "unitDesc" + i,
                    descriptions[i],
                    "Tutorial",
                    false,
                    GameState.GRID_WIDTH * (int)Tile.TILE_SIZE + 40,
                    new Vector2((int)Tile.TILE_SIZE * 2 + 10 + 5, 15),
                    Rectangle.Empty
                );

                unitBG.AddChild(unitDesc);
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            mRoot.Render(spriteBatch);
        }
    }
}

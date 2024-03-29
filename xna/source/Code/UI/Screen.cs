﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Board_Game.Logic;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Rendering;
using Microsoft.Xna.Framework.Content;
using Board_Game.Creatures;

namespace Board_Game.UI
{
    class Screen
    {
        Shape mRoot;

        public Shape Root { get { return mRoot; } }

        public Screen(GraphicsDevice device, ContentManager content)
        {
            mRoot = content.Load<Shape>("xml/Screen");

            //popup
            Root.GetNode("gridBackground.victorBGEdge").CenterAlign();
            Root.GetNode("gridBackground.victorBGEdge.victorBG").CenterAlign();
        }

        public void Update(GameState gameState)
        {
            mRoot.Update();
            string redString = "HUMAN";
            string blueString = "HUMAN";
            if (!gameState.Red.mIsHuman)
            {
                redString = "AI";
            }

            Label redText = (Label)Root.GetNode("tutorialBackground.redBGEdge.redBG.redText");
            redText.Text = redString;

            if (!gameState.Blue.mIsHuman)
            {
                blueString = "AI";
            }

            Label blueText = (Label)Root.GetNode("tutorialBackground.blueBGEdge.blueBG.blueText");
            blueText.Text = blueString;

            //shows and hides the info box
            Vector2 selectorPosition = gameState.Selector.position;
            Creature occupiedCreature = gameState.mGrid.Occupants[(int)selectorPosition.X, (int)selectorPosition.Y];
            Image creatureInfo = (Image)Root.GetNode("creatureInfo");

            if (occupiedCreature == null)
            {
                creatureInfo.Visible = false;
            }
            else
            {
                creatureInfo.Visible = true;

                CreatureDescription desc = occupiedCreature.Class;
                Image info = (Image)creatureInfo.GetNode("imageBG.image");
                info.TextureName = desc.TextureName;
                info.Texture = desc.Texture;

                if(occupiedCreature.side == Side.Blue)
                {
                    info.Color = Color.Blue;
                }
                else
                {
                    info.Color = Color.Red;
                }

                ((Label)info.GetChild("name")).Text = desc.Name;
                ((Label)info.GetChild("description")).Text = desc.Description;
            }
            

            if (gameState.winner != Side.Neutral)
            {
                //we have a winner
                string victorString = "";
                if (gameState.winner == Side.Red)
                {
                    victorString = "Red has won!";
                }
                else if (gameState.winner == Side.Blue)
                {
                    victorString = "Blue has won";
                }

                Shape victor = Root.GetNode("gridBackground.victorBGEdge");
                victor.Visible = true;

                Label victorText = (Label)victor.GetNode("victorBG.victorText");
                victorText.Text = victorString;

                victor.Width = victorText.Width + 24;
                victor.GetChild("victorBG").Width = victor.Width - 4;

                victor.CenterAlign();
                victor.GetChild("victorBG").CenterAlign();

            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            mRoot.Render(spriteBatch);
        }
    }
}

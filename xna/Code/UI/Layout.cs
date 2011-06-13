using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.UI
{
    class Layout
    {
        /// <summary>
        /// Takes an initial string and adds carriage returns to make it fit within a given width.
        /// </summary>
        /// <param name="width">The width of the space to fit into</param>
        /// <param name="font">The font to render for measuring size</param>
        /// <param name="text">The text to wrap</param>
        /// <returns>The string with carriage returns to fit into the width</returns>
        public static String WrapString(float width, SpriteFont font, String text)
        {
            //split the incoming text on spaces
            string[] words = text.Split(new Char[] { ' ' });

            //add each string to the builder measuring the width each time
            //if its greater than the desired, add a carriage return
            StringBuilder builder = new StringBuilder(text.Length);

            foreach (string word in words)
            {
                if (font.MeasureString(builder.ToString() + word + " ").X > width)
                {
                    builder.Append("\n");
                }

                builder.Append(word + " ");
            }

            return builder.ToString();
        }

        public static Vector2 CenterAlign(Rectangle parentRectangle, string text, SpriteFont font)
        {
            return CenterAlign(parentRectangle, font.MeasureString(text));
        }

        public static Vector2 CenterAlign(Rectangle parentRectangle, Vector2 dimensions)
        {
            return new Vector2(parentRectangle.Center.X - (int)(dimensions.X / 2), parentRectangle.Center.Y - (int)(dimensions.Y / 2));
        }
    }
}

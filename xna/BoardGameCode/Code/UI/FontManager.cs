using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.UI
{
    //controls loading and lookup of fonts
    class FontManager
    {
        Hashtable mFontCollection;

        private static FontManager mInstance;

        public static void Initialize(ContentManager content) { mInstance = new FontManager(content); }
        public static FontManager Get() { return mInstance; }

        public FontManager(ContentManager content)
        {
            mFontCollection = new Hashtable();

            mFontCollection.Add("Button", content.Load<SpriteFont>("fonts/Button"));
            mFontCollection.Add("Tutorial", content.Load<SpriteFont>("fonts/Tutorial"));
            mFontCollection.Add("UnitName", content.Load<SpriteFont>("fonts/UnitName"));
        }

        public SpriteFont Find(string fontName)
        {
            return (SpriteFont)mFontCollection[fontName];
        }
    }
}

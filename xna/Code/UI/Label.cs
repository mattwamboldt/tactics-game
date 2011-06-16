using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Code.Logic;

namespace Board_Game.Code.UI
{
    class Label
    {
        private string mText;
        private SpriteFont mFont;
        private float mMaxWidth;
        private bool mIsCentred;
        private Rectangle mParentContainer;
        private Vector2 mPosition;

        public Label(string text, string fontName, bool centreAlign, float maxWidth, Vector2 position, Rectangle parentRect)
        {
            mText = text;
            mFont = FontManager.Get().Find(fontName);
            mMaxWidth = maxWidth;
            mIsCentred = centreAlign;
            mParentContainer = parentRect;
            mPosition = position;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Vector2 position = mPosition;
            string text = mText;
            if (mMaxWidth != 0)
            {
                text = Layout.WrapString(mMaxWidth, mFont, text);
            }

            if (mIsCentred)
            {
                position = Layout.CenterAlign(mParentContainer, text, mFont);
            }

            spriteBatch.DrawString(mFont, text, position, Color.White);
        }
    }
}

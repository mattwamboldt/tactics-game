using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;

namespace Board_Game.UI
{
    class Label : Shape
    {
        private string mText;
        private SpriteFont mFont;
        private float mMaxWidth;
        private bool mIsCentred;
        private Rectangle mParentContainer;

        public Label(
            string text,
            string fontName,
            bool centreAlign,
            float maxWidth,
            Vector2 position,
            Rectangle parentRect
            ) : base(ShapeType.Label, Color.White, new Vector2 (0,0), position)
        {
            mText = text;
            mFont = FontManager.Get().Find(fontName);
            mMaxWidth = maxWidth;
            mIsCentred = centreAlign;
            mParentContainer = parentRect;
            mPosition = position;
        }

        public override void Render(SpriteBatch spriteBatch)
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

            spriteBatch.DrawString(mFont, text, position, mColor);

            base.Render(spriteBatch);
        }
    }
}

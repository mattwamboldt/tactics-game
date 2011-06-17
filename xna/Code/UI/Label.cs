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
        public string Text { get { return mText; } set { mText = value; mSize = mFont.MeasureString(mText); } }

        private SpriteFont mFont;
        private float mMaxWidth;
        private bool mIsCentred;

        public Label(
            string name,
            string text,
            string fontName,
            bool visibility,
            bool centreAlign,
            float maxWidth,
            Vector2 position
            )
            : base(ShapeType.Label, name, Color.White, new Vector2(0, 0), position, visibility)
        {
            mText = text;
            mFont = FontManager.Get().Find(fontName);
            mMaxWidth = maxWidth;
            mSize = mFont.MeasureString(mText);

            mIsCentred = centreAlign;
            mPosition = position;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (mVisibility)
            {
                Vector2 position = mAbsolutePosition;
                string text = mText;
                if (mMaxWidth != 0)
                {
                    text = Layout.WrapString(mMaxWidth, mFont, text);
                    mSize = mFont.MeasureString(text);
                }

                if (mIsCentred)
                {
                    CenterAlign();
                }

                spriteBatch.DrawString(mFont, text, mAbsolutePosition, mColor);

                base.Render(spriteBatch);
            }
        }
    }
}

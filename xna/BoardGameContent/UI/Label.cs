using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Board_Game.UI
{
    public class Label : Shape
    {
        private SpriteFont mFont;

        private string mFontName;
        public string Font { get { return mFontName; } set { mFontName = value;} }

        private string mText;
        public string Text { get { return mText; } set { mText = value; } }

        private float mMaxWidth;
        public float WrapWidth { get { return mMaxWidth; } set { mMaxWidth = value; } }

        private bool mIsCentred;
        public bool Centered { get { return mIsCentred; } set { mIsCentred = value; } }

        public Label():base() {}
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

        //Call this after the class is loaded in the content manager
        public void LoadFont()
        {
            mFont = FontManager.Get().Find(mFontName);
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
                    mSize = mFont.MeasureString(mText);
                    CenterAlign();
                }

                spriteBatch.DrawString(mFont, text, mAbsolutePosition, mColor);

                base.Render(spriteBatch);
            }
        }
    }

    public class LabelReader : ContentTypeReader<Label>
    {
        protected override Label Read(ContentReader input, Label existingInstance)
        {
            Label label = existingInstance;
            if (label == null)
            {
                label = new Label();
            }

            // read the shape
            input.ReadRawObject<Shape>(label as Shape);

            // read label

            label.Font = input.ReadString();
            label.Text = input.ReadString();
            label.WrapWidth = input.ReadSingle();
            label.Centered = input.ReadBoolean();

            label.LoadFont();

            return label;

        }
    }
}

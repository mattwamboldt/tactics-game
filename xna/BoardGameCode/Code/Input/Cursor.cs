using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Input
{
    class Cursor
    {
        MouseState mPreviousMouseState;
        MouseState mCurrentMouseState;
        Texture2D mTexture;

        public Cursor(Texture2D texture)
        {
            mTexture = texture;
            Update();
        }

        public void Update()
        {
            mPreviousMouseState = mCurrentMouseState;
            mCurrentMouseState = Mouse.GetState();
        }

        public Point GetPosition()
        {
            return new Point(mCurrentMouseState.X, mCurrentMouseState.Y);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, new Rectangle(mCurrentMouseState.X, mCurrentMouseState.Y, mTexture.Width, mTexture.Height), Color.White);
        }

        public bool IsLeftClick()
        {
            return (mCurrentMouseState.LeftButton == ButtonState.Pressed
                && mPreviousMouseState.LeftButton == ButtonState.Released);
        }

        public bool IsRightClick()
        {
            return (mCurrentMouseState.RightButton == ButtonState.Pressed
                && mPreviousMouseState.RightButton == ButtonState.Released);
        }
    }
}

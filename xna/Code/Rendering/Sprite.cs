using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Rendering
{
    class Sprite
    {
        Texture2D mTexture;
        Vector2 mPosition;
        Color mColour;
        Vector2 mDimensions;

        public Color Color { set { mColour = value; } }
        public Vector2 Position {
            get { return mPosition; }
            set { mPosition = value; }
        }

        public Sprite(
            Texture2D texture,
            Vector2 position,
            Color colour,
            Vector2 dimensions
            )
        {
            mTexture = texture;
            mPosition = position;
            mColour = colour;
            mDimensions = dimensions;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            Vector2 renderPosition = new Vector2(
                parentPosition.X + mPosition.X,
                parentPosition.Y + mPosition.Y
            );
            
            float scale = mDimensions.X / mTexture.Width;

            spriteBatch.Draw(
                mTexture,
                renderPosition,
                null,
                mColour,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}

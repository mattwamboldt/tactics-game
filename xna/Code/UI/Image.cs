using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.UI
{
    class Image : Shape
    {
        Texture2D mTexture;

        public Image(Texture2D texture, Vector2 position, Vector2 size, Color color)
            : base(ShapeType.Image, color, size, position)
        {
            mTexture = texture;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)mSize.X, (int)mSize.Y), mColor);
            base.Render(spriteBatch);
        }
    }
}

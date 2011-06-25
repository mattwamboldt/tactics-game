using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Board_Game.Rendering;

namespace Board_Game.UI
{
    public class Image : Shape
    {
        Texture2D mTexture;

        [ContentSerializerIgnore]
        public Texture2D Texture { get { return mTexture; } set { mTexture = value; } }

        string mTextureName;
        public string TextureName { get { return mTextureName; } set { mTextureName = value; } }

        public Image() { }
        public Image(String name, Texture2D texture, Vector2 position, Vector2 size, Color color, bool visibility)
            : base(ShapeType.Image, name, color, size, position, visibility)
        {
            mTexture = texture;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (mVisibility)
            {
                spriteBatch.Draw(mTexture, new Rectangle((int)mAbsolutePosition.X, (int)mAbsolutePosition.Y, (int)Size.X, (int)Size.Y), this.Color);
                base.Render(spriteBatch);
            }
        }
    }

    public class ImageReader : ContentTypeReader<Image>
    {
        protected override Image Read(ContentReader input, Image existingInstance)
        {
            Image image = existingInstance;
            if (image == null)
            {
                image = new Image();
            }

            // read the shape
            input.ReadRawObject<Shape>(image as Shape);

            // read image
            image.TextureName = input.ReadString();
            image.Texture = TextureManager.Get().Find(image.TextureName);
            return image;
        }
    }
}

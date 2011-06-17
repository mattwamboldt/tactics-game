using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Board_Game.Code.Rendering;

namespace Board_Game.Rendering
{
    public class Sprite
    {
        public Sprite() { }
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

        Texture2D mTexture;

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return mTexture; }
            set { mTexture = value; }
        }

        string textureName;
        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }

        Vector2 mPosition;
        public Vector2 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        Color mColour;
        public Color Color
        {
            get { return mColour; }
            set { mColour = value; }
        }
        
        Vector2 mDimensions;
        public Vector2 Dimensions
        {
            get { return mDimensions; }
            set { mDimensions = value; }
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
    
    public class SpriteReader : ContentTypeReader<Sprite>
    {
        protected override Sprite Read(ContentReader input, Sprite existingInstance)
        {
            Sprite output = new Sprite();
            output.Texture = TextureManager.Get().Find(input.ReadString());
            output.Position = input.ReadVector2();
            output.Color = input.ReadColor();
            output.Dimensions = input.ReadVector2();
            return output;
        }
    }
}

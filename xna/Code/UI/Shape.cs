using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.UI
{
    enum ShapeType
    {
        Image,
        Label
    }

    class Shape
    {
        protected Vector2 mPosition;
        protected Vector2 mSize;
        protected Color mColor;
        protected ShapeType mType;

        protected Shape(ShapeType type, Color color, Vector2 size, Vector2 position)
        {
            mType = type;
            mColor = color;
            mSize = size;
            mPosition = position;
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
        }
    }
}

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
        Clip,
        Image,
        Label
    }

    class Shape
    {
        protected ShapeType mType;
        
        protected Vector2 mPosition; //Defines position as an offset from the parent
        protected Vector2 mAbsolutePosition; //Defines screen space position
        protected List<Shape> mChildren;//the items under this in the tree

        protected Vector2 mSize;
        protected Color mColor;

        public Shape(ShapeType type, Color color, Vector2 size, Vector2 position)
        {
            mType = type;
            mColor = color;
            mSize = size;
            mPosition = position;
            mChildren = new List<Shape>();
        }

        public void SetPosition(Vector2 newPosition, bool isRelative)
        {
            if (isRelative)
            {
                //here, the position of the object is being set
                mAbsolutePosition.X += newPosition.X - mPosition.X;
                mAbsolutePosition.Y += newPosition.Y - mPosition.Y;
                mPosition = newPosition;
            }
            else
            {
                //here, the position of the parent is being set
                mAbsolutePosition.X = newPosition.X + mPosition.X;
                mAbsolutePosition.Y = newPosition.Y + mPosition.Y;
            }

            foreach (Shape child in mChildren)
            {
                child.SetPosition(mAbsolutePosition, false);
            }
        }

        public void AddChild(Shape child)
        {
            mChildren.Add(child);
            child.SetPosition(mAbsolutePosition, false);
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            foreach (Shape child in mChildren)
            {
                child.Render(spriteBatch);
            }
        }
    }
}

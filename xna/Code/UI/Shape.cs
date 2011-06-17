using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

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

        protected string mName;
        
        protected Vector2 mPosition; //Defines position as an offset from the parent
        protected Vector2 mAbsolutePosition; //Defines screen space position
        protected Hashtable mChildren;//the items under this in the tree

        protected Vector2 mSize;
        public float Width { get { return mSize.X; } set { mSize.X = value; } }

        protected bool mVisibility;
        public bool Visible { get { return mVisibility; } set { mVisibility = value; } }

        protected Color mColor;
        public Color Color { get { return mColor; } set { mColor = value; } }
        protected Shape mParent;

        public Shape(ShapeType type, string name, Color color, Vector2 size, Vector2 position, bool visibility)
        {
            mType = type;
            mColor = color;
            mSize = size;
            mPosition = position;
            mChildren = new Hashtable();
            mName = name;
            mVisibility = visibility;
            mParent = null;
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

            foreach (Shape child in mChildren.Values)
            {
                child.SetPosition(mAbsolutePosition, false);
            }
        }

        public void AddChild(Shape child)
        {
            mChildren.Add(child.mName, child);
            child.mParent = this;
            child.SetPosition(mAbsolutePosition, false);
        }

        public Shape GetChild(string name)
        {
            return (Shape)mChildren[name];
        }

        /// <summary>
        /// A utility function for getting a shape at a given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Shape GetNode(string path)
        {
            //start at the root
            Shape node = this;

            string[] names = path.Split(new char[] { '.' });
            foreach (string childName in names)
            {
                node = node.GetChild(childName);
            }

            if (node == this)
            {
                return null;
            }

            return node;
        }

        public void CenterAlign()
        {
            SetPosition(new Vector2(mParent.mSize.X / 2 - (int)(mSize.X / 2),mParent.mSize.Y / 2 - (int)(mSize.Y / 2)) , true);
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            if(mVisibility)
            {
                foreach (Shape child in mChildren.Values)
                {
                    child.Render(spriteBatch);
                }
            }
        }
    }
}

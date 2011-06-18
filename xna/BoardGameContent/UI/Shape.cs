using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework.Content;

namespace Board_Game.UI
{
    public enum ShapeType
    {
        Clip,
        Image,
        Label
    }

    public class Shape
    {
        protected ShapeType mType;

        protected string mName;
        public string Name { get { return mName; } set { mName = value; } }
        
        protected Vector2 mPosition; //Defines position as an offset from the parent
        public Vector2 Position { get { return mPosition; } set { mPosition = value; } }

        protected Vector2 mAbsolutePosition; //Defines screen space position

        protected Vector2 mSize;
        public Vector2 Size { get { return mSize; } set { mSize = value; } }

        protected bool mVisibility;
        public bool Visible { get { return mVisibility; } set { mVisibility = value; } }

        protected Color mColor;
        public Color Color { get { return mColor; } set { mColor = value; } }

        protected List<Shape> mChildren;//the items under this in the tree
        public List<Shape> Children { get { return mChildren; } set { mChildren = value; } }

        [ContentSerializerIgnore]
        public float Width { get { return mSize.X; } set { mSize.X = value; } }

        protected Shape mParent;

        public Shape()
        {
            mType = ShapeType.Clip;
            mParent = null;
        }

        public Shape(ShapeType type, string name, Color color, Vector2 size, Vector2 position, bool visibility)
        {
            mType = type;
            mColor = color;
            mSize = size;
            mPosition = position;
            mChildren = new List<Shape>();
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

            foreach (Shape child in mChildren)
            {
                child.SetPosition(mAbsolutePosition, false);
            }
        }

        public void AddChild(Shape child)
        {
            mChildren.Add(child);
            SetAsParent(child);
        }

        //this is split out for the content pipeline so that
        //children aren't added infinitely
        public void SetAsParent(Shape child)
        {
            child.mParent = this;
            child.SetPosition(mAbsolutePosition, false);
        }

        public Shape GetChild(string name)
        {
            foreach (Shape child in mChildren)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }

            return null;
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
                foreach (Shape child in mChildren)
                {
                    child.Render(spriteBatch);
                }
            }
        }
    }

    public class ShapeReader : ContentTypeReader<Shape>
    {
        protected override Shape Read(ContentReader input, Shape existingInstance)
        {
            Shape shape = existingInstance;
            if (shape == null)
            {
                shape = new Shape();
            }

            shape.Name = input.ReadString();
            shape.Position = input.ReadVector2();
            shape.Size = input.ReadVector2();
            shape.Visible = input.ReadBoolean();
            shape.Color = input.ReadColor();
            shape.Children = input.ReadObject<List<Shape>>();

            foreach (Shape child in shape.Children)
            {
                shape.SetAsParent(child);
            }

            return shape;
        }
    }
}

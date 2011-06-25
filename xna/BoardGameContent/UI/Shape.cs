using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using BoardGameContent.UI;

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

        protected Vector2 mAbsolutePosition; //Defines screen space position

        protected bool mVisibility;
        public bool Visible { get { return mVisibility; } set { mVisibility = value; } }

        protected Animation mAnimation;
        public Animation Animation { get { return mAnimation; } set { mAnimation = value; } }

        protected List<Shape> mChildren;//the items under this in the tree
        public List<Shape> Children { get { return mChildren; } set { mChildren = value; } }

        [ContentSerializerIgnore]
        public float Width { get { return mAnimation.CurrentFrame.Width; }  set { mAnimation.CurrentFrame.Width = value; }}

        [ContentSerializerIgnore]
        public Vector2 Size { get { return mAnimation.CurrentFrame.Size; } set { mAnimation.CurrentFrame.Size = value; } }

        [ContentSerializerIgnore]
        public Vector2 Position { get { return mAnimation.CurrentFrame.Position; } set { mAnimation.CurrentFrame.Position = value; } }

        [ContentSerializerIgnore]
        public Color Color { get { return mAnimation.CurrentFrame.Color; } set { mAnimation.CurrentFrame.Color = value; } }

        protected Shape mParent;

        public Shape()
        {
            mType = ShapeType.Clip;
            mParent = null;
        }

        public Shape(ShapeType type, string name, Color color, Vector2 size, Vector2 position, bool visibility)
        {
            mType = type;
            mAnimation = new Animation();
            mAnimation.KeyFrames = new ShapeState[1];
            mAnimation.KeyFrames[0] = new ShapeState();
            mAnimation.KeyFrames[0].Color = color;
            mAnimation.KeyFrames[0].Size = size;
            mAnimation.KeyFrames[0].Position = position;
            mAnimation.KeyFrames[0].Frame = 0;
            mAnimation.Reset();

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
                mAbsolutePosition.X += newPosition.X - mAnimation.CurrentFrame.Position.X;
                mAbsolutePosition.Y += newPosition.Y - mAnimation.CurrentFrame.Position.Y;
                mAnimation.CurrentFrame.Position = newPosition;
            }
            else
            {
                //here, the position of the parent is being set
                mAbsolutePosition.X = newPosition.X + mAnimation.CurrentFrame.Position.X;
                mAbsolutePosition.Y = newPosition.Y + mAnimation.CurrentFrame.Position.Y;
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
            SetPosition(
                new Vector2(
                    mParent.Animation.CurrentFrame.Size.X / 2 - (int)(Animation.CurrentFrame.Size.X / 2),
                    mParent.Animation.CurrentFrame.Size.Y / 2 - (int)(Animation.CurrentFrame.Size.Y / 2)
                ),
                true);
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
            shape.Visible = input.ReadBoolean();
            shape.Animation = input.ReadObject<Animation>();
            shape.Children = input.ReadObject<List<Shape>>();

            foreach (Shape child in shape.Children)
            {
                shape.SetAsParent(child);
            }

            return shape;
        }
    }
}

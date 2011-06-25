using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BoardGameContent.UI
{
    //These are factors of a shape that can change over time in animations
    public class ShapeState
    {
        protected Vector2 mPosition; //Defines position as an offset from the parent
        public Vector2 Position { get { return mPosition; } set { mPosition = value; } }

        protected Vector2 mSize;
        public Vector2 Size { get { return mSize; } set { mSize = value; } }

        protected Color mColor;
        public Color Color { get { return mColor; } set { mColor = value; } }

        protected int mFrame;
        public int Frame { get { return mFrame; } set { mFrame = value; } }

        public ShapeState() { }
        public ShapeState(ShapeState orginal)
        {
            Position = orginal.Position;
            Size = orginal.Size;
            Color = orginal.Color;
            Frame = orginal.Frame;
        }
    }

    public class ShapeStateReader : ContentTypeReader<ShapeState>
    {
        protected override ShapeState Read(ContentReader input, ShapeState existingInstance)
        {
            ShapeState state = new ShapeState();

            state.Position = input.ReadVector2();
            state.Size = input.ReadVector2();
            state.Color = input.ReadColor();
            state.Frame = input.ReadInt32();

            return state;
        }
    }
}

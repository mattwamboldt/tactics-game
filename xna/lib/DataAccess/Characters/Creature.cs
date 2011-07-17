using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;
using Board_Game.Rendering;
using Microsoft.Xna.Framework.Content;
using Board_Game.DB;

namespace Board_Game.Creatures
{
    public enum Side
    {
        Red = 0,
        Blue = 1,
        Neutral = 2
    }

    public struct ClampArea
    {
        public int leftCut;
        public int rightCut;
        public int topCut;
        public int bottomCut;
    };

    //Contains all the code for a Creature.
    public class Creature
    {
        //Exposed properties

        private Point mGridLocation;
        public Point GridLocation
        {
            get { return mGridLocation; }
            set { mGridLocation = value; }
        }

        public int ClassID;

        //Private Data filled in by other areas

        //this allows us to set the colour
        [ContentSerializerIgnore]
        public Side side;
        
        [ContentSerializerIgnore]
        public bool isSelected;

        private CreatureDescription mCreatureDesc;

        [ContentSerializerIgnore]
        public CreatureDescription Class
        {
            get { return mCreatureDesc; }
        }

        private Sprite mSprite;

        public Creature()
        {
            mGridLocation.X = 0;
            mGridLocation.Y = 0;
            side = Side.Neutral;
            isSelected = false;
        }

        public void LinkData()
        {
            mCreatureDesc = DatabaseManager.Get().CreatureTable[ClassID];
            mSprite = new Sprite(mCreatureDesc.Texture, new Vector2(0, 0), Color.White, new Vector2(GridWidth * Tile.TILE_SIZE, GridHeight * Tile.TILE_SIZE));
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            mSprite.Color = Color.White;
            mSprite.Texture = mCreatureDesc.Texture;
            mSprite.Dimensions = new Vector2(GridWidth * Tile.TILE_SIZE, GridHeight * Tile.TILE_SIZE);

            if (side == Side.Red)
            {
                mSprite.Color = Color.Red;
            }
            else if (side == Side.Blue)
            {
                mSprite.Color = Color.Blue;
            }

            if (isSelected)
            {
                mSprite.Color = Color.Yellow;
            }

            mSprite.Render(spriteBatch, parentPosition);
        }

        public void SetLocation(int newX, int newY)
        {
            mGridLocation.X = newX;
            mGridLocation.Y = newY;
            mSprite.Position = new Vector2(newX * Tile.TILE_SIZE, newY * Tile.TILE_SIZE);
        }

        public ClampArea GetClampArea()
        {
            ClampArea returnValue;
            returnValue.leftCut = (int)(Position.X - ScreenDimensions().X);
            returnValue.rightCut = (int)(Position.X + ScreenDimensions().X);
            returnValue.topCut = (int)(Position.Y - ScreenDimensions().Y);
            returnValue.bottomCut = (int)(Position.Y + ScreenDimensions().Y);
            return returnValue;
        }

        //Exposing member attributes for simplicity

        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get { return mSprite.Position; }
        }

        [ContentSerializerIgnore]
        public int GridWidth
        {
            get { return mCreatureDesc.SizeInSpaces.X; }
        }

        [ContentSerializerIgnore]
        public int GridHeight
        {
            get { return mCreatureDesc.SizeInSpaces.Y; }
        }

        [ContentSerializerIgnore]
        public CreatureType Type
        {
            get { return mCreatureDesc.Type; }
        }

        [ContentSerializerIgnore]
        public bool CanFly
        {
            get { return mCreatureDesc.CanFly; }
        }
        
        public int GetX()
        {
            return mGridLocation.X;
        }

        public int GetY()
        {
            return mGridLocation.Y;
        }

        public Vector2 ScreenDimensions()
        {
            return mSprite.Dimensions;
        }

        public void ChangeClass(int classID)
        {
            ClassID = classID;
            LinkData();
        }
    }

    public class CreatureReader : ContentTypeReader<Creature>
    {
        protected override Creature Read(ContentReader input, Creature existingInstance)
        {
            Creature output = new Creature();
            output.GridLocation = input.ReadObject<Point>();
            output.ClassID = input.ReadInt32();
            output.LinkData();
            return output;
        }
    }
}

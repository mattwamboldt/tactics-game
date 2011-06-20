using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Logic;
using Board_Game.Rendering;

namespace Board_Game.Creatures
{
    struct ClampArea
    {
        public int leftCut;
        public int rightCut;
        public int topCut;
        public int bottomCut;
    };

    //Contains all the code for a Creature.
    class Creature
    {
        public Point mGridLocation;
        public Point GridLocation
        {
            get { return mGridLocation; }
            set { mGridLocation = value; }
        }

        //this allows us to set the colour
        public Side side;

        public CreatureDescription mCreatureDesc;

        public Vector2 Position
        {
            get { return mSprite.Position; }
        }

        public bool isSelected;

        private Sprite mSprite;

        public Creature(CreatureDescription CreatureDesc)
        {
            mCreatureDesc = CreatureDesc;
            mGridLocation.X = 0;
            mGridLocation.Y = 0;
            side = Side.Neutral;
            isSelected = false;

            mSprite = new Sprite(mCreatureDesc.Texture, new Vector2(0, 0), Color.White, new Vector2(GridWidth * Tile.TILE_SIZE, GridHeight * Tile.TILE_SIZE));
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            mSprite.Color = Color.White;

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

        public int GridWidth
        {
            get { return mCreatureDesc.SizeInSpaces.X; }
        }

        public int GridHeight
        {
            get { return mCreatureDesc.SizeInSpaces.Y; }
        }

        public CreatureType Type
        {
            get { return mCreatureDesc.Type; }
        }

        public bool CanFly
        {
            get { return mCreatureDesc.CanFly; }
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
    }
}

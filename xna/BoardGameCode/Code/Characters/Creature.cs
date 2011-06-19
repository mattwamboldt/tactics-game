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
        //These allow the Creature to remember its previous location and
        //return there in the event of an invalid move
        public int originalX;
        public int originalY;

        //this allows us to set the colour
        public Side side;

        public CreatureDescription mCreatureDesc;

        public Vector2 position;

        public bool isSelected;

        private Sprite mSprite;

        public Creature(CreatureDescription CreatureDesc)
        {
            mCreatureDesc = CreatureDesc;
            originalX = 0;
            originalY = 0;
            side = Side.Neutral;
            position = new Vector2(0, 0);
            isSelected = false;

            mSprite = new Sprite(mCreatureDesc.Texture, position, Color.White, new Vector2(GridWidth * Tile.TILE_SIZE, GridHeight * Tile.TILE_SIZE));
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            mSprite.Position = position;
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
            originalX = newX;
            originalY = newY;
            position.X = newX * Tile.TILE_SIZE;
            position.Y = newY * Tile.TILE_SIZE;
        }

        public ClampArea GetClampArea()
        {
            ClampArea returnValue;
            returnValue.leftCut = (int)(position.X - ScreenDimensions().X);
            returnValue.rightCut = (int)(position.X + ScreenDimensions().X);
            returnValue.topCut = (int)(position.Y - ScreenDimensions().Y);
            returnValue.bottomCut = (int)(position.Y + ScreenDimensions().Y);
            originalX = (int)((position.X - position.X % ScreenDimensions().X) / Tile.TILE_SIZE);
            originalY = (int)((position.Y - position.Y % ScreenDimensions().Y) / Tile.TILE_SIZE);
            return returnValue;
        }
        
        public int GetX()
        {
            return (int)(position.X / Tile.TILE_SIZE);
        }

        public int GetY()
        {
            return (int)(position.Y / Tile.TILE_SIZE);
        }

        public Vector2 ScreenDimensions()
        {
            return mSprite.Dimensions;
        }
    }
}

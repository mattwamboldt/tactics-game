using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Code
{
    class Tile
    {
        public const float TILE_SIZE = 25;
        public const float MINE_SIZE = 50;

        public bool occupied;
        public Sides side;
        public Units.Unit occupiedUnit;
        public bool isMine;

        public Texture2D texture;
        private Vector2 position;

        public Tile(Texture2D inTexture, int x, int y)
        {
            occupied = false;
            side = new Sides();
            occupiedUnit = null;
            isMine = false;
            position = new Vector2(x, y);
            texture = inTexture;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            float scale;
            Color color = Color.White;
            Vector2 drawPosition = new Vector2();

            if (isMine)
            {
                scale = MINE_SIZE / texture.Width;
                drawPosition.X = position.X * MINE_SIZE;
                drawPosition.Y = position.Y * MINE_SIZE;
                if (side.mColour == Constants.RED)
                {
                    color = Color.Red;
                }
                else if (side.mColour == Constants.BLUE)
                {
                    color = Color.Blue;
                }
            }
            else
            {
                scale = TILE_SIZE / texture.Width;
                drawPosition.X = position.X * TILE_SIZE;
                drawPosition.Y = position.Y * TILE_SIZE;
            }

            drawPosition.X += parentPosition.X;
            drawPosition.Y += parentPosition.Y;

            spriteBatch.Draw(
                texture,
                drawPosition,
                null,
                color,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}

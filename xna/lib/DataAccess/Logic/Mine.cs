﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Rendering;
using Board_Game.Creatures;

namespace Board_Game.Logic
{
    public class Mine
    {
        public Side side;
        public Vector2 position;

        private Sprite mSprite;

        public Mine(Texture2D inTexture, int x, int y)
        {
            side = Side.Neutral;
            position = new Vector2(x, y);
            mSprite = new Sprite(
                inTexture,
                new Vector2(position.X * Tile.TILE_SIZE * 2, position.Y * Tile.TILE_SIZE * 2),
                Color.White,
                new Vector2(Tile.TILE_SIZE * 2, Tile.TILE_SIZE * 2));
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

            mSprite.Render(spriteBatch, parentPosition);
        }
    }
}

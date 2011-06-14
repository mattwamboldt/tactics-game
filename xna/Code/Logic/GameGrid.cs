using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Code.Logic;

namespace Board_Game.Code
{
    class GameGrid
    {
        public Tile[,] mTiles;
        public Mine[,] mMines;
        public Vector2 position;

        public GameGrid(int width, int height, Texture2D tileTexture, Texture2D mineTexture)
        {
            position = new Vector2(20, 20);
            mTiles = new Tile[width, height];
            for (int i = 0; i < width; ++i)
            {
                for (var j = 0; j < height; ++j)
                {
                    mTiles[i, j] = new Tile(tileTexture, j, i);
                }
            }

            mMines = new Mine[width / 2, height / 2];

            for (int i = 0; i < width / 2; ++i)
            {
                for (int j = 0; j < height / 2; ++j)
                {
                    if (i % 2 == j % 2)
                    {
                        mMines[i, j] = new Mine(mineTexture, j, i);

                        if (i < 2)
                        {
                            mMines[i, j].side.TurnBlue();
                        }
                        else if (i >= (height / 2) - 2)
                        {
                            mMines[i, j].side.TurnRed();
                        }

                        mTiles[i * 2, j * 2].mine = mMines[i, j];
                    }
                    else
                    {
                        mMines[i, j] = null;
                    }
                }
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= mTiles.GetUpperBound(0); ++i)
            {
                for (var j = 0; j <= mTiles.GetUpperBound(1); ++j)
                {
                    mTiles[i, j].Render(spriteBatch, position);
                }
            }

            for (int i = 0; i <= mMines.GetUpperBound(0); ++i)
            {
                for (var j = 0; j <= mMines.GetUpperBound(1); ++j)
                {
                    if (mMines[i, j] != null)
                    {
                        mMines[i, j].Render(spriteBatch, position);
                    }
                }
            }
        }
    }
}

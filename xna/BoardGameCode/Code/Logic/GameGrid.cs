using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Logic
{
    class GameGrid
    {
        public Tile[,] mTiles;
        public List<Mine> mMines;
        public Vector2 position;

        public GameGrid(int width, int height, Texture2D tileTexture, Texture2D mineTexture)
        {
            position = new Vector2(0, 0);
            mTiles = new Tile[width, height];
            for (int i = 0; i < width; ++i)
            {
                for (var j = 0; j < height; ++j)
                {
                    mTiles[i, j] = new Tile(tileTexture, j, i);
                }
            }

            mMines = new List<Mine>((width / 4) * (height / 2));

            for (int i = 0; i < width / 2; ++i)
            {
                for (int j = 0; j < height / 2; ++j)
                {
                    if (i % 2 == j % 2)
                    {
                        Mine newMine = new Mine(mineTexture, j, i);

                        if (i < 2)
                        {
                            newMine.side = Side.Blue;
                        }
                        else if (i >= (height / 2) - 2)
                        {
                            newMine.side = Side.Red;
                        }

                        mTiles[i * 2, j * 2].mine = newMine;
                        mMines.Add(newMine);
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

            foreach (Mine mine in mMines)
            {
                mine.Render(spriteBatch, position);
            }
        }

        public float Width()
        {
            return GameState.GRID_WIDTH * Tile.TILE_SIZE;
        }

        public float Height()
        {
            return GameState.GRID_HEIGHT * Tile.TILE_SIZE;
        }
    }
}

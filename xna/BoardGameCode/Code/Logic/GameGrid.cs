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
            position = new Vector2(100, 0);
            mTiles = new Tile[width, height];
            for (int x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    mTiles[x, y] = new Tile(tileTexture, x, y);
                }
            }

            mMines = new List<Mine>((width / 4) * (height / 2));

            for (int x = 0; x < width / 2; ++x)
            {
                for (int y = 0; y < height / 2; ++y)
                {
                    if (x % 2 == y % 2)
                    {
                        Mine newMine = new Mine(mineTexture, x, y);

                        if (y < 2)
                        {
                            newMine.side = Side.Blue;
                        }
                        else if (y >= (height / 2) - 2)
                        {
                            newMine.side = Side.Red;
                        }

                        mTiles[x * 2, y * 2].mine = newMine;
                        mMines.Add(newMine);
                    }
                }
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (int x = 0; x <= mTiles.GetUpperBound(0); ++x)
            {
                for (var y = 0; y <= mTiles.GetUpperBound(1); ++y)
                {
                    mTiles[x, y].Render(spriteBatch, position);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Creatures;
using Microsoft.Xna.Framework.Content;
using Board_Game.Rendering;

namespace Board_Game.Logic
{
    public class GameGrid
    {
        public string TileTextureName;
        public string MineTextureName;
        public Vector2 Position;
        public int Width;
        public int Height;

        public List<Tile> Tiles;
        public List<Mine> Mines;

        [ContentSerializerIgnore]
        public Creature[,] Occupants;

        public GameGrid() { }
        public void Initialize()
        {
            Texture2D tileTexture = TextureManager.Get().Find(TileTextureName);

            Occupants = new Creature[Width, Height];

            int currentTile = 0;
            for (int y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    Tile tile = Tiles[currentTile];
                    tile.ScreenCoordinates = new Point(x * (int)Tile.TILE_SIZE, y * (int)Tile.TILE_SIZE);
                    tile.Texture = tileTexture;
                    currentTile++;
                    Occupants[x, y] = null;
                }
            }

            Texture2D mineTexture = TextureManager.Get().Find(MineTextureName);

            for (int i = 0; i < Mines.Count; i++)
            {
                Mines[i].Initialize(mineTexture);
            }
        }

        public Mine GetMine(int x, int y)
        {
            foreach (Mine mine in Mines)
            {
                if (mine.position.X == x && mine.position.Y == y)
                {
                    return mine;
                }
            }

            return null;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
            {
                tile.Render(spriteBatch, Position);
            }

            foreach (Mine mine in Mines)
            {
                mine.Render(spriteBatch, Position);
            }
        }

        public float PixelWidth()
        {
            return Width * Tile.TILE_SIZE;
        }

        public float PixelHeight()
        {
            return Height * Tile.TILE_SIZE;
        }
    }

    public class GameGridReader : ContentTypeReader<GameGrid>
    {
        protected override GameGrid Read(ContentReader input, GameGrid existingInstance)
        {
            GameGrid output = new GameGrid();

            output.TileTextureName = input.ReadString();
            output.MineTextureName = input.ReadString();
            output.Position = input.ReadVector2();
            output.Width = input.ReadInt32();
            output.Height = input.ReadInt32();

            output.Tiles = input.ReadObject<List<Tile>>();
            output.Mines = input.ReadObject<List<Mine>>();

            output.Initialize();
            return output;
        }
    }
}

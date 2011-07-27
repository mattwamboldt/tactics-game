using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Rendering;
using Board_Game.Creatures;
using Microsoft.Xna.Framework.Content;

namespace Board_Game.Logic
{
    public class Tile
    {
        public const float TILE_SIZE = 32;

        public Point TextureCoordinates;

        [ContentSerializerIgnore]
        public Point ScreenCoordinates;

        [ContentSerializerIgnore]
        public Texture2D Texture;

        public Tile() {}

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            Vector2 renderPosition = new Vector2(
                parentPosition.X + ScreenCoordinates.X,
                parentPosition.Y + ScreenCoordinates.Y
            );

            spriteBatch.Draw(
                Texture,
                new Rectangle((int)renderPosition.X, (int)renderPosition.Y, (int)TILE_SIZE, (int)TILE_SIZE),
                new Rectangle(TextureCoordinates.X * (int)TILE_SIZE, TextureCoordinates.Y * (int)TILE_SIZE, (int)TILE_SIZE, (int)TILE_SIZE),
                Color.White
            );
        }
    }

    public class TileReader : ContentTypeReader<Tile>
    {
        protected override Tile Read(ContentReader input, Tile existingInstance)
        {
            Tile output = new Tile();
            output.TextureCoordinates = input.ReadObject<Point>();
            return output;
        }
    }
}

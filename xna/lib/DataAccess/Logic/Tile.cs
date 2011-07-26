using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Rendering;
using Board_Game.Creatures;

namespace Board_Game.Logic
{
    public class Tile
    {
        public const float TILE_SIZE = 45;

        private Sprite mSprite;

        public Point TextureCoordinates;
        public Point ScreenCoordinates;
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
                new Rectangle(TextureCoordinates.X, TextureCoordinates.Y, (int)TILE_SIZE, (int)TILE_SIZE),
                Color.White
            );
        }
    }
}

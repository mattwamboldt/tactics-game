using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Board_Game.Code.Logic;

namespace Board_Game.Code
{
    class Tile
    {
        public const float TILE_SIZE = 25;

        public bool occupied;
        public Constants.Side side;
        public Units.Unit occupiedUnit;
        public Mine mine;

        public Texture2D texture;
        private Vector2 position;

        public Tile(Texture2D inTexture, int x, int y)
        {
            occupied = false;
            side = Constants.Side.Neutral;
            occupiedUnit = null;
            mine = null;
            position = new Vector2(x, y);
            texture = inTexture;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            Vector2 drawPosition = new Vector2(position.X * TILE_SIZE, position.Y * TILE_SIZE);

            drawPosition.X += parentPosition.X;
            drawPosition.Y += parentPosition.Y;

            spriteBatch.Draw(
                texture,
                drawPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                TILE_SIZE / texture.Width,
                SpriteEffects.None,
                0f
            );
        }
    }
}

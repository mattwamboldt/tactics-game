using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Logic
{
    class Mine
    {
        public const float MINE_SIZE = 50;

        public Side side;

        public Texture2D texture;
        public Vector2 position;

        public Mine(Texture2D inTexture, int x, int y)
        {
            side = Side.Neutral;
            position = new Vector2(x, y);
            texture = inTexture;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            float scale;
            Color color = Color.White;
            Vector2 drawPosition = new Vector2(position.X * MINE_SIZE, position.Y * MINE_SIZE);

            scale = MINE_SIZE / texture.Width;

            if (side == Side.Red)
            {
                color = Color.Red;
            }
            else if (side == Side.Blue)
            {
                color = Color.Blue;
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

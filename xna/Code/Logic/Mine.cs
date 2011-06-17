using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Rendering;

namespace Board_Game.Logic
{
    class Mine
    {
        public const float MINE_SIZE = 50;

        public Side side;

        private Sprite mSprite;
        public Vector2 position;

        public Mine(Texture2D inTexture, int x, int y)
        {
            side = Side.Neutral;
            position = new Vector2(x, y);
            mSprite = new Sprite(
                inTexture,
                new Vector2(position.X * MINE_SIZE, position.Y * MINE_SIZE),
                Color.White,
                new Vector2(MINE_SIZE, MINE_SIZE));
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

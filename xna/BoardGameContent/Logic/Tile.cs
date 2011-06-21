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

        public Side side;
        public Creatures.Creature occupiedCreature;
        public Mine mine;

        public bool Occupied { get { return occupiedCreature != null; } }

        private Sprite mSprite;

        public Tile(Texture2D inTexture, int x, int y)
        {
            side = Side.Neutral;
            occupiedCreature = null;
            mine = null;
            mSprite = new Sprite(
                inTexture,
                new Vector2(x * TILE_SIZE, y * TILE_SIZE),
                Color.White,
                new Vector2(TILE_SIZE,TILE_SIZE));
        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            mSprite.Render(spriteBatch, parentPosition);
        }
    }
}

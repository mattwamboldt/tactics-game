﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Board_Game.Code.Units
{
    class GroundUnit : Unit
    {
        public GroundUnit(GameGrid grid, AI AIRef, Texture2D texture)
            : base(grid, AIRef, texture)
        {
            CanFly = false;
            height = 1;
            width = 1;
        }

        public override void Move(int newLocationI, int newLocationJ, bool changeTurns)
        {
            if (changeTurns == true)
            {
                mAIRef.ChangeTurns();
            }

            grid.mTiles[originalI, originalJ].occupied = false;
            grid.mTiles[originalI, originalJ].side.TurnNeutral();
            grid.mTiles[originalI, originalJ].occupiedUnit = null;
            SetLocation(newLocationI, newLocationJ);
        }
        
        public override bool CheckOccupied(int i, int j)
        {
            return grid.mTiles[i, j].occupied;
        }

        public override void SetLocation(int newLocationI, int newLocationJ)
        {
            grid.mTiles[newLocationI, newLocationJ].occupied = true;
            grid.mTiles[newLocationI, newLocationJ].side.mColour = side.mColour;
            grid.mTiles[newLocationI, newLocationJ].occupiedUnit = this;

            position.X = newLocationJ * Tile.TILE_SIZE;
            position.Y = newLocationI * Tile.TILE_SIZE;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            float scale = Tile.TILE_SIZE / texture.Width;

            Color color = Color.White;

            if (side.mColour == Constants.RED)
            {
                color = Color.Red;
            }
            else if (side.mColour == Constants.BLUE)
            {
                color = Color.Blue;
            }

            spriteBatch.Draw(
                texture,
                position,
                null,
                color,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override Vector2 ScreenDimensions()
        {
            return new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE);
        }
    }
}

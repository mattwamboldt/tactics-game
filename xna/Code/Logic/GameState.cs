using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Logic
{
    /// <summary>
    /// Will eventually house an entire games logical components, the grid and players.
    /// </summary>
    class GameState
    {
        Player mRed;
        Player mBlue;

        public Player Red { get { return mRed; } }
        public Player Blue { get { return mBlue; } }

        public GameState(AI AIref)
        {
            //passing in the same AI for now, but could be different later
            mRed = new Player(true, Constants.Side.Red, AIref);
            mBlue = new Player(false, Constants.Side.Blue, AIref);
        }

        public void Initialize(
            Texture2D bomberTexture,
            Texture2D fighterTexture,
            Texture2D soldierTexture,
            Texture2D deminerTexture,
            Texture2D grenadierTexture
            )
        {
            mRed.CreateUnits(
                bomberTexture,
                fighterTexture,
                soldierTexture,
                deminerTexture,
                grenadierTexture
            );

            mBlue.CreateUnits(
                bomberTexture,
                fighterTexture,
                soldierTexture,
                deminerTexture,
                grenadierTexture
            );
        }
    }
}

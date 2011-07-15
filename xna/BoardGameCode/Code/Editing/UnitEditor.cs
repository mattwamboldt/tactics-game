#if EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEditor;
using Board_Game.Logic;
using Board_Game.Creatures;

namespace Board_Game.Code.Editing
{
    class UnitEditor
    {
        private Editor mEditorForm;
        private GameState mGameState;

        public UnitEditor(Editor editorForm, GameState gameState)
        {
            mEditorForm = editorForm;
            mGameState = gameState;
        }

        public void ChangeClass(int classID, Creature creature)
        {
            mGameState.ClearArea(creature.GetX(), creature.GetY(), creature.GridWidth, creature.GridHeight);
            creature.ChangeClass(classID);
            mGameState.SetLocation(creature.GetX(), creature.GetY(), creature);
        }

        public void ChangeSide(Side newSide, Creature creature)
        {
            if (newSide != creature.side)
            {
                if (creature.side == Side.Red)
                {
                    mGameState.Red.Creatures.Remove(creature);
                    mGameState.Blue.Creatures.Add(creature);
                }
                else if (creature.side == Side.Blue)
                {
                    mGameState.Blue.Creatures.Remove(creature);
                    mGameState.Red.Creatures.Add(creature);
                }

                creature.side = newSide;

                for (var u = 0; u < creature.GridWidth; ++u)
                {
                    for (var v = 0; v < creature.GridHeight; ++v)
                    {
                        mGameState.mGrid.mTiles[creature.GetX() + u, creature.GetY() + v].side = newSide;
                    }
                }
            }
        }

        public void SetCallbacks()
        {
            mEditorForm.mClassChange = ChangeClass;
            mEditorForm.mSideChange = ChangeSide;
        }
    }
}
#endif
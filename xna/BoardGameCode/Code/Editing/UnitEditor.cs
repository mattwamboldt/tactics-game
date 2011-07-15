﻿#if EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEditor;
using Board_Game.Logic;
using Board_Game.Creatures;
using Microsoft.Xna.Framework;

namespace Board_Game.Code.Editing
{
    class UnitEditor
    {
        private Editor mEditorForm;
        private GameState mGameState;

        Creature mSelectedCreature = null;
        Side mSelectedSide = Side.Neutral;

        public UnitEditor(Editor editorForm, GameState gameState)
        {
            mEditorForm = editorForm;
            mGameState = gameState;
        }

        public void ChangeClass(int classID)
        {
            if (mSelectedCreature != null)
            {
                mGameState.ClearArea(
                    mSelectedCreature.GetX(),
                    mSelectedCreature.GetY(),
                    mSelectedCreature.GridWidth,
                    mSelectedCreature.GridHeight
                );

                mSelectedCreature.ChangeClass(classID);

                mGameState.SetLocation(
                    mSelectedCreature.GetX(),
                    mSelectedCreature.GetY(),
                    mSelectedCreature
                );
            }
        }

        public void ChangeSide(Side newSide)
        {
            mSelectedSide = newSide;
            if (mSelectedCreature != null && newSide != mSelectedCreature.side)
            {
                if (mSelectedCreature.side == Side.Red)
                {
                    mGameState.Red.Creatures.Remove(mSelectedCreature);
                    mGameState.Blue.Creatures.Add(mSelectedCreature);
                }
                else if (mSelectedCreature.side == Side.Blue)
                {
                    mGameState.Blue.Creatures.Remove(mSelectedCreature);
                    mGameState.Red.Creatures.Add(mSelectedCreature);
                }

                mSelectedCreature.side = newSide;

                for (var u = 0; u < mSelectedCreature.GridWidth; ++u)
                {
                    for (var v = 0; v < mSelectedCreature.GridHeight; ++v)
                    {
                        mGameState.mGrid.mTiles[mSelectedCreature.GetX() + u, mSelectedCreature.GetY() + v].side = newSide;
                    }
                }
            }
        }

        public void SelectCreature(Creature selectedCreature)
        {
            if (mSelectedCreature != selectedCreature)
            {
                mSelectedCreature = selectedCreature;
                mEditorForm.UpdateCreature(mSelectedCreature);
            }
            else
            {
                mSelectedCreature = null;
            }
        }

        public void SelectSquare(int x, int y)
        {
            if (mSelectedCreature != null)
            {
                //move the creature
                mGameState.Move(x, y, mSelectedCreature);
            }
            else if(mEditorForm.mSelectedDesc != null
                && mSelectedSide != Side.Neutral)
            {
                //Place a new creature based on the current settings
                Creature copy = new Creature();
                copy.GridLocation = new Point(x, y);
                copy.ClassID = mEditorForm.mSelectedDesc.ID;
                copy.LinkData();
                copy.side = mSelectedSide;

                if (mSelectedSide == Side.Red)
                {
                    mGameState.Blue.Creatures.Add(copy);
                }
                else if (mSelectedSide == Side.Blue)
                {
                    mGameState.Red.Creatures.Add(copy);
                }

                mGameState.SetLocation(x, y, copy);
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
#if EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEditor;
using Board_Game.Logic;
using Board_Game.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Board_Game.Rendering;
using Microsoft.Xna.Framework.Content;
using Board_Game.Characters;
using Board_Game.Code.Util;
using Board_Game.DB;

namespace Board_Game.Code.Editing
{
    class UnitEditor
    {
        private Editor mEditorForm;
        private GameState mGameState;
        private ContentManager mContentManager;

        Creature mSelectedCreature = null;
        Side mSelectedSide = Side.Neutral;
        StorageManager mStorage;

        public UnitEditor(Editor editorForm, GameState gameState,
            ContentManager content, StorageManager storage)
        {
            mEditorForm = editorForm;
            mGameState = gameState;
            mContentManager = content;
            mStorage = storage;
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
            }
        }

        public void ChangeArmy(string armyName)
        {
            Army replacement = mContentManager.Load<Army>("Armies/" + armyName);
            if (replacement.Side == Side.Red)
            {
                mGameState.Red.WipeField();
                mGameState.Red.mArmy = replacement;
                mGameState.Red.PlaceOnField();
            }
            else
            {
                mGameState.Blue.WipeField();
                mGameState.Blue.mArmy = replacement;
                mGameState.Blue.PlaceOnField();
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

        public void DeleteCreature()
        {
            if (mSelectedCreature != null)
            {
                mGameState.ClearArea(
                    mSelectedCreature.GetX(),
                    mSelectedCreature.GetY(),
                    mSelectedCreature.GridWidth,
                    mSelectedCreature.GridHeight
                );

                mGameState.RemoveCreature(mSelectedCreature);
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
                    mGameState.Red.Creatures.Add(copy);
                }
                else if (mSelectedSide == Side.Blue)
                {
                    mGameState.Blue.Creatures.Add(copy);
                }

                mGameState.SetLocation(x, y, copy);
            }
        }

        public void SetCallbacks()
        {
            mEditorForm.mClassChange = ChangeClass;
            mEditorForm.mSideChange = ChangeSide;
            mEditorForm.mArmyChange = ChangeArmy;
            mEditorForm.mUnitSave = Save;
        }

        public void Save(string armyName)
        {
            if (armyName != null)
            {
                Army armyToSave = mGameState.Blue.mArmy;
                if (armyName.Contains("Red"))
                {
                    armyToSave = mGameState.Red.mArmy;
                }

                string armyData = "classId,x,y";

                foreach (Creature unit in armyToSave.Members)
                {
                    armyData += Environment.NewLine;
                    armyData += unit.ClassID + "," + unit.GetX() + "," + unit.GetY();
                }

                mStorage.Save("../../../data/Armies/" + armyName + ".csv", armyData, false);
            }

            List<CreatureDescription> descriptions = DatabaseManager.Get().CreatureTable;
            string descriptionData = "ID,Name,Description,CanFly,Width,Height,Texture,Priority0,Priority1,Priority2,Priority3,Priority4";

            foreach (CreatureDescription desc in descriptions)
            {
                descriptionData += Environment.NewLine;

                descriptionData +=
                    desc.ID + "," +
                    desc.Name + "," +
                    desc.Description.Replace(Environment.NewLine, "\\n") + "," +
                    desc.CanFly + "," +
                    desc.SizeInSpaces.X + "," +
                    desc.SizeInSpaces.Y + "," +
                    desc.TextureName;

                for (int i = 0; i < 5; i++)
                {
                    descriptionData += ",";

                    if (i < desc.AttackPriorities.Length)
                    {
                        descriptionData += (int)desc.AttackPriorities[i];
                    }
                    else
                    {
                        descriptionData += -1;
                    }
                }
            }

            mStorage.Save("../../../data/DB/CreatureDescription.csv", descriptionData, false);

        }

        public void Render(SpriteBatch spriteBatch, Vector2 parentPosition)
        {
            if (mSelectedCreature != null)
            {
                spriteBatch.Draw(
                    TextureManager.Get().Find("RAW"),
                    new Rectangle(
                        (int)(mSelectedCreature.Position.X + parentPosition.X),
                        (int)(mSelectedCreature.Position.Y + parentPosition.Y),
                        (int)mSelectedCreature.ScreenDimensions().X,
                        (int)mSelectedCreature.ScreenDimensions().Y),
                    new Color(Color.Chartreuse, 120)
               );
            }
        }
    }
}
#endif
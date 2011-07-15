﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Board_Game.Creatures;
using Board_Game.DB;
using Board_Game.Rendering;
using Microsoft.Xna.Framework;
using Board_Game.UI;
using BoardGameContent.UI;

namespace GameEditor
{
    public partial class Editor : Form
    {
        CreatureDescription mSelectedDesc = null;
        Creature mSelectedCreature = null;

        public delegate void ClassChanged(int classID, Creature creature);
        public delegate void SideChanged(Side newSide, Creature creature);

        public ClassChanged mClassChange;
        public SideChanged mSideChange;

        public Editor()
        {
            InitializeComponent();
        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Cursor = Cursors.Arrow;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.ShowDialog();
        }

        public void DisplayStartData()
        {
            classList.Items.Clear();
            classList.DisplayMember = "Name";

            List<CreatureDescription> descriptions = DatabaseManager.Get().CreatureTable;

            foreach (CreatureDescription description in descriptions)
            {
                classList.Items.Add(description);
                
            }

            textureList.Items.Clear();
            classList.DisplayMember = "Name";

            textureList.Items.Clear();

            foreach (string name in TextureManager.Get().GetTextureList())
            {
                textureList.Items.Add(name);
            }
        }

        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectedDesc = (CreatureDescription)classList.SelectedItem;

            if (mSelectedCreature != null)
            {
                mClassChange(mSelectedDesc.ID, mSelectedCreature);
            }

            nameText.Text = mSelectedDesc.Name;
            idText.Text = mSelectedDesc.ID.ToString();
            descriptionText.Text = mSelectedDesc.Description;
            textureList.SelectedItem = mSelectedDesc.TextureName;
        }

        private void nameText_TextChanged(object sender, EventArgs e)
        {
            if (mSelectedDesc != null)
            {
                mSelectedDesc.Name = nameText.Text;
            }
        }

        private void descriptionText_TextChanged(object sender, EventArgs e)
        {
            if (mSelectedDesc != null)
            {
                mSelectedDesc.Description = descriptionText.Text;
            }
        }

        private void textureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedDesc != null)
            {
                mSelectedDesc.TextureName = (string)textureList.SelectedItem;
                mSelectedDesc.LoadTexture();
            }
        }

        public void PopulateTree(Shape root)
        {
            TreeNode node = screenTree.Nodes.Add(root.Name);
            node.Tag = root;
            getChildren(node, root);
        }

        private void getChildren(TreeNode parentNode, Shape parent)
        {
            foreach (Shape child in parent.Children)
            {
                TreeNode newNode = parentNode.Nodes.Add(child.Name);
                newNode.Tag = child;
                getChildren(newNode, child);
            }
        }

        private void PopulateFrameList(Shape shape)
        {
            frameList.Items.Clear();
            frameList.DisplayMember = "Frame";

            foreach (ShapeState state in shape.Animation.KeyFrames)
            {
                frameList.Items.Add(state);
            }
        }

        private void screenTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PopulateFrameList((Shape)e.Node.Tag);
        }

        public void CreatureSelected(Creature selectedCreature)
        {
            if (mSelectedCreature != selectedCreature)
            {
                //select the class index
                mSelectedCreature = selectedCreature;
                classList.SelectedIndex = (int)selectedCreature.Type;

                //update editing bits
                sideBox.SelectedIndex = (int)selectedCreature.side;
            }
            else
            {
                mSelectedCreature = null;
            }
        }

        private void sideBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedCreature != null)
            {
                mSideChange((Side)sideBox.SelectedIndex, mSelectedCreature);
            }
        }

        public void SquareSelected(int x, int y)
        {
            if (mSelectedCreature != null)
            {
                //move the creature
            }
            else
            {
                //Place a new creature based on the current description
            }
        }
    }
}

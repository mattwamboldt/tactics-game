using System;
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
        public CreatureDescription mSelectedDesc = null;

        public delegate void ClassChanged(int classID);
        public delegate void SideChanged(Side newSide);
        public delegate void ArmyChanged(string armyName);
        public delegate void UnitSave(string armyName);

        public ClassChanged mClassChange;
        public SideChanged mSideChange;
        public ArmyChanged mArmyChange;
        public ArmyChanged mUnitSave;

        public Editor()
        {
            InitializeComponent();
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

            armyList.Items.Clear();

            foreach (string armyName in DatabaseManager.Get().ArmyTable)
            {
                armyList.Items.Add(armyName);
            }

            textureList.Items.Clear();
            classList.DisplayMember = "Name";

            textureList.Items.Clear();

            foreach (string name in TextureManager.Get().GetTextureList())
            {
                textureList.Items.Add(name);
            }
        }

#region Unit Editing
        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectedDesc = (CreatureDescription)classList.SelectedItem;
            mClassChange(mSelectedDesc.ID);

            nameText.Text = mSelectedDesc.Name;
            idText.Text = mSelectedDesc.ID.ToString();
            descriptionText.Text = mSelectedDesc.Description;
            textureList.SelectedItem = mSelectedDesc.TextureName;
            canFly.Checked = mSelectedDesc.CanFly;
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

        public void UpdateCreature(Creature selectedCreature)
        {
            if (selectedCreature != null)
            {
                classList.SelectedIndex = (int)selectedCreature.Type;
                sideBox.SelectedIndex = (int)selectedCreature.side;
            }
        }

        private void sideBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSideChange((Side)sideBox.SelectedIndex);
        }

        private void armyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            mArmyChange((string)armyList.SelectedItem); 
        }

        private void canFly_CheckedChanged(object sender, EventArgs e)
        {
            if (mSelectedDesc != null)
            {
                mSelectedDesc.CanFly = canFly.Checked;
            }
        }
#endregion

#region Screen Editing
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
#endregion

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mUnitSave((string)armyList.SelectedItem);
        }
    }
}

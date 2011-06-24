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

namespace GameEditor
{
    public partial class Editor : Form
    {
        CreatureDescription mSelectedDesc = null;

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

            nameText.Text = mSelectedDesc.Name;
            idText.Text = mSelectedDesc.ID.ToString();
            descriptionText.Text = mSelectedDesc.Description;
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
        }
    }
}

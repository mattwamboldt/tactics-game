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

namespace GameEditor
{
    public partial class Editor : Form
    {
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
    }
}

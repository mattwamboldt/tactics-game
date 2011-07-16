namespace GameEditor
{
    partial class Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.unitPage = new System.Windows.Forms.TabPage();
            this.armyList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.characterBox = new System.Windows.Forms.GroupBox();
            this.sideBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.classBox = new System.Windows.Forms.GroupBox();
            this.canFly = new System.Windows.Forms.CheckBox();
            this.classList = new System.Windows.Forms.ListBox();
            this.descriptionText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.idText = new System.Windows.Forms.TextBox();
            this.textureList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.screenPage = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.frameList = new System.Windows.Forms.ListBox();
            this.screenTree = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.unitPage.SuspendLayout();
            this.characterBox.SuspendLayout();
            this.classBox.SuspendLayout();
            this.screenPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size(815, 537);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(332, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.unitPage);
            this.tabControl1.Controls.Add(this.screenPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(332, 513);
            this.tabControl1.TabIndex = 3;
            // 
            // unitPage
            // 
            this.unitPage.Controls.Add(this.armyList);
            this.unitPage.Controls.Add(this.label7);
            this.unitPage.Controls.Add(this.characterBox);
            this.unitPage.Controls.Add(this.classBox);
            this.unitPage.Location = new System.Drawing.Point(4, 22);
            this.unitPage.Name = "unitPage";
            this.unitPage.Padding = new System.Windows.Forms.Padding(3);
            this.unitPage.Size = new System.Drawing.Size(324, 487);
            this.unitPage.TabIndex = 0;
            this.unitPage.Text = "Units";
            this.unitPage.UseVisualStyleBackColor = true;
            // 
            // armyList
            // 
            this.armyList.FormattingEnabled = true;
            this.armyList.Location = new System.Drawing.Point(87, 14);
            this.armyList.Name = "armyList";
            this.armyList.Size = new System.Drawing.Size(226, 21);
            this.armyList.TabIndex = 13;
            this.armyList.SelectedIndexChanged += new System.EventHandler(this.armyList_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Current Army";
            // 
            // characterBox
            // 
            this.characterBox.Controls.Add(this.sideBox);
            this.characterBox.Controls.Add(this.label10);
            this.characterBox.Location = new System.Drawing.Point(8, 288);
            this.characterBox.Name = "characterBox";
            this.characterBox.Size = new System.Drawing.Size(305, 188);
            this.characterBox.TabIndex = 11;
            this.characterBox.TabStop = false;
            this.characterBox.Text = "Character";
            // 
            // sideBox
            // 
            this.sideBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sideBox.FormattingEnabled = true;
            this.sideBox.Items.AddRange(new object[] {
            "Red",
            "Blue",
            "Nuetral"});
            this.sideBox.Location = new System.Drawing.Point(9, 32);
            this.sideBox.Name = "sideBox";
            this.sideBox.Size = new System.Drawing.Size(121, 21);
            this.sideBox.TabIndex = 6;
            this.sideBox.SelectedIndexChanged += new System.EventHandler(this.sideBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Side";
            // 
            // classBox
            // 
            this.classBox.Controls.Add(this.canFly);
            this.classBox.Controls.Add(this.classList);
            this.classBox.Controls.Add(this.descriptionText);
            this.classBox.Controls.Add(this.label2);
            this.classBox.Controls.Add(this.label3);
            this.classBox.Controls.Add(this.idText);
            this.classBox.Controls.Add(this.textureList);
            this.classBox.Controls.Add(this.label4);
            this.classBox.Controls.Add(this.nameText);
            this.classBox.Controls.Add(this.label1);
            this.classBox.Location = new System.Drawing.Point(8, 44);
            this.classBox.Name = "classBox";
            this.classBox.Size = new System.Drawing.Size(305, 238);
            this.classBox.TabIndex = 10;
            this.classBox.TabStop = false;
            this.classBox.Text = "Class";
            // 
            // canFly
            // 
            this.canFly.AutoSize = true;
            this.canFly.Location = new System.Drawing.Point(236, 185);
            this.canFly.Name = "canFly";
            this.canFly.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.canFly.Size = new System.Drawing.Size(61, 17);
            this.canFly.TabIndex = 11;
            this.canFly.Text = "Can Fly";
            this.canFly.UseVisualStyleBackColor = true;
            // 
            // classList
            // 
            this.classList.FormattingEnabled = true;
            this.classList.Items.AddRange(new object[] {
            "Bomber",
            "Fighter",
            "Miner",
            "Grenadier",
            "Soldier"});
            this.classList.Location = new System.Drawing.Point(6, 19);
            this.classList.Name = "classList";
            this.classList.Size = new System.Drawing.Size(120, 160);
            this.classList.TabIndex = 0;
            this.classList.SelectedIndexChanged += new System.EventHandler(this.classList_SelectedIndexChanged);
            // 
            // descriptionText
            // 
            this.descriptionText.Location = new System.Drawing.Point(135, 74);
            this.descriptionText.Multiline = true;
            this.descriptionText.Name = "descriptionText";
            this.descriptionText.Size = new System.Drawing.Size(162, 105);
            this.descriptionText.TabIndex = 3;
            this.descriptionText.TextChanged += new System.EventHandler(this.descriptionText_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "ID";
            // 
            // idText
            // 
            this.idText.Location = new System.Drawing.Point(241, 35);
            this.idText.Name = "idText";
            this.idText.ReadOnly = true;
            this.idText.Size = new System.Drawing.Size(56, 20);
            this.idText.TabIndex = 4;
            // 
            // textureList
            // 
            this.textureList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textureList.FormattingEnabled = true;
            this.textureList.Location = new System.Drawing.Point(6, 208);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(291, 21);
            this.textureList.TabIndex = 8;
            this.textureList.SelectedIndexChanged += new System.EventHandler(this.textureList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Texture";
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(135, 35);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(100, 20);
            this.nameText.TabIndex = 2;
            this.nameText.TextChanged += new System.EventHandler(this.nameText_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name";
            // 
            // screenPage
            // 
            this.screenPage.Controls.Add(this.label6);
            this.screenPage.Controls.Add(this.label5);
            this.screenPage.Controls.Add(this.frameList);
            this.screenPage.Controls.Add(this.screenTree);
            this.screenPage.Location = new System.Drawing.Point(4, 22);
            this.screenPage.Name = "screenPage";
            this.screenPage.Padding = new System.Windows.Forms.Padding(3);
            this.screenPage.Size = new System.Drawing.Size(324, 487);
            this.screenPage.TabIndex = 1;
            this.screenPage.Text = "Screen";
            this.screenPage.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(171, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Frames";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Hierarchy";
            // 
            // frameList
            // 
            this.frameList.FormattingEnabled = true;
            this.frameList.Location = new System.Drawing.Point(174, 34);
            this.frameList.Name = "frameList";
            this.frameList.Size = new System.Drawing.Size(100, 446);
            this.frameList.TabIndex = 1;
            // 
            // screenTree
            // 
            this.screenTree.Indent = 5;
            this.screenTree.Location = new System.Drawing.Point(8, 34);
            this.screenTree.Name = "screenTree";
            this.screenTree.PathSeparator = ".";
            this.screenTree.Size = new System.Drawing.Size(159, 445);
            this.screenTree.TabIndex = 0;
            this.screenTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.screenTree_AfterSelect);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 537);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Game Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.unitPage.ResumeLayout(false);
            this.unitPage.PerformLayout();
            this.characterBox.ResumeLayout(false);
            this.characterBox.PerformLayout();
            this.classBox.ResumeLayout(false);
            this.classBox.PerformLayout();
            this.screenPage.ResumeLayout(false);
            this.screenPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage unitPage;
        private System.Windows.Forms.TabPage screenPage;
        private System.Windows.Forms.ListBox classList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idText;
        private System.Windows.Forms.TextBox descriptionText;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox textureList;
        private System.Windows.Forms.TreeView screenTree;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox frameList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox classBox;
        private System.Windows.Forms.GroupBox characterBox;
        private System.Windows.Forms.ComboBox sideBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox canFly;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox armyList;
    }
}


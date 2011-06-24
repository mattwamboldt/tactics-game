﻿namespace GameEditor
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
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.unitPage = new System.Windows.Forms.TabPage();
            this.classListLabel = new System.Windows.Forms.Label();
            this.classList = new System.Windows.Forms.ListBox();
            this.databasePage = new System.Windows.Forms.TabPage();
            this.nameText = new System.Windows.Forms.TextBox();
            this.descriptionText = new System.Windows.Forms.TextBox();
            this.idText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textureList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.unitPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // openFile
            // 
            this.openFile.Title = "Open File";
            this.openFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFile_FileOk);
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
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(840, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.unitPage);
            this.tabControl1.Controls.Add(this.databasePage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(840, 513);
            this.tabControl1.TabIndex = 3;
            // 
            // unitPage
            // 
            this.unitPage.Controls.Add(this.texturePreview);
            this.unitPage.Controls.Add(this.label4);
            this.unitPage.Controls.Add(this.textureList);
            this.unitPage.Controls.Add(this.label3);
            this.unitPage.Controls.Add(this.label2);
            this.unitPage.Controls.Add(this.label1);
            this.unitPage.Controls.Add(this.idText);
            this.unitPage.Controls.Add(this.descriptionText);
            this.unitPage.Controls.Add(this.nameText);
            this.unitPage.Controls.Add(this.classListLabel);
            this.unitPage.Controls.Add(this.classList);
            this.unitPage.Location = new System.Drawing.Point(4, 22);
            this.unitPage.Name = "unitPage";
            this.unitPage.Padding = new System.Windows.Forms.Padding(3);
            this.unitPage.Size = new System.Drawing.Size(832, 487);
            this.unitPage.TabIndex = 0;
            this.unitPage.Text = "Units";
            this.unitPage.UseVisualStyleBackColor = true;
            // 
            // classListLabel
            // 
            this.classListLabel.AutoSize = true;
            this.classListLabel.Location = new System.Drawing.Point(8, 12);
            this.classListLabel.Name = "classListLabel";
            this.classListLabel.Size = new System.Drawing.Size(51, 13);
            this.classListLabel.TabIndex = 1;
            this.classListLabel.Text = "Class List";
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
            this.classList.Location = new System.Drawing.Point(6, 28);
            this.classList.Name = "classList";
            this.classList.Size = new System.Drawing.Size(120, 160);
            this.classList.TabIndex = 0;
            this.classList.SelectedIndexChanged += new System.EventHandler(this.classList_SelectedIndexChanged);
            // 
            // databasePage
            // 
            this.databasePage.Location = new System.Drawing.Point(4, 22);
            this.databasePage.Name = "databasePage";
            this.databasePage.Padding = new System.Windows.Forms.Padding(3);
            this.databasePage.Size = new System.Drawing.Size(832, 487);
            this.databasePage.TabIndex = 1;
            this.databasePage.Text = "tabPage2";
            this.databasePage.UseVisualStyleBackColor = true;
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(132, 46);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(100, 20);
            this.nameText.TabIndex = 2;
            this.nameText.TextChanged += new System.EventHandler(this.nameText_TextChanged);
            // 
            // descriptionText
            // 
            this.descriptionText.Location = new System.Drawing.Point(132, 85);
            this.descriptionText.Multiline = true;
            this.descriptionText.Name = "descriptionText";
            this.descriptionText.Size = new System.Drawing.Size(162, 103);
            this.descriptionText.TabIndex = 3;
            this.descriptionText.TextChanged += new System.EventHandler(this.descriptionText_TextChanged);
            // 
            // idText
            // 
            this.idText.Location = new System.Drawing.Point(238, 46);
            this.idText.Name = "idText";
            this.idText.ReadOnly = true;
            this.idText.Size = new System.Drawing.Size(56, 20);
            this.idText.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "ID";
            // 
            // textureList
            // 
            this.textureList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textureList.FormattingEnabled = true;
            this.textureList.Location = new System.Drawing.Point(5, 207);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(289, 21);
            this.textureList.TabIndex = 8;
            this.textureList.SelectedIndexChanged += new System.EventHandler(this.textureList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Texture";
            // 
            // texturePreview
            // 
            this.texturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.texturePreview.Location = new System.Drawing.Point(6, 235);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(288, 244);
            this.texturePreview.TabIndex = 10;
            this.texturePreview.TabStop = false;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 537);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Editor";
            this.Text = "Game Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.unitPage.ResumeLayout(false);
            this.unitPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage unitPage;
        private System.Windows.Forms.TabPage databasePage;
        private System.Windows.Forms.Label classListLabel;
        private System.Windows.Forms.ListBox classList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idText;
        private System.Windows.Forms.TextBox descriptionText;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox textureList;
        private System.Windows.Forms.PictureBox texturePreview;
    }
}

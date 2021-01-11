namespace TileImageViewer
{
    partial class FrmFolderSelect
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFolderSelect));
            this.horizontalSplitContainer = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CloseStartWindow = new System.Windows.Forms.Button();
            this.ToolBarPictureBox = new System.Windows.Forms.PictureBox();
            this.verticalSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.favoritesButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.directoryTree = new System.Windows.Forms.TreeView();
            this.directoryIcons = new System.Windows.Forms.ImageList(this.components);
            this.DirectoryTreeBarPictureBox = new System.Windows.Forms.PictureBox();
            this.filelistFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.filesIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.searchText = new TileImageViewer.Controls.TextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSplitContainer)).BeginInit();
            this.horizontalSplitContainer.Panel1.SuspendLayout();
            this.horizontalSplitContainer.Panel2.SuspendLayout();
            this.horizontalSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToolBarPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalSplitContainer)).BeginInit();
            this.verticalSplitContainer.Panel1.SuspendLayout();
            this.verticalSplitContainer.Panel2.SuspendLayout();
            this.verticalSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DirectoryTreeBarPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // horizontalSplitContainer
            // 
            this.horizontalSplitContainer.BackColor = System.Drawing.Color.Black;
            this.horizontalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.horizontalSplitContainer.IsSplitterFixed = true;
            this.horizontalSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.horizontalSplitContainer.Name = "horizontalSplitContainer";
            this.horizontalSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // horizontalSplitContainer.Panel1
            // 
            this.horizontalSplitContainer.Panel1.Controls.Add(this.pictureBox1);
            this.horizontalSplitContainer.Panel1.Controls.Add(this.CloseStartWindow);
            this.horizontalSplitContainer.Panel1.Controls.Add(this.ToolBarPictureBox);
            // 
            // horizontalSplitContainer.Panel2
            // 
            this.horizontalSplitContainer.Panel2.Controls.Add(this.verticalSplitContainer);
            this.horizontalSplitContainer.Size = new System.Drawing.Size(951, 574);
            this.horizontalSplitContainer.SplitterDistance = 36;
            this.horizontalSplitContainer.TabIndex = 12;
            this.horizontalSplitContainer.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 28);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // CloseStartWindow
            // 
            this.CloseStartWindow.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CloseStartWindow.BackColor = System.Drawing.SystemColors.ControlText;
            this.CloseStartWindow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CloseStartWindow.BackgroundImage")));
            this.CloseStartWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseStartWindow.FlatAppearance.BorderSize = 0;
            this.CloseStartWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseStartWindow.Location = new System.Drawing.Point(451, 5);
            this.CloseStartWindow.Name = "CloseStartWindow";
            this.CloseStartWindow.Size = new System.Drawing.Size(42, 27);
            this.CloseStartWindow.TabIndex = 15;
            this.toolTip1.SetToolTip(this.CloseStartWindow, "关闭启动窗口");
            this.CloseStartWindow.UseVisualStyleBackColor = false;
            this.CloseStartWindow.Visible = false;
            this.CloseStartWindow.Click += new System.EventHandler(this.CloseStartWindow_Click);
            // 
            // ToolBarPictureBox
            // 
            this.ToolBarPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolBarPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.ToolBarPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ToolBarPictureBox.BackgroundImage")));
            this.ToolBarPictureBox.Location = new System.Drawing.Point(-4, 2);
            this.ToolBarPictureBox.Name = "ToolBarPictureBox";
            this.ToolBarPictureBox.Size = new System.Drawing.Size(958, 38);
            this.ToolBarPictureBox.TabIndex = 14;
            this.ToolBarPictureBox.TabStop = false;
            // 
            // verticalSplitContainer
            // 
            this.verticalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.verticalSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.verticalSplitContainer.Name = "verticalSplitContainer";
            // 
            // verticalSplitContainer.Panel1
            // 
            this.verticalSplitContainer.Panel1.Controls.Add(this.searchText);
            this.verticalSplitContainer.Panel1.Controls.Add(this.ReloadButton);
            this.verticalSplitContainer.Panel1.Controls.Add(this.favoritesButton);
            this.verticalSplitContainer.Panel1.Controls.Add(this.searchButton);
            this.verticalSplitContainer.Panel1.Controls.Add(this.directoryTree);
            this.verticalSplitContainer.Panel1.Controls.Add(this.DirectoryTreeBarPictureBox);
            this.verticalSplitContainer.Panel1MinSize = 281;
            // 
            // verticalSplitContainer.Panel2
            // 
            this.verticalSplitContainer.Panel2.Controls.Add(this.filelistFlowLayoutPanel);
            this.verticalSplitContainer.Panel2MinSize = 300;
            this.verticalSplitContainer.Size = new System.Drawing.Size(951, 534);
            this.verticalSplitContainer.SplitterDistance = 281;
            this.verticalSplitContainer.SplitterWidth = 3;
            this.verticalSplitContainer.TabIndex = 0;
            this.verticalSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.verticalSplitContainer_SplitterMoved);
            // 
            // ReloadButton
            // 
            this.ReloadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ReloadButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_reload;
            this.ReloadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadButton.FlatAppearance.BorderSize = 0;
            this.ReloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadButton.Location = new System.Drawing.Point(1, 0);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(46, 37);
            this.ReloadButton.TabIndex = 15;
            this.toolTip1.SetToolTip(this.ReloadButton, "重新加载目录");
            this.ReloadButton.UseVisualStyleBackColor = false;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // favoritesButton
            // 
            this.favoritesButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_favorite_press;
            this.favoritesButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.favoritesButton.Enabled = false;
            this.favoritesButton.FlatAppearance.BorderSize = 0;
            this.favoritesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.favoritesButton.Location = new System.Drawing.Point(232, 1);
            this.favoritesButton.Name = "favoritesButton";
            this.favoritesButton.Size = new System.Drawing.Size(44, 38);
            this.favoritesButton.TabIndex = 14;
            this.toolTip1.SetToolTip(this.favoritesButton, "收藏");
            this.favoritesButton.UseVisualStyleBackColor = true;
            this.favoritesButton.Click += new System.EventHandler(this.favoritesButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_search_press;
            this.searchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.searchButton.Enabled = false;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.searchButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.searchButton.Location = new System.Drawing.Point(174, 2);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(54, 37);
            this.searchButton.TabIndex = 13;
            this.toolTip1.SetToolTip(this.searchButton, "检索");
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.search_Click);
            // 
            // directoryTree
            // 
            this.directoryTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.directoryTree.BackColor = System.Drawing.Color.White;
            this.directoryTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directoryTree.HideSelection = false;
            this.directoryTree.ImageIndex = 0;
            this.directoryTree.ImageList = this.directoryIcons;
            this.directoryTree.Location = new System.Drawing.Point(1, 41);
            this.directoryTree.Name = "directoryTree";
            this.directoryTree.SelectedImageIndex = 0;
            this.directoryTree.Size = new System.Drawing.Size(277, 493);
            this.directoryTree.TabIndex = 0;
            this.directoryTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.directoryTree_BeforeExpand);
            this.directoryTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.directoryTree_AfterExpand);
            this.directoryTree.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.directoryTree_DrawNode);
            this.directoryTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.directoryTree_BeforeSelect);
            this.directoryTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.directoryTree_AfterSelect);
            // 
            // directoryIcons
            // 
            this.directoryIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("directoryIcons.ImageStream")));
            this.directoryIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.directoryIcons.Images.SetKeyName(0, "Computer.ico");
            this.directoryIcons.Images.SetKeyName(1, "folder.ico");
            this.directoryIcons.Images.SetKeyName(2, "My Documents.ico");
            this.directoryIcons.Images.SetKeyName(3, "fixed drive.ico");
            this.directoryIcons.Images.SetKeyName(4, "Open Folder.ico");
            this.directoryIcons.Images.SetKeyName(5, "MyPictures.ico");
            this.directoryIcons.Images.SetKeyName(6, "MyFavorites.ico");
            this.directoryIcons.Images.SetKeyName(7, "desktop.ico");
            // 
            // DirectoryTreeBarPictureBox
            // 
            this.DirectoryTreeBarPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DirectoryTreeBarPictureBox.BackgroundImage")));
            this.DirectoryTreeBarPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DirectoryTreeBarPictureBox.Location = new System.Drawing.Point(0, -1);
            this.DirectoryTreeBarPictureBox.Name = "DirectoryTreeBarPictureBox";
            this.DirectoryTreeBarPictureBox.Size = new System.Drawing.Size(278, 41);
            this.DirectoryTreeBarPictureBox.TabIndex = 16;
            this.DirectoryTreeBarPictureBox.TabStop = false;
            // 
            // filelistFlowLayoutPanel
            // 
            this.filelistFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filelistFlowLayoutPanel.AutoScroll = true;
            this.filelistFlowLayoutPanel.BackColor = System.Drawing.Color.Silver;
            this.filelistFlowLayoutPanel.Location = new System.Drawing.Point(-1, 2);
            this.filelistFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.filelistFlowLayoutPanel.Name = "filelistFlowLayoutPanel";
            this.filelistFlowLayoutPanel.Padding = new System.Windows.Forms.Padding(5);
            this.filelistFlowLayoutPanel.Size = new System.Drawing.Size(670, 532);
            this.filelistFlowLayoutPanel.TabIndex = 9;
            this.filelistFlowLayoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filelistFlowLayoutPanel_MouseClick);
            // 
            // filesIcons
            // 
            this.filesIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.filesIcons.ImageSize = new System.Drawing.Size(100, 256);
            this.filesIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ShowAlways = true;
            // 
            // searchText
            // 
            this.searchText.DecLength = 2;
            this.searchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchText.InputType = TileImageViewer.TextInputType.NotControl;
            this.searchText.Location = new System.Drawing.Point(53, 9);
            this.searchText.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.searchText.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.searchText.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.searchText.Name = "searchText";
            this.searchText.OldText = null;
            this.searchText.PromptColor = System.Drawing.Color.Gray;
            this.searchText.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.searchText.PromptText = "";
            this.searchText.RegexPattern = "";
            this.searchText.Size = new System.Drawing.Size(115, 22);
            this.searchText.TabIndex = 17;
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            this.searchText.Enter += new System.EventHandler(this.searchText_Enter);
            this.searchText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchText_KeyPress);
            this.searchText.Leave += new System.EventHandler(this.searchText_Leave);
            // 
            // FrmFolderSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(951, 574);
            this.Controls.Add(this.horizontalSplitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmFolderSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "迪赛特 视景";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmFolderSelect_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.horizontalSplitContainer.Panel1.ResumeLayout(false);
            this.horizontalSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSplitContainer)).EndInit();
            this.horizontalSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToolBarPictureBox)).EndInit();
            this.verticalSplitContainer.Panel1.ResumeLayout(false);
            this.verticalSplitContainer.Panel1.PerformLayout();
            this.verticalSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.verticalSplitContainer)).EndInit();
            this.verticalSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DirectoryTreeBarPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer horizontalSplitContainer;
        private System.Windows.Forms.SplitContainer verticalSplitContainer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CloseStartWindow;
        private System.Windows.Forms.PictureBox ToolBarPictureBox;
        private Controls.TextBoxEx searchText;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.Button favoritesButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TreeView directoryTree;
        private System.Windows.Forms.PictureBox DirectoryTreeBarPictureBox;
        private System.Windows.Forms.FlowLayoutPanel filelistFlowLayoutPanel;
        private System.Windows.Forms.ImageList directoryIcons;
        private System.Windows.Forms.ImageList filesIcons;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
namespace TileImageViewer
{
    partial class FrmAnnotationSetting
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
            this.components = new System.ComponentModel.Container();
            this.AnnotationsListView = new System.Windows.Forms.ListView();
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DetailLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.titleLable = new System.Windows.Forms.Label();
            this.ExitButton = new TileImageViewer.Controls.UCBtnImg();
            this.ucPanelParent2 = new TileImageViewer.Controls.UCPanelParent();
            this.CopyButton = new TileImageViewer.Controls.UCBtnImg();
            this.DeleteButton = new TileImageViewer.Controls.UCBtnImg();
            this.HideButton = new TileImageViewer.Controls.UCBtnImg();
            this.SaveButton = new TileImageViewer.Controls.UCBtnImg();
            this.ExportButton = new TileImageViewer.Controls.UCBtnImg();
            this.ColorButton = new TileImageViewer.Controls.UCBtnImg();
            this.ucPanelParent1 = new TileImageViewer.Controls.UCPanelParent();
            this.ucPanelParent3 = new TileImageViewer.Controls.UCPanelParent();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ucPanelParent2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AnnotationsListView
            // 
            this.AnnotationsListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.AnnotationsListView.BackColor = System.Drawing.Color.White;
            this.AnnotationsListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AnnotationsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title});
            this.AnnotationsListView.Font = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AnnotationsListView.FullRowSelect = true;
            this.AnnotationsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.AnnotationsListView.HideSelection = false;
            this.AnnotationsListView.LabelEdit = true;
            this.AnnotationsListView.Location = new System.Drawing.Point(13, 77);
            this.AnnotationsListView.Name = "AnnotationsListView";
            this.AnnotationsListView.Size = new System.Drawing.Size(306, 146);
            this.AnnotationsListView.TabIndex = 0;
            this.AnnotationsListView.UseCompatibleStateImageBehavior = false;
            this.AnnotationsListView.View = System.Windows.Forms.View.Details;
            this.AnnotationsListView.SelectedIndexChanged += new System.EventHandler(this.AnnotationsListView_SelectedIndexChanged);
            // 
            // Title
            // 
            this.Title.Width = 283;
            // 
            // DetailLabel
            // 
            this.DetailLabel.AutoSize = true;
            this.DetailLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DetailLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DetailLabel.ForeColor = System.Drawing.Color.Black;
            this.DetailLabel.Location = new System.Drawing.Point(13, 244);
            this.DetailLabel.Name = "DetailLabel";
            this.DetailLabel.Size = new System.Drawing.Size(64, 16);
            this.DetailLabel.TabIndex = 3;
            this.DetailLabel.Text = "Detail.";
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_toolbar;
            this.pictureBox1.Location = new System.Drawing.Point(-2, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(339, 29);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // titleLable
            // 
            this.titleLable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.titleLable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleLable.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLable.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.titleLable.Location = new System.Drawing.Point(7, 3);
            this.titleLable.Name = "titleLable";
            this.titleLable.Size = new System.Drawing.Size(37, 19);
            this.titleLable.TabIndex = 15;
            this.titleLable.Text = "标注";
            this.titleLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.White;
            this.ExitButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_exit;
            this.ExitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ExitButton.BtnBackColor = System.Drawing.Color.White;
            this.ExitButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ExitButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ExitButton.BtnText = "";
            this.ExitButton.ConerRadius = 5;
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.EnabledMouseEffect = true;
            this.ExitButton.FillColor = System.Drawing.Color.Transparent;
            this.ExitButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ExitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ExitButton.Image = null;
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ExitButton.ImageFontIcons = null;
            this.ExitButton.IsRadius = true;
            this.ExitButton.IsShowRect = true;
            this.ExitButton.IsShowTips = false;
            this.ExitButton.Location = new System.Drawing.Point(305, -2);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.RectColor = System.Drawing.Color.Transparent;
            this.ExitButton.RectWidth = 1;
            this.ExitButton.Size = new System.Drawing.Size(26, 25);
            this.ExitButton.TabIndex = 11;
            this.ExitButton.TabStop = false;
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ExitButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.ExitButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ExitButton.TipsText = "";
            this.ExitButton.BtnClick += new System.EventHandler(this.ExitButton_Click);
            // 
            // ucPanelParent2
            // 
            this.ucPanelParent2.BackColor = System.Drawing.Color.DimGray;
            this.ucPanelParent2.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent2.ConerRadius = 20;
            this.ucPanelParent2.Controls.Add(this.CopyButton);
            this.ucPanelParent2.Controls.Add(this.DeleteButton);
            this.ucPanelParent2.Controls.Add(this.HideButton);
            this.ucPanelParent2.Controls.Add(this.SaveButton);
            this.ucPanelParent2.Controls.Add(this.ExportButton);
            this.ucPanelParent2.Controls.Add(this.ColorButton);
            this.ucPanelParent2.FillColor = System.Drawing.Color.DimGray;
            this.ucPanelParent2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent2.IsRadius = true;
            this.ucPanelParent2.IsShowRect = true;
            this.ucPanelParent2.Location = new System.Drawing.Point(-6, 25);
            this.ucPanelParent2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent2.Name = "ucPanelParent2";
            this.ucPanelParent2.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent2.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent2.RectWidth = 10;
            this.ucPanelParent2.Size = new System.Drawing.Size(351, 47);
            this.ucPanelParent2.TabIndex = 12;
            // 
            // CopyButton
            // 
            this.CopyButton.BackColor = System.Drawing.Color.White;
            this.CopyButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_copy;
            this.CopyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CopyButton.BtnBackColor = System.Drawing.Color.White;
            this.CopyButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.CopyButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.CopyButton.BtnText = "";
            this.CopyButton.ConerRadius = 5;
            this.CopyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CopyButton.Enabled = false;
            this.CopyButton.EnabledMouseEffect = true;
            this.CopyButton.FillColor = System.Drawing.Color.Transparent;
            this.CopyButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CopyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.CopyButton.Image = null;
            this.CopyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CopyButton.ImageFontIcons = null;
            this.CopyButton.IsRadius = true;
            this.CopyButton.IsShowRect = true;
            this.CopyButton.IsShowTips = false;
            this.CopyButton.Location = new System.Drawing.Point(68, 4);
            this.CopyButton.Margin = new System.Windows.Forms.Padding(0);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.RectColor = System.Drawing.Color.White;
            this.CopyButton.RectWidth = 1;
            this.CopyButton.Size = new System.Drawing.Size(41, 33);
            this.CopyButton.TabIndex = 13;
            this.CopyButton.TabStop = false;
            this.CopyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CopyButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.CopyButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.CopyButton.TipsText = "";
            this.CopyButton.BtnClick += new System.EventHandler(this.CopyButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.BackColor = System.Drawing.Color.White;
            this.DeleteButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_delete;
            this.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DeleteButton.BtnBackColor = System.Drawing.Color.White;
            this.DeleteButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.DeleteButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.DeleteButton.BtnText = "";
            this.DeleteButton.ConerRadius = 5;
            this.DeleteButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DeleteButton.EnabledMouseEffect = true;
            this.DeleteButton.FillColor = System.Drawing.Color.Transparent;
            this.DeleteButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DeleteButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.DeleteButton.Image = null;
            this.DeleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteButton.ImageFontIcons = null;
            this.DeleteButton.IsRadius = true;
            this.DeleteButton.IsShowRect = true;
            this.DeleteButton.IsShowTips = false;
            this.DeleteButton.Location = new System.Drawing.Point(288, 4);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(0);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.RectColor = System.Drawing.Color.White;
            this.DeleteButton.RectWidth = 1;
            this.DeleteButton.Size = new System.Drawing.Size(41, 33);
            this.DeleteButton.TabIndex = 10;
            this.DeleteButton.TabStop = false;
            this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.DeleteButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.DeleteButton.TipsText = "";
            this.DeleteButton.BtnClick += new System.EventHandler(this.DeleteButton_Click);
            // 
            // HideButton
            // 
            this.HideButton.BackColor = System.Drawing.Color.White;
            this.HideButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_hide;
            this.HideButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HideButton.BtnBackColor = System.Drawing.Color.White;
            this.HideButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.HideButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.HideButton.BtnText = "";
            this.HideButton.ConerRadius = 5;
            this.HideButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HideButton.EnabledMouseEffect = true;
            this.HideButton.FillColor = System.Drawing.Color.Transparent;
            this.HideButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.HideButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.HideButton.Image = null;
            this.HideButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HideButton.ImageFontIcons = null;
            this.HideButton.IsRadius = true;
            this.HideButton.IsShowRect = true;
            this.HideButton.IsShowTips = false;
            this.HideButton.Location = new System.Drawing.Point(233, 4);
            this.HideButton.Margin = new System.Windows.Forms.Padding(0);
            this.HideButton.Name = "HideButton";
            this.HideButton.RectColor = System.Drawing.Color.White;
            this.HideButton.RectWidth = 1;
            this.HideButton.Size = new System.Drawing.Size(41, 33);
            this.HideButton.TabIndex = 10;
            this.HideButton.TabStop = false;
            this.HideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HideButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.HideButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.HideButton.TipsText = "";
            this.HideButton.BtnClick += new System.EventHandler(this.HideButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.Color.White;
            this.SaveButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_save;
            this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveButton.BtnBackColor = System.Drawing.Color.White;
            this.SaveButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.SaveButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.SaveButton.BtnText = "";
            this.SaveButton.ConerRadius = 5;
            this.SaveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveButton.EnabledMouseEffect = true;
            this.SaveButton.FillColor = System.Drawing.Color.Transparent;
            this.SaveButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SaveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.SaveButton.Image = null;
            this.SaveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveButton.ImageFontIcons = null;
            this.SaveButton.IsRadius = true;
            this.SaveButton.IsShowRect = true;
            this.SaveButton.IsShowTips = false;
            this.SaveButton.Location = new System.Drawing.Point(13, 4);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.RectColor = System.Drawing.Color.White;
            this.SaveButton.RectWidth = 1;
            this.SaveButton.Size = new System.Drawing.Size(41, 33);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.TabStop = false;
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.SaveButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.SaveButton.TipsText = "";
            this.SaveButton.BtnClick += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.BackColor = System.Drawing.Color.White;
            this.ExportButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_export;
            this.ExportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ExportButton.BtnBackColor = System.Drawing.Color.White;
            this.ExportButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ExportButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ExportButton.BtnText = "";
            this.ExportButton.ConerRadius = 5;
            this.ExportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExportButton.EnabledMouseEffect = true;
            this.ExportButton.FillColor = System.Drawing.Color.Transparent;
            this.ExportButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ExportButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ExportButton.Image = null;
            this.ExportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ExportButton.ImageFontIcons = null;
            this.ExportButton.IsRadius = true;
            this.ExportButton.IsShowRect = true;
            this.ExportButton.IsShowTips = false;
            this.ExportButton.Location = new System.Drawing.Point(178, 4);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.RectColor = System.Drawing.Color.White;
            this.ExportButton.RectWidth = 1;
            this.ExportButton.Size = new System.Drawing.Size(41, 33);
            this.ExportButton.TabIndex = 10;
            this.ExportButton.TabStop = false;
            this.ExportButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ExportButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.ExportButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ExportButton.TipsText = "";
            this.ExportButton.BtnClick += new System.EventHandler(this.ExportButton_Click);
            // 
            // ColorButton
            // 
            this.ColorButton.BackColor = System.Drawing.Color.White;
            this.ColorButton.BackgroundImage = global::DST.TileImageViewer.Properties.Resources.icon_anlist_color;
            this.ColorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ColorButton.BtnBackColor = System.Drawing.Color.White;
            this.ColorButton.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ColorButton.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ColorButton.BtnText = "";
            this.ColorButton.ConerRadius = 5;
            this.ColorButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ColorButton.EnabledMouseEffect = true;
            this.ColorButton.FillColor = System.Drawing.Color.Transparent;
            this.ColorButton.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ColorButton.Image = null;
            this.ColorButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ColorButton.ImageFontIcons = null;
            this.ColorButton.IsRadius = true;
            this.ColorButton.IsShowRect = true;
            this.ColorButton.IsShowTips = false;
            this.ColorButton.Location = new System.Drawing.Point(123, 4);
            this.ColorButton.Margin = new System.Windows.Forms.Padding(0);
            this.ColorButton.Name = "ColorButton";
            this.ColorButton.RectColor = System.Drawing.Color.White;
            this.ColorButton.RectWidth = 1;
            this.ColorButton.Size = new System.Drawing.Size(41, 33);
            this.ColorButton.TabIndex = 10;
            this.ColorButton.TabStop = false;
            this.ColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ColorButton.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.ColorButton.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ColorButton.TipsText = "";
            this.ColorButton.BtnClick += new System.EventHandler(this.ColorButton_Click);
            // 
            // ucPanelParent1
            // 
            this.ucPanelParent1.BackColor = System.Drawing.Color.Silver;
            this.ucPanelParent1.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.ConerRadius = 20;
            this.ucPanelParent1.FillColor = System.Drawing.Color.White;
            this.ucPanelParent1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent1.IsRadius = true;
            this.ucPanelParent1.IsShowRect = true;
            this.ucPanelParent1.Location = new System.Drawing.Point(1, 65);
            this.ucPanelParent1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent1.Name = "ucPanelParent1";
            this.ucPanelParent1.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent1.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.RectWidth = 10;
            this.ucPanelParent1.Size = new System.Drawing.Size(331, 169);
            this.ucPanelParent1.TabIndex = 11;
            // 
            // ucPanelParent3
            // 
            this.ucPanelParent3.BackColor = System.Drawing.Color.DimGray;
            this.ucPanelParent3.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent3.ConerRadius = 20;
            this.ucPanelParent3.FillColor = System.Drawing.Color.WhiteSmoke;
            this.ucPanelParent3.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent3.IsRadius = true;
            this.ucPanelParent3.IsShowRect = true;
            this.ucPanelParent3.Location = new System.Drawing.Point(1, 234);
            this.ucPanelParent3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent3.Name = "ucPanelParent3";
            this.ucPanelParent3.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent3.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent3.RectWidth = 10;
            this.ucPanelParent3.Size = new System.Drawing.Size(331, 60);
            this.ucPanelParent3.TabIndex = 13;
            // 
            // FrmAnnotationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(334, 298);
            this.Controls.Add(this.titleLable);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ucPanelParent2);
            this.Controls.Add(this.DetailLabel);
            this.Controls.Add(this.AnnotationsListView);
            this.Controls.Add(this.ucPanelParent1);
            this.Controls.Add(this.ucPanelParent3);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAnnotationSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "注释";
            this.Load += new System.EventHandler(this.FrmAnnotationSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ucPanelParent2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView AnnotationsListView;
        private System.Windows.Forms.Label DetailLabel;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ToolTip toolTip1;
        private Controls.UCBtnImg SaveButton;
        private Controls.UCBtnImg ExportButton;
        private Controls.UCBtnImg ColorButton;
        private Controls.UCBtnImg DeleteButton;
        private Controls.UCBtnImg HideButton;
        private Controls.UCPanelParent ucPanelParent1;
        private Controls.UCPanelParent ucPanelParent2;
        private Controls.UCBtnImg CopyButton;
        private Controls.UCPanelParent ucPanelParent3;
        private Controls.UCBtnImg ExitButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label titleLable;
    }
}
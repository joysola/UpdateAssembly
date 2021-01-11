namespace TileImageViewer
{
    partial class SlicesCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ucPanelParent1 = new TileImageViewer.Controls.UCPanelParent();
            this.slicesPicturerbox = new System.Windows.Forms.PictureBox();
            this.ucPanelParent2 = new TileImageViewer.Controls.UCPanelParent();
            this.RotatePictureBox = new System.Windows.Forms.PictureBox();
            this.barCodePicturebox = new System.Windows.Forms.PictureBox();
            this.ucPanelParent1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slicesPicturerbox)).BeginInit();
            this.ucPanelParent2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotatePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barCodePicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Silver;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("隶书", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(1, 394);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(121, 18);
            this.textBox1.TabIndex = 13;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ucPanelParent1
            // 
            this.ucPanelParent1.BackColor = System.Drawing.Color.Transparent;
            this.ucPanelParent1.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucPanelParent1.ConerRadius = 20;
            this.ucPanelParent1.Controls.Add(this.slicesPicturerbox);
            this.ucPanelParent1.FillColor = System.Drawing.Color.White;
            this.ucPanelParent1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent1.IsRadius = true;
            this.ucPanelParent1.IsShowRect = true;
            this.ucPanelParent1.Location = new System.Drawing.Point(1, 2);
            this.ucPanelParent1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent1.Name = "ucPanelParent1";
            this.ucPanelParent1.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent1.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.RectWidth = 10;
            this.ucPanelParent1.Size = new System.Drawing.Size(123, 272);
            this.ucPanelParent1.TabIndex = 11;
            // 
            // slicesPicturerbox
            // 
            this.slicesPicturerbox.BackColor = System.Drawing.Color.White;
            this.slicesPicturerbox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.slicesPicturerbox.Location = new System.Drawing.Point(8, 8);
            this.slicesPicturerbox.Name = "slicesPicturerbox";
            this.slicesPicturerbox.Size = new System.Drawing.Size(106, 255);
            this.slicesPicturerbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.slicesPicturerbox.TabIndex = 4;
            this.slicesPicturerbox.TabStop = false;
            this.slicesPicturerbox.Click += new System.EventHandler(this.slicesPicturerbox_Click);
            // 
            // ucPanelParent2
            // 
            this.ucPanelParent2.BackColor = System.Drawing.Color.Transparent;
            this.ucPanelParent2.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucPanelParent2.ConerRadius = 20;
            this.ucPanelParent2.Controls.Add(this.RotatePictureBox);
            this.ucPanelParent2.Controls.Add(this.barCodePicturebox);
            this.ucPanelParent2.FillColor = System.Drawing.Color.White;
            this.ucPanelParent2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent2.IsRadius = true;
            this.ucPanelParent2.IsShowRect = true;
            this.ucPanelParent2.Location = new System.Drawing.Point(1, 284);
            this.ucPanelParent2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent2.Name = "ucPanelParent2";
            this.ucPanelParent2.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent2.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent2.RectWidth = 10;
            this.ucPanelParent2.Size = new System.Drawing.Size(122, 102);
            this.ucPanelParent2.TabIndex = 12;
            // 
            // RotatePictureBox
            // 
            this.RotatePictureBox.BackColor = System.Drawing.Color.White;
            this.RotatePictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.RotatePictureBox.Image = global::DST.TileImageViewer.Properties.Resources.icons_rotate_180;
            this.RotatePictureBox.Location = new System.Drawing.Point(93, 11);
            this.RotatePictureBox.Name = "RotatePictureBox";
            this.RotatePictureBox.Size = new System.Drawing.Size(19, 21);
            this.RotatePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RotatePictureBox.TabIndex = 10;
            this.RotatePictureBox.TabStop = false;
            this.RotatePictureBox.Click += new System.EventHandler(this.RotatePictureBox_Click);
            // 
            // barCodePicturebox
            // 
            this.barCodePicturebox.BackColor = System.Drawing.Color.White;
            this.barCodePicturebox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.barCodePicturebox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.barCodePicturebox.Location = new System.Drawing.Point(7, 11);
            this.barCodePicturebox.Name = "barCodePicturebox";
            this.barCodePicturebox.Size = new System.Drawing.Size(105, 78);
            this.barCodePicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.barCodePicturebox.TabIndex = 9;
            this.barCodePicturebox.TabStop = false;
            this.barCodePicturebox.Click += new System.EventHandler(this.barCodePicturebox_Click);
            // 
            // SlicesCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.ucPanelParent1);
            this.Controls.Add(this.ucPanelParent2);
            this.Controls.Add(this.textBox1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(17);
            this.Name = "SlicesCtrl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(125, 415);
            this.Load += new System.EventHandler(this.SlicesCtrl_Load);
            this.Click += new System.EventHandler(this.SlicesCtrl_Click);
            this.ucPanelParent1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slicesPicturerbox)).EndInit();
            this.ucPanelParent2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RotatePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barCodePicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.UCPanelParent ucPanelParent1;
        private System.Windows.Forms.PictureBox slicesPicturerbox;
        private Controls.UCPanelParent ucPanelParent2;
        private System.Windows.Forms.PictureBox RotatePictureBox;
        private System.Windows.Forms.PictureBox barCodePicturebox;
        private System.Windows.Forms.TextBox textBox1;
    }
}

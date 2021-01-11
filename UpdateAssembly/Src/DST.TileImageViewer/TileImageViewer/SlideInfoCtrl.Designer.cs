namespace TileImageViewer
{
    partial class SlideInfoCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlideInfoCtrl));
            this.lbltp1 = new System.Windows.Forms.Label();
            this.lbltp2 = new System.Windows.Forms.Label();
            this.lbltp3 = new System.Windows.Forms.Label();
            this.lbltp4 = new System.Windows.Forms.Label();
            this.lbltp5 = new System.Windows.Forms.Label();
            this.lblAbsPoint = new System.Windows.Forms.Label();
            this.lblPos = new System.Windows.Forms.Label();
            this.lblRGB = new System.Windows.Forms.Label();
            this.ucPanelParent1 = new TileImageViewer.Controls.UCPanelParent();
            this.label1 = new System.Windows.Forms.Label();
            this.ucBtnImg1 = new TileImageViewer.Controls.UCBtnImg();
            this.ucPanelParent1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbltp1
            // 
            this.lbltp1.AutoSize = true;
            this.lbltp1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lbltp1.Location = new System.Drawing.Point(18, 34);
            this.lbltp1.Name = "lbltp1";
            this.lbltp1.Size = new System.Drawing.Size(39, 19);
            this.lbltp1.TabIndex = 0;
            this.lbltp1.Text = "坐标";
            this.lbltp1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lbltp2
            // 
            this.lbltp2.AutoSize = true;
            this.lbltp2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbltp2.Location = new System.Drawing.Point(18, 56);
            this.lbltp2.Name = "lbltp2";
            this.lbltp2.Size = new System.Drawing.Size(69, 20);
            this.lbltp2.TabIndex = 1;
            this.lbltp2.Text = "切片坐标";
            // 
            // lbltp3
            // 
            this.lbltp3.AutoSize = true;
            this.lbltp3.Location = new System.Drawing.Point(18, 78);
            this.lbltp3.Name = "lbltp3";
            this.lbltp3.Size = new System.Drawing.Size(39, 20);
            this.lbltp3.TabIndex = 2;
            this.lbltp3.Text = "位置";
            // 
            // lbltp4
            // 
            this.lbltp4.AutoSize = true;
            this.lbltp4.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lbltp4.Location = new System.Drawing.Point(18, 105);
            this.lbltp4.Name = "lbltp4";
            this.lbltp4.Size = new System.Drawing.Size(39, 19);
            this.lbltp4.TabIndex = 3;
            this.lbltp4.Text = "强速";
            // 
            // lbltp5
            // 
            this.lbltp5.AutoSize = true;
            this.lbltp5.Location = new System.Drawing.Point(18, 129);
            this.lbltp5.Name = "lbltp5";
            this.lbltp5.Size = new System.Drawing.Size(39, 20);
            this.lbltp5.TabIndex = 4;
            this.lbltp5.Text = "RGB";
            this.lbltp5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblAbsPoint
            // 
            this.lblAbsPoint.AutoSize = true;
            this.lblAbsPoint.Location = new System.Drawing.Point(109, 56);
            this.lblAbsPoint.Name = "lblAbsPoint";
            this.lblAbsPoint.Size = new System.Drawing.Size(53, 20);
            this.lblAbsPoint.TabIndex = 5;
            this.lblAbsPoint.Text = "label1";
            // 
            // lblPos
            // 
            this.lblPos.AutoSize = true;
            this.lblPos.Location = new System.Drawing.Point(109, 78);
            this.lblPos.Name = "lblPos";
            this.lblPos.Size = new System.Drawing.Size(53, 20);
            this.lblPos.TabIndex = 6;
            this.lblPos.Text = "label1";
            // 
            // lblRGB
            // 
            this.lblRGB.AutoSize = true;
            this.lblRGB.Location = new System.Drawing.Point(109, 129);
            this.lblRGB.Name = "lblRGB";
            this.lblRGB.Size = new System.Drawing.Size(53, 20);
            this.lblRGB.TabIndex = 7;
            this.lblRGB.Text = "label1";
            // 
            // ucPanelParent1
            // 
            this.ucPanelParent1.BackColor = System.Drawing.Color.Transparent;
            this.ucPanelParent1.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.ConerRadius = 5;
            this.ucPanelParent1.Controls.Add(this.label1);
            this.ucPanelParent1.FillColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent1.IsRadius = true;
            this.ucPanelParent1.IsShowRect = true;
            this.ucPanelParent1.Location = new System.Drawing.Point(5, 5);
            this.ucPanelParent1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent1.Name = "ucPanelParent1";
            this.ucPanelParent1.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent1.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.RectWidth = 10;
            this.ucPanelParent1.Size = new System.Drawing.Size(233, 24);
            this.ucPanelParent1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(0, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "像素信息";
            // 
            // ucBtnImg1
            // 
            this.ucBtnImg1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ucBtnImg1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucBtnImg1.BtnBackColor = System.Drawing.SystemColors.WindowFrame;
            this.ucBtnImg1.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ucBtnImg1.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.BtnText = "";
            this.ucBtnImg1.ConerRadius = 5;
            this.ucBtnImg1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnImg1.EnabledMouseEffect = true;
            this.ucBtnImg1.FillColor = System.Drawing.SystemColors.WindowFrame;
            this.ucBtnImg1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnImg1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.Image = ((System.Drawing.Image)(resources.GetObject("ucBtnImg1.Image")));
            this.ucBtnImg1.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.ImageFontIcons = null;
            this.ucBtnImg1.IsRadius = true;
            this.ucBtnImg1.IsShowRect = true;
            this.ucBtnImg1.IsShowTips = false;
            this.ucBtnImg1.Location = new System.Drawing.Point(217, 4);
            this.ucBtnImg1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnImg1.Name = "ucBtnImg1";
            this.ucBtnImg1.RectColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.RectWidth = 1;
            this.ucBtnImg1.Size = new System.Drawing.Size(20, 20);
            this.ucBtnImg1.TabIndex = 1;
            this.ucBtnImg1.TabStop = false;
            this.ucBtnImg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.TipPosition = TileImageViewer.Controls.UCBtnExt.TipPositionEnum.Bottom;
            this.ucBtnImg1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnImg1.TipsText = "关闭";
            this.ucBtnImg1.BtnClick += new System.EventHandler(this.ucBtnImg1_BtnClick);
            // 
            // SlideInfoCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucBtnImg1);
            this.Controls.Add(this.ucPanelParent1);
            this.Controls.Add(this.lblRGB);
            this.Controls.Add(this.lblPos);
            this.Controls.Add(this.lblAbsPoint);
            this.Controls.Add(this.lbltp5);
            this.Controls.Add(this.lbltp4);
            this.Controls.Add(this.lbltp3);
            this.Controls.Add(this.lbltp2);
            this.Controls.Add(this.lbltp1);
            this.Name = "SlideInfoCtrl";
            this.Size = new System.Drawing.Size(243, 165);
            this.ucPanelParent1.ResumeLayout(false);
            this.ucPanelParent1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbltp1;
        private System.Windows.Forms.Label lbltp2;
        private System.Windows.Forms.Label lbltp3;
        private System.Windows.Forms.Label lbltp4;
        private System.Windows.Forms.Label lbltp5;
        private System.Windows.Forms.Label lblAbsPoint;
        private System.Windows.Forms.Label lblPos;
        private System.Windows.Forms.Label lblRGB;
        private Controls.UCPanelParent ucPanelParent1;
        private System.Windows.Forms.Label label1;
        private Controls.UCBtnImg ucBtnImg1;
    }
}

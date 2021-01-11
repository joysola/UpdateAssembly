namespace TileImageViewer
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.lblLeft1 = new System.Windows.Forms.Label();
            this.lblRight1 = new System.Windows.Forms.Label();
            this.lblRight2 = new System.Windows.Forms.Label();
            this.lblLeft3 = new System.Windows.Forms.Label();
            this.lblLeft4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLeft1
            // 
            this.lblLeft1.AutoSize = true;
            this.lblLeft1.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLeft1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblLeft1.Location = new System.Drawing.Point(19, 17);
            this.lblLeft1.Name = "lblLeft1";
            this.lblLeft1.Size = new System.Drawing.Size(82, 21);
            this.lblLeft1.TabIndex = 0;
            this.lblLeft1.Text = "label1";
            // 
            // lblRight1
            // 
            this.lblRight1.AutoSize = true;
            this.lblRight1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRight1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblRight1.Location = new System.Drawing.Point(394, 17);
            this.lblRight1.Name = "lblRight1";
            this.lblRight1.Size = new System.Drawing.Size(49, 14);
            this.lblRight1.TabIndex = 1;
            this.lblRight1.Text = "label1";
            this.lblRight1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblRight1.Visible = false;
            // 
            // lblRight2
            // 
            this.lblRight2.AutoSize = true;
            this.lblRight2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRight2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblRight2.Location = new System.Drawing.Point(394, 42);
            this.lblRight2.Name = "lblRight2";
            this.lblRight2.Size = new System.Drawing.Size(49, 14);
            this.lblRight2.TabIndex = 2;
            this.lblRight2.Text = "label1";
            this.lblRight2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblRight2.Visible = false;
            // 
            // lblLeft3
            // 
            this.lblLeft3.AutoSize = true;
            this.lblLeft3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLeft3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblLeft3.Location = new System.Drawing.Point(21, 283);
            this.lblLeft3.Name = "lblLeft3";
            this.lblLeft3.Size = new System.Drawing.Size(49, 14);
            this.lblLeft3.TabIndex = 3;
            this.lblLeft3.Text = "label1";
            // 
            // lblLeft4
            // 
            this.lblLeft4.AutoSize = true;
            this.lblLeft4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLeft4.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblLeft4.Location = new System.Drawing.Point(21, 316);
            this.lblLeft4.Name = "lblLeft4";
            this.lblLeft4.Size = new System.Drawing.Size(49, 14);
            this.lblLeft4.TabIndex = 4;
            this.lblLeft4.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::DST.TileImageViewer.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(23, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(428, 191);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(23, 306);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 1);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuText;
            this.ClientSize = new System.Drawing.Size(477, 349);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblLeft4);
            this.Controls.Add(this.lblLeft3);
            this.Controls.Add(this.lblRight2);
            this.Controls.Add(this.lblRight1);
            this.Controls.Add(this.lblLeft1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.Text = "FrmAbout";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLeft1;
        private System.Windows.Forms.Label lblRight1;
        private System.Windows.Forms.Label lblRight2;
        private System.Windows.Forms.Label lblLeft3;
        private System.Windows.Forms.Label lblLeft4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
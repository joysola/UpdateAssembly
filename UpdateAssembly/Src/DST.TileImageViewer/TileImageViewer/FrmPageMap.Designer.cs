namespace TileImageViewer
{
    partial class FrmPageMap
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
            this.pbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox
            // 
            this.pbox.Location = new System.Drawing.Point(0, 0);
            this.pbox.Margin = new System.Windows.Forms.Padding(0);
            this.pbox.Name = "pbox";
            this.pbox.Size = new System.Drawing.Size(256, 256);
            this.pbox.TabIndex = 0;
            this.pbox.TabStop = false;
            this.pbox.SizeChanged += new System.EventHandler(this.pbox_SizeChanged);
            // 
            // FrmPageMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 253);
            this.ControlBox = false;
            this.Controls.Add(this.pbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPageMap";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmPageMap";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox;
    }
}
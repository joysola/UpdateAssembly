namespace TileImageViewer
{
    partial class MagCtrl
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
            this.SuspendLayout();
            // 
            // MagCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FillColor = System.Drawing.Color.Transparent;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MagCtrl";
            this.Size = new System.Drawing.Size(142, 123);
            this.Load += new System.EventHandler(this.MagCtrl_Load);
            this.VisibleChanged += new System.EventHandler(this.MagCtrl_VisibleChanged);
            this.DoubleClick += new System.EventHandler(this.MagCtrl_DoubleClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MagCtrl_MouseMove);
            this.Move += new System.EventHandler(this.MagCtrl_Move);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

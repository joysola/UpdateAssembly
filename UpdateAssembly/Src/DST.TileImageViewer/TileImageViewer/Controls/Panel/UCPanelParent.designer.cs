// ***********************************************************************
// Assembly         : TileImageViewer
// Created          : 08-17-2019
//
// ***********************************************************************
// <copyright file="UCPanelDialog.Designer.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************
namespace TileImageViewer.Controls
{
    /// <summary>
    /// Class UCPanelDialog.
    /// Implements the <see cref="TileImageViewer.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="TileImageViewer.Controls.UCControlBase" />
    partial class UCPanelParent
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UCPanelParent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ConerRadius = 20;
            this.FillColor = System.Drawing.Color.White;
            this.IsRadius = true;
            this.IsShowRect = true;
            this.Name = "UCPanelParent";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.RectWidth = 10;
            this.Size = new System.Drawing.Size(230, 170);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

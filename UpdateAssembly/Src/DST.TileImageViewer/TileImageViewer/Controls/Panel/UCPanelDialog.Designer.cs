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
    partial class UCPanelDialog
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
            this.ucBtnImg1 = new TileImageViewer.Controls.UCBtnImg();
            this.SuspendLayout();
            // 
            // ucBtnImg1
            // 
            this.ucBtnImg1.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.BtnBackColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ucBtnImg1.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.BtnText = "";
            this.ucBtnImg1.ConerRadius = 5;
            this.ucBtnImg1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnImg1.EnabledMouseEffect = false;
            this.ucBtnImg1.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnImg1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.Image = global::DST.TileImageViewer.Properties.Resources.input_clear;
            this.ucBtnImg1.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.ImageFontIcons = null;
            this.ucBtnImg1.IsRadius = true;
            this.ucBtnImg1.IsShowRect = true;
            this.ucBtnImg1.IsShowTips = false;
            this.ucBtnImg1.Location = new System.Drawing.Point(240, 8);
            this.ucBtnImg1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnImg1.Name = "ucBtnImg1";
            this.ucBtnImg1.RectColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.RectWidth = 5;
            this.ucBtnImg1.Size = new System.Drawing.Size(18, 18);
            this.ucBtnImg1.TabIndex = 0;
            this.ucBtnImg1.TabStop = false;
            this.ucBtnImg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.TipsColor = System.Drawing.Color.Black;
            this.ucBtnImg1.TipsText = "关闭";
            this.ucBtnImg1.BtnClick += new System.EventHandler(this.ucBtnImg1_BtnClick);
            // 
            // UCPanelDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ConerRadius = 20;
            this.Controls.Add(this.ucBtnImg1);
            this.FillColor = System.Drawing.Color.White;
            this.IsRadius = true;
            this.IsShowRect = true;
            this.Name = "UCPanelDialog";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.RectWidth = 10;
            this.Size = new System.Drawing.Size(266, 266);
            this.ResumeLayout(false);

        }

        #endregion

        private UCBtnImg ucBtnImg1;
    }
}

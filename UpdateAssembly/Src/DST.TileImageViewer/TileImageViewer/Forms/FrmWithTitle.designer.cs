// ***********************************************************************
// Assembly         : TileImageViewer
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="FrmWithTitle.Designer.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************
namespace TileImageViewer.Forms
{
    /// <summary>
    /// Class FrmWithTitle.
    /// Implements the <see cref="TileImageViewer.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="TileImageViewer.Forms.FrmBase" />
    partial class FrmWithTitle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWithTitle));
            this.ucSplitLine_H1 = new TileImageViewer.Controls.UCSplitLine_H();
            this.ucPanelParent1 = new TileImageViewer.Controls.UCPanelParent();
            this.btnClose = new TileImageViewer.Controls.UCBtnImg();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ucBtnImg1 = new TileImageViewer.Controls.UCBtnImg();
            this.ucPanelParent1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSplitLine_H1
            // 
            this.ucSplitLine_H1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucSplitLine_H1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSplitLine_H1.Location = new System.Drawing.Point(0, 0);
            this.ucSplitLine_H1.Name = "ucSplitLine_H1";
            this.ucSplitLine_H1.Size = new System.Drawing.Size(427, 1);
            this.ucSplitLine_H1.TabIndex = 0;
            this.ucSplitLine_H1.TabStop = false;
            // 
            // ucPanelParent1
            // 
            this.ucPanelParent1.BackColor = System.Drawing.Color.Transparent;
            this.ucPanelParent1.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.ConerRadius = 1;
            this.ucPanelParent1.Controls.Add(this.btnClose);
            this.ucPanelParent1.Controls.Add(this.lblTitle);
            this.ucPanelParent1.Controls.Add(this.ucBtnImg1);
            this.ucPanelParent1.FillColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucPanelParent1.IsRadius = true;
            this.ucPanelParent1.IsShowRect = true;
            this.ucPanelParent1.Location = new System.Drawing.Point(0, 0);
            this.ucPanelParent1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelParent1.Name = "ucPanelParent1";
            this.ucPanelParent1.Padding = new System.Windows.Forms.Padding(1);
            this.ucPanelParent1.RectColor = System.Drawing.SystemColors.WindowFrame;
            this.ucPanelParent1.RectWidth = 10;
            this.ucPanelParent1.Size = new System.Drawing.Size(427, 33);
            this.ucPanelParent1.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnClose.BtnBackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnClose.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.btnClose.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnClose.BtnText = "";
            this.btnClose.ConerRadius = 5;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.EnabledMouseEffect = true;
            this.btnClose.FillColor = System.Drawing.SystemColors.WindowFrame;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.ImageFontIcons = null;
            this.btnClose.IsRadius = true;
            this.btnClose.IsShowRect = true;
            this.btnClose.IsShowTips = false;
            this.btnClose.Location = new System.Drawing.Point(397, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.RectColor = System.Drawing.Color.Transparent;
            this.btnClose.RectWidth = 1;
            this.btnClose.Size = new System.Drawing.Size(29, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.TabStop = false;
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnClose.TipsText = "";
            this.btnClose.BtnClick += new System.EventHandler(this.btnClose_BtnClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(5, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(39, 20);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "标题";
            // 
            // ucBtnImg1
            // 
            this.ucBtnImg1.BackColor = System.Drawing.Color.White;
            this.ucBtnImg1.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnImg1.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ucBtnImg1.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.BtnText = "";
            this.ucBtnImg1.ConerRadius = 5;
            this.ucBtnImg1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnImg1.EnabledMouseEffect = true;
            this.ucBtnImg1.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnImg1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.Image = ((System.Drawing.Image)(resources.GetObject("ucBtnImg1.Image")));
            this.ucBtnImg1.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.ImageFontIcons = null;
            this.ucBtnImg1.IsRadius = true;
            this.ucBtnImg1.IsShowRect = true;
            this.ucBtnImg1.IsShowTips = false;
            this.ucBtnImg1.Location = new System.Drawing.Point(459, 4);
            this.ucBtnImg1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnImg1.Name = "ucBtnImg1";
            this.ucBtnImg1.RectColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.RectWidth = 1;
            this.ucBtnImg1.Size = new System.Drawing.Size(29, 26);
            this.ucBtnImg1.TabIndex = 0;
            this.ucBtnImg1.TabStop = false;
            this.ucBtnImg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnImg1.TipsText = "";
            // 
            // FrmWithTitle
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(427, 310);
            this.Controls.Add(this.ucPanelParent1);
            this.Controls.Add(this.ucSplitLine_H1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsFullSize = false;
            this.IsShowMaskDialog = true;
            this.IsShowRegion = true;
            this.Name = "FrmWithTitle";
            this.Redraw = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmWithTitle";
            this.Shown += new System.EventHandler(this.FrmWithTitle_Shown);
            this.SizeChanged += new System.EventHandler(this.FrmWithTitle_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.FrmWithTitle_VisibleChanged);
            this.ucPanelParent1.ResumeLayout(false);
            this.ucPanelParent1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// The uc split line h1
        /// </summary>
        private Controls.UCSplitLine_H ucSplitLine_H1;
        private Controls.UCPanelParent ucPanelParent1;
        private System.Windows.Forms.Label lblTitle;
        private Controls.UCBtnImg ucBtnImg1;
        private Controls.UCBtnImg btnClose;
    }
}
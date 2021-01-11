// ***********************************************************************
// Assembly         : TileImageViewer
// Created          : 08-17-2019
//
// ***********************************************************************
// <copyright file="UCPanelDialog.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TileImageViewer.Controls
{
    /// <summary>
    /// Class UCPanelParent.
    /// Implements the <see cref="TileImageViewer.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="TileImageViewer.Controls.UCControlBase" />
    public partial class UCPanelParent : UCControlBase, IContainerControl
    {
        /// <summary>
        /// The m int maximum height
        /// </summary>
        private int m_intMaxHeight = 0;

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        [Description("边框颜色"), Category("自定义")]
        public Color BorderColor
        {
            get { return this.RectColor; }
            set
            {
                this.RectColor = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCPanelParent" /> class.
        /// </summary>
        public UCPanelParent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The bit down
        /// </summary>
        private Bitmap bitDown = null;

        /// <summary>
        /// The bit up
        /// </summary>
        private Bitmap bitUp = null;

        /// <summary>
        /// Gets the img.
        /// </summary>
        /// <param name="blnRefresh">if set to <c>true</c> [BLN refresh].</param>
        /// <returns>Bitmap.</returns>
        private Bitmap GetImg(bool blnRefresh = false)
        {
            if (bitUp == null || blnRefresh)
            {
                bitUp = new Bitmap(24, 24);
                Graphics g = Graphics.FromImage(bitUp);
                g.SetGDIHigh();
                GraphicsPath path = new GraphicsPath();
                path.AddLine(3, 19, 21, 19);
                path.AddLine(21, 19, 12, 5);
                path.AddLine(12, 5, 3, 19);
                g.FillPath(new SolidBrush(ForeColor), path);
                g.Dispose();
            }
            return bitUp;
        }

        private void ucBtnImg1_BtnClick(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
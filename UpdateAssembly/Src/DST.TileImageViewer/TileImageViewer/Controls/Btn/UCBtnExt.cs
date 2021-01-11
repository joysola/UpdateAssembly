// ***********************************************************************
// Assembly         : TileImageViewer
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="UCBtnExt.cs">
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
using System.Windows.Forms;

namespace TileImageViewer.Controls
{
    /// <summary>
    /// Class UCBtnExt.
    /// Implements the <see cref="TileImageViewer.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="TileImageViewer.Controls.UCControlBase" />
    [DefaultEvent("BtnClick")]
    public partial class UCBtnExt : UCControlBase
    {
        #region 字段属性

        private bool enabledMouseEffect = true;

        [Description("是否启用鼠标效果"), Category("自定义")]
        public bool EnabledMouseEffect
        {
            get { return enabledMouseEffect; }
            set { enabledMouseEffect = value; }
        }

        /// <summary>
        /// 是否显示角标
        /// </summary>
        /// <value><c>true</c> if this instance is show tips; otherwise, <c>false</c>.</value>
        [Description("是否显示角标"), Category("自定义")]
        public bool IsShowTips
        {
            get
            {
                return this.lblTips.Visible;
            }
            set
            {
                this.lblTips.Visible = value;
            }
        }

        /// <summary>
        /// 角标文字
        /// </summary>
        /// <value>The tips text.</value>
        [Description("角标文字"), Category("自定义")]
        public string TipsText
        {
            get
            {
                return this.lblTips.Text;
            }
            set
            {
                this.lblTips.Text = value;
            }
        }

        /// <summary>
        /// The BTN back color
        /// </summary>
        private Color _btnBackColor = Color.White;

        /// <summary>
        /// 按钮背景色
        /// </summary>
        /// <value>The color of the BTN back.</value>
        [Description("按钮背景色"), Category("自定义")]
        public Color BtnBackColor
        {
            get { return _btnBackColor; }
            set
            {
                _btnBackColor = value;
                this.BackColor = value;
            }
        }

        /// <summary>
        /// The BTN fore color
        /// </summary>
        private Color _btnForeColor = Color.White;

        /// <summary>
        /// 按钮字体颜色
        /// </summary>
        /// <value>The color of the BTN fore.</value>
        [Description("按钮字体颜色"), Category("自定义")]
        public virtual Color BtnForeColor
        {
            get { return _btnForeColor; }
            set
            {
                _btnForeColor = value;
                this.lbl.ForeColor = value;
            }
        }

        /// <summary>
        /// The BTN font
        /// </summary>
        private Font _btnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        /// <summary>
        /// 按钮字体
        /// </summary>
        /// <value>The BTN font.</value>
        [Description("按钮字体"), Category("自定义")]
        public Font BtnFont
        {
            get { return _btnFont; }
            set
            {
                _btnFont = value;
                this.lbl.Font = value;
            }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        [Description("按钮点击事件"), Category("自定义")]
        public event EventHandler BtnClick;

        /// <summary>
        /// The BTN text
        /// </summary>
        private string _btnText;

        /// <summary>
        /// 按钮文字
        /// </summary>
        /// <value>The BTN text.</value>
        [Description("按钮文字"), Category("自定义")]
        public virtual string BtnText
        {
            get { return _btnText; }
            set
            {
                _btnText = value;
                lbl.Text = value;
            }
        }

        /// <summary>
        /// The m tips color
        /// </summary>
        private Color m_tipsColor = Color.FromArgb(53, 109, 137);

        /// <summary>
        /// 角标颜色
        /// </summary>
        /// <value>The color of the tips.</value>
        [Description("角标颜色"), Category("自定义")]
        public Color TipsColor
        {
            get { return m_tipsColor; }
            set { m_tipsColor = value; }
        }

        [Description("鼠标效果生效时发生，需要和MouseEffected同时使用，否则无效"), Category("自定义")]
        public event EventHandler MouseEffecting;

        [Description("鼠标效果结束时发生，需要和MouseEffecting同时使用，否则无效"), Category("自定义")]
        public event EventHandler MouseEffected;

        #endregion 字段属性

        /// <summary>
        /// Initializes a new instance of the <see cref="UCBtnExt" /> class.
        /// </summary>
        public UCBtnExt()
        {
            InitializeComponent();
            this.TabStop = false;

            toolTip1.OwnerDraw = true;
            toolTip1.UseAnimation = false;
            toolTip1.UseFading = false;
            toolTip1.BackColor = this.m_tipsColor;
            toolTip1.ForeColor = lblTips.ForeColor;

            toolTip1.Draw += toolTip1_Draw;
            toolTip1.Popup += toolTip1_Popup;

            this.lbl.MouseEnter += lbl_MouseEnter;
            this.lbl.MouseLeave += lbl_MouseLeave;
        }

        public enum TipPositionEnum
        {
            Top, Bottom
        }

        private TipPositionEnum tipPosition = TipPositionEnum.Bottom;

        [Description("角标位置"), Category("自定义")]
        public TipPositionEnum TipPosition
        {
            get { return tipPosition; }
            set { tipPosition = value; }
        }

        private Color m_cacheColor = Color.Empty;

        private void lbl_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Active = false;
            toolTip1.RemoveAll();
        }

        private void lbl_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Active = true;
            int currentTipPosition = this.lbl.Height + 10;
            if (this.TipPosition == TipPositionEnum.Top)
            {
                currentTipPosition = -this.lbl.Height - 10;
            }
            toolTip1.Show(TipsText, this, 10, currentTipPosition);
        }

        /// <summary>
        /// Handles the Paint event of the lblTips control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        private void lblTips_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.SetGDIHigh();
            //e.Graphics.FillEllipse(new SolidBrush(m_tipsColor), new Rectangle(0, 0, lblTips.Width - 1, lblTips.Height - 1));
            //System.Drawing.SizeF sizeEnd = e.Graphics.MeasureString(TipsText, lblTips.Font);

            //e.Graphics.DrawString(TipsText, lblTips.Font, new SolidBrush(lblTips.ForeColor), new PointF((lblTips.Width - sizeEnd.Width) / 2, (lblTips.Height - sizeEnd.Height) / 2 + 1));
        }

        /// <summary>
        /// Handles the MouseDown event of the lbl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.BtnClick != null)
                BtnClick(this, e);
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            SizeF sizeEnd = TextRenderer.MeasureText(TipsText, lblTips.Font);
            e.ToolTipSize = new Size(sizeEnd.Width.ToInt(), sizeEnd.Height.ToInt() + 10);
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            //e.DrawBorder();
            e.DrawText();
        }
    }
}
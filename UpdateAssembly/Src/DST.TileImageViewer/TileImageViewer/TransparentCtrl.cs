using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TileImageViewer.Controls;

namespace TileImageViewer
{
    public partial class TransparentCtrl : UCPanelParent
    {
        private string panelTitle = "";

        public TransparentCtrl()
        {
            InitializeComponent();
        }

        private int _TransparentValue = 200;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("透明度"), Category("自定义")]
        public int TransparentValue
        {
            get { return this._TransparentValue; }
            set { this._TransparentValue = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("容器标题"), Category("自定义")]
        public string Title
        {
            get { return this.panelTitle; }
            set { this.panelTitle = value; }
        }

        /// <summary>
        /// The transparent control fill color
        /// </summary>
        private Color _TransParentColor = Color.FromArgb(0, 0, 0);

        /// <summary>
        /// 透明容器填充颜色
        /// </summary>
        /// <value>The color of the transparent control.</value>
        [Description("透明容器填充颜色"), Category("自定义")]
        public Color TransParentColor
        {
            get { return _TransParentColor; }
            set { _TransParentColor = value; }
        }

        /**
         * 当设置title时，设置panel的自动宽度
         **/

        public void setAutoWidth(String drawString)
        {
            Font drawFont = new Font(this.Font.FontFamily, this.Font.Size);
            // 获取标签名称宽度所占像素
            Graphics graphics = CreateGraphics();
            SizeF sizeF = graphics.MeasureString(drawString, drawFont);
            int labelWidth = sizeF.Width.ToInt() + 10;
            this.Width = labelWidth;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //获取画布的绘制指针
            Graphics g = e.Graphics;

            //在前面的矩形中绘制字符串
            string drawString = this.panelTitle;
            //创建字符串的绘制字体和画刷
            Font drawFont = new Font(this.Font.FontFamily, this.Font.Size);
            SolidBrush drawBrush = new SolidBrush(this.ForeColor);
            // 获取标签名称宽度所占像素
            SizeF sizeF = g.MeasureString(drawString, drawFont);
            int labelWidth = sizeF.Width.ToInt() + 10;
            //绘制一个矩形
            int leftWidth = this.Size.Width;

            Rectangle rc = new Rectangle(0, 0, this.Size.Width, this.Size.Height);
            g.DrawRectangle(new Pen(Color.Transparent), rc);
            //利用带透明度的画刷填充矩形
            SolidBrush sb = new SolidBrush(Color.FromArgb(this.TransparentValue, this.TransParentColor));
            g.FillRectangle(sb, rc);

            //设置字符串格式
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            //在矩形中绘制字符串，并且使用drawFormat的性质
            g.DrawString(drawString, drawFont, drawBrush, rc, drawFormat);
        }
    }
}
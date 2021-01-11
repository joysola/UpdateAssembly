using System;
using System.Drawing;
using System.Windows.Forms;
using TileImageViewer.Controls;

namespace TileImageViewer
{
    /// <summary>
    /// 放大镜控件
    /// </summary>
    public partial class MagCtrl : UCPanelParent
    {
        // imgctrl数据引用
        private ImgCtrlPage Page;

        //图像
        private Bitmap Image;

        //地图范围
        public MapRectangle MapR { get; set; }

        public delegate ImgCtrlPage ReloadImgCtrlPage();

        public ReloadImgCtrlPage ReloadPageDelegate;
        public float Scale { get; set; } = 1;
        public Point CurrentMouseAbsPoint { get; set; }

        public MagCtrl()
        {
            InitializeComponent();
            Constants.OnSettingChange += SettingChanged;
        }

        /// <summary>
        /// 覆盖Component的Dispose方法，释放非托管资源
        /// </summary>
        public new void Dispose()
        {
            // 解绑委托
            Constants.OnSettingChange -= SettingChanged;

            Image?.Dispose();
            Image = null;

            Page?.Dispose();
            Page = null;
            base.Dispose();
        }

        private void SettingChanged()
        {
            ReloadImage();
        }

        private void Reload()
        {
            if (ReloadPageDelegate != null)
            {
                Page = ReloadPageDelegate();
                ReloadImage();
            }
        }

        private void MagCtrl_Load(object sender, EventArgs e)
        {
            Reload();
        }

        public void ReloadImage()
        {
            Point absPoint = CurrentAbsPoint();

            if (absPoint.IsEmpty)
            {
                return;
            }

            int level = 0;
            if (Constants.SettingDetail.EnlargeSelect == 1)
            {
                level = Page.CurrLevel +
                     Convert.ToInt16(Constants.SettingDetail.EnlargeRelationVlaue);
            }
            else
            {
                level = Convert.ToInt16(Constants.SettingDetail.EnlargeAbsoluteVlaue);
            }

            level = Math.Min(level, Page.ScanPage.MaxLevel);
            //MapR = new MapRectangle(Page.ScanPage.GetColsRows(Page.CurrLevel + 1));
            MapRectangle MapRS = ImgCtrlTransHelper.AbsPointIn(Page.ScanPage, level, absPoint);

            MapRS.ColStart = Math.Max(0, MapRS.ColStart - 2);
            MapRS.RowStart = Math.Max(0, MapRS.RowStart - 2);
            MapRS.ColEnd = Math.Min(MapRS.ColEnd + 2, MapRS.ColMaxEnd);
            MapRS.RowEnd = Math.Min(MapRS.RowEnd + 2, MapRS.RowMaxEnd);

            if ((MapR == null || MapR.GetHashCode() != MapRS.GetHashCode()) && MapRS.IsValid())
            {
                MapR = MapRS;
                //重新拼接图像
                Image = ImgHelper.JoinImage(Page.ScanPage.BaseFilePath, Page.ScanPage.GetSampleName(), MapR);
                if (Image == null)
                {
                    return;
                }

                //绘制标注
                foreach (Annotation an in Page.ScanPage.Annotations)
                {
                    if (an.BShow)
                    {
                        Point p0 = this.MagCtrlPoint(an.Graph.Point0, Page.Degree);
                        Point p1 = this.MagCtrlPoint(an.Graph.Point1, Page.Degree);
                        Point p2 = this.MagCtrlPoint(an.Graph.Point2, Page.Degree);
                        Point p3 = this.MagCtrlPoint(an.Graph.Point3, Page.Degree);

                        an.Graph.Draw(Graphics.FromImage(Image), p0, p1, p2, p3, new Point(0, 0));
                    }
                }
            }

            Invalidate();
        }

        /// <summary>
        /// 根据绝对坐标转换放大镜组件的控件坐标
        /// </summary>
        /// <param name="absPoint">绝对坐标</param>
        /// <param name="degreeA">角度</param>
        /// <returns>控件坐标</returns>
        private Point MagCtrlPoint(Point absPoint, float degreeA)
        {
            ScanPageLevel currLCR = Page.ScanPage.GetColsRows(MapR.Level);
            ScanPageLevel maxLCR = Page.ScanPage.GetColsRows(Page.ScanPage.MaxLevel);
            Point p = ImgCtrlTransHelper.ImgPoint(currLCR, maxLCR, MapR, absPoint, degreeA, Scale);

            Point ret0 = new Point(Convert.ToInt32(p.X),
                Convert.ToInt32(p.Y));

            return ret0;
        }

        /// <summary>
        /// 双击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MagCtrl_DoubleClick(object sender, EventArgs e)
        {
            Point p = new Point(Location.X + Size.Width / 2, Location.Y + Size.Height / 2);
            Point absPoint = CurrentAbsPoint();
            Point p2 = CenterImgPoint();
        }

        /// <summary>
        /// 获得当前视野控件组件中心点的绝对坐标
        /// </summary>
        /// <returns></returns>
        private Point CurrentAbsPoint()
        {
            if (Page == null)
            {
                return new Point(0, 0);
            }

            Point p = new Point(Location.X + Size.Width / 2, Location.Y + Size.Height / 2);
            Point absPoint = Point.Empty;
            if (!this.CurrentMouseAbsPoint.IsEmpty)
            {
                absPoint = this.CurrentMouseAbsPoint;
            }
            else
            {
                absPoint = Page.AbsPoint(p, Page.Degree);
            }
            return absPoint;
        }

        private Point CenterImgPoint()
        {
            if (Page == null)
            {
                return new Point(0, 0);
            }

            ScanPageLevel currLCR = Page.ScanPage.GetColsRows(MapR.Level);
            ScanPageLevel maxLCR = Page.ScanPage.GetColsRows(Page.ScanPage.MaxLevel);
            Point absPoint = CurrentAbsPoint();

            Point p = ImgCtrlTransHelper.ImgPoint(currLCR, maxLCR, MapR, absPoint, 0, 1);
            return p;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graphics = pe.Graphics;
            Rectangle rc = new Rectangle(1, 1, this.Size.Width - 2, this.Size.Height - 2);
            Rectangle ctrlRect = rc;

            if (MapR != null && MapR.IsValid() && Image != null)
            {
                ScanPageLevel currLCR = Page.ScanPage.GetColsRows(MapR.Level);
                ScanPageLevel maxLCR = Page.ScanPage.GetColsRows(Page.ScanPage.MaxLevel);
                Point absPoint = CurrentAbsPoint();
                Point p = CenterImgPoint();
                Rectangle imgRect = new Rectangle(p.X - this.Width / 2, p.Y - this.Height / 2,
                    Convert.ToInt16(rc.Width),
                    Convert.ToInt16(rc.Height));

                graphics.DrawImage(Image, ctrlRect, imgRect, GraphicsUnit.Pixel);
            }

            base.OnPaint(pe);
        }

        private void MagCtrl_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void MagCtrl_Move(object sender, EventArgs e)
        {
            Reload();
            Invalidate();
        }

        private void MagCtrl_VisibleChanged(object sender, EventArgs e)
        {
            Reload();
            Invalidate();
        }
    }
}
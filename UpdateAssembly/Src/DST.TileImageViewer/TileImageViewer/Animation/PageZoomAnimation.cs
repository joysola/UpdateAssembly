using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace TileImageViewer.Animation
{
    /// <summary>
    /// 放大缩小动画
    /// </summary>
    internal class PageZoomAnimation : Animate
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 起始页面模型
        /// </summary>
        public ImgCtrlPage FromPage { get; set; }

        /// <summary>
        /// 结束页面模型
        /// </summary>
        public ImgCtrlPage ToPage { get; set; }

        /// <summary>
        /// 起始页面图片宽度（为了加快速度不用每次都计算）
        /// </summary>
        public int FromPageBitmapWidth { get; set; }

        /// <summary>
        /// 动画时常
        /// </summary>
        private int AnimateLength = 500;

        private Thread loadImgThread;
        private Task loadImgTask;
        private Bitmap preLoadedImg;

        private Rectangle ctrlRect;
        private Rectangle imgRect;

        private delegate void UpdateUI();

        /// <summary>
        /// 必须处理掉bitmap的资源
        /// </summary>
        public override void Dispose()
        {
            this.FromPage?.Dispose();
            this.FromPage = null;

            this.ToPage?.Dispose();
            this.ToPage = null;

            this.preLoadedImg?.Dispose();
            this.preLoadedImg = null;
        }

        public void ResetToPage(ImgCtrlPage toPage, ImgCtrl imgCtrl)
        {
            ToPage.CurrLevel = toPage.CurrLevel;
            AnimateLength = 500;
            PreLoadImg(imgCtrl);
        }

        public PageZoomAnimation(int bitmapWidth, ImgCtrlPage fromPage, ImgCtrlPage toPage)
        {
            FromPageBitmapWidth = bitmapWidth;
            FromPage = fromPage.Clone();
            ToPage = toPage.Clone();
            logger.Debug("=========================== PageZoomAnimation Start :" + FromPage.CurrLevel + " To " + ToPage.CurrLevel + " At " + CenterAbsPoint.ToString());
            if (ToPage.CurrLevel >= 5)
            {
                AnimateLength = AnimateLength + Convert.ToInt16((ToPage.CurrLevel - 5) * 0.1);
            }
        }

        /// <summary>
        /// 计算某一时间点图像的宽度
        /// </summary>
        /// <param name="timestamp">时间节点</param>
        /// <returns>宽度</returns>
        private double WidthAt(long timestamp)
        {
            double fromWidth = FromPage.ScanPage.GetColsRows(FromPage.CurrLevel).EndCol * Constants.PicW * FromPage.Scale;
            double toWidth = ToPage.ScanPage.GetColsRows(ToPage.CurrLevel).EndCol * Constants.PicW * ToPage.Scale;

            double fromWidth1 = FromPage.MapRectangle.Width(FromPage.Degree) * FromPage.Scale;
            //toWidth = ToPage.MapRectangle.Width(ToPage.Degree);

            //匀速
            double fullwidthspan = ((toWidth - fromWidth) / AnimateLength) * timestamp;
            double cntWidth = fromWidth1 + fullwidthspan * (fromWidth1 / fromWidth);
            return cntWidth;
        }

        /// <summary>
        /// 图片预加载
        /// </summary>
        /// <param name="imgCtrl"></param>
        private void PreLoadImg(ImgCtrl imgCtrl)
        {
            logger.Debug("PageZoomAnimation PreLoadImg Start");

            CImgTag tag = imgCtrl.GetImgTag();
            int degree = Convert.ToInt32(tag.Degree);
            Point ctrlPoint = FromPage.ImgCtrlPoint(CenterAbsPoint, tag.Degree);

            ToPage.MovePage(ctrlPoint, CenterAbsPoint);
            ToPage.Refresh(ctrlPoint, imgCtrl.ClientSize, degree);
            // 需要计算两次
            ToPage.Refresh(ctrlPoint, imgCtrl.ClientSize, degree);
            preLoadedImg = ToPage.JoinImage(tag);
            if (preLoadedImg != null)
            {
                CImgTag preloadImgTag = (CImgTag)preLoadedImg.Tag;
                if (tag.Degree != preloadImgTag.Degree)
                {
                    preLoadedImg = ImgHelper.GetRotateImage(preLoadedImg, Convert.ToInt16(tag.Degree));
                    preloadImgTag.Degree = tag.Degree;
                }
            }

            logger.Debug("PageZoomAnimation PreLoadImg Done");
        }

        public override void ExecStartWork(ImgCtrl imgCtrl)
        {
            logger.Debug("ExecStartWork ...........");
            //异步加载图像
            //loadImgThread = new Thread(() => PreLoadImg(imgCtrl));
            //loadImgThread.Start();
            loadImgTask = Task.Run(() => { PreLoadImg(imgCtrl); });
            //loadImgTask.Start();

            //imgCtrl.LoadImg(imgCtrl.Page, imgCtrl.Page.ScaledBaseBitmap);
            //LoadFakeFromBitmap(imgCtrl);
        }

        public override void ExecAnimateAt(long timestamp, ImgCtrl imgCtrl)
        {
            if (StartAt == 0)
            {
                StartAt = timestamp;
                ExecStartWork(imgCtrl);
                return;
            }

            timestamp = timestamp - StartAt;

            if (timestamp > AnimateLength
                || Constants.SettingDetail.SmoothSlideNavigationSwitch != 1)
            {
                ExecFinishWork(imgCtrl);
                this.Finished = true;
                return;
            }

            logger.Debug("ExecAnimateAt start");
            double zoomScale = WidthAt(timestamp) / FromPageBitmapWidth;
            CImgTag tag = (CImgTag)imgCtrl.Image.Tag;
            Point fromCtrlPoint = FromPage.ImgCtrlPoint(CenterAbsPoint, tag.Degree);
            Point toCtrlPoint = FromPage.ImgCtrlPoint(CenterAbsPoint, tag.Degree, zoomScale);

            // Level 7 to level 8 using level 6 to scale
            Point offsetPt = new Point(fromCtrlPoint.X - toCtrlPoint.X, fromCtrlPoint.Y - toCtrlPoint.Y);
            Rectangle rc = new Rectangle();

            if (FromPage.CurrLevel < ToPage.CurrLevel)
            {
                // 放大
                rc = new Rectangle(-FromPage.ImgOffset.X, -FromPage.ImgOffset.Y,
              ToPage.PageWidthInt()
              - Math.Min(0, ToPage.ImgOffset.X - ToPage.MapRectangle.ColStart * Constants.PicW),
             ToPage.PageHeightInt()
              - Math.Min(0, ToPage.ImgOffset.Y - ToPage.MapRectangle.RowStart * Constants.PicH));
            }
            else
            {
                //缩小
                rc = new Rectangle(-FromPage.ImgOffset.X, -FromPage.ImgOffset.Y,
              FromPage.PageWidthInt()
              - Math.Min(0, FromPage.ImgOffset.X - FromPage.MapRectangle.ColStart * Constants.PicW),
              FromPage.PageHeightInt()
              - Math.Min(0, FromPage.ImgOffset.Y - FromPage.MapRectangle.RowStart * Constants.PicH));
            }

            rc.Offset(offsetPt);

            double riw = rc.Width / zoomScale;
            double rih = rc.Height / zoomScale;

            double riscale_w = riw / rc.Width;
            double riscale_h = rih / rc.Height;

            Rectangle ri = new Rectangle(Convert.ToInt32(-rc.X / zoomScale),
                Convert.ToInt32(-rc.Y / zoomScale),
               Convert.ToInt32(riscale_w * imgCtrl.Width)
               , Convert.ToInt32(riscale_h * imgCtrl.Height)
               );
            rc = new Rectangle(0, 0, imgCtrl.Width, imgCtrl.Height);

            imgCtrl.ctrlRect = rc;
            imgCtrl.imgRect = ri;

            ctrlRect = imgCtrl.ctrlRect;
            imgRect = imgCtrl.imgRect;

            logger.Debug("ExecStartWork ........... ctrlRect:" + ctrlRect.ToString() + " imgRect:" + imgRect.ToString());

            imgCtrl.Invalidate();
        }

        public void LoadFakeFromBitmap(ImgCtrl imgCtrl)
        {
            CImgTag tag = (CImgTag)imgCtrl.Image.Tag;

            Point ctrlPoint = FromPage.ImgCtrlPoint(CenterAbsPoint, tag.Degree);
            imgCtrl.imgRect = new Rectangle();

            Bitmap fullPageImg = null;
            imgCtrl.Page.BitmapBuffer.TryGetValue(Constants.MemoryCachedMinLevel, out fullPageImg);
            if (fullPageImg != null)
            {
                imgCtrl.LoadImg(FromPage, fullPageImg);
                imgCtrl.Page.MovePage(ctrlPoint, CenterAbsPoint);
            }
            imgCtrl.Invalidate();
        }

        /// <summary>
        /// （重写改为异步方法）
        /// </summary>
        /// <param name="imgCtrl"></param>
        public override async void ExecFinishWork(ImgCtrl imgCtrl)
        {
            CImgTag tag = (CImgTag)imgCtrl.Image.Tag;

            Point ctrlPoint = FromPage.ImgCtrlPoint(CenterAbsPoint, tag.Degree);
            //等待之前的异步加载完成
            //loadImgThread.Join();
            await loadImgTask;
            //回调
            UpdateUI update = delegate
            {
                imgCtrl.LoadImg(ToPage, preLoadedImg, ctrlRect, imgRect,
                      (Bitmap)imgCtrl.Image.Clone());
            };
            imgCtrl.Invoke(update);

            imgCtrl.imgRect = new Rectangle();
            //调取目标图像后，目标图像归位
            ToPage.MovePage(CenterCtrlPoint, CenterAbsPoint);

            logger.Debug("=========================== ExecFinishWork at :" + CenterAbsPoint.ToString() + CenterCtrlPoint.ToString());
        }
    }
}
using DST.Common.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileImageViewer.Controls;
using TileImageViewer.Model;

namespace TileImageViewer
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class FrmMain : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private FrmSettings frmSettings;

        // 放大镜模式切换开关
        private bool magnifierSwith = true;

        private string sampleName = "";

        // 窗体距离顶部菜单的高度
        private int windowToTop = 50;

        private ImgCtrlPage ReloadPage()
        {
            return imgBox.Page;
        }

        private ImgCtrlPage floorPage;
        private Bitmap mapPageClearBitmap;

        private string startPath;

        public FrmMain(string s, Size initSize)
        {
            InitializeComponent();
            Constants.OnSettingChange += SettingChanged;
            Constants.OnColorChange += ColorChanged;
            AutoResize();
            startPath = s;
            lblDgTxt.Hide();
            start(initSize);
            magCtrl1.ReloadPageDelegate = new MagCtrl.ReloadImgCtrlPage(ReloadPage);
            StartPosition = FormStartPosition.CenterScreen;
            ImgHelper.InitPicDict();
            SetLanguage();
        }

        /// <summary>
        /// 覆盖Component的Dispose方法，手动释放该控件非托管资源
        /// </summary>
        public new void Dispose()
        {
            Constants.OnSettingChange -= SettingChanged;
            Constants.OnColorChange -= ColorChanged;

            mapPageClearBitmap?.Dispose();
            mapPageClearBitmap = null;

            this.floorPage.Dispose();
            this.floorPage = null;

            // 获取所有UCBtnImg控件并释放
            var ucBtnImgList = this.GetControls<UCBtnImg>();
            foreach (var item in ucBtnImgList)
            {
                item.Dispose();
            }
            // 获取所有PictureBox并释放
            var picboxs = this.GetControls<PictureBox>();
            foreach (var item in picboxs)
            {
                item.Dispose();
            }

            // 获取MagCtrl并释放
            var magCtrls = this.GetControls<MagCtrl>();
            foreach (var item in magCtrls)
            {
                item.Dispose();
            }

            this.imgBox.NeedPic -= this.imgBox_NeedPic;
            this.imgBox.ChangeTips -= this.imgBox_ChangeTips;

            this.imgBox?.Dispose();
            this.imgBox = null;

            #region 旧处理

            //this.saveSnapShotBtn.Dispose();
            //this.saveSnapShotBtn = null;
            //this.snapShotCloseBtn.Dispose();
            //this.snapShotCloseBtn = null;
            //this.toFolderSelectBtn.Dispose();
            //this.toFolderSelectBtn = null;

            //this.userGuideBtn.Dispose();
            //this.userGuideBtn = null;
            //this.aboutUsBtn.Dispose();
            //this.aboutUsBtn = null;
            //this.touchpadGestureBtn.Dispose();
            //this.touchpadGestureBtn = null;
            //this.rotateSave.Dispose();
            //this.rotateSave = null;
            //this.rotateVerBtn.Dispose();
            //this.rotateVerBtn = null;
            //this.rotateHorBtn.Dispose();
            //this.rotateHorBtn = null;
            //this.rotateResetBtn.Dispose();
            //this.rotateResetBtn = null;
            //this.TMAToolStripButton.Dispose();
            //this.TMAToolStripButton = null;

            //this.AnnoListStripButton.Dispose();
            //this.AnnoListStripButton = null;
            //this.arrowToolStripButton.Dispose();
            //this.arrowToolStripButton = null;
            //this.fixCircleToolStripButton.Dispose();
            //this.fixCircleToolStripButton = null;
            //this.fixRectangleToolStripButton.Dispose();
            //this.fixRectangleToolStripButton = null;
            //this.lineToolStripButton.Dispose();
            //this.lineToolStripButton = null;
            //this.circleToolStripButton.Dispose();
            //this.circleToolStripButton = null;
            //this.rectangleToolStripButton.Dispose();
            //this.rectangleToolStripButton = null;

            //this.ucBtnPixelInfo.Dispose();
            //this.ucBtnPixelInfo = null;
            //this.ucBtnSampleInfo.Dispose();
            //this.ucBtnSampleInfo = null;
            //this.showMapTraceBtn.Dispose();
            //this.showMapTraceBtn = null;

            ////this.tSBScaleTo = new TileImageViewer.Controls.UCBtnExt();
            //this.ucBtnImg1.Dispose();
            //this.ucBtnImg1 = null;
            //this.mapbox.Dispose(); // picturebox
            //this.mapbox = null; // picturebox
            //this.barcodeBox.Dispose();//   picturebox
            //this.barcodeBox = null;//   picturebox
            //this.barcodeRotateBtn.Dispose();
            //this.barcodeRotateBtn = null;
            //this.magCtrl1.Dispose();// MagCtrl
            //this.magCtrl1 = null;// MagCtrl
            //this.ucBtnTuding.Dispose(); ;
            //this.ucBtnTuding = null;

            //this.tsbSpRealPixel.Dispose();
            //this.tsbSpRealPixel = null;
            //this.tsbSpFullScreen.Dispose();
            //this.tsbSpFullScreen = null;
            //this.tsbSp40XBtn.Dispose();
            //this.tsbSp40XBtn = null;
            //this.tsbSp20XBtn.Dispose();
            //this.tsbSp20XBtn = null;
            //this.tsbHelp.Dispose();
            //this.tsbHelp = null;

            //this.tsbSettingsBtn.Dispose();
            //this.tsbSettingsBtn = null;
            //this.tsbColorCorrecionBtn.Dispose();
            //this.tsbColorCorrecionBtn = null;

            //this.ucBtnImg15.Dispose();
            //this.ucBtnImg15 = null;
            //this.tsbSp10XBtn.Dispose();
            //this.tsbSp10XBtn = null;
            //this.tsbSp5XBtn.Dispose();
            //this.tsbSp5XBtn = null;
            //this.tsbSp2XBtn.Dispose();
            //this.tsbSp2XBtn = null;
            //this.tsbSp1XBtn.Dispose();
            //this.tsbSp1XBtn = null;

            //this.MeasureToolStripButton.Dispose();
            //this.MeasureToolStripButton = null;
            //this.tsbSpShotBtn.Dispose();
            //this.tsbSpShotBtn = null;
            //this.tsbRotate.Dispose();
            //this.tsbRotate = null;
            //this.annotationButton.Dispose();
            //this.annotationButton = null;
            //this.tsbMouseSwitchBtn.Dispose();
            //this.tsbMouseSwitchBtn = null;

            //this.tsbSpBarcodeBtn.Dispose();
            //this.tsbSpBarcodeBtn = null;
            //this.tsbSpZoomBtn.Dispose();
            //this.tsbSpZoomBtn = null;
            //this.tsbSpMapBtn.Dispose();
            //this.tsbSpMapBtn = null;
            //this.tsbSpOpBtn.Dispose();
            //this.tsbSpOpBtn = null;

            #endregion 旧处理

            ImgHelper.ClearData();

            base.Dispose();
            GC.Collect();

            //GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 多语言
        /// </summary>
        private void SetLanguage()
        {
            this.Text = GlobalData.GlobalLanguage.Text("App_Name");
            this.saveSnapShotBtn.TipsText = GlobalData.GlobalLanguage.Text("Save_snapshot");
            this.snapShotCloseBtn.TipsText = GlobalData.GlobalLanguage.Text("close");
            this.toFolderSelectBtn.TipsText = GlobalData.GlobalLanguage.Text("Open_startup_screen");
            this.userGuideBtn.TipsText = GlobalData.GlobalLanguage.Text("Help_documents");
            this.aboutUsBtn.TipsText = GlobalData.GlobalLanguage.Text("about");
            this.touchpadGestureBtn.TipsText = GlobalData.GlobalLanguage.Text("Gesture_rules_of_touchpad");
            this.rotateSave.TipsText = GlobalData.GlobalLanguage.Text("preservation");
            this.rotateVerBtn.TipsText = GlobalData.GlobalLanguage.Text("Flip_vertically");
            this.rotateHorBtn.TipsText = GlobalData.GlobalLanguage.Text("Flip_horizontally");
            this.rotateResetBtn.TipsText = GlobalData.GlobalLanguage.Text("Reset");
            this.AnnoListStripButton.TipsText = GlobalData.GlobalLanguage.Text("Notes_list");
            this.arrowToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Insert_arrow");
            this.fixCircleToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Fixed_size_circle");
            this.fixRectangleToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Fixed_size_rectangle");
            this.lineToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Linear_measurement_note");
            this.circleToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Adjustable_size_circle");
            this.rectangleToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Adjustable_rectangle");
            this.ucBtnPixelInfo.TipsText = GlobalData.GlobalLanguage.Text("Pixel_information");
            this.ucBtnSampleInfo.TipsText = GlobalData.GlobalLanguage.Text("Slide_description");
            this.showMapTraceBtn.TipsText = GlobalData.GlobalLanguage.Text("Browse_history");
            this.ucBtnImg1.TipsText = GlobalData.GlobalLanguage.Text("close");
            this.tsbSpRealPixel.TipsText = GlobalData.GlobalLanguage.Text("Actual_pixel_browsing");
            this.tsbSpFullScreen.TipsText = GlobalData.GlobalLanguage.Text("Section_panorama");
            this.tsbSp40XBtn.TipsText = GlobalData.GlobalLanguage.Text("zoom_to") + " 40x";
            this.tsbSp20XBtn.TipsText = GlobalData.GlobalLanguage.Text("zoom_to") + " 20x";

            this.tsbHelp.TipsText = GlobalData.GlobalLanguage.Text("help");
            this.tsbSettingsBtn.TipsText = GlobalData.GlobalLanguage.Text("set_up");
            this.tsbColorCorrecionBtn.TipsText = GlobalData.GlobalLanguage.Text("Color_correction");
            this.tsbSp10XBtn.TipsText = GlobalData.GlobalLanguage.Text("zoom_to") + " 10x";
            this.tsbSp5XBtn.TipsText = GlobalData.GlobalLanguage.Text("zoom_to") + " 5x";
            this.tsbSp2XBtn.TipsText = GlobalData.GlobalLanguage.Text("zoom_to") + " 2x";
            this.tsbSp1XBtn.TipsText = GlobalData.GlobalLanguage.Text("zoom_to") + " 1x";

            this.MeasureToolStripButton.TipsText = GlobalData.GlobalLanguage.Text("Measuring_distance");
            this.tsbSpShotBtn.TipsText = GlobalData.GlobalLanguage.Text("Screenshot");
            this.tsbRotate.TipsText = GlobalData.GlobalLanguage.Text("Rotate_Flip");
            this.annotationButton.TipsText = GlobalData.GlobalLanguage.Text("notes");
            this.tsbMouseSwitchBtn.TipsText = GlobalData.GlobalLanguage.Text("Selection_control");
            this.tsbSpBarcodeBtn.TipsText = GlobalData.GlobalLanguage.Text("Label_area");
            this.tsbSpZoomBtn.TipsText = GlobalData.GlobalLanguage.Text("Magnified_length");
            this.tsbSpMapBtn.TipsText = GlobalData.GlobalLanguage.Text("Slide_overview");
            this.tsbSpOpBtn.TipsText = GlobalData.GlobalLanguage.Text("Slide_operation");
        }

        /// <summary>
        /// 初始化imgbox
        /// </summary>
        /// <param name="initSize"></param>
        private void start(Size initSize)
        {
            Constants.LoadColorInfo(startPath);

            imgBox.Page = new ImgCtrlPage(startPath);
            Point p = new Point(Convert.ToInt32(imgBox.Page.MapRectangle.Width(0)) / 2 - initSize.Width / 2
                , Convert.ToInt32(imgBox.Page.MapRectangle.Height(0)) / 2 - initSize.Height / 2);

            imgBox.Page.ImgOffset = p;
            //imgBox.loadSettingInfo();
            //imgBox.Page.LoadScaledBitmap();
            if (Constants.SettingDetail.MemCachedSwitch == 1)
            {
                imgBox.InitFullPageBitmapBuffer();
            }

            //首次加载样本的第三层，不需要切割，故加载全部图像
            imgBox.LoadFullLevelImg();

            //加载标注
            imgBox.LoadAnnotations();
            if (imgBox.Page.ScanPage.DbState > 0)
                this.annotationButton.Enabled = false;
            //加载鹰眼图数据
            this.initPageMap();
            //加载样本二维码数据
            this.initBarcodeDialog();
            //设置标签名称
            this.setSampleName();
            this.setToScaleTxt(imgBox.Page.ImgCtrlScaleVal);

            //窗体中的控件对齐
            this.initPosition();

            //主窗体加载完成
            logger.Debug("Hello World");
        }

        #region 鹰眼图

        /// <summary>
        /// 初始化鹰眼图
        /// </summary>
        private void initPageMap()
        {
            floorPage = imgBox.Page.FloorPage();

            MapReload(true);
        }

        private long lastMapReloadTime = 0;
        /**
         * 重新绘制鹰眼图
         *
         */

        public void MapReload(bool isAdded)
        {
            CImgTag tag = null;
            if (mapbox.Image != null)
            {
                tag = (CImgTag)mapbox.Image.Tag;
            }
            bool forceReload = false;
            if (tag == null || tag.Degree != imgBox.Page.Degree)
            {
                forceReload = true;
            }

            if (panPageMap.Visible == false || forceReload)
            {
                //加快mapbox读取速度
                if (mapbox.Image == null || forceReload)
                {
                    Bitmap floorImage = floorPage.JoinImageByLevel();
                    mapbox.Image = ImgHelper.GetRotateImage(floorImage, Convert.ToInt16(imgBox.Page.Degree));
                    //mapbox.BackgroundImageLayout = ImageLayout.Stretch;
                    //mapbox.SizeMode = PictureBoxSizeMode.StretchImage;
                    mapbox.Size = mapbox.Image.Size;

                    mapPageClearBitmap = (Bitmap)mapbox.Image.Clone();
                    mapPageClearBitmap.Tag = mapbox.Image.Tag;
                }

                return;
            }

            long ss = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            if (ss - lastMapReloadTime < 100)
            {
                return;
            }
            lastMapReloadTime = ss;

            //logger.Debug("+++++++++++++++++++");
            mapbox.Image = (Bitmap)mapPageClearBitmap.Clone();
            mapbox.Image.Tag = mapPageClearBitmap.Tag;

            //画轨迹
            if (!PanPageMapTrackedPathList.IsEmpty() && PanPageMapTrackPath)
            {
                Graphics graphics = Graphics.FromImage((Bitmap)mapbox.Image);

                foreach (MAbsRectangle item in PanPageMapTrackedPathList)
                {
                    Point[] itemrect = floorPage.MRectOnAbs(item, true);
                    graphics.FillPolygon(new SolidBrush(Color.FromArgb(125, Color.LightGreen)), itemrect);
                }
                graphics.Dispose(); // joysola
            }

            float w = (float)(2);
            Pen pen = new Pen(Color.Yellow, w);
            //画外框

            floorPage.DrawMRectOnAbs((Bitmap)mapbox.Image, pen, imgBox.VisableAbsRect());
        }

        #endregion 鹰眼图

        #region 记录轨迹相关

        //
        private Boolean PanPageMapTrackPath = false;

        private LinkedList<MAbsRectangle> PanPageMapTrackedPathList = new LinkedList<MAbsRectangle>();

        /// <summary>
        /// 记录轨迹
        /// </summary>
        /// <returns>是否有新轨迹</returns>
        private bool RecordTrackPath()
        {
            bool add = true;
            if (PanPageMapTrackPath
                // for debug
                && imgBox.Page.CurrLevel > 3)
            {
                //记录轨迹

                MAbsRectangle imgres = imgBox.VisableAbsRect();

                foreach (MAbsRectangle item in PanPageMapTrackedPathList)
                {
                    if (ImgHelper.IsIntersect(item, imgres))
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    PanPageMapTrackedPathList.AddLast(imgres);
                }
            }

            return add;
        }

        #endregion 记录轨迹相关

        public delegate ImgCtrl GetImgCtrlDelegate();

        public ImgCtrl GetImgCtrl()
        {
            return this.imgBox;
        }

        #region 鼠标事件

        /// <summary>
        /// 鼠标滚轮事件，主要处理放大缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImgBox_MouseWheel(object sender, MouseEventArgs e)
        {
            //防止抖动
            if (imgBox.IsWorking())
            {
                return;
            }

            if (imgBox.Page == null || imgBox.Page.ScanPage == null)
            {
                return;
            }

            tsbZoomBtnSelect(0);
            // 反转鼠标缩放
            bool reverseMouseFlag = false;
            if (Constants.SettingDetail.ReverseMouseSwitch == 1)
            {
                if (e.Delta < 0)
                {
                    reverseMouseFlag = true;
                }
                else
                {
                    reverseMouseFlag = false;
                }
            }
            else
            {
                if (e.Delta < 0)
                {
                    reverseMouseFlag = false;
                }
                else
                {
                    reverseMouseFlag = true;
                }
            }
            if (!reverseMouseFlag)
            {
                ZoomInOut(imgBox.Page.CurrLevel + 1, 1, true);
            }
            else if (imgBox.Page.CurrLevel != 1)
            {
                //缩小
                ZoomInOut(imgBox.Page.CurrLevel - 1, 1, true);
            }
            //if (e.Delta < 0)
            //{
            //    ZoomInOut(imgBox.Page.CurrLevel + 1, 1, true);
            //}
            //else if (imgBox.Page.CurrLevel != 1)
            //{
            //    //缩小
            //    ZoomInOut(imgBox.Page.CurrLevel - 1, 1, true);

            //}
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var temp = imgBox.Image.Width; // 如果Image还没完成，则跳过
            }
            catch
            {
                return;
            }

            if (imgBox.Page == null || imgBox.Image == null)
            {
                return;
            }

            CImgTag tag = (CImgTag)this.imgBox.Image.Tag;

            //this.imgBox.WaitingForRender();

            MapRectangle visMapR = this.imgBox.Page.VisableMapRectangle(imgBox.Size);
            Point point = imgBox.PointToClient(Control.MousePosition);
            Point abPoint = imgBox.Page.AbsPoint(point, tag.Degree);
            Color color = imgBox.GetImgPixel(point);
            Point rePoint = imgBox.Page.ImgCtrlPoint(abPoint, tag.Degree);
            MapRectangle ctrlPointMap = imgBox.Page.AbsPointIn(abPoint);
            int degree = Convert.ToInt16(((CImgTag)imgBox.Image.Tag).Degree);

            double[] edgeDists = imgBox.Page.MaxEdgeDistance2(imgBox.Page.VisableMapRectangle(imgBox.Size), imgBox.Size, degree);
            ScanPageLevel pageLevel = imgBox.Page.GetCurretColsRows();

            //更新“像素信息”

            #region 旧方法处理 更新像素信息（不在UI线程上异步可能出问题）

            //Func<bool> slideCtrlUpdate = () => slideCtrl.Update(abPoint, imgBox.Page.Scale, ctrlPointMap, color);

            //slideCtrlUpdate.BeginInvoke((result) =>
            //{
            //    bool ret = slideCtrlUpdate.EndInvoke(result);
            //}, null);

            #endregion 旧方法处理 更新像素信息（不在UI线程上异步可能出问题）

            // UI线程异步更新UI元素
            slideCtrl.BeginInvoke((Func<Point, float, MapRectangle, Color, bool>)slideCtrl.Update, new object[] { abPoint, imgBox.Page.Scale, ctrlPointMap, color });

            if (imgBox.IsWorking())
            {
                return;
            }
            //刷新放大镜显示
            magRefresh(abPoint);
            // edgeDists数组长度必须为4，否则报错
            if (Constants.SettingDetail.ShowDebugInfoSwitch == 1 && edgeDists.Length == 4)
            {
                tssLb.Text = tag.Degree + "|" + abPoint.X.ToString() + "，" + abPoint.Y.ToString() + "( at row " +
                ctrlPointMap.RowStart + "/" + ctrlPointMap.RowEnd + " , col " + ctrlPointMap.ColStart + " / " + ctrlPointMap.ColEnd
                + ") | ImgCtrlPoint: " +
                point.X.ToString() + "，" + point.Y.ToString() + " | ImgOffset :" +
                this.imgBox.Page.ImgOffset.X + "，" + this.imgBox.Page.ImgOffset.Y.ToString() + " | VisableMapRectangle C/R:" +
                visMapR.ColStart + "，" + visMapR.ColEnd + " | " +
                visMapR.RowStart + "，" + visMapR.RowEnd + " | MapRectangle: C/R " +
                imgBox.Page.MapRectangle.ColStart + "，" + imgBox.Page.MapRectangle.ColEnd + " | " +
                imgBox.Page.MapRectangle.RowStart + "，" + imgBox.Page.MapRectangle.RowEnd + " | " +
                    " | ScaleInfo:  " + imgBox.Page.Scale + "( " + imgBox.Image.Size.Width + " x " + imgBox.Image.Size.Height + ")" +
                    " | " + Convert.ToInt16(edgeDists[0]) + "," + Convert.ToInt16(edgeDists[1]) + "," + Convert.ToInt16(edgeDists[2]) + "," + Convert.ToInt16(edgeDists[3]);
            }
            else
            {
                tssLb.Text = "";
            }

            //if (frmPageMap != null)
            //{
            //    frmPageMap.Reload();
            //}

            //打开鹰眼图时，记录轨迹
            if (panPageMap != null && panPageMap.Visible)
            {
                #region 旧处理

                //Func<bool> func = () => RecordTrackPath();
                //func.BeginInvoke((result) =>
                //{
                //    bool ret = func.EndInvoke(result);
                //    this.BeginInvoke(new Action<bool>(MapReload), ret);
                //}, null);

                #endregion 旧处理

                this.BeginInvoke((Action)(() =>
                {
                    bool ret = RecordTrackPath();
                    MapReload(ret);
                }));
            }
        }

        #endregion 鼠标事件

        #region 放大缩小相关

        private void ZoomInOutByMenuBtn(int toLevel, float toScale)
        {
            Point ctrlPoint = new Point(imgBox.Width / 2, imgBox.Height / 2);

            ZoomInOut(toLevel, toScale, false, ctrlPoint);
        }

        private void ZoomInOut(int toLevel, float toScale, bool movePageByMousePos)
        {
            Point ctrlPoint = imgBox.PointToClient(Control.MousePosition);
            ZoomInOut(toLevel, toScale, movePageByMousePos, ctrlPoint);
        }

        /// <summary>
        /// 放大缩小
        /// </summary>
        /// <param name="toLevel">目标层级</param>
        /// <param name="toScale">放大倍率</param>
        /// <param name="movePageByMousePos">是否根据鼠标位置对其</param>
        /// <param name="ctrlPoint">鼠标位置的控件坐标</param>
        private void ZoomInOut(int toLevel, float toScale, bool movePageByMousePos, Point ctrlPoint)
        {
            CImgTag tag = (CImgTag)this.imgBox.Image.Tag;
            Point absPoint = imgBox.Page.AbsPoint(ctrlPoint, tag.Degree);
            //absPoint = new Point(Math.Min(absPoint.X, imgBox.Page.ScanPage.MaxAbsPoint().X), Math.Min(absPoint.Y, imgBox.Page.ScanPage.MaxAbsPoint().Y));

            // absPoint = new Point(imgBox.Page.ScanPage.MaxAbsPoint().X / 2, imgBox.Page.ScanPage.MaxAbsPoint().Y / 2);

            if (!imgBox.IsWorking())
            {
                CurrentAbsPoint = absPoint;
                CurrentCtrlPoint = ctrlPoint;
            }

            ZoomInOut(imgBox.Page, ctrlPoint, toLevel, toScale, movePageByMousePos);
        }

        private void ZoomInOut(ImgCtrlPage fromPage, Point ctrlPoint, int toLevel, float toScale, bool movePageByMousePos)
        {
            if (imgBox.IsWorking())
            {
                //return;
            }
            ImgCtrlPage toPage = fromPage.Clone();

            if (toLevel > fromPage.ScanPage.MaxLevel || toLevel < 1)
            {
                return;
            }
            if (imgBox.Page.ScanPage.CalcColsRows(toLevel))
            {
                toPage.CurrLevel = toLevel;
                toPage.Scale = toScale;
                CImgTag tag = (CImgTag)this.imgBox.Image.Tag;
                int degree = Convert.ToInt16(tag.Degree);
                logger.Debug("ZoomInOut at :" + CurrentAbsPoint.ToString() + ctrlPoint.ToString());
                if (imgBox.Page.MapRectangleBuffer.ContainsKey(toLevel))
                {
                    toPage.MapRectangle = imgBox.Page.MapRectangleBuffer[toLevel];
                }
                else
                {
                    toPage.Refresh(ctrlPoint, imgBox.ClientSize, degree);
                }

                imgBox.LoadImg(fromPage, toPage, CurrentAbsPoint, CurrentCtrlPoint);
                //imgBox.LoadImg(oriPage);
                if (movePageByMousePos)
                {
                    // imgBox.Page.MovePage(ctrlPoint, absPoint);
                }
                else
                {
                    //imgBox.Page.ImgOffset = new Point(0, 0);
                }

                float imgCtrlScalVal = imgBox.Page.ScanPage.ImgScaleVal(toScale, toLevel);

                this.setToScaleTxt(imgCtrlScalVal);
            }
        }

        #endregion 放大缩小相关

        private Point CurrentAbsPoint, CurrentCtrlPoint;

        /// <summary>
        /// 重载图像
        /// </summary>
        private void Reload()
        {
            if (imgBox.Image == null)
            {
                return;
            }
            Point ctrlPoint = PointToClient(MousePosition);
            int degree = Convert.ToInt16(((CImgTag)imgBox.Image.Tag).Degree);

            imgBox.Page.Refresh(ctrlPoint, imgBox.ClientSize, degree);
            imgBox.LoadImg(imgBox.Page);
        }

        #region 顶部菜单按钮点击事件

        /// <summary>
        /// 样本操作点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSpOpButton_Click(object sender, EventArgs e)
        {
            if (ucPanelSampleOpr.Visible)
            {
                ucPanelSampleOpr.Hide();
                setBtnActive(this.tsbSpOpBtn, false);
            }
            else
            {
                setBtnActive(this.tsbSpOpBtn, true);
                ucPanelSampleOpr.Parent = imgBox;
                ucPanelSampleOpr.Top = this.windowToTop;
                ucPanelSampleOpr.Left = 8;
                ucPanelSampleOpr.Show();
            }
        }

        /// <summary>
        /// 玻片信息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucBtnSampleInfo_BtnClick(object sender, EventArgs e)
        {
            ucPanelSampleOpr.Hide();
            setBtnActive(this.tsbSpOpBtn, false);
            FrmSampleInfo frmSampleInfo = new FrmSampleInfo(startPath);
            frmSampleInfo.ShowDialog();
        }

        /// <summary>
        /// 像素信息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucBtnPixelInfo_BtnClick(object sender, EventArgs e)
        {
            ucPanelSampleOpr.Hide();
            setBtnActive(this.tsbSpOpBtn, false);
            slideCtrl.Top = this.windowToTop;
            slideCtrl.Left = 10;
            slideCtrl.Show();
        }

        /// <summary>
        /// 截图按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSpShotBtn_Click(object sender, EventArgs e)
        {
            if (snapShotPanel.Visible)
            {
                setBtnActive(this.tsbSpShotBtn, false);
                snapShotPanel.Hide();
            }
            else
            {
                snapShotPanel.Top = (this.Height - this.snapShotPanel.Height) / 2;
                snapShotPanel.Left = (this.Width - this.snapShotPanel.Width) / 2;
                setBtnActive(this.tsbSpShotBtn, true);
                snapShotPanel.Show();
            }
        }

        /// <summary>
        /// 条形码按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSpBarcodeBtn_Click(object sender, EventArgs e)
        {
            if (barcodePanel.Visible)
            {
                setBtnActive(this.tsbSpBarcodeBtn, false);
                barcodePanel.Hide();
            }
            else
            {
                setBtnActive(this.tsbSpBarcodeBtn, true);
                barcodePanel.Show();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.imgBox.ClearMemory();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 鹰眼图按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSpMapBtn_Click(object sender, EventArgs e)
        {
            if (!panPageMap.Visible)
            {
                setBtnActive(this.tsbSpMapBtn, true);
                panPageMap.Show();
            }
            else
            {
                setBtnActive(this.tsbSpMapBtn, false);
                panPageMap.Hide();
            }
        }

        private void tsbScaleBtn_Click(object sender, EventArgs e)
        {
            if (tsbScaleDialog.Visible)
            {
                tsbScaleDialog.Hide();
                tsbScaleBtn.Checked = false;
            }
            else
            {
                tsbScaleDialog.Top = 10;
                tsbScaleDialog.Left = 276;
                tsbScaleDialog.Show();
                tsbScaleBtn.Checked = true;
            }
        }

        private void tSBScaleTo_Click(object sender, EventArgs e)
        {
            String s = tSTBScale.InputText;
            if (s == null || s.Length == 0)
            {
                tsbScaleDialog.Hide();
                tsbScaleBtn.Checked = false;
                return;
            }
            float toScale = Convert.ToSingle(s);
            ScanPageLevel target = imgBox.Page.ScanPage.GetByScale(toScale);
            ZoomInOut(target.Level, target.ToScale, false);
            imgBox.PutImageInCenter(true);
            tsbScaleDialog.Hide();
            tsbScaleBtn.Checked = false;
        }

        private void tSTBScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsPunctuation(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;//消除不合适字符
            }
            else if (Char.IsPunctuation(e.KeyChar))
            {
                if (e.KeyChar != '.' || this.tSTBScale.Text.Length == 0)//小数点
                {
                    e.Handled = true;
                }
                if (tSTBScale.Text.LastIndexOf('.') != -1)
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 旋转按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRotate_Click(object sender, EventArgs e)
        {
            if (rotatePanel.Visible)
            {
                setBtnActive(this.tsbRotate, false);
                this.rotatePanel.Hide();
            }
            else
            {
                setBtnActive(this.tsbRotate, true);
                this.rotatePanel.Parent = imgBox;
                this.rotatePanel.Top = this.windowToTop;
                this.rotatePanel.Left = this.tsbRotate.Left - 160;
                this.rotatePanel.BringToFront();
                this.rotatePanel.Show();
            }
            //

            // Point p1 = ImgHelper.PointRotate( new Point(0, 0),new Point(150, 150), 360 - 90);
            //CImgTag tag = (CImgTag)this.imgBox.Image.Tag;
            //imgBox.RotatePage(tag.Degree + 20);
        }

        /// <summary>
        /// 设置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSettingsBtn_Click(object sender, EventArgs e)
        {
            frmSettings = new FrmSettings(this, imgBox);
            setBtnActive(this.tsbSettingsBtn, true);
            frmSettings.StartPosition = FormStartPosition.CenterParent; // 窗体在父窗体理居中
            frmSettings.ShowDialog();
            //tsbSettingsBtn.Checked = true;
        }

        private bool mouseSwitchActive = true;

        private void tsbMouseSwitchBtn_BtnClick(object sender, EventArgs e)
        {
            this.mouseSwitchActive = !this.mouseSwitchActive;
            if (this.mouseSwitchActive)
            {
                imgBox.CancleAdjust();
                imgBox.CancleMeasure();
                setBtnActive(this.tsbMouseSwitchBtn, true);
                //this.ResetAllAnnoBtn();
                imgBox.Cursor = Cursors.Default;
                imgBox.DType = AnnotationType.Stop;
                this.annoPanel.Hide();
                setBtnActive(this.annotationButton, false);
                setBtnActive(this.MeasureToolStripButton, false);
            }
            else
            {
                //setBtnActive(this.tsbMouseSwitchBtn, false);
            }
        }

        private bool touchpadGestureActive = false;

        private void touchpadGestureBtn_BtnClick(object sender, EventArgs e)
        {
            this.touchpadGestureActive = !this.touchpadGestureActive;
            if (this.touchpadGestureActive)
            {
                setBtnActive(this.touchpadGestureBtn, true);
            }
            else
            {
                setBtnActive(this.touchpadGestureBtn, false);
            }
        }

        private bool userGuideActive = false;

        private void userGuideBtn_BtnClick(object sender, EventArgs e)
        {
            this.userGuideActive = !this.userGuideActive;
            if (this.userGuideActive)
            {
                setBtnActive(this.userGuideBtn, true);
            }
            else
            {
                setBtnActive(this.userGuideBtn, false);
            }
        }

        /// <summary>
        /// 关于按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutUsBtn_BtnClick(object sender, EventArgs e)
        {
            FrmAbout frmAbout = new FrmAbout();
            setBtnActive(this.aboutUsBtn, false);
            frmAbout.StartPosition = FormStartPosition.CenterParent; // 在父窗体中居中
            frmAbout.ShowDialog();
        }

        /// <summary>
        /// 帮助按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbHelp_BtnClick(object sender, EventArgs e)
        {
            if (!this.helpPanel.Visible)
            {
                UCBtnImg btnImg = (UCBtnImg)sender;
                var startX = btnImg.Location.X; // 帮助按钮的x轴位置
                setBtnActive(this.tsbHelp, true);
                this.helpPanel.Parent = imgBox;
                this.helpPanel.Top = this.windowToTop;
                //this.helpPanel.Left = this.Width - this.helpPanel.Width - 20;
                this.helpPanel.Left = startX;
                this.helpPanel.Show();
            }
            else
            {
                setBtnActive(this.tsbHelp, false);
                this.helpPanel.Hide();
            }
        }

        public const Int32 AW_VER_POSITIVE = 0x00000004; // 从上到下打开窗口
        public const Int32 AW_VER_NEGATIVE = 0x00000008; // 从下到上打开窗口

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(
            IntPtr hwnd, // handle to window
        int dwTime, // duration of animation
        int dwFlags // animation type
        );

        private void toFolderSelectBtn_BtnClick(object sender, EventArgs e)
        {
            this.imgBox.ClearMemory();

            this.DialogResult = DialogResult.OK;
        }

        private bool showMapTraceActive = false;

        /// <summary>
        /// 显示跟踪轨迹按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showMapTraceBtn_BtnClick(object sender, EventArgs e)
        {
            this.showMapTraceActive = !this.showMapTraceActive;
            this.PanPageMapTrackPath = !PanPageMapTrackPath;
            if (this.showMapTraceActive)
            {
                setBtnActive(this.showMapTraceBtn, true);
            }
            else
            {
                setBtnActive(this.showMapTraceBtn, false);
            }
        }

        /// <summary>
        /// 截图界面关闭按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void snapShotCloseBtn_BtnClick(object sender, EventArgs e)
        {
            snapShotPanel.Hide();
            setBtnActive(this.tsbSpShotBtn, false);
        }

        /// <summary>
        /// 截图按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSnapShotBtn_BtnClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            setBtnActive(this.saveSnapShotBtn, true);
            saveFileDialog1.Filter = "jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "capture.jpg";//设置默认文件名为111
            saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory.ToString();//设置默认目录为本程序目录

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imgBox.CaptureScreenShot(saveFileDialog1.FileName,
                    cbShowAnno.Checked,
                    cbShowQrcode.Checked);
                setBtnActive(this.saveSnapShotBtn, false);
                setBtnActive(this.tsbSpShotBtn, false);
                this.snapShotPanel.Hide();
            }
            else
            {
                setBtnActive(this.saveSnapShotBtn, false);
            }
        }

        /// <summary>
        /// 旋转保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void rotateSave_BtnClick(object sender, EventArgs e)
        {
            setBtnActive(this.rotateSave, true);
            //Thread.Sleep(100);
            await Task.Delay(100);
            CImgTag tag = (CImgTag)this.imgBox.Image.Tag;
            imgBox.RotatePage(tag.Degree + this.rotateTrackBar.Value.ToInt());
            MapReload(true);

            setBtnActive(this.rotateSave, false);
            this.rotatePanel.Hide();
            setBtnActive(this.tsbRotate, false);
        }

        /// <summary>
        /// 实际像素浏览按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSpRealPixel_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSpRealPixel);
            ZoomInOutByMenuBtn(9, 1.48F);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 切片全貌按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tsbSpFullScreen_BtnClick(object sender, EventArgs e)
        {
            setBtnActive(this.tsbSpFullScreen, true);
            //Thread.Sleep(100);
            await Task.Delay(100);
            zoomBtnClick(this.tsbSpFullScreen);
            setBtnActive(this.tsbSpFullScreen, false);
            ZoomInOutByMenuBtn(3, 1);
            imgBox.PutImageInCenter(false);
        }

        #region 放大按钮

        /// <summary>
        /// 1x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp1XBtn_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSp1XBtn);
            ZoomInOutByMenuBtn(1, 1);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 2x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp2XBtn_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSp2XBtn);
            ZoomInOutByMenuBtn(2, 1);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 5x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp5XBtn_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSp5XBtn);
            ZoomInOutByMenuBtn(5, 1);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 10x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp10XBtn_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSp10XBtn);
            ZoomInOutByMenuBtn(7, 1);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 20x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp20XBtn_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSp20XBtn);
            ZoomInOutByMenuBtn(8, 1);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 40x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp40XBtn_BtnClick(object sender, EventArgs e)
        {
            zoomBtnClick(this.tsbSp40XBtn);
            ZoomInOutByMenuBtn(9, 1);
            imgBox.PutImageInCenter(false);
        }

        /// <summary>
        /// 9x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp9XBtn_Click(object sender, EventArgs e)
        {
            ZoomInOut(9, 1, false);
            imgBox.PutImageInCenter(false);
            tsbZoomBtnSelect(9);
        }

        /// <summary>
        /// 2x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp2XBtn_Click(object sender, EventArgs e)
        {
            ZoomInOut(2, 1, false);
            imgBox.PutImageInCenter(false);
            tsbZoomBtnSelect(2);
        }

        /// <summary>
        /// 1x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp1XBtn_Click(object sender, EventArgs e)
        {
            ZoomInOut(1, 1, false);
            imgBox.PutImageInCenter(false);
            tsbZoomBtnSelect(1);
        }

        /// <summary>
        /// 6x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSp6XBtn_Click(object sender, EventArgs e)
        {
            ZoomInOut(6, 1, false);
            imgBox.PutImageInCenter(false);
            tsbZoomBtnSelect(6);
        }

        /// <summary>
        /// 倍率按钮点击事件
        /// </summary>
        /// <param name="btn"></param>
        public void zoomBtnClick(UCBtnImg btn)
        {
            string btnName = btn.Name;

            Color activeColor = Color.FromArgb(23, 56, 87);
            Color unActiveColor = Color.FromArgb(84, 86, 87);
            switch (btnName)
            {
                case "tsbSpFullScreen":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSp1XBtn":
                    this.tsbSp1XBtn.FillColor = activeColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSp2XBtn":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = activeColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSp5XBtn":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = activeColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSp10XBtn":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = activeColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSp20XBtn":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = activeColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSp40XBtn":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = activeColor;
                    setBtnActive(this.tsbSpRealPixel, false);
                    break;

                case "tsbSpRealPixel":
                    this.tsbSp1XBtn.FillColor = unActiveColor;
                    this.tsbSp2XBtn.FillColor = unActiveColor;
                    this.tsbSp5XBtn.FillColor = unActiveColor;
                    this.tsbSp10XBtn.FillColor = unActiveColor;
                    this.tsbSp20XBtn.FillColor = unActiveColor;
                    this.tsbSp40XBtn.FillColor = unActiveColor;
                    setBtnActive(this.tsbSpRealPixel, true);
                    break;

                default:
                    break;
            }
        }

        //自定义放大倍率按钮点击
        private void tsbSpZoomBtn_Click(object sender, EventArgs e)
        {
            if (magCtrl1.Visible)
            {
                setBtnActive(this.tsbSpZoomBtn, false);
                magCtrl1.Hide();
            }
            else
            {
                setBtnActive(this.tsbSpZoomBtn, true);
                magCtrl1.Show();
                magCtrl1.BringToFront();
            }
        }

        #endregion 放大按钮

        /// <summary>
        /// 颜色调整按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbColorCorrecionBtn_Click(object sender, EventArgs e)
        {
            FrmColorSet frmColorSet = new FrmColorSet(this, startPath);

            setBtnActive(this.tsbColorCorrecionBtn, true);

            string fileName = "\\" + 6 + "\\" + 5 + "\\" + 6 + ".jpg";
            string fullFileName = startPath + fileName;
            Bitmap image = (Bitmap)Image.FromFile(fullFileName);
            frmColorSet.setPreviewImg(image);
            frmColorSet.StartPosition = FormStartPosition.CenterParent; // 显示位置居于父窗体中间
            frmColorSet.ShowDialog();
            //tsbColorCorrecionBtn.Checked = false;
        }

        #endregion 顶部菜单按钮点击事件

        private void tsbZoomBtnSelect(int zoomScale)
        {
            switch (zoomScale)
            {
                case 1:
                    this.tsbSp1XBtn.Height = 48;
                    this.tsbSp2XBtn.Height = 42;
                    this.tsbSp5XBtn.Height = 42;
                    this.tsbSp10XBtn.Height = 42;
                    break;

                case 2:
                    this.tsbSp2XBtn.Height = 48;
                    this.tsbSp1XBtn.Height = 42;
                    this.tsbSp5XBtn.Height = 42;
                    this.tsbSp10XBtn.Height = 42;
                    break;

                case 6:
                    this.tsbSp5XBtn.Height = 48;
                    this.tsbSp2XBtn.Height = 42;
                    this.tsbSp1XBtn.Height = 42;
                    this.tsbSp10XBtn.Height = 42;
                    break;

                case 9:
                    this.tsbSp10XBtn.Height = 48;
                    this.tsbSp2XBtn.Height = 42;
                    this.tsbSp5XBtn.Height = 42;
                    this.tsbSp1XBtn.Height = 42;
                    break;

                default:
                    this.tsbSp10XBtn.Height = 42;
                    this.tsbSp2XBtn.Height = 42;
                    this.tsbSp5XBtn.Height = 42;
                    this.tsbSp1XBtn.Height = 42;
                    break;
            }
        }

        #region 窗体事件

        private void FrmMain_Load(object sender, EventArgs e)
        {
            imgBox.MouseWheel -= ImgBox_MouseWheel;//
            imgBox.MouseWheel += ImgBox_MouseWheel;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            imgBox.RefreshSize();
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            imgBox.LoadImg(imgBox.Page);
            imgBox.Invalidate();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            return;

            if (e.CloseReason == CloseReason.UserClosing && this.DialogResult != DialogResult.OK)
            {
                if (MessageBox.Show(

                    GlobalData.GlobalLanguage.Text("Close_Confirm"),
                    GlobalData.GlobalLanguage.Text("close"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    ) == DialogResult.Yes)
                {
                    System.Environment.Exit(0);
                    //this.DialogResult = DialogResult.OK;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion 窗体事件

        #region 标注相关按钮点击

        /// <summary>
        /// 重置所有标注按钮状态
        /// </summary>
        private void ResetAllAnnoBtn()
        {
            setBtnActive(this.rectangleToolStripButton, false);
            setBtnActive(this.circleToolStripButton, false);
            setBtnActive(this.lineToolStripButton, false);
            setBtnActive(this.fixRectangleToolStripButton, false);
            setBtnActive(this.fixCircleToolStripButton, false);
            setBtnActive(this.arrowToolStripButton, false);
            //setBtnActive(this.tsbMouseSwitchBtn, false);
        }

        private void annotationButton_Click(object sender, EventArgs e)
        {
            imgBox.CancleMeasure();
            imgBox.CancleAdjust();
            this.ResetAllAnnoBtn();
            if (!annoPanel.Visible)
            {
                annoPanel.Parent = imgBox;
                annoPanel.Left = 276;
                annoPanel.Top = this.windowToTop;
                annoPanel.BringToFront();
                setBtnActive(this.annotationButton, true);
                // 矩形框
                imgBox.Cursor = Cursors.Cross;
                imgBox.DType = AnnotationType.Rectangle;
                setBtnActive(this.rectangleToolStripButton, true);
                // 鼠标手型切换
                this.mouseSwitchActive = false;
                setBtnActive(this.tsbMouseSwitchBtn, false);
                annoPanel.Show();

                setBtnActive(this.MeasureToolStripButton, false);
            }
            else
            {
                imgBox.Cursor = Cursors.Default;
                setBtnActive(this.annotationButton, false);
                annoPanel.Hide();
                imgBox.DType = AnnotationType.Stop;

                this.mouseSwitchActive = true;
                setBtnActive(this.tsbMouseSwitchBtn, true);
            }
        }

        private void rectangleToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.Rectangle)
            {
                imgBox.DType = AnnotationType.Rectangle;
                this.ResetAllAnnoBtn();
                setBtnActive(this.rectangleToolStripButton, true);
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
                setBtnActive(this.rectangleToolStripButton, false);
            }
        }

        private void circleToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.Circle)
            {
                imgBox.DType = AnnotationType.Circle;
                this.ResetAllAnnoBtn();
                setBtnActive(this.circleToolStripButton, true);
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
                setBtnActive(this.circleToolStripButton, true);
            }
        }

        private void lineToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.Line)
            {
                imgBox.DType = AnnotationType.Line;
                this.ResetAllAnnoBtn();
                setBtnActive(this.lineToolStripButton, true);
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
                setBtnActive(this.lineToolStripButton, false);
            }
        }

        private void fixRectangleToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.FixRectangle)
            {
                imgBox.DType = AnnotationType.FixRectangle;
                this.ResetAllAnnoBtn();
                setBtnActive(this.fixRectangleToolStripButton, true);
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
                setBtnActive(this.fixRectangleToolStripButton, false);
            }
        }

        private void fixCircleToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.FixCircle)
            {
                imgBox.DType = AnnotationType.FixCircle;
                this.ResetAllAnnoBtn();
                setBtnActive(this.fixCircleToolStripButton, true);
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
                setBtnActive(this.fixCircleToolStripButton, false);
            }
        }

        private void arrowToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.Arrow)
            {
                imgBox.DType = AnnotationType.Arrow;
                this.ResetAllAnnoBtn();
                setBtnActive(this.arrowToolStripButton, true);
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
                setBtnActive(this.arrowToolStripButton, false);
            }
        }

        private void TMAToolStripButton_Click(object sender, EventArgs e)   //new
        {
            if (imgBox.DType != AnnotationType.TMA)
            {
                imgBox.DType = AnnotationType.TMA;
            }
            else
            {
                imgBox.DType = AnnotationType.Stop;
            }
        }

        #endregion 标注相关按钮点击

        #region imgbox 事件

        private void imgBox_Resize(object sender, EventArgs e)
        {
            imgBox.imgRect = new Rectangle();
            Reload();
            AutoResize();
        }

        private void imgBox_Click(object sender, EventArgs e)
        {
            // logger.Debug("ImgBox Page.ImgOffset.X:" + imgBox.Page.ImgOffset.X + " Page.ImgOffset.Y:" + imgBox.Page.ImgOffset.Y);
        }

        private void imgBox_NeedPic(object sender, ImgCtrl.ImageEventArgs e)
        {
            // logger.Debug("Imgbox need pic!~!!!!");
        }

        private void imgBox_ChangeTips(object sender, ImgCtrl.ImageEventArgs e)
        {
            if (e.Tips.IsEmpty())
            {
                try
                {
                    lblLoading.Hide();
                }
                catch (Exception exp)
                {
                }
            }
            else
            {
                try
                {
                    lblLoading.Text = e.Tips;
                    lblLoading.Show();
                }
                catch (Exception exp)
                {
                }
            }
        }

        #endregion imgbox 事件

        /// <summary>
        /// 测量按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeasureToolStripButton_Click(object sender, EventArgs e)
        {
            if (imgBox.DType != AnnotationType.MeasureLine)
            {
                //anno
                ResetAllAnnoBtn();
                setBtnActive(this.annotationButton, false);
                annoPanel.Hide();

                //mouseSwitch
                this.mouseSwitchActive = false;
                setBtnActive(this.tsbMouseSwitchBtn, false);

                //measure
                setBtnActive(this.MeasureToolStripButton, true);
                imgBox.StartMeasure();
            }
            else
            {
                //mouse switch
                this.mouseSwitchActive = true;
                setBtnActive(this.tsbMouseSwitchBtn, true);

                setBtnActive(this.MeasureToolStripButton, false);
                imgBox.CancleMeasure();
            }
        }

        #region 放大镜控件相关

        private bool MagCtrlMoveFlag;
        private Point MagCtrlPoint;

        private void magCtrl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MagCtrlMoveFlag)
            {
                magCtrl1.Left += Convert.ToInt16(e.X - MagCtrlPoint.X);//设置x坐标.
                magCtrl1.Top += Convert.ToInt16(e.Y - MagCtrlPoint.Y);//设置y坐标.
            }
        }

        private void magCtrl1_MouseUp(object sender, MouseEventArgs e)
        {
            MagCtrlMoveFlag = false;
        }

        private void magCtrl1_MouseDown(object sender, MouseEventArgs e)
        {
            MagCtrlMoveFlag = true;
            MagCtrlPoint = new Point(e.X, e.Y);
        }

        #endregion 放大镜控件相关

        #region 样本信息控件相关

        private bool SlideInfoCtrlMoveFlag;
        private Point SlideInfoPoint;

        private void slideCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            SlideInfoCtrlMoveFlag = true;
            SlideInfoPoint = new Point(e.X, e.Y);
        }

        private void slideCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            SlideInfoCtrlMoveFlag = false;
        }

        private void slideCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (SlideInfoCtrlMoveFlag)
            {
                slideCtrl.Left += Convert.ToInt16(e.X - SlideInfoPoint.X);//设置x坐标.
                slideCtrl.Top += Convert.ToInt16(e.Y - SlideInfoPoint.Y);//设置y坐标.
            }
        }

        #endregion 样本信息控件相关

        #region 条形码相关

        private void initBarcodeDialog()
        {
            string folderPath = this.startPath;
            if (ScanPage.IsValidScanPage(folderPath).IsEmpty())
            {
                string barcodeImage = folderPath + Constants.ScanPageBarcodeFilePath;
                Image img2 = null;
                if (!File.Exists(barcodeImage))
                {
                    img2 = DST.TileImageViewer.Properties.Resources.barcode;
                }
                else
                {
                    img2 = Image.FromFile(barcodeImage);
                }
                if (Constants.SettingDetail.LabelOrientationSwitch == 1)
                {
                    barcodeBox.Image = ImgHelper.GetRotateImage((Bitmap)img2.Clone(), Convert.ToInt16(180));
                }
                else
                {
                    barcodeBox.Image = img2;
                }
            }
        }

        /**
         * 旋转barcode按钮
         *
         */

        private void barcodeRotateBtn_BtnClick(object sender, EventArgs e)
        {
            if (barcodeBox.Image == null)
            {
                return;
            }
            barcodeBox.Image = ImgHelper.GetRotateImage((Bitmap)barcodeBox.Image.Clone(), Convert.ToInt16(180));
            SettingManager sm = new SettingManager();
            Setting st = Constants.SettingDetail;
            if (st.LabelOrientationSwitch == 1)
            {
                st.LabelOrientationSwitch = 0;
            }
            else
            {
                st.LabelOrientationSwitch = 1;
            }
            sm.UpdateToDb(st);
        }

        #endregion 条形码相关

        private void SettingChanged()
        {
            this.initBarcodeDialog();
            ZoomInOut(imgBox.Page.CurrLevel, 1, true);
        }

        private void ucBtnImg1_BtnClick(object sender, EventArgs e)
        {
            setBtnActive(this.tsbSpMapBtn, false);
            panPageMap.Hide();
        }

        private void ColorChanged()
        {
            if (imgBox?.Image == null)
            {
                return;
            }

            imgBox.ChangeColorAsync();
        }

        /// <summary>
        /// 设置标签名称
        /// </summary>
        private void setSampleName()
        {
            // string showText = startPath;
            // int lastPosition = showText.LastIndexOf("\\");
            this.sampleName = imgBox.Page.ScanPage.GetSampleName();
            sampleNumLabel.Parent = imgBox;
            sampleNumLabel.Title = this.sampleName;
            sampleNumLabel.setAutoWidth(this.sampleName);
            sampleNumLabel.Left = (this.Size.Width - sampleNumLabel.Width) / 2;
            sampleNumLabel.Top = 60;
        }

        private long lastDoubleClickTime = 0;

        /// <summary>
        /// 鼠标左键在显示样本的区域双击在最小倍率和最大倍率间切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgBox_DoubleClick(object sender, EventArgs e)
        {
            long ss = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            if (ss - lastDoubleClickTime < 500)
            {
                return;
            }
            lastDoubleClickTime = ss;

            if (imgBox.Page.CurrLevel < imgBox.Page.ScanPage.MaxLevel)
            {
                ZoomInOut(imgBox.Page.ScanPage.MaxLevel, 1, true);
            }
            else
            {
                ZoomInOut(1, 1, true);
            }
            imgBox.PutImageInCenter(true);
        }

        /// <summary>
        /// 放大镜模式切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucBtnTuding_BtnClick(object sender, EventArgs e)
        {
            this.magnifierSwith = !this.magnifierSwith;
            if (this.magnifierSwith)
            {
                this.ucBtnTuding.Image = DST.TileImageViewer.Properties.Resources.tu_ding_close_24;
            }
            else
            {
                this.ucBtnTuding.Image = DST.TileImageViewer.Properties.Resources.tu_ding_open_24;
            }
        }

        /// <summary>
        /// 两种模式：
        /// 1. 放大鼠标的位置
        /// 2. 放大放大镜的位置
        /// </summary>
        /// <param name="abPoint"></param>
        /// <returns></returns>
        private bool magRefresh(Point abPoint)
        {
            if (magCtrl1.Visible)
            {
                // 1. 放大鼠标的位置
                if (this.magnifierSwith)
                {
                    magCtrl1.CurrentMouseAbsPoint = abPoint;
                    magCtrl1.ReloadImage();
                }
                else
                {
                    // 2.放大放大镜的位置
                    magCtrl1.CurrentMouseAbsPoint = Point.Empty;
                    magCtrl1.ReloadImage();
                }
            }
            return true;
        }

        private void toolStrip1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush sb = new SolidBrush(Color.FromArgb(20, 0, 0, 0));
            g.FillRectangle(sb, 0, 0, toolStrip1.Width, toolStrip1.Height);
        }

        public string GetCurrentFilePath()
        {
            return startPath;
        }

        private void toScaleTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String s = toScaleTxt.Text;
                if (s == null || s.Length == 0)
                {
                    return;
                }

                float toScale = Convert.ToSingle(s);
                if (toScale > 40)
                {
                    toScaleTxt.Text = "40";
                    toScale = 40;
                }
                if (imgBox.Page.ImgCtrlScaleVal == toScale)
                {
                    // return;
                }

                ScanPageLevel target = imgBox.Page.ScanPage.GetByScale(toScale);

                imgBox.Page.ImgCtrlScaleVal = toScale;

                ZoomInOut(target.Level, target.ToScale, false);
                imgBox.PutImageInCenter(true);
            }
        }

        /// <summary>
        /// 标注列表按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnoListStripButton_BtnClick(object sender, EventArgs e)
        {
            imgBox.DType = AnnotationType.Stop;
            imgBox.Cursor = Cursors.Default;
            annoPanel.Hide();
            setBtnActive(this.annotationButton, false);

            //mouse switch
            this.mouseSwitchActive = true;
            setBtnActive(this.tsbMouseSwitchBtn, true);

            TileImageViewer.FrmAnnotationSetting frm = new TileImageViewer.FrmAnnotationSetting(imgBox);
            if (frm != null)
            {
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        public void setToScaleTxt(float currentLevel)
        {
            toScaleTxt.Text = currentLevel.ToString();
        }

        private bool rotateHorActive = false;
        private bool rotateVerActive = false;

        private async void rotateResetBtn_BtnClick(object sender, EventArgs e)
        {
            setBtnActive(this.rotateResetBtn, true);
            this.rotateTrackBar.Value = 0;
            //Thread.Sleep(30);
            await Task.Delay(30);
            setBtnActive(this.rotateResetBtn, false);
        }

        /// <summary>
        /// 左右对称旋转(180)按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotateHorBtn_BtnClick(object sender, EventArgs e)
        {
            this.rotateHorActive = !this.rotateHorActive;
            CImgTag tag = (CImgTag)this.imgBox.Image.Tag;
            if (this.rotateHorActive)
            {
                setBtnActive(this.rotateHorBtn, true);
                imgBox.RotatePage(tag.Degree + 180);
            }
            else
            {
                setBtnActive(this.rotateHorBtn, false);
                imgBox.RotatePage(tag.Degree - 180);
            }
        }

        /// <summary>
        /// 上下对称旋转(270)点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotateVerBtn_BtnClick(object sender, EventArgs e)
        {
            this.rotateVerActive = !this.rotateVerActive;
            CImgTag tag = (CImgTag)this.imgBox.Image.Tag;
            if (this.rotateVerActive)
            {
                setBtnActive(this.rotateVerBtn, true);
                imgBox.RotatePage(tag.Degree + 270);
            }
            else
            {
                setBtnActive(this.rotateVerBtn, false);
                imgBox.RotatePage(tag.Degree - 270);
            }
        }

        private void rotateTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.rotateTxt.Text = this.rotateTrackBar.Value.ToString();
        }

        private void mapbox_Click(object sender, EventArgs e)
        {
            Point floorPageCtrlPoint = mapbox.PointToClient(Control.MousePosition);
            Point absPoint = floorPage.AbsPoint(floorPageCtrlPoint, floorPage.Degree);
            Point centerCtrlPoint = new Point(imgBox.Size.Width / 2, imgBox.Size.Height / 2);
            imgBox.Page.MovePage(centerCtrlPoint, absPoint);
            imgBox.LoadBufferEdge(centerCtrlPoint);

            //imgBox.Page.Refresh(centerCtrlPoint,imgBox.Size,imgBox.Page.Degree);

            MapReload(true);
            //MessageBox.Show(absPoint.ObjToString());
        }

        #region 按钮状态调整

        /// <summary>
        /// 按钮状态调整
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="active"></param>
        public void setBtnActive(UCBtnImg btn, bool active)
        {
            string btnName = btn.Name;
            switch (btnName)
            {
                case "tsbSpOpBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_bottle_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_bottle;
                    }
                    break;

                case "tsbSpMapBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_compass_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_compass;
                    }
                    break;

                case "tsbSpZoomBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_mag_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_mag;
                    }
                    break;

                case "tsbSpBarcodeBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_qr_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_qr;
                    }
                    break;

                case "annotationButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_pen_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_pen;
                    }
                    break;

                case "tsbRotate":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_rotate_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_rotate;
                    }
                    break;

                case "tsbColorCorrecionBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_colorplate_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_colorplate;
                    }
                    break;

                case "tsbSettingsBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_setting_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_setting;
                    }
                    break;

                case "rectangleToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_rectangle_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_rectangle;
                    }
                    break;

                case "circleToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_circle_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_circle;
                    }
                    break;

                case "lineToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_line_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_line;
                    }
                    break;

                case "fixRectangleToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_fixrectangle_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_fixrectangle;
                    }
                    break;

                case "fixCircleToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_fixcircle_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_fix_circle;
                    }
                    break;

                case "arrowToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_an_arrow_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_An_Arrow;
                    }
                    break;

                case "MeasureToolStripButton":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_ruler_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_ruler;
                    }
                    break;

                case "rotateResetBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_reset_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_reset;
                    }
                    break;

                case "rotateHorBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_rotate_hor_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_rotate_hor;
                    }
                    break;

                case "rotateVerBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_rotate_ver_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_rotate_ver;
                    }
                    break;

                case "tsbMouseSwitchBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_mouse_and_hand_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_mouse_and_hand;
                    }
                    break;

                case "tsbHelp":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_help_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_help;
                    }
                    break;

                case "touchpadGestureBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_touchpad_gesture_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_touchpad_gesture;
                    }
                    break;

                case "userGuideBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_user_guide_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_user_guide;
                    }
                    break;

                case "aboutUsBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_about_us_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_about_us;
                    }
                    break;

                case "showMapTraceBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.show_map_trace_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.show_map_trace;
                    }
                    break;

                case "tsbSpShotBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_camera_pressed;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_camera;
                    }
                    break;

                case "saveSnapShotBtn":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_save_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_save;
                    }
                    break;

                case "rotateSave":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_save_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_save;
                    }
                    break;

                case "tsbSpRealPixel":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_real_pixel_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_real_pixel;
                    }
                    break;

                case "tsbSpFullScreen":
                    if (active)
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_full_screen_press;
                    }
                    else
                    {
                        btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_full_screen;
                    }
                    break;
            }
        }

        #endregion 按钮状态调整

        private void ucPanelParent1_Load(object sender, EventArgs e)
        {
        }

        private void toScaleTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }

        private void mapbox_SizeChanged(object sender, EventArgs e)
        {
            int borderWidth = 10;
            panPageMap.Size = new Size(mapbox.Image.Size.Width + borderWidth, mapbox.Image.Size.Height + borderWidth);
            ucBtnImg1.Left = panPageMap.Left + panPageMap.Size.Width - ucBtnImg1.Width - borderWidth + 2;
        }

        #region 控件位置及大小调整

        /// <summary>
        /// 初始化控件位置
        /// </summary>
        private void initPosition()
        {
            transparentCtrl1.Parent = imgBox;
            bottomPanel.Parent = imgBox;
            toFolderSelectPanel.Parent = imgBox;
            //toFolderSelectPanel.Top = bottomPanel.Top - toFolderSelectPanel.Height;
            //toFolderSelectPanel.Left = (bottomPanel.Width - toFolderSelectPanel.Width) / 2;
        }

        /// <summary>
        /// 自动缩放相关控件
        /// </summary>
        private void AutoResize()
        {
            lblLoading.Top = this.windowToTop;
            lblLoading.Left = this.Size.Width - 100;

            barcodePanel.Top = this.windowToTop;
            barcodePanel.Left = this.Size.Width - 220;

            toFolderSelectPanel.Top = bottomPanel.Top - toFolderSelectPanel.Height + 5;
            toFolderSelectPanel.Left = (bottomPanel.Width - toFolderSelectPanel.Width) / 2;

            sampleNumLabel.Left = (this.Size.Width - sampleNumLabel.Width) / 2;
        }

        #endregion 控件位置及大小调整
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileImageViewer.Animation;

/// <summary>
/// 图像组件
/// </summary>
public class ImgCtrl : Control
{
    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    public ImgCtrlPage Page { get; set; }

    //创建动画timer新实例
    private System.Timers.Timer animationTimer = new System.Timers.Timer();

    //动画队列，连续操作时动画按顺序播放
    private Queue<Animate> animates = new Queue<Animate>();

    //图像范围
    public Rectangle imgRect;

    //控件范围
    public Rectangle ctrlRect;

    //需要画标注
    private bool NeedDrawAnno = false;

    //需要画测量线
    private bool NeedDrawMeasure = false;

    #region 内部变量

    private bool _isTracking = false;

    //图像
    private Bitmap _image;

    //控件大小
    private int _ctrlWidth, _ctrlHeight;

    //移动标注时的起始坐标
    private int _startX, _startY;

    //图像相对控件的偏移坐标
    private int _imagePreX, _imagePreY;

    //移动标注的偏移向量，暂未使用
    private int _movedX = 0, _movedY = 0;

    #endregion 内部变量

    private Rectangle mRect;

    #region 画标注需要的内部变量

    private bool _isMouseLeftPressed;

    private Point Point0;
    private Point Point2;
    public AnnotationType DType { get; set; } = AnnotationType.Stop;

    //是否正在画标注
    private bool _isDrawingAnno = false;

    //正在操作的当前标注
    public Annotation Anno { get; set; }

    #endregion 画标注需要的内部变量

    private Rectangle preLoadCtrlRect;
    private Rectangle preLoadImgRect;
    private Bitmap preLoadImg;

    //强制对位，用于动画完成后
    private bool preLoad = false;

    // 未改变的原始图像
    public Bitmap RawImage;

    // 图像尺寸，用于改善多线程锁的性能
    private Size ImgSize;

    //图像对象封装
    [Browsable(false)]
    public Bitmap Image
    {
        get
        {
            return _image;
        }
        set
        {
            if (value != null)
            {
                if (value.Tag == null)
                {
                    value.Tag = new CImgTag();
                }
                //_image = value;
                try
                {
                    ImgSize = value.Size;
                    RotateScaleAndColorTaskAsync(false, value);
                }
                catch (Exception)
                {
                }
                // 设置图像后异步执行缩放及颜色修改任务
            }

            _ctrlHeight = base.ClientRectangle.Height - 2;
            _ctrlWidth = base.ClientRectangle.Width - 2;

            Invalidate();
        }
    }

    public event EventHandler<ImageEventArgs> NeedPic;

    public event EventHandler<ImageEventArgs> ChangeTips;

    private Point _annotationMovePoint;
    private bool _isMoveAnnotation = false;

    private int _rectX, _rectY;

    private bool _isAdjusting = false;
    public bool IsAdjusting { get => _isAdjusting; set => _isAdjusting = value; }
    private int iCorner = -1;

    private CancellationTokenSource InitFullPageBufferTaskTokenSource;
    private TaskStatus InitFullPageTaskStatus;

    private CancellationTokenSource RotateScaleAndColorTaskTokenSource;
    private TaskStatus RotateScaleAndColorTaskStatus;

    private CancellationTokenSource JoinImageTaskTokenSource;
    private TaskStatus JoinImageTaskStatus;

    public ImgCtrl()
    {
        base.MouseUp += FilterPreview_MouseUp;
        base.MouseMove += FilterPreview_MouseMove;
        base.MouseDown += FilterPreview_MouseDown;

        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);

        InitTimer();
    }

    public new void Dispose()
    {
        // 解绑事件
        base.MouseUp -= FilterPreview_MouseUp;
        base.MouseMove -= FilterPreview_MouseMove;
        base.MouseDown -= FilterPreview_MouseDown;

        this._image.Dispose();
        this._image = null;

        // 停止计时器
        animationTimer.Stop();
        animationTimer.Close();

        // 释放Animate队列（实际是PageZoomAnimation）
        foreach (var anima in animates)
        {
            anima.Dispose();
        }
        animates.Clear();

        this.Page?.Dispose();
        this.Page = null;

        this.Anno = null;

        RawImage?.Dispose();
        RawImage = null;

        ScanPageCache.Clear();
        base.Dispose();
        GC.Collect();
    }

    /// <summary>
    /// 获得当前控件图像的tag
    /// </summary>
    /// <returns></returns>
    public CImgTag GetImgTag()
    {
        if (_image == null)
        {
            return null;
        }

        CImgTag tag = (CImgTag)_image.Tag;
        return tag;
    }

    /// <summary>
    /// 判断是否有后台任务正在执行
    /// </summary>
    /// <returns></returns>
    public bool IsWorking()
    {
        bool ret = false;

        if (animates.Count > 0)
        {
            ret = true;
        }

        if (RotateScaleAndColorTaskStatus == TaskStatus.Running ||
            JoinImageTaskStatus == TaskStatus.Running ||
            InitFullPageTaskStatus == TaskStatus.Running)
        {
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// 控件需要加载更多图片时的回调事件
    /// </summary>
    public class ImageEventArgs : EventArgs
    {
        public bool NeedRightPic;

        public bool NeedBottomPic;

        public bool NeedLeftPic;

        public bool NeedTopPic;

        public String Tips;
    }

    #region 动画timer相关

    /// <summary>
    /// 初始化Timer控件
    /// </summary>
    private void InitTimer()
    {
        //设置定时间隔(毫秒为单位)
        int interval = 1000 / 300;
        animationTimer = new System.Timers.Timer(interval);
        //设置执行一次（false）还是一直执行(true)
        animationTimer.AutoReset = false;
        //设置是否执行System.Timers.Timer.Elapsed事件
        animationTimer.Enabled = true;
        //绑定Elapsed事件
        animationTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerUp);
    }

    /// <summary>
    /// Timer类执行定时到点事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerUp(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            if (Page != null)
            {
                if (animates.Count() > 0)
                {
                    Animate animate = animates.Peek();
                    long unixTimestamp = Constants.CurrentTimestamp();

                    if (animate.Finished)
                    {
                        animates.Dequeue();
                        //var dequeueAnimate = animates.Dequeue(); // 记录下出队动画
                        // DequeueList.Add(dequeueAnimate);
                    }
                    else
                    {
                        animate.ExecAnimateAt(unixTimestamp, this);
                    }
                }

                Invalidate();
            }
        }
        catch (Exception eee)
        {
            logger.Error(eee);
        }
        finally
        {
            // 必须保证执行，不得中途返回
            animationTimer.Start();
        }
    }

    #endregion 动画timer相关

    #region 加载图像相关

    /// <summary>
    /// 控件显示scanpage当前图层，map区域下的数据，并偏移控件坐标系 (atX,atY)
    /// </summary>
    /// <param name="fromPage"></param>
    /// <param name="toPage"></param>
    /// <param name="centerAbsPoint"></param>
    /// <param name="centerCtrlPoint"></param>
    public void LoadImg(ImgCtrlPage fromPage, ImgCtrlPage toPage, Point centerAbsPoint, Point centerCtrlPoint)
    {
        CancelAllTask();

        if (animates.Count > 0)
        {
            //如果有动画没播放完，得继续播放
            PageZoomAnimation last = (PageZoomAnimation)animates.Peek();
            fromPage = last.FromPage.Clone();
        }

        // 创建动画
        PageZoomAnimation pageZoomAnimation = new PageZoomAnimation(ImgSize.Width, fromPage, toPage);
        pageZoomAnimation.CenterAbsPoint = centerAbsPoint;
        pageZoomAnimation.CenterCtrlPoint = centerCtrlPoint;
        animates.Clear();
        animates.Enqueue(pageZoomAnimation);

        //LoadImg(toPage);
    }

    /// <summary>
    /// 控件显示scanpage当前图层，map区域下的数据，并偏移控件坐标系 (atX,atY)
    /// </summary>
    /// <param name="iPage"></param>
    public void LoadImg(ImgCtrlPage iPage)
    {
        if (iPage.MapRectangle.IsValid())
        {
            Bitmap image = iPage.JoinImage(GetImgTag());
            LoadImg(iPage, image);
        }
    }

    /// <summary>
    /// 向控件装载要显示的图像
    /// </summary>
    /// <param name="iPage">控件数据</param>
    /// <param name="image">图像</param>
    public void LoadImg(ImgCtrlPage iPage, Bitmap image)
    {
        this.Page = iPage;
        SetImage(image);
    }

    /// <summary>
    /// 向控件装载要显示的图像并保留先前的状态，用于动画完成后正确显示新图层的位置
    /// </summary>
    /// <param name="iPage"></param>
    /// <param name="image"></param>
    /// <param name="preLoadCtrlRect1"></param>
    /// <param name="preLoadImgRect1"></param>
    /// <param name="preLoadImg1"></param>
    public void LoadImg(ImgCtrlPage iPage, Bitmap image,
        Rectangle preLoadCtrlRect1,
        Rectangle preLoadImgRect1,
        Bitmap preLoadImg1)
    {
        preLoadCtrlRect = preLoadCtrlRect1;
        preLoadImgRect = preLoadImgRect1;
        preLoadImg = preLoadImg1;
        preLoad = true;
        LoadImg(iPage, image);
    }

    /// <summary>
    /// 控件显示scanpage当前图层的所有数据
    /// </summary>
    public void LoadFullLevelImg()
    {
        SetImage(this.Page.JoinImageByLevel());
        // PutImageInCenter(true);
    }

    #endregion 加载图像相关

    /// <summary>
    /// 事件完成后，显示提示
    /// </summary>
    private void LoadDone()
    {
        ImageEventArgs eventArgs = new ImageEventArgs();
        eventArgs.Tips = "";
        if (this.ChangeTips != null)
        {
            ChangeTips(this, eventArgs);
        }
    }

    /// <summary>
    /// 设置图片的封装，可用于区分image.set的公共方法或私有方法，目前一样
    /// </summary>
    /// <param name="img"></param>
    private void SetImage(Bitmap img)
    {
        this.Image = img;
    }

    public void ReloadImage()
    {
        ScanPageCache.Clear();
        this.SetImage(Page.JoinImage(this.GetImgTag()));
    }

    /// <summary>
    /// 调整图像至控件中心
    /// </summary>
    /// <param name="resetPos"></param>
    public void PutImageInCenter(bool resetPos)
    {
        //WaitingForRender();

        if (resetPos)
        {
            Page.ImgOffset = new Point((ImgSize.Width - _ctrlWidth) / 2, (ImgSize.Height - _ctrlHeight) / 2);
        }
        DrawAnnotations();
    }

    /// <summary>
    /// 画标注及测量
    /// </summary>
    private void DrawAnnoAndMeasure()
    {
        if (Page != null)
        {
            DrawAnnotations();
            DrawMeasure();
        }
        //LoadDone(t);
    }

    /// <summary>
    /// 重新加载图片
    /// </summary>
    private void ReloadBitmap()
    {
        JoinImageAsync(false);
    }

    /// <summary>
    /// 根据当前鼠标位置的控件坐标调整页面位置
    /// </summary>
    private void CalibartePagePos()
    {
        Point ctrlPoint = PointToClient(MousePosition);
        Point absPoint = Page.AbsPoint(ctrlPoint, Page.Degree);
        //把当前页面的绝对坐标移动到对应的控件坐标处
        Page.MovePage(ctrlPoint, absPoint);
    }

    /// <summary>
    /// 旋转页面
    /// </summary>
    /// <param name="degree">角度</param>
    public void RotatePage(int degree)
    {
        Page.Degree = degree % 360;
        //ReloadBitmap();
        LoadImg(Page);
        Invalidate();
        //animationTimer.Enabled = !animationTimer.Enabled;
    }

    public int Threshold { get; set; }

    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="showAnno"></param>
    /// <param name="showQrCode"></param>
    public void CaptureScreenShot(String filePath, bool showAnno, bool showQrCode)
    {
        //new bitmap object to save the image
        Bitmap bmp = new Bitmap(this.Width, this.Height);
        Pen outterPen = new Pen(Color.Gray, 4);
        Graphics g = Graphics.FromImage(bmp);

        CImgTag tag = (CImgTag)_image.Tag;

        Bitmap targetM = ImgHelper.GetRotateImage(RawImage, Convert.ToInt16(Page.Degree));
        if (showAnno)
        {
            using (var gg = Graphics.FromImage(targetM))
            {
                Page.DrawAnnotations(gg);
            }
            //Page.DrawAnnotations(Graphics.FromImage(targetM));
        }

        g.Clear(Color.White);
        g.DrawImage(targetM, ctrlRect, imgRect, GraphicsUnit.Pixel);
        g.DrawRectangle(outterPen, new Rectangle(0, 0, bmp.Width, bmp.Height));

        if (showQrCode)
        {
            Bitmap qrcodeImg = Page.ScanPage.QrCode();

            int width = 200;
            float height = qrcodeImg.Height * width / qrcodeImg.Width;

            Rectangle destRect = new Rectangle(10, 10, width, Convert.ToInt32(height));
            g.DrawImage(qrcodeImg, destRect);
            g.DrawRectangle(outterPen, destRect);
        }

        string text = Page.ScanPage.GetSampleName() + "\n" + DateTime.Now.ToString();

        ImgHelper.DrawNoticeText(g, text, 10, bmp.Height - 40, 10);

        bmp.Save(filePath);
        g.Dispose(); // joysola
    }

    public void RefreshSize()
    {
        _ctrlWidth = base.ClientRectangle.Width - 2;
        _ctrlHeight = base.ClientRectangle.Height - 2;
    }

    //解决Label闪烁
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;
            return cp;
        }
    }

    #region 鼠标事件

    /// <summary>
    /// 鼠标压下事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FilterPreview_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            _isMouseLeftPressed = true;

        if (IsAdjusting)
        {
            iCorner = this.GetAdjustPosition(Anno, e.Location);
            if (iCorner < 0)
                this.CancleAdjust();

            return;
        }

        if (_image != null && e.Button == MouseButtons.Left)
        {
            Annotation an = Page.FixAnnotation(e.Location);
            if (an != null)
            {   //左键按下标签区域并且不是在调整大小状态
                if (DType == AnnotationType.Stop && Page.ScanPage.DbState == 0)
                {
                    Anno = an;
                    Anno.BShow = false;

                    this.ReloadImage();
                    //ReloadBitmap();
                    _annotationMovePoint = e.Location;
                    _isMoveAnnotation = true;
                    this.Cursor = Cursors.SizeAll;
                }
            }
            else if (DType == AnnotationType.Stop)
            {
                //停止画图
                _isTracking = true;
                base.Capture = true;
                _startX = e.X;
                _startY = e.Y;
                _imagePreX = Page.ImgOffset.X;
                _imagePreY = Page.ImgOffset.Y;
                _movedX = 0;
                _movedY = 0;
                Cursor = Cursors.Hand;
            }
            else if (DType == AnnotationType.MeasureLine)
            {
                //画测量线
                if (!_isDrawingAnno)
                {
                    _isDrawingAnno = true;
                    Point0 = new Point(e.X, e.Y);
                    DoMeasure(Point0);
                }
                else
                {
                    _isDrawingAnno = false;
                    this.DrawMeasure();
                    Invalidate();
                }
            }
            else
            {
                _isDrawingAnno = true;
                Cursor = Cursors.Cross;
                Point0 = new Point(e.X, e.Y);
                String strTitle = "";
                if (Constants.SettingDetail.AutoNameSwitch > 0) //自动命名
                {
                    if (DType != AnnotationType.TMA)
                    {
                        if (Constants.SettingDetail.PrefixStr.Count() > 0) //标题模板
                            strTitle = Constants.SettingDetail.PrefixStr;
                        else
                            strTitle = "Annotation";
                    }
                    //else
                    //{
                    //    if (settingInfo.TMAPrefixStr.Count() > 0)
                    //        strTitle += settingInfo.TMAPrefixStr;
                    //    else
                    //        strTitle = "TMA ";
                    //}
                }

                Point startAbs = Page.AbsPoint(Point0, Page.Degree);
                Point endAbs = startAbs;
                Point p1 = endAbs;
                Point p3 = endAbs;

                Anno = new Annotation(strTitle, DType, startAbs, p1, endAbs, p3, Constants.AnnoationColor, Constants.AnnoationPenWidth);

                int x = startAbs.X;
                int y = startAbs.Y;
                //根据不同的标签类型画标签
                if (DType == AnnotationType.FixCircle || DType == AnnotationType.TMA)
                {
                    Decimal dr = (Decimal)Constants.SettingDetail.CircularRadius;
                    double d = Decimal.ToDouble(dr);
                    if (Constants.SettingDetail.CircularUnit > 0)
                        d = d * 1000;

                    d = d > 0 ? d : Constants.FixCircleR; //um
                    int r = Convert.ToInt32(d / (Page.ScanPage.ConvertUnit() * 2));
                    startAbs = new Point(x - r, y - r);
                    endAbs = new Point(x + r, y + r);
                }
                else if (DType == AnnotationType.FixRectangle)
                {
                    Decimal deciw = (Decimal)Constants.SettingDetail.RectAngleWidth;
                    Decimal decih = (Decimal)Constants.SettingDetail.RectAngleHeight;
                    double dw = Decimal.ToDouble(deciw);
                    double dh = decimal.ToDouble(decih);
                    if (Constants.SettingDetail.CircularUnit > 0)
                    {
                        dw = dw * 1000;
                        dh = dh * 1000;
                    }
                    dw = dw > 0 ? dw : Constants.FixRectangleWidth;
                    dh = dh > 0 ? dh : Constants.FixRectangleHeight; //um
                    int ww = Convert.ToInt32(dw / (Page.ScanPage.ConvertUnit() * 2));
                    int hh = Convert.ToInt32(dh / (Page.ScanPage.ConvertUnit() * 2));
                    startAbs = new Point(x - ww, y - hh);
                    p1 = new Point(x + ww, y - hh);
                    endAbs = new Point(x + ww, y + hh);
                    p3 = new Point(x - ww, y + hh);
                }
                else if (DType == AnnotationType.Arrow)
                {
                    endAbs = new Point(0, 0);
                }

                Anno.SetLocation(startAbs, p1, endAbs, p3, Page.ScanPage.ConvertUnit());
            }
        }
    }

    /// <summary>
    /// 鼠标移动事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FilterPreview_MouseMove(object sender, MouseEventArgs e)
    {
        if (IsAdjusting) //正在调整
        {
            if (iCorner >= 0 && _isMouseLeftPressed)
            {
                this.AdjustAnnotation(iCorner, e.Location);
                Invalidate();
            }
            return;
        }

        if ((_isMouseLeftPressed && _image != null))
        {
            if (_isMoveAnnotation && Anno != null) //点击区域为标注内
            {
                int movex = e.Location.X - _annotationMovePoint.X;
                int movey = e.Location.Y - _annotationMovePoint.Y;
                _annotationMovePoint = e.Location;
                Point p0 = Page.ImgCtrlPoint(Anno.Graph.Point0, Page.Degree);
                Point p1 = Page.ImgCtrlPoint(Anno.Graph.Point1, Page.Degree);
                Point p2 = Page.ImgCtrlPoint(Anno.Graph.Point2, Page.Degree);
                Point p3 = Page.ImgCtrlPoint(Anno.Graph.Point3, Page.Degree);

                Anno.Graph.Point0 = Page.AbsPoint(new Point(p0.X + movex, p0.Y + movey), Page.Degree);
                Anno.Graph.Point1 = Page.AbsPoint(new Point(p1.X + movex, p1.Y + movey), Page.Degree);
                Anno.Graph.Point2 = Page.AbsPoint(new Point(p2.X + movex, p2.Y + movey), Page.Degree);
                Anno.Graph.Point3 = Page.AbsPoint(new Point(p3.X + movex, p3.Y + movey), Page.Degree);

                this.Invalidate();
            }
            else if (_isTracking)
            {
                //移动样本
                int num = e.X - _startX;
                int num2 = e.Y - _startY;

                Page.ImgOffset = new Point(_imagePreX - num, _imagePreY - num2);

                _movedX += num;
                _movedY += num2;

                _rectX = mRect.Location.X + num;
                _rectY = mRect.Location.Y + num2;
                //LoadBufferEdge();
            }
            else if (DType != AnnotationType.Stop && _isDrawingAnno)
            {
                Point2 = e.Location;
                if (Anno != null && DType != AnnotationType.FixCircle
                    && DType != AnnotationType.FixRectangle && DType != AnnotationType.TMA && DType != AnnotationType.Arrow)
                {
                    Point sAbsPoint = Page.AbsPoint(Point0, Page.Degree);
                    Point eAbsPoint = Page.AbsPoint(Point2, Page.Degree);
                    Point p1 = new Point(eAbsPoint.X, sAbsPoint.Y);
                    Point p3 = new Point(sAbsPoint.X, eAbsPoint.Y);

                    Anno.SetLocation(sAbsPoint, p1, eAbsPoint, p3, Page.ScanPage.ConvertUnit());
                }
            }

            Invalidate();
        }
        else //左键未按下
        {
            if (_isDrawingAnno && DType == AnnotationType.MeasureLine && Anno != null && Anno.Graph.Type == AnnotationType.MeasureLine)
            {
                Point2 = e.Location;
                Point sAbsPoint = Page.AbsPoint(Point0, Page.Degree);
                Point eAbsPoint = Page.AbsPoint(Point2, Page.Degree);
                Point p1 = new Point(eAbsPoint.X, sAbsPoint.Y);
                Point p3 = new Point(sAbsPoint.X, eAbsPoint.Y);

                Anno.SetLocation(sAbsPoint, p1, eAbsPoint, p3, Page.ScanPage.ConvertUnit());

                Invalidate();
            }
        }
    }

    private void FilterPreview_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isMouseLeftPressed = false;
            if (IsAdjusting)
                return;

            if (_isMoveAnnotation && Anno != null)
            {
                Page.MoveAnnotation(Anno);
                Anno.BShow = true;
                using (var gg = Graphics.FromImage(Image)) // joysola
                {
                    Page.DrawAnnotation(gg, Anno, Page.ImgOffset);
                }
                //Page.DrawAnnotation(Graphics.FromImage(Image), Anno, Page.ImgOffset);
                //this.SetImage(Page.JoinImage());

                Anno = null;
                _isMoveAnnotation = false;
                this.Cursor = Cursors.Default;
            }
            else if (!_isTracking && DType != AnnotationType.Stop && DType != AnnotationType.MeasureLine)
            {
                if (Anno != null && Anno.Check())
                {
                    int id = Page.ScanPage.InsertAnnotationToDb(Anno);
                    if (id > 0)
                    {
                        Anno.Id = id;
                        if (Constants.SettingDetail.AutoNumSwitch > 0)
                            Anno.Title += id.ToString();
                        Page.ScanPage.Annotations.Add(Anno);
                    }
                    using (var gg = Graphics.FromImage(Image)) // joysola
                    {
                        Page.DrawAnnotation(gg, Anno, Page.ImgOffset);
                    }
                    //Page.DrawAnnotation(Graphics.FromImage(Image), Anno, Page.ImgOffset);
                }
                else
                {
                    ReloadBitmap();
                }

                Anno = null;
                base.Capture = false;
            }
            else
            {
                if (_isTracking && DType == AnnotationType.Stop)
                {
                    _isTracking = false;
                    base.Capture = false;
                    Cursor = Cursors.Default;
                    ImageEventArgs imageEventArgs = new ImageEventArgs();
                    //MoveR(imageEventArgs);
                    Point ctrlPoint = PointToClient(MousePosition);
                    LoadBufferEdge(ctrlPoint);

                    if (this.NeedPic != null && (imageEventArgs.NeedLeftPic || imageEventArgs.NeedRightPic || imageEventArgs.NeedTopPic || imageEventArgs.NeedBottomPic))
                    {
                        this.NeedPic(this, imageEventArgs);
                    }
                }
            }
        }
        else //右键up
        {
            if (IsAdjusting) //正在调整大小
            {
                this.CancleAdjust();
            }
            else
            {
                Annotation an = Page.FixAnnotation(e.Location);
                if (an != null && !IsAdjusting && DType == AnnotationType.Stop) //点击标签范围内,未在调整大小
                {
                    Anno = an;
                    ContextMenuStrip menu = new TileImageViewer.RightClickMenu(this, Anno);
                    menu.Show(this, new Point(e.X, e.Y));
                }
            }
        }
    }

    #endregion 鼠标事件

    /// <summary>
    /// 获取某点的颜色信息
    /// </summary>
    /// <param name="imgCtrlPoint">控件坐标</param>
    /// <returns></returns>
    public Color GetImgPixel(Point imgCtrlPoint)
    {
        if (this.Image == null && RotateScaleAndColorTaskStatus == TaskStatus.Running)
        {
            //有任务执行时不获取，优化性能

            return new Color();
        }

        Point retPoint = new Point();
        Color ret = new Color();
        retPoint = new Point(Convert.ToInt32((Page.ImgOffset.X + imgCtrlPoint.X - Page.CtrlOffset.X)),
            Convert.ToInt32((Page.ImgOffset.Y + imgCtrlPoint.Y - Page.CtrlOffset.Y)));
        if (retPoint.X > 0 && retPoint.Y > 0 && retPoint.X < this.Image.Width && retPoint.Y < this.Image.Height)
        {
            ret = this.Image.GetPixel(retPoint.X, retPoint.Y);
        }

        return ret;
    }

    /// <summary>
    /// 加载更多区域的图像
    /// </summary>
    /// <param name="ctrlPoint"></param>
    public void LoadBufferEdge(Point ctrlPoint)
    {
        Point absPoint = Page.AbsPoint(ctrlPoint, Page.Degree);
        bool refreshed = Page.Refresh(ctrlPoint, ClientSize, Convert.ToInt16(Page.Degree));
        if (refreshed)
        {
            SetImage(JoinImageSync());
            CalibartePagePos();

            //  JoinImageAsync(true);
        }
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
        //处理基础变量
        Graphics graphics = pe.Graphics;
        Rectangle clientRectangle = base.ClientRectangle;
        int num = clientRectangle.Width;
        int num2 = clientRectangle.Height;
        if (_image != null)
        {
            num = _ctrlWidth + 2;
            num2 = _ctrlHeight + 2;
        }
        int num3 = Convert.ToInt16((clientRectangle.Width - num) / 2);
        int num4 = Convert.ToInt16((clientRectangle.Height - num2) / 2);
        //graphics.DrawRectangle(_pen, 0, 0, clientRectangle.Width - 1, clientRectangle.Height - 1);
        //BorderX = num3;
        //BorderY = num4;
        num3++;
        num4++;
        //LoadImgThread.Join();

        if (_image != null)
        {
            //开始渲染
            Page.CtrlOffset = new Point(num3, num4);

            //在控件的位置及大小画图像的对应部分
            if (imgRect == null || imgRect.Width == 0 || animates.Count == 0)
            {
                Rectangle rc = new Rectangle(-Page.ImgOffset.X, -Page.ImgOffset.Y,
                  Page.PageWidthInt() - Math.Min(Page.ImgOffset.X, 0),
                  Page.PageHeightInt() - Math.Min(Page.ImgOffset.Y, 0));
                ctrlRect = rc;
                imgRect = new Rectangle(0, 0,
                    Convert.ToInt32(rc.Width),
                    Convert.ToInt32(rc.Height));
            }
            if (animates.Count == 0)
            {
                //确定没有动画在执行时，才画标注
                if (NeedDrawAnno)
                {
                    //Page.DrawAnnotations(Graphics.FromImage(this.Image));
                    using (var gg = Graphics.FromImage(this.Image))
                    {
                        Page.DrawAnnotations(gg);
                    }
                }
                NeedDrawAnno = false;

                if (NeedDrawMeasure)
                {
                    using (Graphics g = Graphics.FromImage(this.Image))
                    {
                        Page.DrawAnnotation(g, Anno, Page.ImgOffset);
                    }
                }
                NeedDrawMeasure = false;
            }

            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //WaitingForRender();
            // graphics.RotateTransform(45);

            try
            {
                if (preLoad)
                {
                    //根据某预加载的区域强制绘制
                    graphics.DrawImage(preLoadImg, preLoadCtrlRect, preLoadImgRect, GraphicsUnit.Pixel);
                }
                else
                {
                    CImgTag cImgTag = (CImgTag)_image.Tag;
                    if (cImgTag.Degree == Page.Degree)
                    {
                        graphics.DrawImage(_image, ctrlRect, imgRect, GraphicsUnit.Pixel);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Warn("onpaint image is error ：" + e.Message);
            }

            // 画调试信息
            if (Constants.SettingDetail.ShowDebugInfoSwitch == 1)
            {
                CImgTag tag = (CImgTag)_image.Tag;

                int degree = 0;

                if (tag != null)
                {
                    degree = Convert.ToInt32(tag.Degree);
                }

                double[] edgeDists = Page.MaxEdgeDistance2(Page.VisableMapRectangle(this.Size), this.Size, degree);

                if (degree != 0)
                {
                    double angle = tag.Degree % 90;
                    double sinx = Math.Sin(ImgHelper.DegreeToRaidens(angle));
                    double cosx = Math.Cos(ImgHelper.DegreeToRaidens(angle));

                    graphics.DrawRectangle(new Pen(Color.Red),
                          new Rectangle(-Page.ImgOffset.X + tag.RotateCenter.X,
                          -Page.ImgOffset.Y + tag.RotateCenter.Y, 10, 10));

                    Point[] points = Page.MapRectangle.CornerImgCtrlPoints(Page.Scale, tag.Degree, Page.ImgOffset);
                    for (int i = 0; i < points.Length; i++)
                    {
                        Point offsetedPoint = points[i];

                        graphics.DrawRectangle(new Pen(Color.Red),
                        new Rectangle(offsetedPoint.X, offsetedPoint.Y, 10, 10));
                        ImgHelper.DrawNoticeText(graphics, Convert.ToString(i) + "," + edgeDists[i], offsetedPoint.X, offsetedPoint.Y);
                    }
                }
            }

            if (Anno != null)
            {
                if ((DType != AnnotationType.Stop && _isDrawingAnno) || _isMoveAnnotation || IsAdjusting)
                {
                    Page.DrawAnnotation(graphics, Anno, new Point(0, 0));
                }
            }

            base.OnPaint(pe);
        }
    }

    #region 清理内存

    public void ClearMemory()
    {
        if (InitFullPageTaskStatus == TaskStatus.Running)
        {
            try
            {
                //Task.WaitAll(new Task[] { InitFullPageBufferTask });
            }
            catch (Exception)
            {
                Console.WriteLine($"\n{nameof(Exception)} thrown\n");
            }
            finally
            {
                InitFullPageBufferTaskTokenSource.Dispose();
            }
        }

        Page?.ClearBitmapBuffer();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ClearMemory();
        }

        base.Dispose(disposing);
    }

    #endregion 清理内存

    #region 测量边界

    private int GetLeftDis()
    {
        if (_image == null)
        {
            return -1;
        }
        return Page.ImgOffset.X;
    }

    private int GetRightDis()
    {
        if (_image == null)
        {
            return -1;
        }
        return _image.Width - _ctrlWidth - Page.ImgOffset.X;
    }

    private int GetTopDis()
    {
        if (_image == null)
        {
            return -1;
        }
        return Page.ImgOffset.Y;
    }

    public int GetBottomDis()
    {
        if (_image == null)
        {
            return -1;
        }
        return _image.Height - _ctrlHeight - Page.ImgOffset.Y;
    }

    #endregion 测量边界

    /// <summary>
    /// 开始画标注
    /// </summary>
    public void DrawAnnotations()
    {
        NeedDrawAnno = true;
    }

    /// <summary>
    /// 测量可见视野的绝对坐标范围
    /// </summary>
    /// <returns>绝对坐标边框</returns>
    public MAbsRectangle VisableAbsRect()
    {
        Point leftTopAbPoint = Page.AbsPoint(new Point(0, 0), Page.Degree);
        Point rightTopAP = Page.AbsPoint(new Point(Size.Width, 0), Page.Degree);
        Point rightBottomAbPoint = Page.AbsPoint(new Point(Size.Width, Size.Height), Page.Degree);
        Point leftBottomAbPoint = Page.AbsPoint(new Point(0, Size.Height), Page.Degree);

        return new MAbsRectangle(leftTopAbPoint, rightTopAP, leftBottomAbPoint, rightBottomAbPoint);
    }

    /// <summary>
    /// 读取保存的标注并绘制
    /// </summary>
    public void LoadAnnotations()
    {
        Page.ScanPage.LoadAnnotationFromDb();

        this.DrawAnnotations();
    }

    #region 测量功能相关

    private void DoMeasure(Point p)
    {
        if (Anno != null && Anno.Graph.Type == AnnotationType.MeasureLine)
        {
            Anno = null;
            //this.SetImage(Page.JoinImage());
            ReloadBitmap();
        }
        Anno = new Annotation("", AnnotationType.MeasureLine, new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0), Color.Gray, 3);
    }

    public void DrawMeasure()
    {
        if (DType == AnnotationType.MeasureLine && Anno != null)
        {
            NeedDrawMeasure = true;
        }
    }

    public void StartMeasure()
    {
        this.CancleAdjust();

        _isDrawingAnno = false;
        DType = AnnotationType.MeasureLine;
        this.Cursor = Cursors.Cross;

        this.ReloadImage();
    }

    public void CancleMeasure()
    {
        if (DType == AnnotationType.MeasureLine)
        {
            _isDrawingAnno = false;
            DType = AnnotationType.Stop;
            this.Cursor = Cursors.Default;
            Anno = null;
            //this.SetImage(Page.JoinImage());
            //ReloadBitmap();
            this.ReloadImage();
        }
    }

    public void StartAdjust(Annotation an)
    {
        this.CancleMeasure();
        DType = AnnotationType.Stop;
        _isDrawingAnno = false;
        this.Cursor = Cursors.Default;
        if (an != null && (an.Graph.Type == AnnotationType.Rectangle || an.Graph.Type == AnnotationType.Circle || an.Graph.Type == AnnotationType.Line))
        {
            Anno = an;
            Anno.BShow = false;
            //ReloadBitmap();
            this.ReloadImage();

            Anno.Graph.Adjusting = true;
            IsAdjusting = true;
        }
    }

    public void CancleAdjust()
    {
        if (IsAdjusting)
        {
            if (Anno != null)
            {
                Anno.BShow = true;
                Anno.Graph.Adjusting = false;
                using (var gg = Graphics.FromImage(Image)) // joysola
                {
                    Page.DrawAnnotation(gg, Anno, Page.ImgOffset);
                }
                //Page.DrawAnnotation(Graphics.FromImage(Image), Anno, Page.ImgOffset);
                Page.ScanPage.UpdateAnnotationToDb(Anno);
                Anno = null;
            }

            IsAdjusting = false;
        }

        iCorner = -1;
    }

    private int GetAdjustPosition(Annotation an, Point location)
    {
        if (an == null || !an.Graph.Adjusting)
            return -1;

        int iRet = -1;
        Rectangle[] rt = an.Graph.Corners;
        for (int i = 0; i < rt.Length; i++)
        {
            if (((location.X) > rt[i].Left) && ((location.X) < rt[i].Right)) //x坐标
            {
                if ((location.Y) > rt[i].Top && ((location.Y) < rt[i].Bottom))
                {
                    iRet = i;
                    break;
                }
            }
        }

        switch (iRet)
        {
            case 0:
                {
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                }
            case 1:
                {
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                }
            case 2:
                {
                    this.Cursor = Cursors.SizeNESW;
                    break;
                }
            case 3:
                {
                    this.Cursor = Cursors.SizeNESW;
                    break;
                }
            default:
                {
                    this.Cursor = Cursors.Default;
                    break;
                }
        }

        return iRet;
    }

    private void AdjustAnnotation(int iRet, Point location)
    {
        Point abs = Page.AbsPoint(location, Page.Degree);
        switch (iRet)
        {
            case 0: //left-top
                {
                    if (Anno.Graph.Type == AnnotationType.Line)
                    {
                        Anno.Graph.Point0 = abs;
                    }
                    else if (abs.X < Anno.Graph.Point2.X && abs.Y < Anno.Graph.Point2.Y)
                    {
                        Anno.Graph.Point0 = abs;
                        int x = Anno.Graph.Point1.X;
                        Anno.Graph.Point1 = new Point(x, abs.Y);
                        int y = Anno.Graph.Point3.Y;
                        Anno.Graph.Point3 = new Point(abs.X, y);
                    }
                    break;
                }
            case 1: //right-bottom
                {
                    if (Anno.Graph.Type == AnnotationType.Line)
                    {
                        Anno.Graph.Point2 = abs;
                    }
                    else if (abs.X > Anno.Graph.Point0.X && abs.Y > Anno.Graph.Point0.Y)
                    {
                        Anno.Graph.Point2 = abs;
                        int y = Anno.Graph.Point1.Y;
                        Anno.Graph.Point1 = new Point(abs.X, y);
                        int x = Anno.Graph.Point3.X;
                        Anno.Graph.Point3 = new Point(x, abs.Y);
                    }
                    break;
                }
            case 2: //right-top
                {
                    if (abs.X > Anno.Graph.Point0.X && abs.Y < Anno.Graph.Point2.Y)
                    {
                        Anno.Graph.Point1 = abs;
                        int x = Anno.Graph.Point0.X;
                        Anno.Graph.Point0 = new Point(x, abs.Y);
                        int y = Anno.Graph.Point2.Y;
                        Anno.Graph.Point2 = new Point(abs.X, y);
                    }
                    break;
                }
            case 3: //left-bottom
                {
                    if (abs.X < Anno.Graph.Point2.X && abs.Y > Anno.Graph.Point0.Y)
                    {
                        Anno.Graph.Point3 = abs;
                        int y = Anno.Graph.Point0.Y;
                        Anno.Graph.Point0 = new Point(abs.X, y);
                        int x = Anno.Graph.Point2.X;
                        Anno.Graph.Point2 = new Point(x, abs.Y);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        Anno.SetLocation(Anno.Graph.Point0, Anno.Graph.Point1, Anno.Graph.Point2, Anno.Graph.Point3, Page.ScanPage.ConvertUnit());
    }

    #endregion 测量功能相关

    #region 异步执行任务

    /// <summary>
    /// 旋转、缩放及修改颜色
    /// </summary>
    /// <param name="onlyChangeColor"></param>
    /// <param name="initBitmap"></param>
    /// <returns></returns>
    public async Task RotateScaleAndColorTaskAsync(bool onlyChangeColor, Bitmap initBitmap)
    {
        logger.Debug("RotateScaleAndColorTaskAsync Start");

        RotateScaleAndColorTaskTokenSource = new CancellationTokenSource();
        RotateScaleAndColorTaskStatus = TaskStatus.Created;
        //byte[] imgBytes = ImgHelper.Bitmap2Byte(initBitmap);
        //var tag = initBitmap.Tag;

        Func<Bitmap> initTask = new Func<Bitmap>(() =>
        {
            //Bitmap bitmapM =  ImgHelper.BytesToBitmap(imgBytes);
            //bitmapM.Tag = tag;
            return initBitmap;
        });

        RotateScaleAndColorTaskStatus = TaskStatus.Running;

        Bitmap bitmap = null;
        if (onlyChangeColor)
        {
            bitmap = await Task.Run(initTask, RotateScaleAndColorTaskTokenSource.Token)
                          .ContinueWith(ChangeImageColorCorrectionSync);
        }
        else
        {
            bitmap = await Task.Run(initTask, RotateScaleAndColorTaskTokenSource.Token)
            .ContinueWith(ChangeImageByPageScaleAndDegreeSync)
            .ContinueWith(ChangeImageColorCorrectionSync);
        }
        _image = bitmap;

        if (Page != null && Page.MapRectangle.IsFullLevelMapR())
        {
            ScanPageCache.PutFullPageCache(Page.ScanPage.GetSampleName(), Page.MapRectangle.Level, bitmap);
        }

        ImgSize = bitmap.Size;
        logger.Debug("RotateScaleAndColorTaskAsync DrawAnnoAndMeasure");

        DrawAnnoAndMeasure();
        preLoad = false;

        LoadDone();
        RotateScaleAndColorTaskStatus = TaskStatus.RanToCompletion;

        Invalidate();
        logger.Debug("RotateScaleAndColorTaskAsync  Done");
    }

    /// <summary>
    /// 异步修改颜色
    /// </summary>
    public void ChangeColorAsync()
    {
        RotateScaleAndColorTaskAsync(true, _image);
    }

    /// <summary>
    /// 异步拼接图像
    /// </summary>
    /// <param name="calibartePagePos"></param>
    /// <returns></returns>
    public async Task JoinImageAsync(bool calibartePagePos)
    {
        JoinImageTaskTokenSource = new CancellationTokenSource();
        JoinImageTaskStatus = TaskStatus.Created;
        Func<Bitmap> JoinImageTask = new Func<Bitmap>(JoinImageSync);
        JoinImageTaskStatus = TaskStatus.Running;
        CancellationToken token;
        if (InitFullPageBufferTaskTokenSource == null) // 防止token为空时报错
        {
            token = CancellationToken.None;
        }
        else
        {
            token = InitFullPageBufferTaskTokenSource.Token;
        }
        //Bitmap bitmap = await Task.Run(JoinImageTask, InitFullPageBufferTaskTokenSource.Token);
        Bitmap bitmap = await Task.Run(JoinImageTask, token);
        SetImage(bitmap);
        if (calibartePagePos)
        {
            CalibartePagePos();
        }
        LoadDone();
    }

    /// <summary>
    /// 初始化图像缓存
    /// </summary>
    /// <returns></returns>
    public async Task InitFullPageBitmapBuffer()
    {
        ImageEventArgs eventArgs = new ImageEventArgs();
        eventArgs.Tips = "Loading...";
        if (this.ChangeTips != null)
        {
            ChangeTips(this, eventArgs);
        }
        InitFullPageBufferTaskTokenSource = new CancellationTokenSource();
        InitFullPageTaskStatus = TaskStatus.Created;
        Action InitFullPageBufferTask = new Action(InitFullPageBitmapBufferSync);
        InitFullPageTaskStatus = TaskStatus.Running;
        await Task.Run(InitFullPageBufferTask, InitFullPageBufferTaskTokenSource.Token);
        LoadDone();
    }

    public void InitFullPageBitmapBufferSync()
    {
        for (int i = Constants.MemoryCachedMinLevel; i < Page.ScanPage.MaxLevel; i++)
        {
            if (InitFullPageBufferTaskTokenSource.IsCancellationRequested)
            {
                InitFullPageTaskStatus = TaskStatus.Canceled;
                return;
            }
            Page.InitFullPageBitmapBuffer(i);
        }
        InitFullPageTaskStatus = TaskStatus.RanToCompletion;
    }

    public void CancelAllTask()
    {
        if (InitFullPageTaskStatus == TaskStatus.Running)
        {
            InitFullPageBufferTaskTokenSource.Cancel();
            //Page.ClearBitmapBuffer();
        }
        if (JoinImageTaskStatus == TaskStatus.Running)
        {
            JoinImageTaskTokenSource.Cancel();
        }
        if (RotateScaleAndColorTaskStatus == TaskStatus.Running)
        {
            RotateScaleAndColorTaskTokenSource.Cancel();
        }
    }

    #endregion 异步执行任务

    #region 图像处理相关功能

    /// <summary>
    /// 图像拼接
    /// </summary>
    /// <returns></returns>
    public Bitmap JoinImageSync()
    {
        return Page.JoinImage(GetImgTag());
    }

    /// <summary>
    /// 图像放大及旋转
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private Bitmap ChangeImageByPageScaleAndDegreeSync(Task<Bitmap> task)
    {
        Bitmap bitmap = task.Result;
        CImgTag tag = (CImgTag)bitmap.Tag;

        Bitmap m = bitmap;
        if (Page.Scale != 1)
        {
            m = ImgHelper.ScaleBitmap(bitmap, Convert.ToInt16(bitmap.Width * Page.Scale));
        }
        if (tag.Degree != Page.Degree)
        {
            m = ImgHelper.GetRotateImage((Bitmap)m.Clone(), Convert.ToInt16(Page.Degree));
            tag.Degree = Page.Degree;
        }

        RawImage = (Bitmap)m.Clone();
        RawImage.Tag = tag;

        return m;
    }

    /// <summary>
    /// 图像改变颜色
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private Bitmap ChangeImageColorCorrectionSync(Task<Bitmap> task)
    {
        Bitmap bitmap = task.Result;
        CImgTag tag = (CImgTag)bitmap.Tag;
        ColorCorrection colorCorr = Constants.ColorCorrectionDetail;

        if (tag.ColorCorrection == null && Constants.ColorCorrectionDetail.Equals(ColorCorrection.DefaultSetting()))
        {
            return bitmap;
        }

        if (!colorCorr.Equals(tag.ColorCorrection))
        {
            return ChangeImageColorCorrectionSync(colorCorr, bitmap);
        }
        return bitmap;
    }

    private Bitmap ChangeImageColorCorrectionSync(ColorCorrection colorCor, Bitmap bitmap)
    {
        if (colorCor == null || colorCor.Equals(ColorCorrection.DefaultSetting()))
        {
            //不需要改变颜色
            bitmap = RawImage;
            return bitmap;
        }

        bitmap = ImgHelper.BrightnessAndContrastOpenCV(RawImage, colorCor);
        ((CImgTag)(bitmap.Tag)).ColorCorrection = colorCor.Clone();
        return bitmap;
    }

    #endregion 图像处理相关功能
}
using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// imgCtrl 的数据模型
/// </summary>
public class ImgCtrlPage : IDisposable
{
    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    //当前显示的地图范围
    public MapRectangle MapRectangle { get; set; }

    //扫描样本
    public ScanPage ScanPage { get; set; }

    //角度
    public int Degree;

    //偏离坐标
    private Point _imgOffSet;

    public Point ImgOffset { get { return _imgOffSet; } set { _imgOffSet = value; } }

    //当前层
    private int _currLevel;

    //放大倍率
    public float Scale { get; set; } = 1;

    public float ImgCtrlScaleVal { get; set; } = 1;

    public Point CtrlOffset { get; set; } = new Point(1, 1);

    //放大前的基础图象
    public Bitmap ScaledBaseBitmap;

    //图像缓存
    public Dictionary<int, Bitmap> BitmapBuffer = new Dictionary<int, Bitmap>();

    //地图缓存
    public Dictionary<int, MapRectangle> MapRectangleBuffer = new Dictionary<int, MapRectangle>();

    /// <summary>
    /// 构造函数
    /// </summary>
    private ImgCtrlPage()
    {
    }

    public ImgCtrlPage(ScanPage scanPage)
    {
        ScanPage = scanPage;
    }

    /// <summary>
    /// 构造函数，根据文件位置创建
    /// </summary>
    /// <param name="filePath"></param>
    public ImgCtrlPage(string filePath)
    {
        //默认加载第三层
        int startPageLevel = 3;

        ScanPage = new ScanPage(filePath);
        for (int i = 1; i <= startPageLevel; i++)
        {
            ScanPage.GetColsRows(i);
        }
        CurrLevel = startPageLevel;
        ImgOffset = new Point(0, 0);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        ScanPage?.Dispose();
        ScanPage = null;

        ScaledBaseBitmap?.Dispose();
        ScaledBaseBitmap = null;

        foreach (var item in BitmapBuffer)
        {
            item.Value.Dispose();
        }
        BitmapBuffer.Clear();
    }

    //清除缓存
    public void ClearBitmapBuffer()
    {
        BitmapBuffer = new Dictionary<int, Bitmap>();
        GC.Collect();
    }

    public void LoadScaledBitmap()
    {
        ScaledBaseBitmap = ScanPage.JoinImageByLevel(5);
    }

    public ScanPageLevel GetCurretColsRows()
    {
        return ScanPage.GetColsRows(CurrLevel);
    }

    /// <summary>
    /// 将图像中心点移动到控件中心（重新加载图像后使用）
    /// </summary>
    /// <param name="ctrlPoint">控件位置</param>
    /// <param name="controlSize">控件尺寸</param>
    /// <param name="degree">旋转角度</param>
    /// <returns>是否移动</returns>
    public bool Refresh(Point ctrlPoint, Size controlSize, int degree)
    {
        foreach (KeyValuePair<int, MapRectangle> kvp in MapRectangleBuffer)
        {
            //TODO 确定Imgoffset

            // Refresh(ctrlPoint, VisableMapRectangle(kvp.Value, controlSize), controlSize, tag);
        }

        MapRectangle visMapR = VisableMapRectangle(controlSize);
        Point oldAbsPoint = AbsPoint(ctrlPoint, degree);
        MapRectangle refreshedMapR = ImgCtrlTransHelper.RefreshN(ScanPage, MapRectangle.Level, ctrlPoint, controlSize, oldAbsPoint, degree, Scale);
        // bool ret = ImgCtrlTransHelper.Refresh(ScanPage, MapRectangle,  visMapR, controlSize, degree, ImgOffset);
        bool ret = false;
        if (refreshedMapR.GetHashCode() != MapRectangle.GetHashCode())
        {
            ret = true;
            MapRectangle = refreshedMapR;
            //  以鼠标点为基点，移动
        }
        Point newCtrlPoint = ImgCtrlPoint(oldAbsPoint, degree);
        _imgOffSet.Offset(Convert.ToInt32(newCtrlPoint.X - ctrlPoint.X), Convert.ToInt32(newCtrlPoint.Y - ctrlPoint.Y));

        return ret;
    }

    /// <summary>
    /// 计算在某一地图区域在控件中显示的四边长度
    /// </summary>
    /// <param name="visMapR">区域</param>
    /// <param name="controlSize">控件尺寸</param>
    /// <param name="degree">角度</param>
    /// <returns></returns>
    public double[] MaxEdgeDistance2(MapRectangle visMapR, Size controlSize, int degree)
    {
        Point[] points = MapRectangle.CornerImgCtrlPoints(Scale, degree, ImgOffset);

        return ImgCtrlTransHelper.MaxEdgeDistance2(points, ImgOffset, visMapR, controlSize, degree, Scale);
    }

    public int CurrLevel
    {
        get
        {
            return _currLevel;
        }
        set
        {
            ScanPage.CalcColsRows(value);
            MapRectangle = new MapRectangle(ScanPage.GetColsRows(value));
            MapRectangle.Level = value;
            _currLevel = value;
        }
    }

    /// <summary>
    /// 初始化某层的图像缓存
    /// </summary>
    /// <param name="level">层</param>
    public void InitFullPageBitmapBuffer(int level)
    {
        Bitmap bitmap = ScanPage.JoinImageByLevel(level);
        BitmapBuffer.Add(level, bitmap);
    }

    /// <summary>
    /// 通过控件坐标把页面移动到某绝对坐标
    /// </summary>
    /// <param name="ctrlPoint">控件坐标</param>
    /// <param name="absPoint">绝对坐标</param>
    /// <returns></returns>
    public Point MovePage(Point ctrlPoint, Point absPoint)
    {
        Point targetCtrlPoint = ImgCtrlPoint(absPoint, Degree);
        _imgOffSet = new Point(ImgOffset.X + targetCtrlPoint.X - ctrlPoint.X, ImgOffset.Y + targetCtrlPoint.Y - ctrlPoint.Y);
        return _imgOffSet;
    }

    #region 画标记相关代码

    /// <summary>
    /// 在切片图像上绘画标注
    /// </summary>
    /// <param name="g">图像引用</param>
    public void DrawAnnotations(Graphics g)
    {
        if (ScanPage.Annotations == null)
        {
            return;
        }

        foreach (Annotation an in ScanPage.Annotations)
        {
            if (an.BShow)
            {
                Point p0 = this.ImgCtrlPoint(an.Graph.Point0, Degree);
                Point p2 = this.ImgCtrlPoint(an.Graph.Point2, Degree);
                Point p1 = this.ImgCtrlPoint(an.Graph.Point1, Degree);
                Point p3 = this.ImgCtrlPoint(an.Graph.Point3, Degree);

                an.Graph.Draw(g, p0, p1, p2, p3, ImgOffset);
            }
        }

        foreach (Annotation an in ScanPage.Annotations)
        {
            if (an.BShow)
            {
                Point p1 = this.ImgCtrlPoint(an.Graph.Point0, Degree);
                Point p2 = this.ImgCtrlPoint(an.Graph.Point2, Degree);

                an.DrawDetail(g, p1, p2, ImgOffset);
            }
        }
    }

    /// <summary>
    /// 绘画单个标注
    /// </summary>
    /// <param name="g">图像</param>
    /// <param name="an">标注</param>
    /// <param name="Offset">偏移量</param>
    public void DrawAnnotation(Graphics g, Annotation an, Point Offset)
    {
        Point p0 = this.ImgCtrlPoint(an.Graph.Point0, Degree);
        Point p1 = this.ImgCtrlPoint(an.Graph.Point1, Degree);
        Point p2 = this.ImgCtrlPoint(an.Graph.Point2, Degree);
        Point p3 = this.ImgCtrlPoint(an.Graph.Point3, Degree);

        an.Graph.Draw(g, p0, p1, p2, p3, Offset);
        an.DrawDetail(g, p0, p2, Offset);
    }

    /// <summary>
    /// 画标注的图形（圆形、方形）
    /// </summary>
    /// <param name="g"></param>
    /// <param name="an"></param>
    /// <param name="Offset"></param>
    public void DrawAnnotationGraph(Graphics g, Annotation an, Point Offset)
    {
        if (an.BShow)
        {
            Point p0 = this.ImgCtrlPoint(an.Graph.Point0, Degree);
            Point p1 = this.ImgCtrlPoint(an.Graph.Point1, Degree);
            Point p2 = this.ImgCtrlPoint(an.Graph.Point2, Degree);
            Point p3 = this.ImgCtrlPoint(an.Graph.Point3, Degree);

            an.Graph.Draw(g, p0, p1, p2, p3, Offset);
        }
    }

    /// <summary>
    /// 画标注的明细（标注内容、如周长，面积）
    /// </summary>
    /// <param name="g"></param>
    /// <param name="an"></param>
    /// <param name="Offset"></param>
    public void DrawAnnotationDetail(Graphics g, Annotation an, Point Offset)
    {
        if (an.BShow)
        {
            Point p1 = this.ImgCtrlPoint(an.Graph.Point0, Degree);
            Point p2 = this.ImgCtrlPoint(an.Graph.Point2, Degree);

            an.DrawDetail(g, p1, p2, Offset);
        }
    }

    /// <summary>
    /// 移动标注
    /// </summary>
    /// <param name="an">标注</param>
    public void MoveAnnotation(Annotation an)
    {
        ScanPage.UpdateAnnotationToDb(an);
    }

    /// <summary>
    /// 鼠标点击时，确定点击的是哪个标注
    /// </summary>
    /// <param name="ctrlPoint">点击点的控件坐标</param>
    /// <returns>标注</returns>
    public Annotation FixAnnotation(Point ctrlPoint)
    {
        Annotation anno = null;
        if (ScanPage.DbState == 0)
        {
            int x = ctrlPoint.X + ImgOffset.X;
            int y = ctrlPoint.Y + ImgOffset.Y;

            foreach (Annotation an in ScanPage.Annotations)
            {
                if (an.BShow)
                {
                    if (x > an.DetailRectangle.X && x < (an.DetailRectangle.X + an.DetailRectangle.Width))
                    {
                        if (y > an.DetailRectangle.Y && y < (an.DetailRectangle.Y + an.DetailRectangle.Height))
                        {
                            anno = an;
                            break;
                        }
                    }
                }
            }
        }

        return anno;
    }

    /// <summary>
    /// 显示或隐藏标注
    /// </summary>
    /// <param name="bShow">显示或隐藏</param>
    public void ShowAnnotations(bool bShow)
    {
        foreach (Annotation an in ScanPage.Annotations)
        {
            an.BShow = bShow;
        }
    }

    #endregion 画标记相关代码

    /// <summary>
    /// 通过地图坐标计算当前状态四角的控件坐标
    /// </summary>
    /// <param name="mAbsRectangle">地图边界</param>
    /// <param name="closed">是否封闭</param>
    /// <returns></returns>
    public Point[] MRectOnAbs(MAbsRectangle mAbsRectangle, bool closed)
    {
        Point a = ImgCtrlPoint(mAbsRectangle.Points[0], Degree);
        Point b = ImgCtrlPoint(mAbsRectangle.Points[1], Degree);
        Point c = ImgCtrlPoint(mAbsRectangle.Points[3], Degree);
        Point d = ImgCtrlPoint(mAbsRectangle.Points[2], Degree);
        if (closed)
        {
            return new Point[] { a, b, c, d, a };
        }
        return new Point[] { a, b, c, d };
    }

    /// <summary>
    /// 用绝对坐标画方形
    /// </summary>
    /// <param name="bitmap">图像引用</param>
    /// <param name="pen">笔</param>
    /// <param name="mAbsRectangle">边界</param>
    /// <returns></returns>
    public Point[] DrawMRectOnAbs(Bitmap bitmap, Pen pen, MAbsRectangle mAbsRectangle)
    {
        Point[] points = null;
        //Graphics graphics = Graphics.FromImage(bitmap);
        using (Graphics graphics = Graphics.FromImage(bitmap))// joysola
        {
            points = MRectOnAbs(mAbsRectangle, true);
            graphics.DrawLines(pen, points);
        }

        return points;
    }

    /// <summary>
    /// 用绝对坐标画方形
    /// </summary>
    /// <param name="bitmap">图像引用</param>
    /// <param name="pen">笔</param>
    /// <param name="rectangle">边界</param>
    /// <returns></returns>
    public Rectangle DrawRectOnAbs(Bitmap bitmap, Pen pen, Rectangle rectangle)
    {
        return DrawRectOnAbs(bitmap, pen, rectangle.Location, new Point(rectangle.Right, rectangle.Bottom));
    }

    /// <summary>
    /// 把方形的控件坐标转化为绝对坐标的方形
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public Rectangle RectOnAbs(Rectangle rectangle)
    {
        return RectOnAbs(rectangle.Location, new Point(rectangle.Right, rectangle.Bottom));
    }

    /// <summary>
    /// 根据左上角和右下角计算控件坐标的方形
    /// </summary>
    /// <param name="startAbsPoint">左上角绝对坐标</param>
    /// <param name="endAbsPoint">右下角绝对坐标</param>
    /// <returns></returns>
    public Rectangle RectOnAbs(Point startAbsPoint, Point endAbsPoint)
    {
        Point s = ImgCtrlPoint(startAbsPoint, Degree);
        Point e = ImgCtrlPoint(endAbsPoint, Degree);
        Rectangle ret = new Rectangle(s.X + ImgOffset.X, s.Y + ImgOffset.Y, e.X - s.X, e.Y - s.Y);
        return ret;
    }

    public Rectangle DrawRectOnAbs(Bitmap bitmap, Pen pen, Point startAbsPoint, Point endAbsPoint)
    {
        Rectangle rect;
        //Graphics graphics = Graphics.FromImage(bitmap);
        using (Graphics graphics = Graphics.FromImage(bitmap)) // joysola
        {
            rect = RectOnAbs(startAbsPoint, endAbsPoint);
            graphics.DrawRectangle(pen, rect);
        }

        return rect;
    }

    /// <summary>
    /// 判断绝对坐标是否在画面中
    /// </summary>
    /// <param name="absPoint">绝对坐标</param>
    /// <returns>该绝对坐标点所在的地图块</returns>
    public MapRectangle AbsPointIn(Point absPoint)
    {
        return ImgCtrlTransHelper.AbsPointIn(ScanPage, CurrLevel, absPoint);
    }

    /// <summary>
    /// 根据绝对坐标计算控件坐标
    /// </summary>
    /// <param name="absPoint">绝对坐标</param>
    /// <param name="degreeA">角度</param>
    /// <returns>绝对坐标</returns>
    public Point ImgCtrlPoint(Point absPoint, float degreeA)
    {
        return ImgCtrlPoint(absPoint, degreeA, Scale);
    }

    /// <summary>
    ///  根据绝对坐标计算控件坐标
    /// </summary>
    /// <param name="absPoint">绝对坐标</param>
    /// <param name="degreeA">角度</param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public Point ImgCtrlPoint(Point absPoint, float degreeA, double scale)
    {
        return ImgCtrlPoint(MapRectangle, absPoint, degreeA, scale);
    }

    /// <summary>
    ///  根据绝对坐标计算控件坐标
    /// </summary>
    /// <param name="mapr"></param>
    /// <param name="absPoint">绝对坐标</param>
    /// <param name="degreeA">角度</param>
    /// <param name="scale">放大倍率</param>
    /// <returns>控件坐标</returns>
    public Point ImgCtrlPoint(MapRectangle mapr, Point absPoint, float degreeA, double scale)
    {
        ScanPageLevel currLCR = ScanPage.GetColsRows(CurrLevel);
        ScanPageLevel maxLCR = ScanPage.GetColsRows(ScanPage.MaxLevel);
        Point p = ImgCtrlTransHelper.ImgPoint(currLCR, maxLCR, mapr, absPoint, degreeA, scale);
        Point ret0 = new Point(Convert.ToInt32(p.X - ImgOffset.X + CtrlOffset.X),
            Convert.ToInt32(p.Y - ImgOffset.Y + CtrlOffset.Y));
        return ret0;
    }

    /// <summary>
    /// 根据控件坐标计算绝对坐标
    /// </summary>
    /// <param name="imgCtrlPoint">控件坐标</param>
    /// <param name="degreeA">角度</param>
    /// <returns>绝对坐标</returns>
    public Point AbsPoint(Point imgCtrlPoint, float degreeA)
    {
        Point[] points = MapRectangle.CornerPoints(1, degreeA);

        int turn = Convert.ToInt32(Math.Floor(degreeA / 90));
        double degree = degreeA % 90;

        ScanPageLevel currLCR = ScanPage.GetColsRows(CurrLevel);
        ScanPageLevel maxLCR = ScanPage.GetColsRows(ScanPage.MaxLevel);

        float pitchCol = maxLCR.ColSpan() / currLCR.ColSpan();
        float pitchRow = maxLCR.RowSpam() / currLCR.RowSpam();

        Point ret0;

        Point rlvPt = new Point(ImgOffset.X + imgCtrlPoint.X - CtrlOffset.X,
            ImgOffset.Y + imgCtrlPoint.Y - CtrlOffset.Y);

        rlvPt = new Point(Convert.ToInt32(rlvPt.X / Scale),
            Convert.ToInt32(rlvPt.Y / Scale));

        double cosx = Math.Cos(ImgHelper.DegreeToRaidens(degree));
        double sinx = Math.Sin(ImgHelper.DegreeToRaidens(degree));
        double tanx = Math.Tan(ImgHelper.DegreeToRaidens(degree));
        //TODO tested height degree
        int h = Convert.ToInt32(MapRectangle.Height(0));
        int w = Convert.ToInt32(MapRectangle.Width(0));

        double rx = 0, ry = 0;
        if (turn == 0)
        {
            //第一象限
            rx = (rlvPt.X - (h * cosx - rlvPt.Y) * tanx) * cosx;
            ry = rlvPt.Y / cosx - rx * tanx;
        }
        else if (turn == 1)
        {
            //第二象限
            rx = (rlvPt.Y - (rlvPt.X - w * sinx) * tanx) * cosx;
            ry = (w * sinx + h * cosx - rlvPt.X) / cosx - rx * tanx;
        }
        else if (turn == 3)
        {
            //第四象限
            double h1 = points[0].Y - rlvPt.Y;
            rx = h1 / cosx + (rlvPt.X - h1 * tanx) * sinx;
            ry = (rlvPt.X - h1 * tanx) * cosx;
        }
        else if (turn == 2)
        {
            //第三象限
            double x1 = w * cosx + h * sinx - rlvPt.X;
            double y1 = w * sinx + h * cosx - rlvPt.Y;

            rx = (x1 - (h * cosx - y1) * tanx) * cosx;
            ry = y1 / cosx - rx * tanx;
        }

        ret0 = new Point(Convert.ToInt32((rx + MapRectangle.ColStart * Constants.PicW) * pitchCol),
                  Convert.ToInt32((ry + MapRectangle.RowStart * Constants.PicH) * pitchRow));
        return ret0;
    }

    /// <summary>
    /// 获得当前切片第一层的数据
    /// </summary>
    /// <returns></returns>
    public ImgCtrlPage FloorPage()
    {
        ImgCtrlPage ret = new ImgCtrlPage(ScanPage.BaseFilePath);
        ret.CurrLevel = 1;
        ret.ImgOffset = new Point(0, 0);
        return ret;
    }

    /// <summary>
    /// 根据MapRectangle四角坐标拼接9层图
    /// </summary>
    /// <param name="imgTag">图像数据</param>
    /// <returns>拼接后的图像</returns>
    public Bitmap JoinImage(CImgTag imgTag)
    {
        logger.Debug("JoinImage Start");

        int picCount;
        //MapRectangle.ColStart = Math.Min(0, MapRectangle.ColStart );
        //MapRectangle.RowStart = Math.Min(0, MapRectangle.RowStart );
        if (imgTag != null && MapRectangle.IsFullLevelMapR())
        {
            Bitmap cachedBitmap = ScanPageCache.GetFullPageBitmap(ScanPage.GetSampleName(), MapRectangle.Level);

            if (cachedBitmap != null)
            {
                CImgTag tag = (CImgTag)cachedBitmap.Tag;
                if (tag.Equal(imgTag))
                {
                    logger.Debug("JoinImage Done Cache hit.");
                    return cachedBitmap;
                }
            }
        }

        Bitmap bitmap = ImgHelper.JoinImage(ScanPage.BaseFilePath, ScanPage.GetSampleName(), MapRectangle);

        logger.Debug("JoinImage Done");

        return bitmap;
    }

    /// <summary>
    /// 拼接当前层整个大图
    /// </summary>
    /// <returns></returns>
    public Bitmap JoinImageByLevel()
    {
        return ScanPage.JoinImageByLevel(CurrLevel);
    }

    /// <summary>
    /// 获得当前图层的全尺寸地图区块
    /// </summary>
    /// <returns></returns>
    public MapRectangle GetLevelFullRect()
    {
        return GetLevelFullRect(CurrLevel);
    }

    /// <summary>
    /// 获得某一图层的全尺寸地图区块
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private MapRectangle GetLevelFullRect(int level)
    {
        if (!ScanPage.CalcColsRows(level))
        {
            return null;
        }
        return new MapRectangle(ScanPage.GetColsRows(level));
    }

    /// <summary>
    /// 根据控件尺寸计算当前地图的可视区块范围
    /// </summary>
    /// <param name="imgCtrlSize"></param>
    /// <returns></returns>
    public MapRectangle VisableMapRectangle(Size imgCtrlSize)
    {
        return ImgCtrlTransHelper.VisableMapRectangle(ImgOffset, MapRectangle, imgCtrlSize);
    }

    /// <summary>
    /// 复制对象
    /// </summary>
    /// <returns></returns>
    public ImgCtrlPage Clone()
    {
        ImgCtrlPage ret = MemberwiseClone() as ImgCtrlPage;
        ret.MapRectangle = MapRectangle.Clone();
        return ret;
    }

    /// <summary>
    /// 判断是否为有效切片
    /// </summary>
    /// <returns></returns>
    public Exception IsInvalidCtrlPage()
    {
        String reason = "";
        if (ScanPage.CalcColsRows(ScanPage.MaxLevel - 1) == false)
        {
            reason = "样本缺少Slide.dat扫描信息，无法打开。";
        }

        Exception exception = new Exception(reason);
        if (String.IsNullOrEmpty(reason))
        {
            exception = null;
        }
        return exception;
    }

    /// <summary>
    /// 计算当前页面的像素宽度（整形）
    /// </summary>
    /// <returns></returns>
    public int PageWidthInt()
    {
        return Convert.ToInt32(Math.Ceiling(MapRectangle.Width(Degree)));
    }

    /// <summary>
    /// 计算当前页面的像素高度（整形）
    /// </summary>
    /// <returns></returns>
    public int PageHeightInt()
    {
        return Convert.ToInt32(Math.Ceiling(MapRectangle.Height(Degree)));
    }

    /// <summary>
    /// 计算当前页面的像素宽度（浮点形）
    /// </summary>
    /// <returns></returns>
    public double PageWidth()
    {
        return MapRectangle.Width(Degree);
    }

    /// <summary>
    /// 计算当前页面的像素宽度（浮点形）
    /// </summary>
    /// <returns></returns>
    public double PageHeight()
    {
        return MapRectangle.Height(Degree);
    }
}
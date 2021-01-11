using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Point = System.Drawing.Point;

/// <summary>
/// 图像处理辅助类
/// </summary>
public class ImgHelper
{
    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private static ConcurrentDictionary<int, Bitmap> PicDict = new ConcurrentDictionary<int, Bitmap>();

    public static void InitPicDict()
    {
        //GC.Collect();
        PicDict = new ConcurrentDictionary<int, Bitmap>();
    }

    /// <summary>
    /// 清理ImgHelper中静态属性PicDict
    /// </summary>
    public static void ClearData()
    {
        foreach (var bit in PicDict.Values)
        {
            bit.Dispose();
        }
        PicDict.Clear();
    }

    #region 图片拼接函数

    /// <summary>
    /// 根据地图范围拼接图像
    /// </summary>
    /// <param name="basePath">样本位置</param>
    /// <param name="pageId">样本ID（未使用）</param>
    /// <param name="map">地图范围</param>
    /// <returns>拼接后的图像</returns>
    public static Bitmap JoinImage(String basePath, String pageId, MapRectangle map)
    {
        int outpic;
        return JoinImage(basePath, map.Level, map.ColStart, map.ColEnd, map.RowStart, map.RowEnd, out outpic, null);
    }

    /// <summary>
    /// 根据地图范围拼接图像
    /// </summary>
    /// <param name="basePath">样本路径</param>
    /// <param name="level">层</param>
    /// <param name="colStart">开始列</param>
    /// <param name="colEnd">结束列</param>
    /// <param name="rowStart">开始行</param>
    /// <param name="rowEnd">结束行</param>
    /// <param name="picCount">涉及到的图像数量</param>
    /// <param name="fullPageBitmap">该页面整个图像（如果传入，则可直接切割，提高性能避免磁盘IO）</param>
    /// <returns></returns>
    private static Bitmap JoinImage(String basePath, int level, float colStart, float colEnd, float rowStart, float rowEnd, out int picCount, Bitmap fullPageBitmap)
    {
        int cs = Convert.ToInt16(Math.Ceiling(colStart));
        int ce = Convert.ToInt16(Math.Ceiling(colEnd));
        int rs = Convert.ToInt16(Math.Ceiling(rowStart));
        int re = Convert.ToInt16(Math.Ceiling(rowEnd));

        int imgWidth = (ce - cs) * Constants.PicW;
        int imgHeight = (re - rs) * Constants.PicH;

        if (imgWidth <= 0 || imgHeight <= 0)
        {
            picCount = 0;
            return null;
        }
        return JoinImage(basePath, level, imgWidth, imgHeight, cs, ce, rs, re, out picCount, fullPageBitmap);
    }

    private static Bitmap JoinImage(String basePath, int level,
        int imgWidth, int imgHeight, int colStart, int colEnd, int rowStart, int rowEnd, out int picCount, Bitmap fullPageBitmap)
    {
        logger.Debug("start join image, level:" + level + " colstart:" + colStart + " rowend:" + rowEnd);

        picCount = 0;
        CImgTag newTag = new CImgTag();
        bool reused = false;

        CImgTag cropedTag = new CImgTag();
        newTag.MapRectangle = new MapRectangle();
        newTag.MapRectangle.ColStart = colStart;
        newTag.MapRectangle.ColEnd = colEnd;
        newTag.MapRectangle.RowStart = rowStart;
        newTag.MapRectangle.RowEnd = rowEnd;
        newTag.MapRectangle.Level = level;

        if (fullPageBitmap != null)
        {
            Bitmap ret = CropImage(fullPageBitmap, newTag.MapRectangle);
            return ret;
        }

        Bitmap bitmap = new Bitmap(imgWidth, imgHeight);

        Graphics graphics = Graphics.FromImage(bitmap);
        float w = (float)(2);
        Pen pen = new Pen(Color.Red, w);

        int num = 0;
        for (int i = colStart; i <= colEnd; i++)
        {
            int num2 = 0;

            for (int j = rowStart; j <= rowEnd; j++)
            {
                bool skipped = false;
                if (reused && (
                    (j > cropedTag.MapRectangle.RowStart && j < cropedTag.MapRectangle.RowEnd) &&
                    (i > cropedTag.MapRectangle.ColStart && i < cropedTag.MapRectangle.ColEnd)))
                {
                    skipped = true;
                }

                if (!skipped)
                {
                    string fileName = "\\" + level + "\\" + i + "\\" + j + ".jpg";

                    string fullFileName = basePath + fileName;

                    if (File.Exists(fullFileName))
                    {
                        int hash = fullFileName.GetHashCode();

                        Bitmap image = null;
                        if (!PicDict.TryGetValue(hash, out image))
                        {
                            image = (Bitmap)Image.FromFile(fullFileName);
                            PicDict.GetOrAdd(hash, image);
                        }
                        else
                        {
                            // logger.Debug("Cache hit:" + fullFileName);
                        }

                        graphics.DrawImage(image, num * Constants.PicW, num2 * Constants.PicH, Constants.PicW,
                           Constants.PicH);

                        if (Constants.SettingDetail.ShowDebugInfoSwitch == 1)
                        {
                            string text = fileName;
                            DrawNoticeText(graphics, text, num * Constants.PicW, num2 * Constants.PicH);

                            graphics.DrawRectangle(pen, new Rectangle(num * Constants.PicW, num2 * Constants.PicH, Constants.PicW,
                               Constants.PicH));
                        }
                        picCount++;
                    }
                    else
                    {
                        graphics.FillRectangle(Brushes.White, num * Constants.PicW, num2 * Constants.PicH, Constants.PicW, Constants.PicH);
                    }
                }

                num2++;
            }
            num++;
        }

        //logger.Debug(" JoinImage level:" + level + ", imgWidth:" + imgWidth + ", imgHeight:" + imgHeight
        //    + ", colStart:" + colStart + ", colEnd:" + colEnd + ", rowStart:" + rowStart + ", rowEnd:" + rowEnd + ", picCount:" + picCount);
        if (Constants.SettingDetail.ShowDebugInfoSwitch == 1)
        {
            graphics.DrawRectangle(new Pen(Color.Green, w), new Rectangle(new Point(0, 0), bitmap.Size));
        }

        bitmap.Tag = newTag;

        logger.Debug("end join image, level:" + level + " colstart:" + colStart + " rowend:" + rowEnd);

        graphics.Dispose(); // joysola
        return bitmap;
    }

    #endregion 图片拼接函数

    /// <summary>
    /// 往图像上写字（调试用）
    /// </summary>
    /// <param name="ge"></param>
    /// <param name="text"></param>
    /// <param name="rectX"></param>
    /// <param name="rectY"></param>
    public static void DrawNoticeText(Graphics ge, string text, int rectX, int rectY)
    {
        //字体大小
        float fontSize = 20.0f;
        DrawNoticeText(ge, text, rectX, rectY, fontSize);
    }

    /// <summary>
    /// 往图片上写字
    /// </summary>
    /// <param name="ge"></param>
    /// <param name="text"></param>
    /// <param name="rectX"></param>
    /// <param name="rectY"></param>
    /// <param name="fontSize"></param>
    public static void DrawNoticeText(Graphics ge, string text, int rectX, int rectY, float fontSize)
    {
        //文本的长度
        float textWidth = text.Length * fontSize;
        //下面定义一个矩形区域，以后在这个矩形里画上白底黑字

        float rectWidth = text.Length * (fontSize + 40);
        float rectHeight = fontSize + 40;
        //声明矩形域
        RectangleF textArea = new RectangleF(rectX, rectY, rectWidth, rectHeight);
        //定义字体
        System.Drawing.Font font = new System.Drawing.Font("微软雅黑", fontSize, System.Drawing.FontStyle.Regular);
        //font.Bold = true;
        //白笔刷，画文字用
        Brush whiteBrush = new SolidBrush(System.Drawing.Color.DodgerBlue);
        //黑笔刷，画背景用
        //Brush blackBrush = new SolidBrush(Color.Black);
        //g.FillRectangle(blackBrush, rectX, rectY, rectWidth, rectHeight);
        ge.DrawString(text, font, whiteBrush, textArea);
    }

    /// <summary>
    /// 判断某图像文件是否为切片的有效图片，主要目的是排除切片中的全白图片）
    /// </summary>
    /// <param name="file"></param>
    /// <returns>如果是有效图片则返回所在的行、列</returns>
    public static Rectangle ValidRect(FileInfo file)
    {
        if (!file.Exists)
        {
            return new Rectangle();
        }

        Rectangle ret = new Rectangle(0, 0, Constants.PicW, Constants.PicH);
        Image image = Image.FromFile(file.FullName);
        Bitmap map = new Bitmap(image);

        int colStart = Constants.PicW;
        int rowStart = Constants.PicH;
        int colEnd = 0;
        int rowEnd = 0;

        bool allwhite = true;

        Color color0 = map.GetPixel(0, 0);
        Color color1 = map.GetPixel(0, Constants.PicW - 1);
        Color color2 = map.GetPixel(Constants.PicH - 1, 0);
        Color color3 = map.GetPixel(Constants.PicH - 1, Constants.PicW - 1);

        if (color0.ToArgb() < -1 && color1.ToArgb() < -1 && color2.ToArgb() < -1 && color3.ToArgb() < -1)
        {
            //绝大多数都是全有图像的情况，快速跳过
            return new Rectangle(0, 0, Constants.PicW, Constants.PicH);
        }

        for (int x = 0; x < Constants.PicW; x++)
        {
            for (int y = 0; y < Constants.PicH; y++)
            {
                Color color = map.GetPixel(x, y);
                if (color.ToArgb() < -1)
                {
                    allwhite = false;
                    rowStart = Math.Min(y, rowStart);
                    colStart = Math.Min(x, colStart);

                    rowEnd = Math.Max(y, rowEnd);
                    colEnd = Math.Max(x, colEnd);
                }
            }
        }

        if (allwhite)
        {
            ret = new Rectangle(0, 0, 0, 0);
        }
        else
        {
            ret = new Rectangle(colStart, rowStart, colEnd - colStart + 1, rowEnd - rowStart + 1);
        }

        return ret;
    }

    public static bool IsValidRect(Rectangle rectangle)
    {
        if (rectangle.Width <= 0 || rectangle.Height <= 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 计算两点距离
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns>距离</returns>
    public static double PointsLength(Point p1, Point p2)
    {
        double dX = p2.X - p1.X;
        double dY = p2.Y - p1.Y;

        double multi = dX * dX + dY * dY;
        double rad = Math.Round(Math.Sqrt(multi), 3, MidpointRounding.AwayFromZero);
        return rad;
    }

    /// <summary>
    /// 根据给定的地图范围切割图像
    /// 注意：srcmap必须包含tag信息
    /// </summary>
    /// <param name="srcImg"></param>
    /// <param name="targetMapr"></param>
    /// <returns></returns>
    public static Bitmap CropImage(Bitmap srcImg, MapRectangle targetMapr)
    {
        if (srcImg == null)
        {
            return null;
        }
        CImgTag oldTag = (CImgTag)srcImg.Tag;
        if (targetMapr.Level != oldTag.MapRectangle.Level)
        {
            return null;
        }
        //取交集
        MapRectangle cropedsrc = new MapRectangle();
        cropedsrc.ColStart = Math.Max(oldTag.MapRectangle.ColStart, targetMapr.ColStart);
        cropedsrc.RowStart = Math.Max(oldTag.MapRectangle.RowStart, targetMapr.RowStart);
        cropedsrc.ColEnd = Math.Min(oldTag.MapRectangle.ColEnd, targetMapr.ColEnd);
        cropedsrc.RowEnd = Math.Min(oldTag.MapRectangle.RowEnd, targetMapr.RowEnd);
        cropedsrc.Level = oldTag.MapRectangle.Level;

        Rectangle cropRect = new Rectangle((cropedsrc.ColStart - oldTag.MapRectangle.ColStart) * Constants.PicW,
            (cropedsrc.RowStart - oldTag.MapRectangle.RowStart) * Constants.PicW,
            Convert.ToInt16(cropedsrc.Width(0)),
            Convert.ToInt16(cropedsrc.Height(0)));
        if (!IsValidRect(cropRect))
        {
            return null;
        }

        Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

        using (Graphics g = Graphics.FromImage(target))
        {
            g.DrawImage(srcImg, new Rectangle(0, 0, target.Width, target.Height),
                             cropRect,
                             GraphicsUnit.Pixel);
        }
        oldTag.MapRectangle = cropedsrc;
        target.Tag = oldTag;

        return target;
    }

    #region 图片旋转函数

    /// <summary>
    /// 计算矩形绕中心任意角度旋转后所占区域矩形宽高
    /// </summary>
    /// <param name="width">原矩形的宽</param>
    /// <param name="height">原矩形高</param>
    /// <param name="angle">顺时针旋转角度</param>
    /// <returns></returns>
    public static Rectangle GetRotateRectangle(int width, int height, float angle)
    {
        double radian = angle * Math.PI / 180; ;
        double cos = Math.Cos(radian);
        double sin = Math.Sin(radian);
        //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
        int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
        int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
        return new Rectangle(0, 0, newWidth, newHeight);
    }

    /// <summary>
    /// 获取原图像绕中心任意角度旋转后的图像
    /// </summary>
    /// <param name="rawImg"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Bitmap GetRotateImage(Bitmap srcImage, int angle)
    {
        logger.Info("rotate.......at " + angle);

        angle = angle % 360;
        //原图的宽和高
        int srcWidth = srcImage.Width;
        int srcHeight = srcImage.Height;
        //图像旋转之后所占区域宽和高
        Rectangle rotateRec = GetRotateRectangle(srcWidth, srcHeight, angle);
        int rotateWidth = rotateRec.Width;
        int rotateHeight = rotateRec.Height;
        //目标位图
        Bitmap destImage = null;
        Graphics graphics = null;
        try
        {
            //定义画布，宽高为图像旋转后的宽高
            destImage = new Bitmap(rotateWidth, rotateHeight);
            //graphics根据destImage创建，因此其原点此时在destImage左上角
            graphics = Graphics.FromImage(destImage);
            //要让graphics围绕某矩形中心点旋转N度，分三步
            //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
            //第二步，graphics旋转相应的角度(沿当前原点)
            //第三步，移回（-x,-y）
            //获取画布中心点
            Point centerPoint = new Point(rotateWidth / 2, rotateHeight / 2);
            //将graphics坐标原点移到中心点
            graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
            //graphics旋转相应的角度(绕当前原点)
            graphics.RotateTransform(angle);
            //恢复graphics在水平和垂直方向的平移(沿当前原点)
            graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
            //此时已经完成了graphics的旋转

            //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
            Point Offset = new Point((rotateWidth - srcWidth) / 2, (rotateHeight - srcHeight) / 2);
            //将源图片画到rect里（rotateRec的中心）
            graphics.DrawImage(srcImage, new Rectangle(Offset.X, Offset.Y, srcWidth, srcHeight));

            //重至绘图的所有变换
            graphics.ResetTransform();
            graphics.Save();

            CImgTag tag = new CImgTag();

            tag.Degree = angle;
            tag.RotateCenter = centerPoint;
            destImage.Tag = tag;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (graphics != null)
                graphics.Dispose();
        }
        logger.Info("rotate.......end " + angle);

        return destImage;
    }

    #endregion 图片旋转函数

    public static double RaidensToDegrees(double raidens)
    {
        double degrees = raidens * 180 / Math.PI;
        return degrees;
    }

    public static double DegreeToRaidens(double degrees)
    {
        double radians = degrees * Math.PI / 180;
        return radians;
    }

    /// <summary>
    /// 对一个坐标点按照一个中心进行旋转
    /// </summary>
    /// <param name="center">中心点</param>
    /// <param name="p1">要旋转的点</param>
    /// <param name="angle">旋转角度，笛卡尔直角坐标</param>
    /// <returns></returns>
    public static Point PointRotate(Point center, Point p1, double angle)
    {
        Point tmp = new Point();
        double angleHude = angle * Math.PI / 180;/*角度变成弧度*/
        double x1 = (p1.X - center.X) * Math.Cos(angleHude) + (p1.Y - center.Y) * Math.Sin(angleHude) + center.X;
        double y1 = -(p1.X - center.X) * Math.Sin(angleHude) + (p1.Y - center.Y) * Math.Cos(angleHude) + center.Y;
        tmp.X = (int)x1;
        tmp.Y = (int)y1;
        return tmp;
    }

    /// <summary>
    /// 缩放图片（保持比例）
    /// </summary>
    /// <param name="bitmap">待缩放图片</param>
    /// <param name="w">目标宽度</param>
    /// <returns>缩放后的图片</returns>
    public static System.Drawing.Bitmap ScaleBitmap(System.Drawing.Bitmap bitmap, int w)
    {
        if (w == bitmap.Width || w == 0)
        {
            return bitmap;
        }

        int towidth = w;

        //指定宽，高按比例
        int toheight = bitmap.Height * w / bitmap.Width;

        System.Drawing.Bitmap map = new System.Drawing.Bitmap(w, toheight);
        map.Tag = bitmap.Tag;

        System.Drawing.Graphics gra = System.Drawing.Graphics.FromImage(map);
        gra.Clear(System.Drawing.Color.Transparent);//清空画布并以透明背景色填充
        gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; //使绘图质量最高，即消除锯齿
        gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        gra.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        int x = 0;
        int y = 0;
        int ow = bitmap.Width;
        int oh = bitmap.Height;

        gra.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

        gra.Flush();
        gra.Dispose();
        bitmap.Dispose();

        return map;
    }

    /// <summary>
    /// 图像对比度调整
    /// </summary>
    /// <param name="b">原始图</param>
    /// <param name="degree">对比度[-100, 100]</param>
    /// <returns></returns>
    public static Bitmap KiContrast(Bitmap b, int degree)
    {
        if (b == null)
        {
            return null;
        }

        if (degree < -100) degree = -100;
        if (degree > 100) degree = 100;

        try
        {
            double pixel = 0;
            double contrast = (100.0 + degree) / 100.0;
            contrast *= contrast;
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // 处理指定位置像素的对比度
                        for (int i = 0; i < 3; i++)
                        {
                            pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                            if (pixel < 0) pixel = 0;
                            if (pixel > 255) pixel = 255;
                            p[i] = (byte)pixel;
                        } // i
                        p += 3;
                    } // x
                    p += offset;
                } // y
            }
            b.UnlockBits(data);
            return b;
        }
        catch
        {
            return null;
        }
    }

    public static Bitmap BrightnessAndContrastOpenCV(Bitmap bitmap, ColorCorrection color)
    {
        return ImgHelper.BrightnessAndContrastOpenCV(bitmap, color.white, color.gamma, color.blue, color.green, color.red);
    }

    /// <summary>
    /// 使用OpenCV调整图像的亮度、对比度及颜色
    /// </summary>
    /// <param name="a"></param>
    /// <param name="bright"></param>
    /// <param name="contrast"></param>
    /// <param name="b"></param>
    /// <param name="g"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Bitmap BrightnessAndContrastOpenCV(Bitmap a, double bright, double contrast, int b, int g, int r)
    {
        if (a == null)
        {
            return a;
        }
        Mat src;
        var tag = a.Tag;
        src = OpenCvSharp.Extensions.BitmapConverter.ToMat(a);
        for (int i = 0; i < src.Rows; i++)
        {
            for (int j = 0; j < src.Cols; j++)
            {
                Vec3b color = new Vec3b();//新建Vec3b对象（字节的三元组(System.Byte)）
                color.Item0 = (byte)Saturate_cast((src.Get<Vec3b>(i, j).Item0 * contrast + bright + b));//b
                color.Item1 = (byte)Saturate_cast((src.Get<Vec3b>(i, j).Item1 * contrast + bright + g));//g
                color.Item2 = (byte)Saturate_cast((src.Get<Vec3b>(i, j).Item2 * contrast + bright + r));//r

                src.Set(i, j, color);
            }
        }

        Bitmap ret = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(src);
        ret.Tag = tag;
        return ret;
    }

    /// <summary>
    /// 亮度调整
    /// </summary>
    /// <param name="a"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Bitmap BrightnessP(Bitmap a, int v)
    {
        System.Drawing.Imaging.BitmapData bmpData = a.LockBits(new Rectangle(0, 0, a.Width, a.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        int bytes = a.Width * a.Height * 3;
        IntPtr ptr = bmpData.Scan0;
        int stride = bmpData.Stride;
        unsafe
        {
            byte* p = (byte*)ptr;
            int temp;
            for (int j = 0; j < a.Height; j++)
            {
                for (int i = 0; i < a.Width * 3; i++, p++)
                {
                    temp = (int)(p[0] + v);
                    temp = (temp > 255) ? 255 : temp < 0 ? 0 : temp;
                    p[0] = (byte)temp;
                }
                p += stride - a.Width * 3;
            }
        }
        a.UnlockBits(bmpData);
        return a;
    }

    public static void Rect2Pointfs(Rectangle rect, float angle, out PointF[] lpfs)
    {
        using (var graph = new GraphicsPath())
        {
            Point Center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            graph.AddRectangle(rect);
            var a = -angle * (Math.PI / 180);
            var n1 = (float)Math.Cos(a);
            var n2 = (float)Math.Sin(a);
            var n3 = -(float)Math.Sin(a);
            var n4 = (float)Math.Cos(a);
            var n5 = (float)(Center.X * (1 - Math.Cos(a)) + Center.Y * Math.Sin(a));
            var n6 = (float)(Center.Y * (1 - Math.Cos(a)) - Center.X * Math.Sin(a));
            graph.Transform(new Matrix(n1, n2, n3, n4, n5, n6));
            lpfs = graph.PathPoints;
        }
    }

    /// <summary>
    /// 判断点是否在多边形内
    /// </summary>
    /// <param name="checkPoint">需要判断的点</param>
    /// <param name="polygonPoints">组成多边形点的集合</param>
    /// <returns></returns>
    public static bool IsInPolygon2(Point pnt, Point[] points)
    {
        if (points == null || points.Length != 4)
        {
            return false;
        }

        Point[] pntlist = new Point[] { points[0], points[1], points[3], points[2] };
        int j = 0, cnt = 0;
        for (int i = 0; i < pntlist.Length; i++)
        {
            j = (i == pntlist.Length - 1) ? 0 : j + 1;
            if ((pntlist[i].Y != pntlist[j].Y) && (((pnt.Y >= pntlist[i].Y) && (pnt.Y < pntlist[j].Y)) || ((pnt.Y >= pntlist[j].Y) && (pnt.Y < pntlist[i].Y))) && (pnt.X < (pntlist[j].X - pntlist[i].X) * (pnt.Y - pntlist[i].Y) / (pntlist[j].Y - pntlist[i].Y) + pntlist[i].X)) cnt++;
        }
        return (cnt % 2 > 0) ? true : false;
    }

    /// <summary>
    /// 判断两个地图范围是否相交
    /// </summary>
    /// <param name="rectA"></param>
    /// <param name="rectB"></param>
    /// <returns></returns>
    public static bool IsIntersect(MAbsRectangle rectA, MAbsRectangle rectB)
    {
        bool ret = false;

        foreach (Point p in rectA.Points)
        {
            ret = ret || IsInPolygon2(p, rectB.Points);
        }

        return ret;
    }

    /// <summary>
    /// 判断两个矩形是否相交
    /// </summary>
    /// <param name="rectA"></param>
    /// <param name="rectB"></param>
    /// <returns></returns>
    public static bool IsIntersect(RectangleF rectA, RectangleF rectB)
    {
        double RecAleftX = rectA.X;
        double RecAleftY = rectA.Y;
        double RecArightX = rectA.X + rectA.Width;
        double RecArightY = rectA.Y + rectA.Height;

        double RecBleftX = rectB.X;
        double RecBleftY = rectB.Y;
        double RecBrightX = rectB.X + rectB.Width;
        double RecBrightY = rectB.Y + rectB.Height;

        bool isIntersect = false;
        try
        {
            double zx = Math.Abs(RecAleftX + RecArightX - RecBleftX - RecBrightX);
            double x = Math.Abs(RecAleftX - RecArightX) + Math.Abs(RecBleftX - RecBrightX);
            double zy = Math.Abs(RecAleftY + RecArightY - RecBleftY - RecBrightY);
            double y = Math.Abs(RecAleftY - RecArightY) + Math.Abs(RecBleftY - RecBrightY);

            if (zx <= x && zy <= y)  //需要确认两矩形边线重合是否定义为相交 此处定义为相交。
            {
                isIntersect = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
        return isIntersect;
    }

    /// <summary>
    /// 要确保运算后的像素值在正确的范围内
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static double Saturate_cast(double n)
    {
        if (n <= 0)
        {
            return 0;
        }
        else if (n > 255)
        {
            return 255;
        }
        else
        {
            return n;
        }
    }

    public static byte[] Bitmap2Byte(Bitmap bitmap)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] data = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            return data;
        }
    }

    /// <summary>
    /// 字节流转图像
    /// </summary>
    /// <param name="Bytes"></param>
    /// <returns></returns>
    public static Bitmap BytesToBitmap(byte[] Bytes)
    {
        MemoryStream stream = null;
        try
        {
            stream = new MemoryStream(Bytes);
            return new Bitmap((Image)new Bitmap(stream));
        }
        catch (ArgumentNullException ex)
        {
            throw ex;
        }
        catch (ArgumentException ex)
        {
            throw ex;
        }
        finally
        {
            stream.Close();
        }
    }
}
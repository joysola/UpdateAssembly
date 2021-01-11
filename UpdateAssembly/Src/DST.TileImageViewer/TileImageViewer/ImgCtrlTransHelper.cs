using System;
using System.Drawing;

/// <summary>
/// imgctrl控件坐标转换辅助类
/// </summary>
internal class ImgCtrlTransHelper
{
    /// <summary>
    /// 判断绝对坐标是否在画面中
    /// </summary>
    /// <param name="scanPage">切片图层数据</param>
    /// <param name="currLevel">第几层</param>
    /// <param name="absPoint">绝对坐标</param>
    /// <returns>该绝对坐标点所在的地图块</returns>
    public static MapRectangle AbsPointIn(ScanPage scanPage, int currLevel, Point absPoint)
    {
        ScanPageLevel currLCR = scanPage.GetColsRows(currLevel);
        ScanPageLevel maxLCR = scanPage.GetColsRows(scanPage.MaxLevel);
        MapRectangle ret = new MapRectangle(currLCR);

        float pitchCol = maxLCR.ColSpan() / currLCR.ColSpan();
        float pitchRow = maxLCR.RowSpam() / currLCR.RowSpam();

        ret.ColStart = Convert.ToInt32(Math.Floor(absPoint.X / pitchCol / Constants.PicW));
        ret.ColEnd = ret.ColStart + 1;
        ret.RowStart = Convert.ToInt32(Math.Floor(absPoint.Y / pitchRow / Constants.PicW));
        ret.RowEnd = ret.RowStart + 1;
        return ret;
    }

    /// <summary>
    /// 计算当前视野下最为合适的地图范围
    /// </summary>
    /// <param name="scanPage">切片图层数据</param>
    /// <param name="level">第几层</param>
    /// <param name="ctrlPoint">控件位置</param>
    /// <param name="ctrlSize">控件尺寸</param>
    /// <param name="absCtrlPoint">中心点绝对坐标</param>
    /// <param name="degree">角度</param>
    /// <param name="scale">放大倍率</param>
    /// <returns>地图范围</returns>
    public static MapRectangle RefreshN(ScanPage scanPage, int level, Point ctrlPoint, Size ctrlSize, Point absCtrlPoint, int degree, float scale)
    {
        MapRectangle mapRectangle = new MapRectangle(scanPage.GetColsRows(level));

        MapRectangle absMapR = AbsPointIn(scanPage, level, absCtrlPoint);
        SizeF blockSize = new SizeF(Constants.PicW * scale, Constants.PicH * scale);

        if (level <= Constants.DefaultFullPageMaxLevel)
        {
            mapRectangle.ColStart = 0;
            mapRectangle.RowStart = 0;
            mapRectangle.RowEnd = mapRectangle.RowMaxEnd;
            mapRectangle.ColEnd = mapRectangle.ColMaxEnd;
        }
        else
        {
            int leftblock = Convert.ToInt16(Math.Ceiling((ctrlPoint.X) / blockSize.Width + Constants.ImgColsOutter));
            int rightblock = Convert.ToInt16(Math.Ceiling((ctrlSize.Width - ctrlPoint.X) / blockSize.Width + Constants.ImgColsOutter));
            int topblock = Convert.ToInt16(Math.Ceiling((ctrlPoint.Y) / blockSize.Height + +Constants.ImgRowsOutter));
            int bottomblock = Convert.ToInt16(Math.Ceiling((ctrlSize.Height - ctrlPoint.Y) / blockSize.Height + Constants.ImgRowsOutter));

            if (degree <= 45 || degree > 315)
            {
                mapRectangle.ColStart = absMapR.ColStart - leftblock;
                mapRectangle.ColEnd = absMapR.ColEnd + rightblock;
                mapRectangle.RowStart = absMapR.RowStart - topblock;
                mapRectangle.RowEnd = absMapR.RowEnd + bottomblock;
            }
            else if (degree >= 45 || degree < 135)
            {
                mapRectangle.ColStart = absMapR.ColStart - topblock;
                mapRectangle.ColEnd = absMapR.ColEnd + bottomblock;
                mapRectangle.RowStart = absMapR.RowStart - leftblock;
                mapRectangle.RowEnd = absMapR.RowEnd + rightblock;
            }
            else if (degree >= 135 || degree < 225)
            {
                mapRectangle.ColStart = absMapR.ColStart - rightblock;
                mapRectangle.ColEnd = absMapR.ColEnd + leftblock;
                mapRectangle.RowStart = absMapR.RowStart - bottomblock;
                mapRectangle.RowEnd = absMapR.RowEnd + topblock;
            }
            else if (degree >= 225 || degree < 315)
            {
                mapRectangle.ColStart = absMapR.ColStart - topblock;
                mapRectangle.ColEnd = absMapR.ColEnd + bottomblock;
                mapRectangle.RowStart = absMapR.RowStart - leftblock;
                mapRectangle.RowEnd = absMapR.RowEnd + rightblock;
            }
        }

        mapRectangle.ColStart = Math.Min(mapRectangle.ColMaxEnd, Math.Max(0, mapRectangle.ColStart));
        mapRectangle.ColEnd = Math.Min(mapRectangle.ColMaxEnd, Math.Max(mapRectangle.ColStart, mapRectangle.ColEnd));
        mapRectangle.RowStart = Math.Min(mapRectangle.RowMaxEnd, Math.Max(0, mapRectangle.RowStart));
        mapRectangle.RowEnd = Math.Min(mapRectangle.RowMaxEnd, Math.Max(mapRectangle.RowStart, mapRectangle.RowEnd));

        return mapRectangle;
    }

    /// <summary>
    /// 计算当前视野下最为合适的地图范围（候选方案）
    /// </summary>
    /// <param name="scanPage"></param>
    /// <param name="currMapR"></param>
    /// <param name="visMapR"></param>
    /// <param name="controlSize"></param>
    /// <param name="degree"></param>
    /// <param name="imgOffSet"></param>
    /// <returns></returns>
    public static bool Refresh(ScanPage scanPage, MapRectangle currMapR,
       MapRectangle visMapR, Size controlSize, int degree, Point imgOffSet)
    {
        MapRectangle oldMapR = currMapR.Clone();
        //MapRectangle oldAbsMapR = AbsPointIn(scanPage, currMapR.Level, oldAbsPoint);

        if (visMapR.Level <= Constants.DefaultFullPageMaxLevel)
        {
            currMapR.ColStart = 0;
            currMapR.RowStart = 0;
            currMapR.RowEnd = currMapR.RowMaxEnd;
            currMapR.ColEnd = currMapR.RowMaxEnd;
        }
        else if (degree == 0)
        {
            int colStartStep = 2 + Math.Max(0, -imgOffSet.X / Constants.PicW);
            currMapR.ColStart = Math.Max(0, visMapR.ColStart - colStartStep);
            int colEndStep = 2 + (controlSize.Width - (Constants.PicW * (visMapR.ColEnd - visMapR.ColStart) - imgOffSet.X)) / Constants.PicW;
            currMapR.ColEnd = Math.Min(currMapR.ColMaxEnd, visMapR.ColEnd + colEndStep);

            int oldRowStart = currMapR.RowStart;
            int oldRowEnd = currMapR.RowEnd;

            int rowStartStep = 2 + Math.Max(0, -imgOffSet.Y / Constants.PicH);
            currMapR.RowStart = Math.Max(0, visMapR.RowStart - rowStartStep);

            int rowEndStep = 2 + (controlSize.Height - (Constants.PicH * (visMapR.RowEnd - visMapR.RowStart) - imgOffSet.Y)) / Constants.PicH;
            currMapR.RowEnd = Math.Min(currMapR.RowMaxEnd, visMapR.RowEnd + rowEndStep);
        }
        else
        {
            Point[] oldCorners = currMapR.CornerPoints(currMapR.ToScale, degree);
            double[] edgeDists = MaxEdgeDistance2(oldCorners, imgOffSet, visMapR, controlSize, degree, currMapR.ToScale);
            int colstart = visMapR.ColStart - Convert.ToInt32(edgeDists[0] / Constants.PicW) - 4;
            int rowstart = visMapR.RowStart - Convert.ToInt32(edgeDists[3] / Constants.PicH) - 4;
            int colend = visMapR.ColEnd + Convert.ToInt32(edgeDists[2] / Constants.PicW) + 4;
            int rowend = visMapR.RowEnd + Convert.ToInt32(edgeDists[1] / Constants.PicH) + 4;

            currMapR.ColStart = Math.Min(currMapR.ColMaxEnd - 1, Math.Max(Convert.ToInt32(0), colstart));
            currMapR.RowStart = Math.Min(currMapR.RowMaxEnd - 1, Math.Max(Convert.ToInt32(0), rowstart));
            currMapR.ColEnd = Math.Max(1, Math.Min(currMapR.ColMaxEnd, colend));
            currMapR.RowEnd = Math.Max(1, Math.Min(currMapR.RowMaxEnd, rowend));
        }

        if (oldMapR.GetHashCode() == currMapR.GetHashCode())
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 图像四个角与对边的距离
    /// </summary>
    /// <param name="points"></param>
    /// <param name="imgOffset"></param>
    /// <param name="visMapR"></param>
    /// <param name="controlSize"></param>
    /// <param name="degree"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static double[] MaxEdgeDistance2(Point[] points, Point imgOffset,
        MapRectangle visMapR, Size controlSize, int degree, float scale)
    {
        double angleA = degree;
        int turn = Convert.ToInt32(Math.Floor(angleA / 90));
        double angle = angleA % 90;

        double edge01 = 0, edge12 = 0, edge23 = 0, edge30 = 0;
        Point rlvPt = new Point(-imgOffset.X, -imgOffset.Y);

        double w = visMapR.Width(0) * scale;
        double h = visMapR.Height(0) * scale;
        double[] ret = { };

        if (turn == 1)
        {
            points = new Point[] { points[1], points[2], points[3], points[0] };
        }
        else if (turn == 2)
        {
            points = new Point[] { points[2], points[3], points[0], points[1] };
        }
        else if (turn == 3)
        {
            points = new Point[] { points[3], points[0], points[1], points[2] };
        }

        if (angle == 0)
        {
            edge30 = rlvPt.Y;
            edge01 = rlvPt.X;
            edge23 = controlSize.Width - rlvPt.X - w;
            edge12 = controlSize.Height - rlvPt.Y - h;
        }
        else
        {
            edge01 = MaxEdge(rlvPt, points[0], points[1], angle);
            edge30 = MaxEdge(new Point(rlvPt.Y, Convert.ToInt32(controlSize.Width - w - rlvPt.X)),
                new Point(points[3].Y, Convert.ToInt32(controlSize.Width - points[3].X)),
                new Point(points[0].Y, Convert.ToInt32(controlSize.Width - points[0].X)), angle);
            edge23 = MaxEdge(new Point(Convert.ToInt32(controlSize.Width - w - rlvPt.X),
                Convert.ToInt32(controlSize.Height - h - rlvPt.Y)),
              new Point(Convert.ToInt32(controlSize.Width - points[2].X),
                Convert.ToInt32(controlSize.Height - points[2].Y)),
              new Point(Convert.ToInt32(controlSize.Width - points[3].X),
                Convert.ToInt32(controlSize.Height - points[3].Y)), angle);
            edge12 = MaxEdge(new Point(rlvPt.X,
                Convert.ToInt32(controlSize.Height - h - rlvPt.Y)),
              new Point(points[1].X,
                Convert.ToInt32(controlSize.Height - points[1].Y)),
              new Point(points[2].X,
                Convert.ToInt32(controlSize.Height - points[2].Y)), angle);
        }
        if (turn == 0)
        {
            ret = new double[] { edge01, edge12, edge23, edge30 };
        }
        else if (turn == 1)
        {
            ret = new double[] { edge30, edge01, edge12, edge23 };
        }
        else if (turn == 2)
        {
            ret = new double[] { edge23, edge30, edge01, edge12 };
        }
        else if (turn == 3)
        {
            ret = new double[] { edge12, edge23, edge30, edge01 };
        }
        return ret;
    }

    /// <summary>
    /// 计算rlvpt点到p0,p1直线的垂直距离
    /// </summary>
    /// <param name="rlvPt">点</param>
    /// <param name="p0">直线点0</param>
    /// <param name="p1">直线点1</param>
    /// <param name="angleA">图像角度</param>
    /// <returns>距离</returns>
    public static double MaxEdge(Point rlvPt, Point p0, Point p1, double angleA)
    {
        double angle = angleA % 90;
        double tanx = Math.Tan(ImgHelper.DegreeToRaidens(angle));
        double sinx = Math.Sin(ImgHelper.DegreeToRaidens(angle));
        double cosx = Math.Cos(ImgHelper.DegreeToRaidens(angle));

        int h1 = Math.Abs(Math.Max(0, p1.Y) - Math.Max(0, p0.Y));
        int w1 = Math.Abs(Math.Max(0, p0.X) - Math.Max(0, p1.X));
        double edge0101 = h1 * sinx;
        double edge0102 = w1 * cosx;
        double edge01 = Math.Min(edge0101, edge0102);
        double edge0103 = Math.Max(0, rlvPt.X) / cosx;
        if (rlvPt.Y < Math.Max(0, rlvPt.X) * tanx)
        {
            edge0103 = edge0103 - (Math.Max(0, rlvPt.X) * tanx - rlvPt.Y) / sinx;
        }

        edge01 = edge01 + Math.Max(edge0103, 0);
        return edge01;
    }

    /// <summary>
    /// 把绝对坐标转换为控件坐标
    /// </summary>
    /// <param name="currLCR">当前切片当前层数据</param>
    /// <param name="maxLCR">当前切片最大层数据</param>
    /// <param name="mapr">当前视野的地图范围数据</param>
    /// <param name="absPoint">绝对坐标</param>
    /// <param name="degreeA">角度</param>
    /// <param name="scale">放大倍率</param>
    /// <returns>控件坐标</returns>
    public static Point ImgPoint(ScanPageLevel currLCR, ScanPageLevel maxLCR,
       MapRectangle mapr, Point absPoint, float degreeA, double scale)
    {
        Point[] points = mapr.CornerPoints(1, degreeA);

        int turn = Convert.ToInt32(Math.Floor(degreeA / 90));
        double degree = degreeA % 90;

        float pitchCol = maxLCR.ColSpan() / currLCR.ColSpan();
        float pitchRow = maxLCR.RowSpam() / currLCR.RowSpam();

        double cosx = Math.Cos(ImgHelper.DegreeToRaidens(degree));
        double sinx = Math.Sin(ImgHelper.DegreeToRaidens(degree));
        double tanx = Math.Tan(ImgHelper.DegreeToRaidens(degree));
        int h = Convert.ToInt32(mapr.Height(0));
        int w = Convert.ToInt32(mapr.Width(0));

        Point rp = new Point(Convert.ToInt32(absPoint.X / pitchCol - mapr.ColStart * Constants.PicW),
            Convert.ToInt32(absPoint.Y / pitchRow - mapr.RowStart * Constants.PicH));
        double rx = rp.X;
        double ry = rp.Y;
        double rlvPtX = 0, rlvPtY = 0;

        if (turn == 0)
        {
            //第一象限
            rlvPtY = (ry + rx * tanx) * cosx;
            rlvPtX = rx / cosx + (h * cosx - rlvPtY) * tanx;
        }
        else if (turn == 1)
        {
            //第二象限
            rlvPtX = w * sinx + h * cosx - (ry + rx * tanx) * cosx;
            rlvPtY = rx / cosx + (rlvPtX - w * sinx) * tanx;
        }
        else if (turn == 3)
        {
            //第四象限
            double h1 = (rx - ry / cosx * sinx) * cosx;
            rlvPtY = points[0].Y - h1;
            rlvPtX = ry / cosx + h1 * tanx;
        }
        else if (turn == 2)
        {
            //第三象限
            double y1 = (ry + rx * tanx) * cosx;
            double x1 = rx / cosx + (h * cosx - y1) * tanx;
            rlvPtX = w * cosx + h * sinx - x1;
            rlvPtY = w * sinx + h * cosx - y1;
        }

        Point ret0 = new Point(Convert.ToInt32(rlvPtX * scale),
            Convert.ToInt32(rlvPtY * scale));
        return ret0;
    }

    /// <summary>
    /// 计算当前视野下控件范围内的地图范围
    /// </summary>
    /// <param name="imgOffset">图像偏移量</param>
    /// <param name="mapr">超出视野范围的任意大小地图范围</param>
    /// <param name="imgCtrlSize">控件尺寸</param>
    /// <returns>地图范围</returns>
    public static MapRectangle VisableMapRectangle(Point imgOffset, MapRectangle mapr, Size imgCtrlSize)
    {
        MapRectangle ret = new MapRectangle();

        ret.RowStart = Math.Max(mapr.RowStart, mapr.RowStart + imgOffset.Y / Constants.PicH);
        ret.RowEnd = Math.Min(mapr.RowEnd,
            ret.RowStart + imgCtrlSize.Height / Constants.PicH + 1
            + Math.Min(0, imgOffset.Y / Constants.PicH));

        ret.ColStart = Math.Max(mapr.ColStart, mapr.ColStart + imgOffset.X / Constants.PicW);
        ret.ColEnd = Math.Min(mapr.ColEnd,
            ret.ColStart + imgCtrlSize.Width / Constants.PicW + 1
            + Math.Min(0, imgOffset.X / Constants.PicW));
        ret.Level = mapr.Level;

        return ret;
    }
}
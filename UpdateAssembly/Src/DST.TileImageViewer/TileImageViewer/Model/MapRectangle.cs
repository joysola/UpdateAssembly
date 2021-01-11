using System;
using System.Drawing;

/// <summary>
/// 九宫格地图方形
/// </summary>
public class MapRectangle
{
    public int ColStart { get; set; }
    public int ColEnd { get; set; }
    public int RowStart { get; set; }
    public int RowEnd { get; set; }
    public int Level { get; set; }
    public int RowMaxEnd { get; set; }
    public int ColMaxEnd { get; set; }
    public float ToScale { get; set; } = 1;

    public bool IsFullLevelMapR()
    {
        if (ColStart == 0 && RowStart == 0 && ColEnd == ColMaxEnd && RowEnd == RowMaxEnd)
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + ColStart.GetHashCode();
            hash = hash * 23 + ColEnd.GetHashCode();
            hash = hash * 23 + RowStart.GetHashCode();
            hash = hash * 23 + RowEnd.GetHashCode();
            hash = hash * 23 + Level.GetHashCode();
            hash = hash * 23 + RowMaxEnd.GetHashCode();
            hash = hash * 23 + ColMaxEnd.GetHashCode();
            hash = hash * 23 + ToScale.GetHashCode();
            return hash;
        }
    }

    public MapRectangle()
    {
    }

    public MapRectangle Clone()
    {
        return MemberwiseClone() as MapRectangle;
    }

    public Point[] CornerPoints(double scale, double angleA, int cs, int ce, int rs, int re)
    {
        int turn = Convert.ToInt16(Math.Floor(angleA / 90));
        double angle = angleA % 90;

        Point[] startPointsA = CornerPoints(scale, angleA);
        Point[] startPoints = CornerPoints(scale, angle);

        double sinx = Math.Sin(ImgHelper.DegreeToRaidens(angle));
        double cosx = Math.Cos(ImgHelper.DegreeToRaidens(angle));
        double tanx = Math.Tan(ImgHelper.DegreeToRaidens(angle));
        double ctanx = 1 / Math.Tan(ImgHelper.DegreeToRaidens(angle));

        double csl = Math.Abs((cs - ColStart) * Constants.PicW) * scale;
        double cse = Math.Abs((ce - ColStart) * Constants.PicW) * scale;
        double rsl = Math.Abs((rs - RowStart) * Constants.PicH) * scale;
        double rse = Math.Abs((re - RowStart) * Constants.PicH) * scale;

        double cser = Math.Abs((ColEnd - ce) * Constants.PicW) * scale;
        double rser = Math.Abs((RowEnd - re) * Constants.PicH) * scale;

        double rspanl = ((re - rs) * Constants.PicH) * scale;
        double cspanl = ((ce - cs) * Constants.PicW) * scale;

        double csespanl = Math.Abs((ColEnd - cs) * Constants.PicW) * scale;
        double rsespanl = Math.Abs((RowEnd - rs) * Constants.PicH) * scale;

        Point[] points = { };

        if (turn == 0 || turn == 2)
        {
            Point p0 = new Point(Convert.ToInt16(startPoints[0].X + csl * cosx - rsl * sinx),
                Convert.ToInt16(csl * sinx + rsl * cosx));

            Point p1 = new Point(Convert.ToInt16(p0.X - rspanl * sinx),
                Convert.ToInt16(p0.Y + rspanl * cosx));

            Point p3 = new Point(Convert.ToInt16(p0.X + cspanl * cosx),
                Convert.ToInt16(p0.Y + cspanl * sinx));

            Point p2 = new Point(Convert.ToInt16(startPoints[0].X + cse * cosx - rse * sinx),
                Convert.ToInt16(cse * sinx + rse * cosx));

            if (turn == 0)
            {
                points = new Point[] { p0, p1, p2, p3 };
            }
            else
            {
                points = new Point[] { p2, p3, p0, p1 };
            }
        }
        else if (turn == 1)
        {
            Point p0 = new Point(Convert.ToInt16(startPointsA[1].X + rser * cosx - csl * sinx),
              Convert.ToInt16(rser * sinx + csl * cosx));

            Point p1 = new Point(Convert.ToInt16(p0.X + rspanl * cosx),
                Convert.ToInt16(p0.Y + rspanl * sinx));

            Point p3 = new Point(Convert.ToInt16(p0.X - cspanl * sinx),
                Convert.ToInt16(p0.Y + cspanl * cosx));

            Point p2 = new Point(Convert.ToInt16(p3.X + rspanl * cosx), Convert.ToInt16(p3.Y + rspanl * sinx));

            points = new Point[] { p1, p0, p3, p2 };
        }
        else
        {
            Point p0 = new Point(Convert.ToInt16(startPointsA[3].X + rse * cosx - cser * sinx),
              Convert.ToInt16(rse * sinx + cser * cosx));

            Point p1 = new Point(Convert.ToInt16(p0.X - rspanl * cosx),
            Convert.ToInt16(p0.Y - rspanl * sinx));

            Point p2 = new Point(Convert.ToInt16(p0.X - cspanl * sinx),
            Convert.ToInt16(p0.Y + cspanl * cosx));
            Point p3 = new Point(Convert.ToInt16(p2.X - rspanl * cosx),
                  Convert.ToInt16(p2.Y - rspanl * sinx));
            points = new Point[] { p3, p2, p0, p1, };
        }

        return points;
    }

    public Point[] CornerImgCtrlPoints(double scale, double angleA, Point imgOffset)
    {
        Point[] points = CornerPoints(scale, angleA);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Point(points[i].X - imgOffset.X, points[i].Y - imgOffset.Y);
        }
        return points;
    }

    public Point[] CornerPoints(double scale, double angleA)
    {
        int turn = Convert.ToInt16(Math.Floor(angleA / 90));
        double angle = angleA % 90;

        double rowspan = (RowEnd - RowStart) * Constants.PicH * scale;
        double colspan = (ColEnd - ColStart) * Constants.PicW * scale;

        double d1 = rowspan * Math.Sin(ImgHelper.DegreeToRaidens(angle));
        double d2 = rowspan * Math.Cos(ImgHelper.DegreeToRaidens(angle));

        double d3 = colspan * Math.Cos(ImgHelper.DegreeToRaidens(angle));
        double d4 = colspan * Math.Sin(ImgHelper.DegreeToRaidens(angle));
        Point[] points = { };

        if (turn == 1 || turn == 3)
        {
            Point p1w = new Point(Convert.ToInt16(d4), 0);
            Point p2w = new Point(0, Convert.ToInt16(d3));
            Point p3w = new Point(Convert.ToInt16(d2 + d4), Convert.ToInt16(d1));
            Point p4w = new Point(Convert.ToInt16(d2), Convert.ToInt16(d3 + d1));
            if (turn == 1)
            {
                points = new Point[] { p3w, p1w, p2w, p4w };
            }
            else if (turn == 3)
            {
                points = new Point[] { p2w, p4w, p3w, p1w };
            }
        }
        else
        {
            Point p1 = new Point(Convert.ToInt32(d1), 0);
            Point p2 = new Point(0, Convert.ToInt32(d2));
            Point p3 = new Point(Convert.ToInt32(d3), Convert.ToInt32(d2 + d4));
            Point p4 = new Point(Convert.ToInt32(d3 + d1), Convert.ToInt32(d4));
            if (turn == 0)
            {
                points = new Point[] { p1, p2, p3, p4 };
            }
            else if (turn == 2)
            {
                points = new Point[] { p3, p4, p1, p2 };
            }
        }
        return points;
    }

    public MapRectangle(int colStart, int colEnd, int colMaxEnd, int rowStart, int rowEnd, int rowMaxEnd, int level)
    {
        ColStart = colStart;
        ColEnd = colEnd;
        ColMaxEnd = colMaxEnd;
        RowMaxEnd = rowMaxEnd;
        RowStart = rowStart;
        RowEnd = rowEnd;
        Level = level;
    }

    public MapRectangle(ScanPageLevel levelColsRows)
    {
        ColStart = 0;
        ColEnd = ScanPageLevel.GetActual(levelColsRows.EndCol);
        ColMaxEnd = ScanPageLevel.GetActual(levelColsRows.EndCol);

        RowStart = 0;
        RowEnd = ScanPageLevel.GetActual(levelColsRows.EndRow);
        RowMaxEnd = ScanPageLevel.GetActual(levelColsRows.EndRow);

        Level = levelColsRows.Level;
        ToScale = levelColsRows.ToScale;
    }

    public Point ToMapPoint(Point ctrlPoint)
    {
        Point point = new Point(ctrlPoint.X * 9 / Level, ctrlPoint.Y * 9 / Level);
        return point;
    }

    public double Height(int degree)
    {
        int angle = degree % 90;

        double sinx = Math.Sin(ImgHelper.DegreeToRaidens(angle));
        double cosx = Math.Cos(ImgHelper.DegreeToRaidens(angle));

        int rl = (RowEnd - RowStart) * Constants.PicH;
        int cl = (ColEnd - ColStart) * Constants.PicW;

        return cl * sinx + rl * cosx;
    }

    public double Width(int degree)
    {
        int angle = degree % 90;
        double sinx = Math.Sin(ImgHelper.DegreeToRaidens(angle));
        double cosx = Math.Cos(ImgHelper.DegreeToRaidens(angle));

        int rl = (RowEnd - RowStart) * Constants.PicH;
        int cl = (ColEnd - ColStart) * Constants.PicW;

        return cl * cosx + rl * sinx;
    }

    public bool IsValid()
    {
        if (ColMaxEnd < ColEnd ||
            RowMaxEnd < RowEnd || ColStart < 0 || RowStart < 0 || ColStart > ColEnd || RowStart > RowEnd)
        {
            return false;
        }
        return true;
    }

    public Point ImgOffSetIn(ScanPage scanPage,
        Point ctrlPoint, Point absPoint, float degree)
    {
        ScanPageLevel currLCR = scanPage.GetColsRows(Level);
        ScanPageLevel maxLCR = scanPage.GetColsRows(scanPage.MaxLevel);
        Point p = ImgCtrlTransHelper.ImgPoint(currLCR, maxLCR, this, absPoint, degree, ToScale);
        Point ret = new Point(p.X - ctrlPoint.X, p.Y - ctrlPoint.Y);

        return p;
    }
}
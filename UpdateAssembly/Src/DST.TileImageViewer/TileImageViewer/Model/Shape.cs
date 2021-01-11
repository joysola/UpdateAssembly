using System;
using System.Drawing;

//绘制枚举类型
public enum AnnotationType
{
    Stop = 0, Line = 1, Rectangle = 2, Circle = 3, Sketch = 4, FixRectangle = 5, FixCircle = 6, Arrow = 7, TMA = 8, MeasureLine = 9
}

/// <summary>
/// 形状抽象类
/// </summary>
public abstract class Shape
{
    private Color penColor;
    private float penWidth;

    private AnnotationType type;

    private Rectangle[] corners;
    private bool adjusting;

    public AnnotationType Type { get => type; set => type = value; }
    public Color PenColor { get => penColor; set => penColor = value; }
    public float PenWidth { get => penWidth; set => penWidth = value; }
    public Point Point0 { get; set; }
    public Point Point1 { get; set; }
    public Point Point2 { get; set; }
    public Point Point3 { get; set; }

    public bool Adjusting { get => adjusting; set => adjusting = value; }
    public Rectangle[] Corners { get => corners; set => corners = value; }

    public abstract void Location(Point p0, Point p1, Point p2, Point p3, double unit);

    public abstract void Draw(Graphics g, Point p0, Point p1, Point p2, Point p3, Point offSet);

    public abstract string Detail(bool mm = false);

    public abstract Rectangle[] CalcCorners(Point start, Point end);
}

/// <summary>
/// 方形
/// </summary>
public class MyRectangle : Shape
{
    private double length;
    private double area;

    public double Length { get => length; set => length = value; }
    public double Area { get => area; set => area = value; }

    public MyRectangle(Point p0, Point p1, Point p2, Point p3, Color cc, float ww, AnnotationType t)
    {
        this.Point0 = p0;
        this.Point1 = p1;
        this.Point2 = p2;
        this.Point3 = p3;

        this.Type = t;

        PenColor = cc;
        PenWidth = ww;

        Corners = new Rectangle[4];
    }

    public override Rectangle[] CalcCorners(Point start, Point end)
    {
        if (Corners != null)
        {
            Corners[0] = new Rectangle(start.X - 8, start.Y - 8, 16, 16); //left-top
            Corners[1] = new Rectangle(end.X - 8, end.Y - 8, 16, 16);     //right-bottom
            Corners[2] = new Rectangle(end.X - 8, start.Y - 8, 16, 16);   //right-top
            Corners[3] = new Rectangle(start.X - 8, end.Y - 8, 16, 16);   //left-bottom
        }

        return Corners;
    }

    public override void Location(Point p0, Point p1, Point p2, Point p3, double unit)
    {
        this.Point0 = p0;
        this.Point2 = p2;
        this.Point1 = p1;
        this.Point3 = p3;

        double w = Math.Abs(this.Point0.X - this.Point2.X) * unit;
        double h = Math.Abs(this.Point0.Y - this.Point2.Y) * unit;
        this.Length = Math.Round(2 * (w + h), 2); //um
        this.Area = Math.Round(w * h, 2); //um
    }

    public override void Draw(Graphics g, Point p0, Point p1, Point p2, Point p3, Point offSet)
    {
        int x1 = p0.X;
        int y1 = p0.Y;
        int x2 = p2.X;
        int y2 = p2.Y;
        p0.Offset(offSet);
        p1.Offset(offSet);
        p2.Offset(offSet);
        p3.Offset(offSet);
        Point[] points = { p0, p1, p2, p3, p0 };

        Pen pen = new Pen(PenColor, PenWidth);
        //g.DrawRectangle(pen, new Rectangle(x1 + offSet.X, y1 + offSet.Y, x2 - x1, y2 - y1));
        g.DrawLines(pen, points);

        if (Adjusting)
        {
            Rectangle[] crt = CalcCorners(p0, p2);
            Pen p = new Pen(Color.LightSlateGray, 1.5f);
            g.DrawRectangles(p, crt);
            g.FillRectangles(new SolidBrush(Color.LightYellow), crt);
        }

        return;
    }

    public override string Detail(bool mm = false)
    {
        if (Length > 4000)
            mm = true;

        string strDetail;
        if (mm)
            strDetail = "周长: " + Math.Round(this.Length / 1000, 2).ToString() + "mm\n面积: " + Math.Round(this.Area / (1000 * 1000), 2).ToString() + "mm²";
        else
            strDetail = "周长: " + this.Length.ToString() + "um\n面积: " + this.Area.ToString() + "um²";

        return strDetail;
    }
}

/// <summary>
/// 直线
/// </summary>
public class MyLine : Shape
{
    private double length;

    public double Length { get => length; set => length = value; }

    public MyLine(Point s, Point e, Color cc, float ww, AnnotationType type)
    {
        this.Point0 = s;
        this.Point2 = e;
        this.Type = type;

        PenColor = cc;
        PenWidth = ww;

        Corners = new Rectangle[2];
    }

    public override Rectangle[] CalcCorners(Point start, Point end)
    {
        if (Corners != null)
        {
            Corners[0] = new Rectangle(start.X - 8, start.Y - 8, 16, 16);//left-top
            Corners[1] = new Rectangle(end.X - 8, end.Y - 8, 16, 16);    //right-bottom
        }

        return Corners;
    }

    public override void Location(Point p0, Point p1, Point p2, Point p3, double unit)
    {
        this.Point0 = p0;
        this.Point2 = p2;

        double w = Math.Abs(this.Point0.X - this.Point2.X) * unit;
        double h = Math.Abs(this.Point0.Y - this.Point2.Y) * unit;

        this.Length = Math.Round(Math.Sqrt(w * w + h * h), 2); //um
    }

    public override void Draw(Graphics g, Point p0, Point p1, Point p2, Point p3, Point offSet)
    {
        Point pp1 = new Point(p0.X + offSet.X, p0.Y + offSet.Y);
        Point pp2 = new Point(p2.X + offSet.X, p2.Y + offSet.Y);

        Pen pen = new Pen(PenColor, PenWidth);
        g.DrawLine(pen, pp1, pp2);

        if (this.Type == AnnotationType.MeasureLine)
        {
            g.DrawLine(pen, new Point(p0.X + offSet.X - 8, p0.Y + offSet.Y), new Point(p0.X + offSet.X + 8, p0.Y + offSet.Y));
            g.DrawLine(pen, new Point(p0.X + offSet.X, p0.Y + offSet.Y - 8), new Point(p0.X + offSet.X, p0.Y + offSet.Y + 8));
            g.DrawLine(pen, new Point(p2.X + offSet.X - 8, p2.Y + offSet.Y), new Point(p2.X + offSet.X + 8, p2.Y + offSet.Y));
            g.DrawLine(pen, new Point(p2.X + offSet.X, p2.Y + offSet.Y - 8), new Point(p2.X + offSet.X, p2.Y + offSet.Y + 8));
        }

        if (Adjusting)
        {
            p0.Offset(offSet);
            p2.Offset(offSet);
            Rectangle[] crt = CalcCorners(p0, p2);
            Pen p = new Pen(Color.LightSlateGray, 1.5f);
            g.DrawRectangles(p, crt);
            g.FillRectangles(new SolidBrush(Color.LightYellow), crt);
        }

        return;
    }

    public override string Detail(bool mm = false)
    {
        if (Length > 10000)
            mm = true;

        string strDetail = "";
        if (Length > 0)
        {
            if (mm)
                strDetail = "长: " + Math.Round(this.Length / 1000, 2).ToString() + "mm";
            else
                strDetail = "长: " + this.Length.ToString() + "um";
        }

        return strDetail;
    }
}

/// <summary>
/// 圆形
/// </summary>
public class MyCircle : Shape
{
    private double length;
    private double area;
    private double a;
    private double b;

    public double Length { get => length; set => length = value; }
    public double Area { get => area; set => area = value; }
    public double A { get => a; set => a = value; }
    public double B { get => b; set => b = value; }

    public MyCircle(Point s, Point e, Color cc, float ww, AnnotationType t)
    {
        this.Point0 = s;
        this.Point2 = e;
        this.Type = t;

        PenColor = cc;
        PenWidth = ww;

        Corners = new Rectangle[4];
    }

    public override Rectangle[] CalcCorners(Point start, Point end)
    {
        if (Corners != null)
        {
            Corners[0] = new Rectangle(start.X - 8, start.Y - 8, 16, 16);//left-top
            Corners[1] = new Rectangle(end.X - 8, end.Y - 8, 16, 16);    //right-bottom
            Corners[2] = new Rectangle(end.X - 8, start.Y - 8, 16, 16);  //right-top
            Corners[3] = new Rectangle(start.X - 8, end.Y - 8, 16, 16);  //left-bottom
        }

        return Corners;
    }

    public override void Location(Point p0, Point p1, Point p2, Point p3, double unit)
    {
        this.Point0 = p0;
        this.Point2 = p2;
        this.Point1 = p1;
        this.Point3 = p3;

        double w = Math.Abs(this.Point0.X - this.Point2.X) * unit;
        double h = Math.Abs(this.Point0.Y - this.Point2.Y) * unit;
        A = Math.Round(w / 2, 2);
        B = Math.Round(h / 2, 2);

        if (A < B)
        {
            double t = A;
            A = B;
            B = t;
        }

        this.Length = Math.Round((2 * Math.PI * B + 4 * (A - B)), 2); //um
        this.Area = Math.Round((3.14 * A * B), 2); //um
    }

    public override void Draw(Graphics g, Point p0, Point p1, Point p2, Point p3, Point offSet)
    {
        Pen pen = new Pen(PenColor, PenWidth);
        g.DrawEllipse(pen, p0.X + offSet.X, p0.Y + offSet.Y, p2.X - p0.X, p2.Y - p0.Y);

        if (Adjusting)
        {
            p0.Offset(offSet);
            p2.Offset(offSet);
            Rectangle[] crt = CalcCorners(p0, p2);
            Pen p = new Pen(Color.LightSlateGray, 1.5f);
            g.DrawRectangles(p, crt);
            g.FillRectangles(new SolidBrush(Color.LightYellow), crt);
        }

        return;
    }

    public override string Detail(bool mm = false)
    {
        if (Length > 3600)
            mm = true;

        string strDetail;
        if (mm)
            strDetail = "周长: " + Math.Round(this.Length / 1000, 2).ToString() + "mm\n面积: " + Math.Round(this.Area / (1000 * 1000), 2).ToString() + "mm²";
        else
            strDetail = "周长: " + this.Length.ToString() + "um\n面积: " + this.Area.ToString() + "um²";

        return strDetail;
    }
}

public class MyTMA : Shape
{
    private double d1;
    private double d2;

    public double D1 { get => d1; set => d1 = value; }
    public double D2 { get => d2; set => d2 = value; }

    public MyTMA(Point s, Point e, Color cc, float ww)
    {
        this.Point0 = s;
        this.Point2 = e;
        this.Type = AnnotationType.TMA;

        PenColor = cc;
        PenWidth = ww;

        Corners = new Rectangle[4];
    }

    public override Rectangle[] CalcCorners(Point start, Point end)
    {
        return Corners;
    }

    public override void Location(Point p0, Point p1, Point p2, Point p3, double unit)
    {
        this.Point0 = p0;
        this.Point2 = p2;

        double d = Math.Round(Math.Abs(Point0.X - Point2.X) * unit, 2);
        D1 = Math.Round((d - 20 * unit), 2);
        D2 = Math.Round((d + 20 * unit));
    }

    public override void Draw(Graphics g, Point p0, Point p1, Point p2, Point p3, Point offSet)
    {
        Pen pen = new Pen(new SolidBrush(Color.FromArgb(125, PenColor)), PenWidth);
        g.DrawEllipse(pen, p0.X + offSet.X, p0.Y + offSet.Y, p2.X - p0.X, p2.Y - p0.Y);

        return;
    }

    public override string Detail(bool mm = false)
    {
        if (D1 > 2000)
            mm = true;

        string strDetail;
        if (mm)
            strDetail = "d1:" + Math.Round((D1 - 20 * Constants.AbsLenToRealLen_um) / 1000, 2).ToString() + "mm\\d2: " + Math.Round((D2 + 20 * Constants.AbsLenToRealLen_um) / 1000, 2).ToString() + "mm";
        else
            strDetail = "d1:" + Math.Round((D1 - 20 * Constants.AbsLenToRealLen_um), 2).ToString() + "um\\d2: " + Math.Round((D2 + 20 * Constants.AbsLenToRealLen_um), 2).ToString() + "um";

        return strDetail;
    }
}

/// <summary>
/// 箭头
/// </summary>
public class MyArrow : Shape
{
    public MyArrow(Point s, Color cc, float ww, AnnotationType t)
    {
        this.Point0 = s;
        this.PenColor = cc;
        this.PenWidth = ww;
        this.Type = t;
    }

    public override Rectangle[] CalcCorners(Point start, Point end)
    {
        return null;
    }

    private static void PointRotate(Point center, ref Point p1, double angle)
    {
        double x1 = (p1.X - center.X) * Math.Cos(angle) + (p1.Y - center.Y) * Math.Sin(angle) + center.X;
        double y1 = -(p1.X - center.X) * Math.Sin(angle) + (p1.Y - center.Y) * Math.Cos(angle) + center.Y;
        p1.X = (int)x1;
        p1.Y = (int)y1;

        return;
    }

    public override void Draw(Graphics g, Point p0, Point p1, Point p2, Point p3, Point offSet)
    {
        p0.Offset(offSet);
        Point p11 = new Point(p0.X + 20, p0.Y + 10);
        Point p22 = new Point(p0.X + 20, p0.Y + 5);
        Point p33 = new Point(p0.X + 60, p0.Y + 5);
        Point p44 = new Point(p0.X + 60, p0.Y - 5);
        Point p55 = new Point(p0.X + 20, p0.Y - 5);
        Point p66 = new Point(p0.X + 20, p0.Y - 10);

        double angle = 45 * Math.PI / 180;
        MyArrow.PointRotate(p0, ref p11, angle);
        MyArrow.PointRotate(p0, ref p22, angle);
        MyArrow.PointRotate(p0, ref p33, angle);
        MyArrow.PointRotate(p0, ref p44, angle);
        MyArrow.PointRotate(p0, ref p55, angle);
        MyArrow.PointRotate(p0, ref p66, angle);

        Pen pen = new Pen(PenColor, 3);
        Point[] ps = { p0, p0, p11, p22, p33, p44, p55, p66, p0 };
        g.DrawLines(pen, ps);

        return;
    }

    public override void Location(Point p0, Point p1, Point p2, Point p3, double unit)
    {
        return;
    }

    public override string Detail(bool mm = false)
    {
        return "Arrow";
    }
}
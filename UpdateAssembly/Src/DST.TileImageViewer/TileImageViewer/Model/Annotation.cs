using System;
using System.Drawing;

/// <summary>
/// 标注模型
/// </summary>
public class Annotation
{
    public int Id { get; set; }
    public String Title { get; set; }
    public Shape Graph { get; set; }
    private Rectangle detailRectangle;
    private bool bShow;

    public Rectangle DetailRectangle { get => detailRectangle; set => detailRectangle = value; }
    public bool BShow { get => bShow; set => bShow = value; }

    private static Brush b1 = new SolidBrush(Color.Yellow);
    private static Brush b2 = new SolidBrush(Color.Black);
    private static Pen p1 = new Pen(Color.LightSlateGray, 1.5f);

    public Annotation(String stitle, AnnotationType type, Point p0, Point p1, Point p2, Point p3, Color cc, float ww)
    {
        this.Title = stitle;
        BShow = true;

        switch (type)
        {
            case AnnotationType.Line:
            case AnnotationType.MeasureLine:
                this.Graph = new MyLine(p0, p2, cc, ww, type);
                break;

            case AnnotationType.Rectangle:
            case AnnotationType.FixRectangle:
                this.Graph = new MyRectangle(p0, p1, p2, p3, cc, ww, type);
                break;

            case AnnotationType.Circle:
            case AnnotationType.FixCircle:
                this.Graph = new MyCircle(p0, p2, cc, ww, type);
                break;

            case AnnotationType.TMA:
                this.Graph = new MyTMA(p0, p2, cc, 20);
                break;

            case AnnotationType.Arrow:
                this.Graph = new MyArrow(p0, cc, ww, type);
                break;

            default:
                break;
        }
        detailRectangle = new Rectangle();
    }

    public bool Check()
    {
        return this.Graph.Point0 == this.Graph.Point2 ? false : true;
    }

    public void SetLocation(Point p0, Point p1, Point p2, Point p3, double unit)
    {
        this.Graph.Location(p0, p1, p2, p3, unit);
    }

    public void DrawDetail(Graphics g, Point start, Point end, Point offSet)
    {
        if (this.Graph.Type == AnnotationType.Arrow)
            return;

        String strDetail = this.Detail();
        if (strDetail.Length == 0)
            return;

        int fontSize = Constants.SettingDetail.AnnotationLabelSize > 0 ? Convert.ToInt32(Constants.SettingDetail.AnnotationLabelSize) : 12;
        Font F = new Font("宋体", fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        SizeF sf = g.MeasureString(strDetail, F);
        Size sz = new Size(Convert.ToInt32(sf.Width) + 2, Convert.ToInt32(sf.Height));
        int penWidth = Convert.ToInt32(Graph.PenWidth);

        switch (this.Graph.Type)
        {
            case AnnotationType.MeasureLine:
                {
                    detailRectangle.X = (start.X + end.X) / 2 + offSet.X;
                    detailRectangle.Y = (start.Y + end.Y) / 2 + offSet.Y;
                    detailRectangle.Size = sz;
                    break;
                }
            case AnnotationType.Line:
                {
                    detailRectangle.X = start.X + offSet.X;
                    detailRectangle.Y = start.Y - sz.Height - penWidth / 2 - 5 + offSet.Y;
                    detailRectangle.Size = sz;
                    break;
                }
            case AnnotationType.Rectangle:
            case AnnotationType.FixRectangle:
                {
                    detailRectangle.X = start.X + (int)(end.X - start.X - sz.Width) / 2 + offSet.X;
                    detailRectangle.Y = start.Y - sz.Height - penWidth / 2 - 5 + offSet.Y;
                    if (start.Y > end.Y) // start 和 end 位置反了（翻转后），因此需要重新计算y起始坐标，目前默认在矩形上方
                    {
                        detailRectangle.Y = end.Y - sz.Height - penWidth / 2 - 5 + offSet.Y;
                    }
                    detailRectangle.Size = sz;
                    break;
                }
            case AnnotationType.Circle:
            case AnnotationType.FixCircle:
                {
                    detailRectangle.X = start.X + (int)(end.X - start.X - sz.Width) / 2 + offSet.X;
                    detailRectangle.Y = start.Y - sz.Height - penWidth / 2 - 5 + offSet.Y;
                    if (start.Y > end.Y) // start 和 end 位置反了（翻转后），因此需要重新计算y起始坐标，目前默认在矩形上方
                    {
                        detailRectangle.Y = end.Y - sz.Height - penWidth / 2 - 5 + offSet.Y;
                    }
                    detailRectangle.Size = sz;
                    break;
                }
            case AnnotationType.TMA:
                {
                    detailRectangle.X = start.X + (int)(end.X - start.X - sz.Width) / 2 + offSet.X;
                    detailRectangle.Y = start.Y - sz.Height - penWidth / 2 - 5 + offSet.Y;
                    detailRectangle.Size = sz;
                    break;
                }
            default:
                break;
        }

        g.FillRectangle(b1, DetailRectangle);
        g.DrawRectangle(p1, new Rectangle(DetailRectangle.X, DetailRectangle.Y, DetailRectangle.Width, DetailRectangle.Height));
        g.DrawString(strDetail, F, b2, DetailRectangle);
    }

    public string Detail()
    {
        bool bShowTitle = true;
        bool bShowSize = true;

        if (Constants.SettingDetail.ShowNameSwitch == 0)
            bShowTitle = false;
        if (Constants.SettingDetail.ShowAnnotationSizeSwitch == 0)
            bShowSize = false;

        String strRet = "";
        if (bShowTitle && Title.Length > 0)
            strRet = Title;

        if (bShowSize)
        {
            if (strRet.Length > 0)
                strRet = strRet + "\n" + this.Graph.Detail();
            else
                strRet = this.Graph.Detail();
        }

        return strRet;
    }
}
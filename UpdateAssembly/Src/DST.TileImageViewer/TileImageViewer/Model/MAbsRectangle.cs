using System.Drawing;

/// <summary>
/// 绝对坐标计算的的方形
/// </summary>
public class MAbsRectangle
{
    public Point[] Points { get; set; }

    public MAbsRectangle(Point leftTop, Point rightTop, Point leftBottom, Point rightBottom)
    {
        Points = new Point[] { leftTop, rightTop, leftBottom, rightBottom };
    }
}
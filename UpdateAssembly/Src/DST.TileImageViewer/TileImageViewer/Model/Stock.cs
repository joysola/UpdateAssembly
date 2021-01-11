using SqlSugar;
using System;
using System.Drawing;

internal class Stock
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //是主键, 还是标识列
    public int Id { get; set; }

    public string Title { get; set; }
    public int? Type { get; set; }
    public int? Pax { get; set; }
    public int? Pay { get; set; }
    public int? Pbx { get; set; }
    public int? Pby { get; set; }
    public int? Pcx { get; set; }
    public int? Pcy { get; set; }
    public int? Pdx { get; set; }
    public int? Pdy { get; set; }
    public string Width { get; set; }
    public byte? Red { get; set; }
    public byte? Green { get; set; }
    public byte? Blue { get; set; }

    public Point GetPoint0()
    {
        return new Point(Convert.ToInt32(Pax), Convert.ToInt32(Pay));
    }

    public Point GetPoint1()
    {
        return new Point(Convert.ToInt32(Pbx), Convert.ToInt32(Pby));
    }

    public Point GetPoint2()
    {
        return new Point(Convert.ToInt32(Pcx), Convert.ToInt32(Pcy));
    }

    public Point GetPoint3()
    {
        return new Point(Convert.ToInt32(Pdx), Convert.ToInt32(Pdy));
    }

    public static Stock MakeStock(Annotation an)
    {
        Stock st = new Stock();
        st.Id = an.Id;
        st.Pax = an.Graph.Point0.X;
        st.Pay = an.Graph.Point0.Y;
        st.Pcx = an.Graph.Point2.X;
        st.Pcy = an.Graph.Point2.Y;
        st.Pbx = an.Graph.Point1.X;
        st.Pby = an.Graph.Point1.Y;
        st.Pdx = an.Graph.Point3.X;
        st.Pdy = an.Graph.Point3.Y;
        st.Title = an.Title;
        st.Type = (int)an.Graph.Type;
        st.Red = an.Graph.PenColor.R;
        st.Green = an.Graph.PenColor.G;
        st.Blue = an.Graph.PenColor.B;
        st.Width = an.Graph.PenWidth.ToString();
        return st;
    }
}
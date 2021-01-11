using System;

/// <summary>
/// 切片的层
/// </summary>
public class ScanPageLevel
{
    public float EndRow { get; set; } = 0; //Item2
    public float EndCol { get; set; } = 0; //Item1
    public float StartRow { get; set; } = 0;
    public float StartCol { get; set; } = 0;
    public float ToScale { get; set; } = 1;
    public int Level { get; set; } = 0;

    public static int GetActual(float a)
    {
        return Convert.ToInt32(Math.Ceiling(a));
    }

    public float RowSpam()
    {
        return EndRow - StartRow;
    }

    public float ColSpan()
    {
        return EndCol - StartCol;
    }
}
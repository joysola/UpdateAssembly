using SqlSugar;

/// <summary>
/// 颜色调整模型
/// </summary>
public class ColorCorrection
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //是主键, 还是标识列
    public int Id { get; set; }

    // 黑色
    public int black { get; set; }

    // 伽马
    public double gamma { get; set; }

    // 白色
    public int white { get; set; }

    // R
    public int red { get; set; }

    // G
    public int green { get; set; }

    // B
    public int blue { get; set; }

    public ColorCorrection Clone()
    {
        return MemberwiseClone() as ColorCorrection;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            //hash = hash * 23 + black.GetHashCode();
            hash = hash * 23 + gamma.GetHashCode();
            hash = hash * 23 + white.GetHashCode();
            hash = hash * 23 + red.GetHashCode();
            hash = hash * 23 + green.GetHashCode();
            hash = hash * 23 + blue.GetHashCode();

            return hash;
        }
    }

    public bool Equals(ColorCorrection other)
    {
        //this非空，obj如果为空，则返回false
        if (ReferenceEquals(null, other)) return false;

        //如果为同一对象，必然相等
        if (ReferenceEquals(this, other)) return true;

        //对比各个字段值
        if (other.GetHashCode() != this.GetHashCode())
            return false;

        //如果基类不是从Object继承，需要调用base.Equals(other)
        //如果从Object继承，直接返回true
        return true;
    }

    public override bool Equals(object obj)
    {
        //this非空，obj如果为空，则返回false
        if (ReferenceEquals(null, obj)) return false;

        //如果为同一对象，必然相等
        if (ReferenceEquals(this, obj)) return true;

        //如果类型不同，则必然不相等
        if (obj.GetType() != this.GetType()) return false;

        //调用强类型对比
        return Equals((ColorCorrection)obj);
    }

    public static ColorCorrection DefaultSetting()
    {
        return new ColorCorrection
        {
            black = 0,
            gamma = 1,
            white = 0,

            red = 0,
            green = 0,
            blue = 0
        };
    }
}
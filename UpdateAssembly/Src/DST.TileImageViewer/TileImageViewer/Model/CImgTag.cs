/// <summary>
/// imgbox的tag模型
/// </summary>
public class CImgTag
{
    //角度
    private int degree;

    public int Degree
    {
        get
        {
            return degree;
        }
        set
        {
            degree = value % 360;
        }
    }

    //所属的九宫格地图方形
    public MapRectangle MapRectangle { get; set; }

    //旋转中心坐标
    public System.Drawing.Point RotateCenter { get; set; }

    //颜色修正数据
    public ColorCorrection ColorCorrection { get; set; }

    public CImgTag Clone()
    {
        return MemberwiseClone() as CImgTag;
    }

    public bool Equal(CImgTag cImg)
    {
        bool ret = false;
        if (cImg == null)
        {
            return false;
        }
        // 注意 cImg.ColorCorrection 和 ColorCorrection任意一个为null则不相等
        if ((cImg.ColorCorrection != null && ColorCorrection != null) && cImg.degree == degree &&
           (

               (cImg.ColorCorrection == null && ColorCorrection == null) ||
               (cImg.ColorCorrection.GetHashCode() == ColorCorrection.GetHashCode())
               )
           )
        {
            ret = true;
        }
        return ret;
    }
}
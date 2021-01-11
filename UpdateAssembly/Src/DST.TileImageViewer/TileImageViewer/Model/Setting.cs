using SqlSugar;
using System;

/// <summary>
/// 设置
/// </summary>
public class Setting
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //是主键, 还是标识列
    public int Id { get; set; }

    // 开关字段 1代表是  0代表否

    /**
     * 浏览Tab
     *
     * */

    // 放大选项 相关或绝对值
    public int EnlargeSelect { get; set; }

    // 放大相关值
    public int EnlargeRelationVlaue { get; set; }

    // 放大绝对值
    public int EnlargeAbsoluteVlaue { get; set; }

    // 应用缩放限定
    public int? ApplyZoomLimitSwitch { get; set; }

    // 流畅导航
    public int? SmoothSlideNavigationSwitch { get; set; }

    // 标签方向
    public int? LabelOrientationSwitch { get; set; }

    // 比例尺
    public int? ScaleBarSwitch { get; set; }

    // 比例颜色
    public string ScaleBarColor { get; set; }

    /**
     * 注释尺寸Tab
     *
     * */

    // 固定矩形宽度
    public Decimal? RectAngleWidth { get; set; }

    // 固定矩形高度
    public Decimal? RectAngleHeight { get; set; }

    // 固定矩形单位 0:μm 1:mm
    public int RectAngleUnit { get; set; }

    // 固定圆形半径
    public Decimal? CircularRadius { get; set; }

    // 固定圆形单位 0:μm 1:mm
    public int? CircularUnit { get; set; }

    // 固定圆形20 单位:mm
    public Decimal? CircularTwentyRadius { get; set; }

    // 固定圆形40 单位:mm
    public Decimal? CircularFortyRadius { get; set; }

    /**
     * 注释属性Tab
     *
     * */

    // 显示名称
    public int? ShowNameSwitch { get; set; }

    // 自动命名
    public int? AutoNameSwitch { get; set; }

    // 自动编号
    public int? AutoNumSwitch { get; set; }

    // 输入前缀
    public string PrefixStr { get; set; }

    // TMA前缀
    public string TMAPrefixStr { get; set; }

    // 显示注释大小
    public int? ShowAnnotationSizeSwitch { get; set; }

    // 切片名称字体大小
    public int? SlicesFontSize { get; set; }

    // 注释标签字体大小
    public int? AnnotationLabelSize { get; set; }

    // 明场颜色
    public string BrightfieldColor { get; set; }

    // 荧光颜色
    public string FluorescentColor { get; set; }

    /**
     * 高级设置Tab
     */

    // 调试信息
    public int ShowDebugInfoSwitch { get; set; }

    public int MemCachedSwitch { get; set; }

    // 反转鼠标缩放
    public int ReverseMouseSwitch { get; set; }

    // 语言
    public string Language { get; set; }
}
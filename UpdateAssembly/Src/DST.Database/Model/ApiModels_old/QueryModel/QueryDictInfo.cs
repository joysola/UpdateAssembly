using DST.Common.Attributes;

namespace DST.Database.Model.ApiModels_old
{
    public class QueryDictInfo
    {
    }

    /// <summary>
    /// 鳞状上皮分析结果字典
    /// </summary>
    [DSTUrl("base/choice/sampleTctResult/")]
    public class SampleTctResDict : IQueryModel
    {
    }

    /// <summary>
    /// 腺上皮细胞分析结果字典
    /// </summary>
    [DSTUrl("base/choice/glandularEpithelialCellResult/")]
    public class GlandularEpithelialCellResDict : IQueryModel
    {
    }

    /// <summary>
    /// 炎性程度
    /// </summary>
    [DSTUrl("base/choice/inflammation/")]
    public class InflammationDict : IQueryModel
    {
    }

    /// <summary>
    /// 省份
    /// </summary>
    [DSTUrl("base/0/area/")]
    public class ProvinceDict : IQueryModel
    {
    }

    /// <summary>
    /// 微生物
    /// </summary>
    [DSTUrl("base/choice/microorganismProject/")]
    public class LabelTypeDict : IQueryModel
    {
    }

    /// <summary>
    /// 检验项目
    /// </summary>
    [DSTUrl("base/choice/GlassSlideTestItemD/")]
    public class GlassSlideTestItemDDict : IQueryModel
    {
    }
}
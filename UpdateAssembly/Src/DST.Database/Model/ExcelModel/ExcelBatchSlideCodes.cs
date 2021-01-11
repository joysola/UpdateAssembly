using DST.Common.Helper;

namespace DST.Common.Model
{
    /// <summary>
    /// 批量样本编号查询
    /// </summary>
    [ExcelModel]
    public class ExcelBatchSlideCodes
    {
        /// <summary>
        /// 样本编码
        /// </summary>
        [ExcelColumn("样本编码", true)]
        public string Sample_Code { get; set; }
    }
}
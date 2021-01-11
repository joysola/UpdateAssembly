using DST.Common.Attributes;

namespace DST.Database.Model.ApiModels_old
{
    [DSTUrl("glassslide/lent/out/")]
    public class QuerySlideAllRecords : IQueryModel
    {
        /// <summary>
        /// 样本编号
        /// </summary>
        public string Sample_Code { get; set; }
    }
}
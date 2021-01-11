namespace DST.Database.Model.ApiModels_old
{
    /// <summary>
    /// 分页查询实体需要实现此接口
    /// </summary>
    public interface IQueryPageModel : IQueryModel
    {
        /// <summary>
        /// 条数
        /// 显示⼀⻚数据条数
        /// </summary>
        string Count { get; set; }

        /// <summary>
        /// 页数
        /// 显示第⼏⻚数据
        /// </summary>
        string Page_Number { get; set; }
    }

    /// <summary>
    /// 总查询实体
    /// </summary>
    public interface IQueryModel
    {
    }
}
namespace DST.Database.Model
{
    /// <summary>
    /// 查询登录实体
    /// </summary>
    public class QueryLoginModel
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
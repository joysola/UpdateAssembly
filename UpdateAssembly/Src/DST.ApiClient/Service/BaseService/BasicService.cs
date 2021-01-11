namespace DST.ApiClient.Service
{
    /// <summary>
    /// 服务父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicService<T> where T : new()
    {
        /// <summary>
        /// 子类实例
        /// </summary>
        public static T Instance { get; } = new T();
    }
}
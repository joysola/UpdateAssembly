namespace DST.Database.WPFCommonModels
{
    /// <summary>
    /// 定义项目中使用的delegate
    /// </summary>
    public class DelegateCommonObject
    {
        /// <summary>
        /// 无参异步函数
        /// </summary>
        public delegate void DelegateMethod();

        /// <summary>
        /// 带参的异步函数
        /// </summary>
        /// <param name="pars"></param>
        public delegate void DelegateParsMethod(params object[] pars);
    }
}
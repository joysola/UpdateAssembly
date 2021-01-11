namespace DST.Database
{
    public class ExtendAppContext
    {
        public static ExtendAppContext Current { get; } = new ExtendAppContext();

        private ExtendAppContext()
        {
        }

        /// <summary>
        /// 登录信息
        /// </summary>
        public LoginModel LoginModel { get; set; } = new LoginModel();
    }
}
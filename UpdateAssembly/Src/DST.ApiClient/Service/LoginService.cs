using DST.ApiClient.Api;
using DST.Database;
using DST.Database.Model;
using HttpClientExtension.ApiClient;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 登录服务
    /// </summary>
    public class LoginService : BasicService<LoginService>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="queryLoginModel"></param>
        /// <returns></returns>
        public LoginModel Login(QueryLoginModel queryLoginModel)
        {
            var result = LoginApi.Client.Login(queryLoginModel);
            HttpClientEx.SetCustomRequestHead("deepsight-auth", $"{result.data.token_type} {result.data.access_token}");
            return result.data;
        }
    }
}
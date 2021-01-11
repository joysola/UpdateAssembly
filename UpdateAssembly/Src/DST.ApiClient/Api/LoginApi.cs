using DST.Database;
using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;

namespace DST.ApiClient.Api
{
    /// <summary>
    /// 登录所需Api
    /// </summary>
    public class LoginApi : BaseApi<LoginApi>
    {
        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="postLoginModel"></param>
        /// <returns></returns>
        [Url("api/deepsight-auth/oauth/login")]
        [HttpPost]
        public ApiResponse<LoginModel> Login([PostContent] QueryLoginModel postLoginModel) => GetResult();
    }
}
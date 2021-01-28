using DST.Database;
using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System.Collections.Generic;

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
        /// <summary>
        /// 获取用户授权后的菜单信息
        /// 1211820832948236289 标记系统=》1. 1305313454723633154 标记页面；2. 1305313661418934274 标记符合页面
        /// </summary>
        /// <returns></returns>
        [Url("api/deepsight-system/system/menu/routes")]
        [HttpGet]
        public ApiResponse<List<MVAuthorizedMenus>> GetAuthorizedMens() => GetResult();
    }
}
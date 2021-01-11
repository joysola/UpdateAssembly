using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DST.Common.Model
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        public string Grant_Type { get; set; } = "password"; // 写死
        public string Client_Id { get; set; } = ConfigurationManager.AppSettings["Client_Id"]; // 读取配置
        public string Client_Secret { get; set; } = ConfigurationManager.AppSettings["Client_Secret"]; // 读取配置
        /// <summary>
        /// 登录后返回的信息
        /// </summary>
        [JsonIgnore]
        public LoginResult Result { get; set; }
        /// <summary>
        /// 错误型信息
        /// </summary>
        [JsonIgnore]
        public LoginError Error { get; set; }
    }
    /// <summary>
    /// 登录后返回的结果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 访问的token
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// access_token过期时间（秒）
        /// </summary>
        public int Expires_In { get; set; }
        /// <summary>
        /// token类型
        /// </summary>
        public string Token_Type { get; set; }
        public string Scope { get; set; }
        /// <summary>
        /// 当access_token过期后，使用此token可以获取新的access_token，目前token的过期时间7天
        /// </summary>
        public string Refresh_Token { get; set; }
    }

    public class LoginError
    {
        public string Error { get; set; }
        public string Error_Description { get; set; }
    }
}

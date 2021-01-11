using DST.Common;
using DST.Common.Logger;
using DST.Database.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DST.ApiClient.Attribute
{
    /// <summary>
    /// http请求处理特性父类
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class BaseHttpAttribute : System.Attribute
    {
        /// <summary>
        /// url处理后的结果
        /// </summary>
        protected class UrlResult
        {
            /// <summary>
            /// 地址
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// post的实体
            /// </summary>
            public object PostModel { get; set; }
        }
        /// <summary>
        /// 序列化结果类型
        /// </summary>
        protected Type apiResponseType = typeof(ApiResponse<>);
        /// <summary>
        /// 获取url信息
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        protected UrlResult GetUrl(object[] arguments, MethodBase methodBase)
        {
            var urlAttribute = methodBase.GetCustomAttribute<UrlAttribute>();
            var baseUrl = urlAttribute.Url; // 请求地址
            object postModel = null; // post实体
            // 构建完整url
            var parameters = methodBase.GetParameters();
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < arguments.Length; i++)
            {
                if (!parameters[i].IsDefined(typeof(PostContentAttribute)))
                {
                    dict.Add(parameters[i].Name, arguments[i]);
                }
                else
                {
                    postModel = arguments[i];
                }
            }
            var paramUrl = string.Empty; // url参数
            if (dict.Count > 0)
            {
                var paramUrlArray = dict.Select(x => $"{x.Key}={x.Value}").ToArray();
                paramUrl = $"?{string.Join("&", paramUrlArray)}";
            }
            var url = $"{baseUrl}{paramUrl}";
            return new UrlResult { Url = url, PostModel = postModel };
        }

        /// <summary>
        /// 从response里获取数据，设置数据
        /// </summary>
        /// <param name="httpResponse">返回数据</param>
        /// <param name="instance"></param>
        /// <param name="rtype"></param>
        protected void SetResultData(HttpResponseMessage httpResponse, object instance, Type rtype)
        {
            var msg = string.Empty;
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // 读取body
                // 预先判断是否返回了正确值
                var preProcess = JsonConvert.DeserializeObject<ApiResponse<object>>(json);
                if (!preProcess.success)
                {
                    throw new ApiException($"WebApi访问失败！原因：{preProcess.msg}");
                }
                // 正确值处理
                dynamic ins = instance; // 转成动态类型
                var baseResult = ins.GetType().GetProperty("BaseResult", BindingFlags.NonPublic | BindingFlags.Instance); // 获取BaseResult属性
                try
                {
                    if (rtype.IsGenericType && rtype.GetGenericTypeDefinition() == typeof(Task<>)) // 异步
                    {
                        var apiResType = apiResponseType.MakeGenericType(rtype.GenericTypeArguments[0]); // 构建泛型
                        dynamic result = JsonConvert.DeserializeObject(json, apiResType);
                        //var result = Convert.ChangeType(zzz.data, rtype.GenericTypeArguments[0]);
                        var taskResult = Task.FromResult(result.data); // 结果装入Task
                                                                       // return taskResult;
                        baseResult.SetValue(ins, taskResult);
                    }
                    else // 同步
                    {
                        var apiResType = apiResponseType.MakeGenericType(rtype); // 构建泛型
                        dynamic result = JsonConvert.DeserializeObject(json, apiResType);
                        // return result.data;
                        baseResult.SetValue(ins, result.data);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("HttpBase出错！", ex);
                    throw;
                }

            }
            else
            {
                throw new ApiException($"WebApi访问失败！错误代码：{(int)httpResponse.StatusCode}");
            }
        }
        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected HttpResponseMessage Post(string url, HttpContent content)
        {
            try
            {
                var result = DSTApiClient.Singleton.PostAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult(); // post方法获取数据
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("ApiPost方法出错！", ex);
                throw new ApiException($"WebApi访问失败！{ex.InnerException.Message}");
            }
        }
        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected HttpResponseMessage Get(string url)
        {
            try
            {
                var result = DSTApiClient.Singleton.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("ApiGet方法出错！", ex);
                throw new ApiException($"WebApi访问失败！{ex.InnerException.Message}");
            }
        }
    }
}

using DST.Common.Converter;

//using DST.Common.Extensions;
using DST.Common.Helper;
using DST.Common.Logger;
using DST.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DST.ApiClient.Service
{
    public class BaseService<T> where T : new()
    {
        /// <summary>
        /// httpclient客户端
        /// </summary>
        private readonly HttpClient apiClient = DSTApiClient.Singleton;

        /// <summary>
        /// 单例
        /// </summary>
        public static T Client { get; } = new T();

        /// <summary>
        /// Json和实体所有属性小写选项
        /// </summary>
        public JsonSerializerOptions LowerCaseOptions { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new LowerCaseNamingPolicy(), // 属性小写
        };

        #region Get

        /// <summary>
        /// 根据Url获取数据
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<DSTApiResponse<S>> GetResultAysnc<S>(string url) where S : new()
        {
            var response = await apiClient.GetAsync(url).ConfigureAwait(false); // 防止死锁
            var str = await response.Content.ReadAsStringAsync(); // 读取响应字符串信息
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,// 忽略属性大小写
            };
            DSTApiResponse<S> result = null;
            try
            {
                result = JsonSerializer.Deserialize<DSTApiResponse<S>>(str, options);
            }
            catch (Exception ex)
            {
                result = new DSTApiResponse<S> { Success = 0, Msg = "序列化错误" };
            }
            return result;
        }

        /// <summary>
        /// 通过请求实体来Get相关json数据
        /// </summary>
        /// <param name="model">请求实体类型</param>
        /// <returns></returns>
        protected async Task<string> GetStrFromModelAsync(IQueryModel model)
        {
            string url = model.GetUrlFromModel(); // 从查询实体返回Url
            //var url2 = model.GetUrlFromModelEX();
            var response = await apiClient.GetAsync(url).ConfigureAwait(false); // 防止死锁
            string resStr = null;
            //response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                resStr = await response.Content.ReadAsStringAsync(); // 读取响应字符串信息
            }
            return resStr;
        }

        #region 分页数据获取

        /// <summary>
        /// 从查询实体获取返回值
        /// </summary>
        /// <typeparam name="S">Samples</typeparam>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        protected async Task<DSTApiResponsePage<S>> GetResponsePageFromModel<S>(IQueryPageModel model) /*where S : ISamplesModel*/
        {
            var resStr = await GetStrFromModelAsync(model).ConfigureAwait(false); // 获取json数据

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // 忽略属性大小写
            };
            options.Converters.Add(new StringIntConverter()); // 实体字段为string时，防止不能转换成int
            DSTApiResponsePage<S> result = new DSTApiResponsePage<S> { Data = new Data<S>() }; // 结果
            try
            {
                result = JsonSerializer.Deserialize<DSTApiResponsePage<S>>(resStr, options); // 反序列化
            }
            catch (Exception ex)
            {
                Logger.Error("反序列化实体出错！", ex);
                result.Msg = "反序列化实体出错！";
            }
            return result;
        }

        /// <summary>
        /// 根据分页查询实体获取samples(异步)
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected async Task<List<S>> GetPageResultAsync<S>(IQueryPageModel model)/* where S : ISamplesModel*/
        {
            List<S> samples = new List<S>();
            var resResult = await GetResponsePageFromModel<S>(model).ConfigureAwait(false); // 返回结果
            if (resResult.Success == 1 && resResult.Data.Result != null) // 成功且有数据
            {
                samples = resResult.Data.Result.ToList();
            }
            return samples;
        }

        /// <summary>
        /// 根据查询实体获取samples(同步)
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        //protected List<S> GetResults<S>(IQueryPageModel model) /*where S : ISamplesModel*/
        //{
        //    var result = GetPageResultAsync<S>(model).Result;
        //    return result;
        //}

        #endregion 分页数据获取

        /// <summary>
        /// 从查询实体获取返回
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected async Task<U> GetResultAsync<U>(IQueryModel model)
        {
            var resStr = await GetStrFromModelAsync(model).ConfigureAwait(false); // 获取json数据
            var response = new DSTApiResponse<U>();
            U result = default(U);
            try
            {
                response = JsonSerializer.Deserialize<DSTApiResponse<U>>(resStr, LowerCaseOptions);
                if (response.Success == 1)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("GetResponseFromModel的反序列化失败！", ex);
            }
            return result;
        }

        #endregion Get

        #region Post

        /// <summary>
        /// 根据查询实体，Post上传json
        /// </summary>
        /// <param name="model"></param>
        /// <param name="json"></param>
        /// <returns>返回HttpResponseMessage</returns>
        protected async Task<HttpResponseMessage> PostJsonAsync(IQueryModel model, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // 必须带上encode和media-type
            var responseMessage = await this.PostAsync(model, content).ConfigureAwait(false);
            return responseMessage;
        }

        /// <summary>
        /// post数据并获取数据集合
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="model"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected async Task<List<S>> PostJsonGetResult<S>(IQueryModel model, string json)
        {
            List<S> result = new List<S>();
            var responseMessage = await this.PostJsonAsync(model, json).ConfigureAwait(false);
            var str = await responseMessage.Content.ReadAsStringAsync();
            var response = new DSTApiResponsePage<S>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,// 忽略属性大小写
            };
            response = JsonSerializer.Deserialize<DSTApiResponsePage<S>>(str, options);
            if (response.Success != 1)
            {
                Logger.Error($"post失败！原因：{response.Msg}");
            }
            if (response.Data?.Result != null)
            {
                result = response.Data.Result.ToList();
            }
            return result;
        }

        /// <summary>
        /// post上传json数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="json"></param>
        /// <returns>返回是否成功</returns>
        protected async Task<bool> PostJsonOKAsync(IQueryModel model, string json)
        {
            var responseMessage = await PostJsonAsync(model, json).ConfigureAwait(false);
            var str = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,// 忽略属性大小写
            };
            var response = JsonSerializer.Deserialize<DSTApiResponse<object>>(str, options);
            if (response.Success != 1)
            {
                Logger.Error($"post失败！原因：{response.Msg}");
            }
            return response.Success == 1;
        }

        /// <summary>
        /// post上传json数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns>返回是否成功</returns>
        protected async Task<bool> PostJsonOKAsync(string url, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // 必须带上encode和media-type
            var responseMessage = await this.PostAsync(url, content).ConfigureAwait(false);
            var str = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,// 忽略属性大小写
            };
            var response = JsonSerializer.Deserialize<DSTApiResponse<object>>(str, options);
            if (response.Success != 1)
            {
                Logger.Error($"post失败！原因：{response.Msg}");
            }
            return response.Success == 1;
            //return response.Msg;
        }

        /// <summary>
        /// 根据查询实体post数据（重载）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostAsync(IQueryModel model, HttpContent content)
        {
            var url = model.GetUrlFromModel();

            var response = await this.PostAsync(url, content).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// 根据url，post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var response = await apiClient.PostAsync(url, content).ConfigureAwait(false);
            return response;
        }

        #endregion Post

        #region 权限

        /// <summary>
        /// 登录成功后加上授权请求头Authorization
        /// </summary>
        protected void AddAuthorizationBearer(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        #endregion 权限
    }
}
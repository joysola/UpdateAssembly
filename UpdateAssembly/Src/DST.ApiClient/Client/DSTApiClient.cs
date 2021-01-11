using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace DST.ApiClient
{
    /// <summary>
    /// 提供HttpClient实例
    /// </summary>
    public class DSTApiClient
    {
        /// <summary>
        /// 单例Httpclient
        /// </summary>
        internal static HttpClient Singleton { get; }
        /// <summary>
        /// 初始化部分信息
        /// </summary>
        static DSTApiClient()
        {
            Singleton = new HttpClient();
            //Singleton.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 初始化Httpclient(用于更改Url)
        /// </summary>
        /// <param name="url"></param>
        public static void InitApiClient(string url)
        {
            Singleton.BaseAddress = new Uri(url);
        }
    }
}

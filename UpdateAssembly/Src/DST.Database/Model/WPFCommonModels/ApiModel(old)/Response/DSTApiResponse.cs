
using DST.Common.Converter;
using DST.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace DST.Common.Model
{
    /// <summary>
    /// api返回结果实体 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DSTApiResponsePage<T>
    {
        /// <summary>
        ///  成功 1 、失败 0
        /// </summary>
        public int Success { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Data<T> Data { get; set; }
        /// <summary>
        /// 错误信息提示（success=0,才有Msg）
        /// </summary>
        public string Msg { get; set; }
    }
    /// <summary>
    /// 数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Data<T>
    {
        /// <summary>
        /// 样本信息集合
        /// </summary>
        public IEnumerable<T> Result { get; set; }
        /// <summary>
        /// 每页的数据总量
        /// </summary>
        [JsonConverter(typeof(StringIntConverter))]
        public int Count { get; set; }
        /// <summary>
        /// 当前分页
        /// </summary>
        [JsonConverter(typeof(StringIntConverter))]
        public int Page_Number { get; set; }
        /// <summary>
        /// 数据总条数
        /// </summary>
        public int Total_Number { get; set; }
        /// <summary>
        /// 共多少分页
        /// </summary>
        public int Num_Pages { get; set; }
    }


    /// <summary>
    /// api返回结果实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DSTApiResponse<T>
    {
        /// <summary>
        ///  成功 1 、失败 0
        /// </summary>
        public int Success { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 错误信息提示（success=0,才有Msg）
        /// </summary>
        public string Msg { get; set; }
    }
}

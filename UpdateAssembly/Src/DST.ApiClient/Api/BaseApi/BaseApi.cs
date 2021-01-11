using DST.ApiClient.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.ApiClient.Api
{
    public class BaseApi<T> where T : new()
    {
        /// <summary>
        /// 子类实例（每次生成一个实例）
        /// </summary>
        public static T Client => new T();
        /// <summary>
        /// 结果值
        /// </summary>
        protected dynamic BaseResult { set; get; }
        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected dynamic GetResult()
        {
            return BaseResult;
        }


    }
}

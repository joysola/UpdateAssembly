using DST.Common.Helper;
using DST.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.ApiClient.Service
{
    public class ExtendApiDictService : BaseService<ExtendApiDictService>
    {
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <typeparam name="D">需要的结果（响应的Data结果的反序列化的类型）</typeparam>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public async Task<D> GetDictAsync<D>(IQueryModel model)
        {
            var data = await GetResultAsync<D>(model); // 
            return data;
        }

        
    }
}

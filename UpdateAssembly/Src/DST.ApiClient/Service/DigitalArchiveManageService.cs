

using DST.Common.Helper;
using DST.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DST.ApiClient.Service
{
    public class DigitalArchiveManageService : BaseService<DigitalArchiveManageService>
    {
        /// <summary>
        /// 根据条件获取原始图Samples
        /// </summary>
        /// <returns></returns>
        public async Task<List<OriginSamples>> GetOriginImgInfo(OriginImgInfo model)
        {
            //var model = new OriginImgInfo
            //{
            //    Scan_Date = new DateTime?[] { Convert.ToDateTime("2020-08-31"), Convert.ToDateTime("2020-09-01") },
            //    Count = 100.ToString(),
            //    Page_Number = 2.ToString(),
            //};
            var result = await GetPageResultAsync<OriginSamples>(model).ConfigureAwait(false);
            return result;
        }


        /// <summary>
        /// 根据条件获取Torch图
        /// </summary>
        /// <returns></returns>
        public async Task<List<TorchSamples>> GetTorchImgInfo(TorchImgInfo model)
        {
            //var model = new TorchImgInfo
            //{
            //    Scan_Date = new DateTime?[] { Convert.ToDateTime("2020-08-31"), Convert.ToDateTime("2020-09-30") },
            //    Count = 100.ToString(),
            //    Page_Number = 1.ToString(),
            //    Enlarge = 20,
            //    Exists = 1
            //};
            var result = await GetPageResultAsync<TorchSamples>(model).ConfigureAwait(false);
            return result;
        }

        public async Task Test(List<ExcelSlideInfo> models)
        {
          
            var json = JsonSerializer.Serialize(models, LowerCaseOptions);
            await PostJsonAsync(json).ConfigureAwait(false);
        }
    }
}

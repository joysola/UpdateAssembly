using DST.Common.Converter;
using DST.Common.Helper;
using DST.Common.Logger;
using DST.Common.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DST.ApiClient.Service
{
    public class InventoryArchivesManageService : BaseService<InventoryArchivesManageService>
    {
        /// <summary>
        /// Excel导入玻片入库信息
        /// </summary>
        /// <param name="slideInfos"></param>
        /// <returns></returns>
        public async Task<bool> PutSildesinStore(List<ExcelSlideInfo> slideInfos)
        {
            bool result = false;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCaseNamingPolicy(), // 属性小写
            };
            options.Converters.Add(new CustomDateTimeStrConverter()); // 接口需要的不是标准json时间格式需要转换
            var json = JsonSerializer.Serialize(slideInfos, options);
            var response = await PostJsonAsync(new QueryPutSlidesinStore(), json).ConfigureAwait(false);
            var resultStr = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<DSTApiResponse<object>>(resultStr, options); // 不介意Data数据
            if (apiResponse.Success == 1) // 成功
            {
                result = true;
            }
            else
            {
                Logger.Info($"入库失败！原因：{apiResponse.Msg}");
            }
            return result;
        }

        /// <summary>
        /// 多条件查询玻片信息
        /// </summary>
        /// <param name="querySlides"></param>
        /// <returns></returns>
        public async Task<List<SlideInfo>> GetSlides(QuerySlides querySlides)
        {
            var response = await GetPageResultAsync<SlideInfo>(querySlides).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// 分页多条件查询玻片信息
        /// </summary>
        /// <param name="querySlides"></param>
        /// <returns></returns>
        public async Task<List<SlideInfo>> GetPageSlides(QuerySlides querySlides, CustomPageModel pageModel)
        {
            // 根据分页实体获取 当前页码 和 每页条数
            querySlides.Count = pageModel.PageSize.ToString();
            querySlides.Page_Number = pageModel.PageIndex.ToString();
            var response = await GetResponsePageFromModel<SlideInfo>(querySlides).ConfigureAwait(false);
            var result = new List<SlideInfo>();
            if (response.Success != 1)
            {
                Logger.Info($"查询玻片信息失败！原因：{response.Msg}");
                return result;
            }
            else // 正确查询时，更新分页实体
            {
                pageModel.TotalNum = response.Data.Total_Number;
                pageModel.TotalPage = response.Data.Num_Pages;
                if (response.Data.Result == null)
                {
                    response.Data.Result = result;
                }
            }
            return response.Data.Result.ToList();
        }

        /// <summary>
        /// 获取借片归还记录
        /// </summary>
        /// <param name="sampleCode"></param>
        /// <returns></returns>
        public async Task<SlideAllRecords> GetSlideAllRecords(string sampleCode)
        {
            var queryModel = new QuerySlideAllRecords { Sample_Code = sampleCode };
            var result = await GetResultAsync<SlideAllRecords>(queryModel).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 创建借片信息(批量)
        /// </summary>
        /// <param name="batchLentSlidesInfo"></param>
        /// <returns>null为正确,其余为报错信息</returns>
        public async Task<bool> CreateLendInfo(BatchLentSlidesInfo batchLentSlidesInfo)
        {
            var json = JsonSerializer.Serialize(batchLentSlidesInfo, LowerCaseOptions);
            var queryModel = new QueryCreateLendInfo();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,// 忽略属性大小写
            };
            //var responseMessage = await PostJsonAsync(queryModel, json).ConfigureAwait(false);
            //var str = await responseMessage.Content.ReadAsStringAsync();
            //var response = JsonSerializer.Deserialize<DSTApiResponse<object>>(str, options);
            var result = await PostJsonOKAsync(queryModel, json).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 创建还片信息(批量)
        /// </summary>
        /// <param name="lendID">批次ID</param>
        /// <param name="batchBackSlidesInfo"></param>
        /// <returns>null为正确,其余为报错信息</returns>
        public async Task<bool> CreateBackInfo(string lendID, BatchBackSlidesInfo batchBackSlidesInfo)
        {
            var queryModel = new QueryCreateBackInfo();
            var url = queryModel.GetUrlFromModel();
            url += lendID + "/";
            var json = JsonSerializer.Serialize(batchBackSlidesInfo, LowerCaseOptions);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,// 忽略属性大小写
            };
            var result = await PostJsonOKAsync(url, json).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 查询某批次借片信息
        /// </summary>
        /// <param name="lendID">批次ID</param>
        /// <returns></returns>
        public async Task<List<SlideLendOutInfo>> GetBatchSlideLendOutInfos(string lendID)
        {
            var queryModel = new QuerySlideLendOutInfos();
            var url = queryModel.GetUrlFromModel();
            url += lendID + "/";
            var response = await GetResultAysnc<List<SlideLendOutInfo>>(url).ConfigureAwait(false);
            List<SlideLendOutInfo> data = new List<SlideLendOutInfo>();
            if (response.Success == 1)
            {
                data = response.Data;
            }
            return data;
        }

        /// <summary>
        /// 批量根据样本编号 查询玻片信息
        /// </summary>
        /// <param name="sampleCodeList">样本编号集合</param>
        /// <param name="count">每页数量</param>
        /// <param name="Page_Number">第几页</param>
        /// <returns></returns>
        public async Task<List<SlideInfo>> GetSlides(List<string> sampleCodeList, string count = null, string Page_Number = null)
        {
            var sampleCodes = new { Sample_Code = sampleCodeList };
            var queryModel = new QuerySlidesbyCodes { Count = count, Page_Number = Page_Number };

            var json = JsonSerializer.Serialize(sampleCodes, LowerCaseOptions);
            var result = await PostJsonGetResult<SlideInfo>(queryModel, json).ConfigureAwait(false);
            return result;
        }
    }
}
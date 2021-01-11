using DST.ApiClient.Api;
using DST.Common.Model;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using System.Collections.Generic;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 共建病理科样本处理服务
    /// </summary>
    public class MBPSampleService : BasicService<MBPSampleService>
    {
        /// <summary>
        /// 获取样本数据
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="queryMBP"></param>
        /// <returns></returns>
        public List<MBPSampleModel> GetMBPSamples(CustomPageModel pageModel, QueryMBPSampleList queryMBP)
        {
            var res = MBPSampleApi.Client.GetMBPSamples(pageModel.PageSize, pageModel.PageIndex, queryMBP);
            var result = res.data;
            pageModel.TotalNum = result.total;
            pageModel.TotalPage = result.pages;
            return result.records;
        }

        /// <summary>
        /// 更新样本信息(有id则更新，无id则新增)
        /// </summary>
        /// <param name="mbpModel"></param>
        /// <returns></returns>
        public bool SaveMBPSample(MBPSampleModel mbpModel)
        {
            var res = MBPSampleApi.Client.SaveMBPSample(mbpModel);
            var result = res.data;
            return result;
        }

        /// <summary>
        /// 样本退单
        /// </summary>
        /// <param name="backMBPSample"></param>
        /// <returns></returns>
        public bool BackMBPSample(BackMBPSample backMBPSample)
        {
            var res = MBPSampleApi.Client.BackMBPSample(backMBPSample);
            var result = res.data;
            return result;
        }
    }
}
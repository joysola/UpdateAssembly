using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.ApiClient.Api
{
    public class MarkingSystemApi : BaseApi<MarkingSystemApi>
    {
        /// <summary>
        /// 获取某个任务的详细信息
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/queryTagVisionBlockDetail")]
        public ApiResponse<MVBlockDetail> QueryBlockDetailofMarkingView(string blockId = "") => GetResult();
        /// <summary>
        /// 获取所有的任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/queryTagVisionBlockIndexList")]
        public ApiResponse<List<string>> QueryBlockIndexListofMarkingView() => GetResult();
        /// <summary>
        /// 获取任务进度信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/getTagVisionBlockFinishProportion")]
        public ApiResponse<MVFinishRatioInfo> GetBlockFinishProportionfromMarkingView() => GetResult();
    }
}

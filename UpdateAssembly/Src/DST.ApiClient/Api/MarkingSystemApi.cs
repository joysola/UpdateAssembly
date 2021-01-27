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
        /// <summary>
        /// 修改标记、新增标记（id为空即可）
        /// 调用结束后需要调用QueryBlockDetailofMarkingView
        /// (成功200 data 1、失败500 data null)
        /// </summary>
        /// <param name="markingInfo">标记实体</param>
        /// <returns></returns>
        [HttpPost]
        [Url("api/deepsight-tag/tag/tag-doctor-cell/saveTagCellDoctor")]
        public ApiResponse<int?> SaveMarkingbyDoctor([PostContent] MVMarkingInfo markingInfo) => GetResult();
        /// <summary>
        /// 提交该任务的标记信息
        /// </summary>
        /// <param name="blockId">任务id</param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/subMitVisionBlock")]
        public ApiResponse<int?> SubmitMarkingsofBlock(string blockId) => GetResult();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class MVFinishRatioInfo
    {
        /// <summary>
        /// 已经完成的任务数量
        /// </summary>
        public int finishCount { get; set; }
        /// <summary>
        /// 新分配任务完成数
        /// </summary>
        public int newestAllotFinishCount { get; set; }
        /// <summary>
        /// 新分配任务总数
        /// </summary>
        public int newestAllotTotalCount { get; set; }
        /// <summary>
        /// 所有任务数量
        /// </summary>
        public int totalCount { get; set; }
    }
}

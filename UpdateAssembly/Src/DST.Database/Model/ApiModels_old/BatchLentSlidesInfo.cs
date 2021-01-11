using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace DST.Database.Model.ApiModels_old
{
    /// <summary>
    /// 批量借出(提交)
    /// </summary>
    public class BatchLentSlidesInfo : ObservableObject
    {
        /// <summary>
        /// 借片机构
        /// </summary>
        public string Org_Title { get; set; }

        /// <summary>
        /// 借片人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 借片人联系方式
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 借出时间 yyyy-MM-dd
        /// </summary>
        public string Out_Time { get; set; }

        /// <summary>
        /// 预计归还时间 yyyy-MM-dd
        /// </summary>
        public string Plan_Back_Time { get; set; }

        /// <summary>
        /// 押金
        /// </summary>
        public string Deposit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 样本编号
        /// </summary>
        public List<string> Sample_Code { get; set; } = new List<string>();
    }
}
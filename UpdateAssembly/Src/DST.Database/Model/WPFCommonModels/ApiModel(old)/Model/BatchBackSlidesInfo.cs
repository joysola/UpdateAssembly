using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Model
{
    /// <summary>
    /// 批量归还(提交)
    /// </summary>
    public class BatchBackSlidesInfo : ObservableObject
    {
        /// <summary>
        /// 还片人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 还片人电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public string Back_Time { get; set; }
        /// <summary>
        /// 批量归还的所有玻片
        /// </summary>
        public List<BackSlide> Slides { get; set; }

    }
    /// <summary>
    /// 批量归还每一个玻片的信息
    /// </summary>
    public class BackSlide : ObservableObject
    {
        public string Sample_Code { get; set; }

        /// <summary>
        /// 玻片状态（非空）
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

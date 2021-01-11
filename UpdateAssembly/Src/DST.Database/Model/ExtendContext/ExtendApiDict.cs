using DST.Database.Model;
using DST.Database.Model.DictModel;
using System.Collections.Generic;

namespace DST.Database
{
    /// <summary>
    /// 字典
    /// </summary>
    public class ExtendApiDict
    {
        public static ExtendApiDict Instance { get; } = new ExtendApiDict();

        private ExtendApiDict()
        {
        }

        /// <summary>
        /// 检查项目状态状态字典
        /// </summary>
        public List<DictItem> CheckProjectStatusDict { get; set; }

        /// <summary>
        /// 导出状态
        /// </summary>
        public List<DictItem> DownFlagDict { get; set; }

        /// <summary>
        /// 性别字典
        /// </summary>
        public List<DictItem> SexDict { get; set; }

        /// <summary>
        /// 医院信息
        /// </summary>
        public HotpitalModel HotpitalInfo { get; set; }

        /// <summary>
        /// 检查项目
        /// </summary>
        public List<ProductModel> ProductDict { get; set; }

        /// <summary>
        /// 送检医生字典
        /// </summary>
        public List<SubmitDoctorModel> SubmitDoctorDict { get; set; }

        /// <summary>
        /// 实验室状态字典
        /// </summary>
        public List<DictItem> ExperimentStatusDict { get; set; }
    }
}
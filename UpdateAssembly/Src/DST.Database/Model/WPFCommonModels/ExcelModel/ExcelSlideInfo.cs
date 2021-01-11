using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DST.Common.Converter;
using DST.Common.Helper;
using DST.Common.Helper.ExcelHelper;
namespace DST.Common.Model
{
    [ExcelModel]
    public class ExcelSlideInfo
    {
        /// <summary>
        /// 省
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("省")]
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("市")]
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("区")]
        public string District { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("医院")]
        public string Hospital { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("姓名")]
        public string Pat_Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("年龄", TableColumnType = typeof(string))]
        public int? Age { get; set; }
        /// <summary>
        /// 实验编码
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("实验编码")]
        public string Experiment_Code { get; set; }
        /// <summary>
        /// 是否异常
        /// </summary>
        [JsonIgnore]
        [ExcelColumn("是否异常")]
        public string IsAbnormal { get; set; }
        /// <summary>
        /// 样本编码
        /// </summary>
        [ExcelColumn("样本编码", true)]
        public string Sample_Code { get; set; }
        /// <summary>
        /// 检查项目
        /// </summary>
        [ExcelColumn("检查项目", true)]
        public string Test_Item { get; set; }
        /// <summary>
        /// 制⽚⽇期(yyyy-MM-dd HH:mm)
        /// </summary>
        [ExcelColumn("制片时间")]
        [JsonConverter(typeof(CustomDateTimeStrConverter))]
        public DateTime Make_Time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumn("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 切片部位
        /// </summary>
        public string Slide_Parts { get; set; }
        /// <summary>
        /// 当前存放科室
        /// </summary>
        public string Keep_Room { get; set; } = "实验室";
        /// <summary>
        /// 玻片提供者
        /// </summary>
        public string Provider_Dept { get; set; } = "实验室";
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Keep_Local { get; set; }
        /// <summary>
        /// 接收玻⽚的部⻔
        /// </summary>
        public string Receiver_Dept { get; set; } = "扫描室";
    }
}

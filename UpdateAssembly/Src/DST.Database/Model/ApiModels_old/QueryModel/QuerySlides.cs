using DST.Common.Attributes;
using GalaSoft.MvvmLight;
using System;

namespace DST.Database.Model.ApiModels_old
{
    [DSTUrl("glassslide/lent/select/")]
    public partial class QuerySlides : ObservableObject, IQueryPageModel
    {
        public string Count { get; set; }
        public string Page_Number { get; set; }

        /// <summary>
        /// 样本编号
        /// </summary>
        public string Sample_Code { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 制片日期
        /// </summary>
        [DSTUrlParam]
        public DateTime?[] Make_Time => new DateTime?[2] { MakeTime_Start, MakeTime_End };

        /// <summary>
        /// 扫描时间
        /// </summary>
        [DSTUrlParam]
        public DateTime?[] Scan_Date => new DateTime?[2] { Scan_Start, Scan_End };

        /// <summary>
        /// 借片日期
        /// </summary>
        [DSTUrlParam]
        public DateTime?[] Out_Time => new DateTime?[2] { Out_Start, Out_End };

        /// <summary>
        /// 借片日期
        /// </summary>
        [DSTUrlParam]
        public DateTime?[] Back_Time => new DateTime?[2] { Back_Start, Back_End };

        /// <summary>
        /// 年龄
        /// </summary>
        [DSTUrlParamAutoFill("-100", "200")]
        public int?[] Age => new int?[2] { Age_Start, Age_End };

        /// <summary>
        /// 地域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 检查项目
        /// </summary>
        public string Test_Item { get; set; }

        /// <summary>
        /// 玻片完整程度
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否外借(1 or 0)
        /// </summary>
        public int? IsLent { get; set; }

        /// <summary>
        /// 腺上⽪细胞分析结果
        /// </summary>
        public string Gec_Result { get; set; }

        /// <summary>
        /// 鳞状上⽪细胞分析结果
        /// </summary>
        public string Sec_Result { get; set; }

        /// <summary>
        /// 微⽣物项⽬
        /// </summary>
        public string Microorganism { get; set; }

        /// <summary>
        /// 炎性程度
        /// </summary>
        public string Inflammation { get; set; }
    }

    public partial class QuerySlides : ObservableObject
    {
        // 制片
        [DSTUrlParamIgnore]
        public DateTime? MakeTime_Start { get; set; }

        [DSTUrlParamIgnore]
        public DateTime? MakeTime_End { get; set; }

        // 扫描
        [DSTUrlParamIgnore]
        public DateTime? Scan_Start { get; set; }

        [DSTUrlParamIgnore]
        public DateTime? Scan_End { get; set; }

        // 借片
        [DSTUrlParamIgnore]
        public DateTime? Out_Start { get; set; }

        [DSTUrlParamIgnore]
        public DateTime? Out_End { get; set; }

        // 归还
        [DSTUrlParamIgnore]
        public DateTime? Back_Start { get; set; }

        [DSTUrlParamIgnore]
        public DateTime? Back_End { get; set; }

        // 年龄
        [DSTUrlParamIgnore]
        public int? Age_Start { get; set; }

        [DSTUrlParamIgnore]
        public int? Age_End { get; set; }
    }
}
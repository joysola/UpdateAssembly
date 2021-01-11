using System;

namespace DST.Database.Model.ApiModels_old
{
    /// <summary>
    /// torch图sample
    /// </summary>
    public class TorchSamples : ISamplesModel
    {
        /// <summary>
        /// 扫描时间
        /// </summary>
        public DateTime? Scan_Time { get; set; }

        /// <summary>
        /// 制⽚时间
        /// </summary>
        public DateTime? Make_Date { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 病⼈姓名
        /// </summary>
        public string Patient_Name { get; set; }

        /// <summary>
        /// 腺上⽪细胞分析结果
        /// </summary>
        public string Gec_Result { get; set; }

        /// <summary>
        /// 鳞状上⽪细胞分析结果
        /// </summary>
        public string Report_Result { get; set; }

        /// <summary>
        /// 炎性程度
        /// </summary>
        public string Inflammation { get; set; }

        /// <summary>
        /// 微⽣物项⽬
        /// </summary>
        public string Microorganism { get; set; }

        /// <summary>
        /// 扫描编码
        /// </summary>
        public string Scan_Code { get; set; }

        /// <summary>
        /// 样本编码
        /// </summary>
        public string Sample_Code { get; set; }

        /// <summary>
        /// 样本原始图⽬录
        /// </summary>
        public string Directory_Path { get; set; }

        /// <summary>
        /// Torch图所在⽬录位置
        /// </summary>
        public string Torch_Path { get; set; }

        /// <summary>
        /// Torch倍率
        /// </summary>
        public int Enlarge { get; set; }
    }
}
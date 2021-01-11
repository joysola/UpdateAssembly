using GalaSoft.MvvmLight;
using System;

namespace DST.Database.Model.ApiModels_old
{
    /// <summary>
    /// 玻片信息
    /// </summary>

    public class SlideInfo : ObservableObject
    {
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>

        /// 玻片状态 { "0", "完好" }, { "1", "损坏" }, { "2", "丢失" }
        /// </summary>

        public string Status { get; set; }
        /// <summary>

        /// 是否借出 1借出、0未借出
        /// </summary>

        public int IsLent { get; set; }

        /// <summary>
        /// 借出批次
        /// </summary>
        public int? Lent_ID { get; set; }

        /// <summary>
        /// 借出时间
        /// </summary>
        public DateTime? Out_Time { get; set; }

        /// <summary>
        /// 检验项目
        /// </summary>

        public string Test_Item { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Patient_Name { get; set; }

        /// <summary>
        /// 样本编号
        /// </summary>
        public string Sample_Code { get; set; }

        private bool isSelected = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }
    }
}
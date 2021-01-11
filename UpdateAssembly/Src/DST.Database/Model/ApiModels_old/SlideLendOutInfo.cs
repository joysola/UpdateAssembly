using GalaSoft.MvvmLight;
using System;

namespace DST.Database.Model.ApiModels_old
{
    /// <summary>
    /// 借片信息（某批次）
    /// </summary>
    public class SlideLendOutInfo : ObservableObject
    {
        /// <summary>
        /// 患者年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? Back_Time { get; set; }

        /// <summary>
        /// 借出时玻片状态
        /// </summary>
        public string Out_Status { get; set; }

        /// <summary>
        /// 归还时玻片状态
        /// </summary>
        public string Back_Status { get; set; }

        public string Sample_Code { get; set; }
        public string Remark { get; set; }

        private bool isSelected = false;

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
using DST.Common.Converter;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DST.Database.Model.ApiModels_old
{
    /// <summary>
    /// 每个玻片借出归还总记录
    /// </summary>
    public class SlideAllRecords : ObservableObject
    {
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        public DateTime? Scan_Time { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? Gather_Time { get; set; }

        /// <summary>
        /// 借片还片历史记录
        /// </summary>
        public List<LendBackRecord> Lent_History { get; set; }
    }

    /// <summary>
    /// 玻片借出归还每次的记录
    /// </summary>
    public class LendBackRecord : ObservableObject
    {
        /// <summary>
        /// 借片机构
        /// </summary>
        public string Org { get; set; }

        /// <summary>
        /// 借片人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 押金
        /// </summary>
        [JsonConverter(typeof(StringNullableIntConverter))]
        public int? Deposit { get; set; }

        /// <summary>
        /// 借片人电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string Handle_By { get; set; }

        /// <summary>
        /// 经办部门
        /// </summary>
        public string Handle_Dept { get; set; }

        /// <summary>
        /// 借出时间
        /// </summary>
        public DateTime? Out_Time { get; set; }

        /// <summary>
        /// 预计归还时间
        /// </summary>
        public DateTime? Plan_Back_Time { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? Back_Time { get; set; }

        /// <summary>
        /// 归还者
        /// </summary>
        public string Back_Name { get; set; }

        /// <summary>
        /// 归还者电话
        /// </summary>
        public string Back_Telephone { get; set; }

        /// <summary>
        /// 归还经办人
        /// </summary>
        public string Back_Handle_By { get; set; }

        /// <summary>
        /// 归还经办机构
        /// </summary>
        public string Back_Handle_Dept { get; set; }

        /// <summary>
        /// 归还备注
        /// </summary>
        public string Back_Remark { get; set; }
    }
}
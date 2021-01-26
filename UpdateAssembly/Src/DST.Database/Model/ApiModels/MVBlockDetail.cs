using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class MVBlockDetail
    {
        public string biopsyResult { get; set; }
        public List<CellResult> cellResultVOList { get; set; }
        public string createDept { get; set; }
        public DateTime? createTime { get; set; }
        public string createUser { get; set; }
        public string glandularEpithelialCellResult { get; set; }
        public string hospitalId { get; set; }
        public string hpvResult { get; set; }
        public string id { get; set; }
        public string imageUrl { get; set; }
        public string isDeleted { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public DateTime? reviewAllotTime { get; set; }
        public string sort { get; set; }
        public int status { get; set; }
        public string tagAllotLogId { get; set; }
        public string tagAllotReviewLogId { get; set; }
        public DateTime? tagAllotTime { get; set; }
        public string tagFirDoctorId { get; set; }
        public string tagFirStatus { get; set; }
        public string tagInfoAge { get; set; }
        public string tagInfoCode { get; set; }
        public string tagInfoId { get; set; }
        public string tagInfoName { get; set; }
        public string tagInfoResult { get; set; }
        public string tagReviewDoctorId { get; set; }
        public string tagReviewStatus { get; set; }
        public string tagSecDoctorId { get; set; }
        public string tagSecStatus { get; set; }
        public string taskCode { get; set; }
        public int taskType { get; set; }
        public DateTime? updateTime { get; set; }
        public string updateUser { get; set; }
        public string visionId { get; set; }

    }
    public class CellResult
    {
        public string blockId { get; set; }
        public string cellDoctorId { get; set; }
        public string cellId { get; set; }
        public string doctorId { get; set; }
        public string position { get; set; }
        public string result { get; set; }
        public string tagFirResult { get; set; }
        public string tagSecResult { get; set; }
        public int taskType { get; set; }
    }
}

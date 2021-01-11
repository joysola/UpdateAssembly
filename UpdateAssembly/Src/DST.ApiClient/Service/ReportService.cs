using DST.Database;
using System.Collections.Generic;
using System.Linq;

/* name      id
   * TCT/HPV   1233733366982651905
   * HPV       1233733106256326658
   * TCT       1233732841448943617
   * 细胞穿刺   1206800484413743105
   * 微生物三项 1315823985142333442
   * B族链球菌  1319179917237800962
   * 叶酸       1249905657332883458
   * 小组织     1317016490469871617
   * 中组织     1317016574762799106
   * 大组织     1317016664758349825
   */

namespace DST.ApiClient.Service
{
    public class ReportService : BasicService<ReportService>
    {
        /// <summary>
        /// 根据id和productid获取pdf链接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<string> GetReport(string id, string productID)
        {
            var result = new List<string>();
            var product = ExtendApiDict.Instance.ProductDict.FirstOrDefault(x => x.id == productID);
            if (product != null)
            {
                product.GetReportFunc.ForEach(f =>
                {
                    string url = f(id).data;
                    if (!string.IsNullOrEmpty(url))
                    {
                        result.Add(url);
                    }
                });
            }

            return result;
        }
    }
}
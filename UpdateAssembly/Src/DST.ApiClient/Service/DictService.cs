using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 字典服务
    /// </summary>
    public class DictService : BasicService<DictService>
    {
        /// <summary>
        /// 获取字典的通用方法
        /// </summary>
        /// <param name="code">键值</param>
        /// <returns></returns>
        private async Task<List<DictItem>> GetDict(string code)
        {
            var dictRes = await Task.Run(() => DictApi.Client.GetDict(code)).ConfigureAwait(false);
            var dictModel = dictRes.data;
            var result = new List<DictItem>();
            if (dictModel.Count > 0)
            {
                result = dictModel[0].children;
            }
            return result;
        }

        /// <summary>
        /// 获取性别字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSexDict()
        {
            string code = "sex";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取导出状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetDownFlagDict()
        {
            string code = "downFlag";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取检查项目状态状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetCheckProjectStatusDict()
        {
            string code = "checkProjectStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取活检字典（0 无活检、1 有活检）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetBiopsyFlagDict()
        {
            string code = "biopsyFlagList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取HPV结果字典（0 阴性、1 阳性）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetHPVResultDict()
        {
            string code = "HPVResultList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取腺上皮细胞分析结果字典（-1 无、0 未见腺上皮内病变及恶性病变、1 非典型腺细胞（无指定）、2 原位腺癌 、3 非典型腺细胞（倾向瘤变）、4 腺癌）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetGlandularEpithelialCellResultDict()
        {
            string code = "glandularEpithelialCellResult";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取样本TCT诊断结果字典（0 未见上皮内病变及恶性病变（NILM)、1 非典型鳞状上皮细胞，无明确诊断意义（ASC-US)、2 非典型鳞状上皮细胞，不除外高度病变（ASC-H)、3 低度鳞状上皮内病变（LSIL)、4 高度鳞状上皮内病变（H-SIL)、5 鳞状细胞癌）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSampleTctResultDict()
        {
            string code = "sampleTctResult";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取样本标记状态字典（0 待发送、1 已发送、2 待标记、3 已完成）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSampleSignStatusDict()
        {
            string code = "sampleSignStatusList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取标记结果字典（1_1 ASC-US、1_2 ASC-H、1_3 LSIL、1_4 HSIL、1_5 gandular、1_6 glandular-adace、1_7 atrophy、1_8 repair、1_9 metaplastic）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSignResultDict()
        {
            string code = "signResultList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取共建病理科医院信息
        /// </summary>
        /// <returns></returns>
        public async Task<HotpitalModel> GetHotpitalInfo()
        {
            var res = await Task.Run(() => DictApi.Client.GetHotpitalInfo()).ConfigureAwait(false);
            var result = res.data;
            return result;
        }

        /// <summary>
        /// 获取所有送检医生字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<SubmitDoctorModel>> GetSubmitDoctorDict()
        {
            var res = await Task.Run(() => DictApi.Client.GetSubmitDoctors()).ConfigureAwait(false);
            var result = res.data;
            return result;
        }

        /// <summary>
        /// 获取所有检查项目字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductModel>> GetProductDict()
        {
            var res = await Task.Run(() => DictApi.Client.GetProductModels()).ConfigureAwait(false);
            var result = res.data;
            AddGetReportInfo(result); // 加入各个检测项目的调用api方法
            return result;
        }

        /// <summary>
        /// 获取实验室状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetExperimentStatusDict()
        {
            string code = "experimentStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

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

        /// <summary>
        /// 在检查项目字典里加入调用报告的函数
        /// </summary>
        /// <param name="productModels"></param>
        private void AddGetReportInfo(List<ProductModel> productModels)
        {
            foreach (var product in productModels)
            {
                switch (product.id)
                {
                    case "1233733366982651905": // TCT/HPV
                        product.GetReportFunc.Add(ReportApi.Client.GetTCTReport);
                        product.GetReportFunc.Add(ReportApi.Client.GetHPVReport);
                        break;

                    case "1233732841448943617": // tct
                        product.GetReportFunc.Add(ReportApi.Client.GetTCTReport);
                        break;

                    case "1317016490469871617": // 小组织
                    case "1317016574762799106": // 中组织
                    case "1317016664758349825": // 大组织
                        product.GetReportFunc.Add(ReportApi.Client.GetTissueReport);
                        break;

                    case "1233733106256326658": // hpv和其他
                    default:
                        product.GetReportFunc.Add(ReportApi.Client.GetHPVReport);
                        break;
                }
            }
        }
    }
}
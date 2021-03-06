﻿using DST.ApiClient.Api;
using DST.ApiClient.Service;
using DST.Database.Model;
using System.Threading.Tasks;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            var postcontent = new QueryLoginModel
            {
                //username = "gjbl-d",
                //password = "123456" 
                username = "18009637088",
                password = "123456"
            };
            #region 共建测试
            //var xx = LoginApi.Client.Login(postcontent);
            //var xx2 = DictApi.Client.GetDict("sex");
            //var xx3 = DictApi.Client.GetDict("downFlag");
            //var xx4 = DictApi.Client.GetDict("checkProjectStatus");
            //var xx5 = DictApi.Client.GetHotpitalInfo();
            //var xx6 = DictApi.Client.GetSubmitDoctors();
            //var xx7 = DictApi.Client.GetProductModels();
            //var xx = LoginService.Instance.Login(postcontent);
            //var t1 = DictService.Instance.GetCheckProjectStatusDict();
            //var t2 = DictService.Instance.GetDownFlagDict();
            //var t3 = DictService.Instance.GetHotpitalInfo();
            //var t4 = DictService.Instance.GetProductDict();
            //var t5 = DictService.Instance.GetSexDict();
            //var t6 = DictService.Instance.GetSubmitDoctorDict();
            //var postcontent2 = new MBPSampleModel
            //{
            //    id = "1339463229227384833",
            //    barCode = "tmh",
            //    clinicalManifestation = "lcbx2",
            //    doctorId = "1338352644732379138",
            //    gatherTime = DateTime.Now,
            //    hospitalId = "1338352014932512770",
            //    patentNumber = "blh",
            //    patientAge = 5200,
            //    patientName = "蔡文姬",
            //    patientPhone = "110110",
            //    patientSex = "1",
            //    productId = "1233732841448943617",
            //    productType = "",
            //    remark = "bz"
            //};
            //var xx2 = MBPSampleService.Instance.SaveMBPSample(postcontent2);
            //var postcontent3 = new QueryMBPSampleList
            //{
            //    code = "",
            //    doctorId = "",
            //    doctorName = "",
            //    downFlag = null,
            //    gatherTimeEnd = new DateTime(2020, 12, 17),
            //    gatherTimeStart = new DateTime(2010, 1, 1),
            //    patientName = "",
            //    productIdList = null,
            //    queryAgeMax = null,
            //    queryAgeMin = null,
            //    reportTimeEnd = null,
            //    reportTimeStart = null,
            //    status = "1"
            //};
            //var xx3 = MBPSampleService.Instance.GetMBPSamples(new CustomPageModel { PageIndex = 1, PageSize = 20 }, postcontent3);
            //var postcontent4 = new BackMBPSample
            //{
            //    chargeBackCause = "钱不够了，求退款",
            //    id = "1339459973998690306"
            //};
            //var xx4 = MBPSampleService.Instance.BackMBPSample(postcontent4);

            //var xx5 = ReportService.Instance.GetReport("1241263542586396673", "1233733366982651905");
            //var xx6 = ReportService.Instance.GetReport("1340156218083418113", "1317016574762799106");
            #endregion
            var xx = LoginService.Instance.Login(postcontent);
            var a1 = LoginApi.Client.GetAuthorizedMens();
            var t1 = DictService.Instance.GetBiopsyFlagDict();
            var t2 = DictService.Instance.GetHPVResultDict();
            var t3 = DictService.Instance.GetGlandularEpithelialCellResultDict();
            var t4 = DictService.Instance.GetSampleTctResultDict();
            var t5 = DictService.Instance.GetSampleSignStatusDict();
            var t6 = DictService.Instance.GetSignResultDict();
            //var h1 = DictService.Instance.GetHotpitalInfo();
            Task.WhenAll(t1, t2, t3, t4, t5, t6/*, h1*/).ConfigureAwait(false).GetAwaiter().GetResult();
            // 标记
            var m1 = MarkingSystemApi.Client.QueryBlockDetailofMarkingView();
            var m2 = MarkingSystemApi.Client.QueryBlockIndexListofMarkingView();
            var m3 = MarkingSystemApi.Client.QueryBlockDetailofMarkingView("1344569693381464066");
            var m4 = MarkingSystemApi.Client.GetBlockFinProportionfromMarkingView();
            var mvmarkingInfo = new MVMarkingInfo
            {
                BlockID = "1344569693381464066",
                ID = "1354711126247477250",
                Position = "2000,2000,3000,3000",
                Result = "1_1",
            };
            var m5 = MarkingSystemApi.Client.SaveMarkingbyDoctor(mvmarkingInfo);
            //var d1 = MarkingSystemApi.Client.DeleteMarkingbyDoctor("1354696183817179137");

            // 复核
            var r1 = MarkingSystemApi.Client.QueryBlockDetailofReviewMarkingView();
            var r2 = MarkingSystemApi.Client.QueryBlockIndexListofReviewMarkingView();
            var r3 = MarkingSystemApi.Client.QueryBlockDetailofReviewMarkingView("1331219727724449795");
            var r4 = MarkingSystemApi.Client.GetBlockReviewFinProportionfromMarkingView();
            var mvmarkingInfo2 = new MVMarkingInfo
            {
                BlockID = "1331219727724449795",
                ID = "1354704983429222401",
                Position = "1716,121,1572,302",
                Result = "1_1",
            };
            var r5 = MarkingSystemApi.Client.SaveMarkingbyReviewer(mvmarkingInfo2);
            //var r6 = MarkingSystemApi.Client.DeleteMarkingbyReivewrer("1354713388944461826");
            //var r7 = MarkingSystemApi.Client.SubmitReviewMarkingbyBlock("1331219727774781442");
        }
    }
}
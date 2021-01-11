using DST.Common.Model;
using DST.Database.Model.ApiModels_old;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel.ViewModel
{
    public class IAM_BatchReturnViewModel : CustomBaseViewModel
    {
        private int Lent_ID;
        private bool selectAll = false;
        private BatchBackSlidesInfo backSlidesInfo = null;
        private ObservableCollection<SlideLendOutInfo> slideLendOutInfoList = null;
        private DateTime? outDate = null; // 送来的第一条数据的借出时间（由于批量借出，所有所有信息都是一样的借出时间）

        public BatchBackSlidesInfo BackSlidesInfo
        {
            get { return this.backSlidesInfo; }
            set
            {
                this.backSlidesInfo = value;
                this.RaisePropertyChanged("BackSlidesInfo");
            }
        }

        /// <summary>
        /// 玻片完整度
        /// </summary>
        public Dictionary<string, string> SlideIntegrityDict => new Dictionary<string, string> { { "0", "完好" }, { "1", "损坏" }, { "2", "丢失" } };

        public ObservableCollection<SlideLendOutInfo> SlideLendOutInfoList
        {
            get { return this.slideLendOutInfoList; }
            set
            {
                this.slideLendOutInfoList = value;
                this.RaisePropertyChanged("SlideLendOutInfoList");
            }
        }

        /// <summary>
        /// 全选标志
        /// </summary>
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                this.SlideLendOutInfoList.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        /// <summary>
        /// 批量归还按钮命令
        /// </summary>
        public ICommand BatchReturnCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public IAM_BatchReturnViewModel()
        {
            this.RegisterCommand();
        }

        /// <summary>
        /// 加载窗体的参数
        /// </summary>
        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1)
            {
                SlideInfo tmp = this.Args[0] as SlideInfo;
                this.outDate = tmp.Out_Time;
                this.Lent_ID = tmp.Lent_ID.Value;
                List<SlideLendOutInfo> tmpList = new List<SlideLendOutInfo>();//InventoryArchivesManageService.Client.GetBatchSlideLendOutInfos(tmp.Lent_ID.Value.ToString()).Result;
                List<SlideLendOutInfo> result = new List<SlideLendOutInfo>();
                tmpList.ForEach(t =>
                {
                    if (string.IsNullOrEmpty(t.Back_Status))
                    {
                        t.Back_Status = t.Out_Status; // 未归还的片子，默认值取自借出时的状态
                        result.Add(t); // 过滤掉已经还的片子
                    }
                });
                this.SlideLendOutInfoList = new ObservableCollection<SlideLendOutInfo>(result);
                this.BackSlidesInfo = new BatchBackSlidesInfo() { Back_Time = DateTime.Now.ToString("yyyy-MM-dd") };
                this.SelectAll = true;
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            // 批量归还
            this.BatchReturnCommand = new RelayCommand(() =>
            {
                this.SaveData();
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected override SaveResult SaveData()
        {
            SaveResult result = SaveResult.Success;
            if (this.CheckData())
            {
                this.BackSlidesInfo.Back_Time = DateTime.Parse(this.BackSlidesInfo.Back_Time).ToString("yyyy-MM-dd");
                List<BackSlide> backSlideList = new List<BackSlide>();
                this.SlideLendOutInfoList.ToList().ForEach(x =>
                {
                    backSlideList.Add(new BackSlide() { Sample_Code = x.Sample_Code, Status = x.Back_Status, Remark = x.Remark });
                });
                this.BackSlidesInfo.Slides = backSlideList;

                bool res = true;//InventoryArchivesManageService.Client.CreateBackInfo(this.Lent_ID.ToString(), this.BackSlidesInfo).Result;
                if (!res)
                {
                    this.ShowMessageBox("还片失败，请联系管理员！");
                }
                else
                {
                    this.CloseContentWindow();
                }
            }

            return result;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        protected override bool CheckData()
        {
            bool result = true;
            if (string.IsNullOrEmpty(this.BackSlidesInfo.Name))
            {
                this.ShowMessageBox("还片人姓名不能为空！");
                result = false;
            }
            else if (string.IsNullOrEmpty(this.BackSlidesInfo.Back_Time) || DateTime.MinValue == DateTime.Parse(this.BackSlidesInfo.Back_Time))
            {
                this.ShowMessageBox("还片时间不能为空！");
                result = false;
            }
            else if (this.SlideLendOutInfoList.Where(x => x.IsSelected).Count() == 0)
            {
                this.ShowMessageBox("未选中样本记录，请重新选择需要归还的列表记录！");
                result = false;
            }
            DateTime backTime = DateTime.MinValue;
            DateTime.TryParse(BackSlidesInfo.Back_Time, out backTime);
            if (SlideLendOutInfoList.Count > 0 && outDate > backTime)
            {
                this.ShowMessageBox("归还时间大于借出时间，请重新填写归还时间！");
                result = false;
            }
            return result;
        }
    }
}
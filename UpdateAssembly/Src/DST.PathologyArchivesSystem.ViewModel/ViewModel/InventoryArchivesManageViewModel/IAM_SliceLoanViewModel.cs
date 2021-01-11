using DST.Common.Model;
using DST.Controls;
using DST.Database.Model.ApiModels_old;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class IAM_SliceLoanViewModel : CustomBaseViewModel
    {
        #region 字段

        private Visibility singleLoanVis = Visibility.Hidden;
        private Visibility batchLoanVis = Visibility.Hidden;
        private int batchCount = 0;
        private SlideInfo curSlideInfo = null;
        private List<SlideInfo> batchLentSlideInfoList = null;
        private BatchLentSlidesInfo lentSlideInfo = null;

        #endregion 字段

        #region 属性

        public BatchLentSlidesInfo LentSlideInfo
        {
            get { return this.lentSlideInfo; }
            set
            {
                this.lentSlideInfo = value;
                this.RaisePropertyChanged("LentSlideInfo");
            }
        }

        public SlideInfo CurSlideInfo
        {
            get { return this.curSlideInfo; }
            set
            {
                this.curSlideInfo = value;
                this.RaisePropertyChanged("CurSlideInfo");
            }
        }

        public Visibility BatchLoanVis
        {
            get { return this.batchLoanVis; }
            set
            {
                this.batchLoanVis = value;
                this.RaisePropertyChanged("BatchLoanVis");
            }
        }

        public Visibility SingleLoanVis
        {
            get { return this.singleLoanVis; }
            set
            {
                this.singleLoanVis = value;
                this.RaisePropertyChanged("SingleLoanVis");
            }
        }

        public int BatchCount
        {
            get { return this.batchCount; }
            set
            {
                this.batchCount = value;
                this.RaisePropertyChanged("BatchCount");
            }
        }

        #endregion 属性

        public ICommand SaveCommand { get; set; }

        public IAM_SliceLoanViewModel()
        {
            this.RegisterCommand();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public override void OnViewLoaded()
        {
            EnumMessageKey msgKey = (EnumMessageKey)this.Args[1];
            List<string> sampleCode = new List<string>();
            if (msgKey == EnumMessageKey.LendOutEdit)
            {
                this.SingleLoanVis = Visibility.Visible;
                this.BatchLoanVis = Visibility.Collapsed;
                this.CurSlideInfo = this.Args[0] as SlideInfo;
                sampleCode.Add(this.CurSlideInfo.Sample_Code);
                this.LentSlideInfo = new BatchLentSlidesInfo() { Sample_Code = sampleCode, Out_Time = DateTime.Now.ToString() };
            }
            else if (msgKey == EnumMessageKey.BatchLendOut)
            {
                this.SingleLoanVis = Visibility.Collapsed;
                this.BatchLoanVis = Visibility.Visible;
                this.batchLentSlideInfoList = this.Args[0] as List<SlideInfo>;
                this.BatchCount = this.batchLentSlideInfoList.Count;
                sampleCode = this.batchLentSlideInfoList.Select(x => x.Sample_Code).ToList();
                this.LentSlideInfo = new BatchLentSlidesInfo() { Sample_Code = sampleCode, Out_Time = DateTime.Now.ToString() };
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            SaveCommand = new RelayCommand(() =>
            {
                if (this.CheckData())
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        this.LentSlideInfo.Out_Time = DateTime.Parse(this.LentSlideInfo.Out_Time).ToString("yyyy-MM-dd");
                        this.LentSlideInfo.Plan_Back_Time = DateTime.Parse(this.LentSlideInfo.Plan_Back_Time).ToString("yyyy-MM-dd");
                        bool res = true;//InventoryArchivesManageService.Client.CreateLendInfo(this.LentSlideInfo).Result;
                        if (!res)
                        {
                            this.ShowMessageBox("借片操作失败，请联系管理员！");
                        }
                        else
                        {
                            this.Result = true;
                            this.CloseContentWindow();
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        }

        protected override bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(this.LentSlideInfo.Org_Title))
            {
                this.ShowMessageBox("借片机构信息未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.LentSlideInfo.Name))
            {
                this.ShowMessageBox("借片人姓名未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.LentSlideInfo.Out_Time))
            {
                this.ShowMessageBox("借片日期信息未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.LentSlideInfo.Telephone))
            {
                this.ShowMessageBox("借片人联系方式未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.LentSlideInfo.Plan_Back_Time))
            {
                this.ShowMessageBox("预期归还时间未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(this.LentSlideInfo.Deposit))
            {
                this.LentSlideInfo.Deposit = "0";
            }
            else if (DateTime.Parse(this.LentSlideInfo.Plan_Back_Time) < DateTime.Parse(this.LentSlideInfo.Out_Time))
            {
                this.ShowMessageBox("预期归还时间不能早于借片时间！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
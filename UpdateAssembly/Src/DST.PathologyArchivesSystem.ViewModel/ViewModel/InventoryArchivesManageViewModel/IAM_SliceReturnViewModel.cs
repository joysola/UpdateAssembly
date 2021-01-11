using DST.Controls;
using DST.Database.Model.ApiModels_old;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class IAM_SliceReturnViewModel : CustomBaseViewModel
    {
        private BackSlide curBackSlide = null;
        private SlideInfo curSlideInfo = null;
        private BatchBackSlidesInfo backSlidesInfo = null;

        public BackSlide CurBackSlide
        {
            get { return this.curBackSlide; }
            set
            {
                this.curBackSlide = value;
                this.RaisePropertyChanged("CurBackSlide");
            }
        }

        public BatchBackSlidesInfo BackSlidesInfo
        {
            get { return this.backSlidesInfo; }
            set
            {
                this.backSlidesInfo = value;
                this.RaisePropertyChanged("BackSlidesInfo");
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

        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// 玻片完整度
        /// </summary>
        public ICommand ChangedStatusCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public IAM_SliceReturnViewModel()
        {
            this.RegisterCommand();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 2)
            {
                this.CurSlideInfo = this.Args[0] as SlideInfo;
                this.CurBackSlide = new BackSlide() { Sample_Code = this.CurSlideInfo.Sample_Code, Status = "0" };
                this.BackSlidesInfo = new BatchBackSlidesInfo() { Back_Time = DateTime.Now.ToString("yyyy-MM-dd"), Slides = new List<BackSlide>() { this.CurBackSlide } };
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            SaveCommand = new RelayCommand(() =>
            {
                if (CheckData())
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        this.BackSlidesInfo.Back_Time = DateTime.Parse(this.BackSlidesInfo.Back_Time).ToString("yyyy-MM-dd");
                        bool res = true;//InventoryArchivesManageService.Client.CreateBackInfo(this.CurSlideInfo.Lent_ID.Value.ToString(), this.BackSlidesInfo).Result;
                        if (!res)
                        {
                            this.ShowMessageBox("还片失败，请联系管理员！");
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

            this.ChangedStatusCommand = new RelayCommand<object>(data =>
            {
                switch (data.ToString())
                {
                    case "完好":
                        this.CurBackSlide.Status = "0";
                        break;

                    case "损坏":
                        this.CurBackSlide.Status = "1";
                        break;

                    case "丢失":
                        this.CurBackSlide.Status = "2";
                        break;
                }
            });
        }

        protected override bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(this.BackSlidesInfo.Name))
            {
                this.ShowMessageBox("还片人姓名未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(this.BackSlidesInfo.Back_Time))
            {
                this.ShowMessageBox("还片时间未填写！", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(this.CurBackSlide.Status))
            {
                this.ShowMessageBox("玻片完整度未填写！");
                return false;
            }
            else if (this.CurSlideInfo.Lent_ID == null)
            {
                this.ShowMessageBox("借片批次编号Lent_ID为空，无法还片！");
                return false;
            }
            DateTime backTime = DateTime.MinValue;
            DateTime.TryParse(this.BackSlidesInfo.Back_Time, out backTime);
            if (this.CurSlideInfo.Out_Time > backTime)
            {
                this.ShowMessageBox("归还时间大于借出时间，请重新填写归还时间！");
                return false;
            }
            return true;
        }
    }
}
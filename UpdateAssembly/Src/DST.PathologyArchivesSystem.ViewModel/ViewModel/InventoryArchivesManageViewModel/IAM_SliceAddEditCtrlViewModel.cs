using DST.Common.Model;
using DST.Controls.Base;
using DST.Database.Model.ApiModels_old;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class IAM_SliceAddEditCtrlViewModel : CustomBaseViewModel
    {
        private bool btnIsEnable = true;
        private string strLendOrBack = "借片";
        private SlideInfo curSlideInfo = null;
        private SlideAllRecords curSlideAllRecords = null;

        public bool BtnIsEnable
        {
            get { return this.btnIsEnable; }
            set
            {
                this.btnIsEnable = value;
                this.RaisePropertyChanged("BtnIsEnable");
            }
        }

        /// <summary>
        /// 外借还是归还的中文信息
        /// </summary>
        public string StrLendOrBack
        {
            get { return this.strLendOrBack; }
            set
            {
                this.strLendOrBack = value;
                this.RaisePropertyChanged("StrLendOrBack");
            }
        }

        /// <summary>
        /// 当前玻片的借还记录
        /// </summary>
        public SlideAllRecords CurSlideAllRecords
        {
            get { return this.curSlideAllRecords; }
            set
            {
                this.curSlideAllRecords = value;
                this.RaisePropertyChanged("CurSlideAllRecords");
            }
        }

        /// <summary>
        /// 当前选中的玻片信息
        /// </summary>
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
        /// 新增命令
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// 借片编辑命令
        /// </summary>
        public ICommand EditLendCommand { get; set; }

        /// <summary>
        /// 还片编辑命令
        /// </summary>
        public ICommand EditGiveCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public IAM_SliceAddEditCtrlViewModel()
        {
            this.RegisterCommand();
            this.RegisterMessenger();
        }

        /// <summary>
        /// 获取传递过来的参数
        /// </summary>
        public override void OnViewLoaded()
        {
            this.CurSlideInfo = this.Args[0] as SlideInfo;
            this.BtnIsEnable = this.CurSlideInfo.Status != "2";
            // 由第二个参数决定显示 借片 还是 还片信息
            var showMark = (EnumMessageKey)this.Args[1];
            if (showMark == EnumMessageKey.LendOutEdit)
            {
                this.StrLendOrBack = "借片";
            }
            else
            {
                this.StrLendOrBack = "还片";
            }

            this.SearchSilceInfo();
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        private void RegisterMessenger()
        {
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            AddCommand = new RelayCommand(() =>
            {
                var args = new object[] { this.CurSlideInfo, null };
                string typeName = "";
                string title = "";
                if (this.StrLendOrBack.Equals("借片"))
                {
                    typeName = "IAM_SliceLoan";
                    title = "玻片外借";
                    args[1] = EnumMessageKey.LendOutEdit;
                }
                else
                {
                    typeName = "IAM_SliceReturn";
                    title = "玻片归还";
                    args[1] = EnumMessageKey.GiveBackEdit;
                }

                ShowContentWindowMessage msg = new ShowContentWindowMessage(typeName, title);
                msg.Width = 900;
                msg.Height = 450;
                msg.Args = args;
                msg.CallBackCommand = new RelayCommand<object>(key =>
                {
                    if (key != null && key is bool && (bool)key)
                    {
                        this.BtnIsEnable = false;
                        SearchSilceInfo();
                    }
                });

                Messenger.Default.Send<ShowContentWindowMessage>(msg);
            });
        }

        /// <summary>
        /// 展示弹窗
        /// </summary>
        /// <param name="args">传入的弹窗需要的数据</param>
        private ShowContentWindowMessage ShowEditSlice(object[] args)
        {
            string typeName = string.Empty;
            string title = string.Empty;
            if (this.StrLendOrBack.Equals("借片"))
            {
                typeName = "IAM_SliceLoan";
                title = "切片外借";
            }
            else
            {
                typeName = "IAM_SliceReturn";
                title = "切片归还";
            }
            ShowContentWindowMessage msg = new ShowContentWindowMessage(typeName, title);
            msg.Width = 900;
            msg.Height = 450;
            msg.Args = args;
            return msg;
        }

        /// <summary>
        /// 搜索借还切片信息
        /// </summary>
        private void SearchSilceInfo()
        {
            this.CurSlideAllRecords = null;//InventoryArchivesManageService.Client.GetSlideAllRecords(this.CurSlideInfo.Sample_Code).Result;
        }
    }
}
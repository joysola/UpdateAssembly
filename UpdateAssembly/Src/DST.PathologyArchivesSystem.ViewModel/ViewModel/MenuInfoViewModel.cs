using DST.Common.Model;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class MenuInfoViewModel : CustomBaseViewModel
    {
        private bool _isShowReButton = false;
        private Visibility thirdMenuVisibility = Visibility.Collapsed;
        private string firstMenuInfo = string.Empty;
        private string secondMenuInfo = string.Empty;
        private string thirdMenuInfo = string.Empty;

        public Visibility ThirdMenuVisibility
        {
            get { return this.thirdMenuVisibility; }
            set
            {
                this.thirdMenuVisibility = value;
                this.RaisePropertyChanged("ThirdMenuVisibility");
            }
        }

        /// <summary>
        /// 一级菜单
        /// </summary>
        public string FirstMenuInfo
        {
            get { return this.firstMenuInfo; }
            set
            {
                this.firstMenuInfo = value;
                this.RaisePropertyChanged("FirstMenuInfo");
            }
        }

        /// <summary>
        /// 二级菜单
        /// </summary>
        public string SecondMenuInfo
        {
            get { return this.secondMenuInfo; }
            set
            {
                this.secondMenuInfo = value;
                this.RaisePropertyChanged("SecondMenuInfo");
            }
        }

        /// <summary>
        /// 三级菜单
        /// </summary>
        public string ThirdMenuInfo
        {
            get { return this.thirdMenuInfo; }
            set
            {
                this.thirdMenuInfo = value;
                this.RaisePropertyChanged("ThirdMenuInfo");
            }
        }

        /// <summary>
        /// 是否显示返回按钮
        /// </summary>
        public bool IsShowReButton
        {
            get { return _isShowReButton; }
            set
            {
                _isShowReButton = value;
                RaisePropertyChanged("IsShowReButton");
            }
        }

        /// <summary>
        /// 返回按钮命令
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// 返回主页命令
        /// </summary>
        public ICommand HomeCommand { get; set; }

        /// <summary>
        /// 一级菜单命令
        /// </summary>
        public ICommand FirstMenuCommand { get; set; }

        /// <summary>
        /// 二级菜单
        /// </summary>
        public ICommand SecondMenuCommand { get; set; }

        /// <summary>
        /// 三级菜单
        /// </summary>
        public ICommand ThirdMenuCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public MenuInfoViewModel()
        {
            this.RegisterCommand();
        }

        /// <summary>
        /// 默认加载数据
        /// </summary>
        public override void LoadData()
        {
            this.FirstMenuInfo = "标本管理";
            this.SecondMenuInfo = "标本查询";
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            /// 刷新当前位置信息
            Messenger.Default.Register<string>(this, EnumMessageKey.RefreshMenuInfo, data =>
            {
                string[] menuInfo = data.Split(new string[] { " => " }, System.StringSplitOptions.RemoveEmptyEntries);
                this.FirstMenuInfo = menuInfo[0];
                this.SecondMenuInfo = menuInfo[1];
                if (menuInfo.Length == 3)
                {
                    this.ThirdMenuVisibility = Visibility.Visible;
                    this.ThirdMenuInfo = menuInfo[2];
                }
                else
                {
                    this.ThirdMenuVisibility = Visibility.Collapsed;
                    this.ThirdMenuInfo = string.Empty;
                }
            });

            // 返回按钮点击
            ReturnCommand = new RelayCommand(() =>
            {
                this.IsShowReButton = false;
                this.ThirdMenuVisibility = Visibility.Collapsed;
                this.ThirdMenuInfo = string.Empty;
                Messenger.Default.Send<bool>(false, EnumMessageKey.ShowReturnButton); // 不显示返回按钮 和 切片
            });

            // 返回主页
            this.HomeCommand = new RelayCommand<object>(dat =>
            {
                Messenger.Default.Send<string>("Home", EnumMessageKey.ShowHome);
            });

            // 一级菜单
            this.FirstMenuCommand = new RelayCommand<string>(data =>
            {
                Messenger.Default.Send<string>(this.FirstMenuInfo, EnumMessageKey.ShowHome);
            });

            // 二级菜单
            this.SecondMenuCommand = new RelayCommand<string>(data =>
            {
                Messenger.Default.Send<string>(this.SecondMenuInfo, EnumMessageKey.ShowHome);
            });
        }
    }
}
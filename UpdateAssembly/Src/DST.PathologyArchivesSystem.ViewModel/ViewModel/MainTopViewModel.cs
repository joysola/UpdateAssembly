using DST.Common.Model;
using DST.Database.WPFCommonModels;
using DST.Joint.Construction.Mgmt.ViewModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace DST.PathologyArchivesSystem.ViewModel.ViewModel
{
    public class MainTopViewModel : CustomBaseViewModel
    {
        private string userName = string.Empty;

        /// <summary>
        /// 登录用户
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = $"用户：{value}";
                this.RaisePropertyChanged("UserName");
            }
        }

        /// <summary>
        /// 最小化按钮
        /// </summary>
        public ICommand MinCommand { get; set; }

        /// <summary>
        /// 关闭观念
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// 注销按钮
        /// </summary>
        public ICommand LoginOutCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public MainTopViewModel()
        {
            this.UserName = Database.ExtendAppContext.Current.LoginModel.user_name;
            this.RegisterCommand();
        }

        /// <summary>
        /// 注册命令事件
        /// </summary>
        private void RegisterCommand()
        {
            this.MinCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
            });

            this.CloseCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.MainWindow.Close();
            });

            this.LoginOutCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(this, EnumMessageKey.ShowLoginOut);
            });

            // 注销登录成功后刷新登陆者信息
            Messenger.Default.Register<object>(this, EnumMessageKey.LoginOut, data =>
            {
                this.UserName = Database.ExtendAppContext.Current.LoginModel.user_name;
            });
        }
    }
}
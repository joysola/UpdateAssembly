using DST.Common.Model;
using DST.Controls;
using DST.Database.WPFCommonModels;
using DST.Joint.Construction.Mgmt.View;
using DST.Joint.Construction.Mgmt.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;

namespace DST.Joint.Construction.Mgmt
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 玻片外借管理
        /// </summary>
        private InventoryArchivesManage inventoryArchivesManage = null;

        /// <summary>
        /// 无参构造
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.DataContext = new MainWindowViewModel();
        }

        /// <summary>
        /// 设置窗口大小，同时加载初始界面
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            base.Left = 0.0;
            base.Top = 0.0;
            base.Height = SystemParameters.WorkArea.Height;
            base.Width = SystemParameters.WorkArea.Width;

            this.inventoryArchivesManage = new InventoryArchivesManage();
            this.gridMainFun.Children.Add(this.inventoryArchivesManage);
            this.lvMenu.SelectionChanged += LvMenu_SelectionChanged;
            this.RegisterMessenger();
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        private void RegisterMessenger()
        {
            // 显示主页
            Messenger.Default.Register<string>(this, EnumMessageKey.ShowHome, data =>
            {
                this.lvMenu.SelectedIndex = 0;
                switch (data)
                {
                    case "Home":
                    case "标本管理":
                    case "标本查询":
                        this.lvMenu.SelectedIndex = 1;
                        break;
                }
            });

            // 显示PDF文档
            Messenger.Default.Register<List<string>>(this, EnumMessageKey.ShowReportPdf, pdfPath =>
            {
                this.showPdf.OpenPdf(pdfPath);
                this.ResethControlsVis(this.showPdf);
                this.menuInfo.menuInfoViewModel.IsShowReButton = true;
            });

            // 显示注销登录界面
            Messenger.Default.Register<object>(this, EnumMessageKey.ShowLoginOut, data =>
            {
                this.Hide();
                Login login = new Login(true);
                login.Show();
            });

            // 注销登录成功后显示主界面
            Messenger.Default.Register<object>(this, EnumMessageKey.LoginOut, data =>
            {
                this.Show();
                this.ShowActivated = true;
            });

            // 从PDF返回到主界面
            Messenger.Default.Register<bool>(this, EnumMessageKey.ShowReturnButton, isShow =>
            {
                this.lvMenu.SelectedIndex = -1;
                this.lvMenu.SelectedIndex = 1;
            });
        }

        /// <summary>
        /// 切换菜单
        /// </summary>
        private void LvMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.gridMainFun != null && this.lvMenu.SelectedItem != null)
            {
                System.Windows.Controls.ListViewItem lvi = this.lvMenu.SelectedItem as System.Windows.Controls.ListViewItem;
                string content = lvi.Content.ToString().Trim();
                string menuInfo = "标本管理 => 标本查询";
                switch (content)
                {
                    case "标本管理":
                    case "标本查询":
                        menuInfo = "标本管理 => 标本查询";
                        this.ResethControlsVis(this.inventoryArchivesManage);
                        this.menuInfo.menuInfoViewModel.IsShowReButton = false;
                        break;
                }

                Messenger.Default.Send<string>(menuInfo, EnumMessageKey.RefreshMenuInfo);
            }
        }

        /// <summary>
        /// 显示指定的控件，其他控件隐藏
        /// </summary>
        /// <param name="ele">需要显示的控件</param>
        private void ResethControlsVis(UIElement ele)
        {
            for (int i = 0; i < this.gridMainFun.Children.Count; i++)
            {
                if (this.gridMainFun.Children[i] != ele)
                {
                    this.gridMainFun.Children[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.gridMainFun.Children[i].Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 退出功能，需要再次确认
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ConfirmMessageBox.Show("", "确认退出程序?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
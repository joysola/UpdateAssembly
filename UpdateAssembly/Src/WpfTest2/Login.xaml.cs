using DST.Common.Model;
using DST.Controls;
using DST.Database.Model;
using DST.Database.WPFCommonModels;

using GalaSoft.MvvmLight.Messaging;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Exceptions;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using DST.Joint.Construction.Mgmt.ViewModel;

namespace WpfTest2
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private bool isLoginOut = false;
        private LoginViewModel loginViewModel = null;

        /// <summary>
        /// 无参构造
        /// </summary>
        public Login()
        {
            InitializeComponent();
            this.InitApiClient();
            this.RegisterMessage();
            this.loginViewModel = new LoginViewModel();
            this.DataContext = this.loginViewModel;
            this.TextUserName.Focus();
        }

        /// <summary>
        /// 注销专用，不许要调用InitApiClient
        /// </summary>
        public Login(bool loginOut)
        {
            InitializeComponent();
            this.isLoginOut = true;
            this.RegisterMessage();
            this.loginViewModel = new LoginViewModel();
            this.DataContext = this.loginViewModel;
            this.TextUserName.Focus();
        }

        /// <summary>
        /// 初始化httpclient客户端
        /// </summary>
        private void InitApiClient()
        {
            var url = ConfigurationManager.AppSettings["MBPApi"];
            if (string.IsNullOrEmpty(url))
            {
                ConfirmMessageBox.Show("请配置Api地址！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //DSTApiClient.InitApiClient(url);
            HttpClientEx.InitApiClient(url);
            Action<dynamic> action = data =>
            {
                if (!data.success)
                {
                    throw new HttpClientException($"WebApi访问失败！原因：{data.msg}");
                }
            };
            HttpClientEx.SetPrePorcess(typeof(ApiResponse<object>), action);
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 注销关闭时处理
            if (this.isLoginOut)
            {
                if (this.loginViewModel.IsLogining)
                {
                    Messenger.Default.Send<object>(this, EnumMessageKey.LoginOut);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 接收关闭登录界面消息
        /// </summary>
        private void RegisterMessage()
        {
            // 登录成功后显示主界面
            Messenger.Default.Register<object>(this, EnumMessageKey.CloseLogin, msg =>
            {
                if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.ShowMainWindow();
                    }));
                }
                else
                {
                    this.ShowMainWindow();
                }
            });

            // 关闭窗口，因为指令是从ViewModel中传来的
            Messenger.Default.Register<object>(this, "ExitLogin", msg =>
            {
                this.Close();
            });
        }

        /// <summary>
        /// 显示主界面
        /// </summary>
        private void ShowMainWindow()
        {
            if (Application.Current.MainWindow.GetType() != typeof(MainWindow))
            {
                MainWindow main = new MainWindow();
                main.Show();
                main.ShowActivated = true;
                Application.Current.MainWindow = main;
            }

            this.Close();
        }
    }

    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
          DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
          DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }

            SetPasswordLength(pb, pb.Password.Length);
        }
    }
}
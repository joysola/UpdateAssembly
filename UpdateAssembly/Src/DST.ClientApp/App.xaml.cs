using DST.Common.Helper.Factory;
using DST.Common.Logger;
using DST.Common.WindowsAPI;
using DST.Controls;
using DST.Controls.Base;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using HttpClientExtension.Exceptions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace DST.ClientApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Mutex mut = null;            // 线程信息，判断软件是否已经启动
        private string restartFile = System.IO.Directory.GetCurrentDirectory() + "\\test.dat";
        private string ExeName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();

        /// <summary>
        /// 无参构造
        /// </summary>
        public App()
        {
            this.CheckIsRestart();
            this.CheckIsNeedRestart();
            this.SetSystemPression();
        }

        /// <summary>
        /// 判断当前运行是否为重启状态
        /// </summary>
        private void CheckIsRestart()
        {
            try
            {
                // 判断当前启动是否为重启路径
                if (System.IO.File.Exists(restartFile))
                {
                    int index = 0;
                    while (index <= 30 && Process.GetProcessesByName(this.ExeName).Length > 1)
                    {
                        WindowsAPI.KillWindow(this.ExeName);
                        System.Threading.Thread.Sleep(1000);
                        index++;
                    }

                    System.IO.File.Delete(restartFile);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 判断当前系统是否已经启动
        /// </summary>
        private void CheckIsNeedRestart()
        {
            bool requestInitialOwnerShip = true;
            bool mutexWasCreated = false;
            this.mut = new Mutex(requestInitialOwnerShip, this.ExeName, out mutexWasCreated);
            if (!(requestInitialOwnerShip && mutexWasCreated))
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("系统正在运行，是否关闭当前系统重新启动？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes || result == System.Windows.Forms.DialogResult.OK)
                {
                    // 杀掉当前正在运行的窗口
                    WindowsAPI.KillWindow(this.ExeName);
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(restartFile, System.IO.FileMode.Create);
                        fs.Close();
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch
                    { }

                    System.Windows.Forms.Application.Restart();
                    System.Environment.Exit(0);
                }
                else
                {
                    this.ActivateWindow();
                    System.Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// 激活原先窗口
        /// </summary>
        private void ActivateWindow()
        {
            Process[] prs = Process.GetProcessesByName(this.ExeName);
            for (int i = 0; i < prs.Length; i++)
            {
                if (prs[i].MainWindowHandle != IntPtr.Zero)
                {
                    WindowsAPI.SwitchToThisWindow(prs[i].MainWindowHandle, true);
                    break;
                }
            }
        }

        /// <summary>
        /// 当前用户是管理员的时候，直接启动应用程序.
        /// 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
        /// </summary>
        private void SetSystemPression()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);

                //判断当前登录用户是否为管理员
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    //创建启动对象
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Assembly.GetExecutingAssembly().Location;
                    //设置启动动作,确保以管理员身份运行
                    startInfo.Verb = "runas";
                    try
                    {
                        System.Diagnostics.Process.Start(startInfo);
                    }
                    catch
                    {
                        return;
                    }

                    //退出
                    System.Windows.Application.Current.Shutdown();
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("设置管理员权限启动失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 重写Startup事件响应
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 添加全局异常捕获
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DispatcherHelper.Initialize();
            this.Register();
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void Register()
        {
            // 注册消息，弹出的对话框需要要主线程中弹出
            Messenger.Default.Register<ShowMessageBoxMessage>(this, message =>
            {
                if (message.IsAsyncShow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var result = ConfirmMessageBox.Show(message.Text, message.SubMessage, message.Button, message.Icon, message.IsAutoClose, message.AutoCloseTime);
                        if (message.CallBack != null)
                        {
                            message.CallBack(result);
                        }
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        var result = ConfirmMessageBox.Show(message.Text, message.SubMessage, message.Button, message.Icon, message.IsAutoClose, message.AutoCloseTime);
                        if (message.CallBack != null)
                        {
                            message.CallBack(result);
                        }
                    }));
                }
            });

            ///注册各个页面类型
            Messenger.Default.Register<ShowContentWindowMessage>(this, message =>
            {
                Type type = ShowContentWindowMessageFactory.CreateContent(message.ContentName); // 由ContentWindow工厂生产对应类型
                if (type != null)
                {
                    message.Content = type;
                    var action = new ShowContentWindowAction(message);
                    action.CallInvoke();
                }
            });
        }

        /// <summary>
        /// UI线程抛出全局异常处理
        /// </summary>
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.Error("UI线程全局异常", e.Exception);
                // 针对Api访问的请求处理
                if (e.Exception.InnerException is HttpClientException)
                {
                    ConfirmMessageBox.Show("", e.Exception.InnerException.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Logger.Error("不可恢复的UI线程全局异常", ex);
                ConfirmMessageBox.Show("应用程序发生不可恢复的异常，即将退出！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常处理
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    Logger.Error("非UI线程全局异常", exception);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("不可恢复的非UI线程全局异常", ex);
                ConfirmMessageBox.Show("应用程序发生不可恢复的异常，即将退出！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
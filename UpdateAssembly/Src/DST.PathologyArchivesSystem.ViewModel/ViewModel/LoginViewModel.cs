using DST.ApiClient.Service;
using DST.Common.Logger;
using DST.Common.Model;
using DST.Controls;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class LoginViewModel : CustomBaseViewModel
    {
        private string strLoginName;                                                        // 登录账户文本
        private string strLoginPwd;                                                         // 登录账户密码
        private string strMessage;                                                          // 提示信息
        private bool isLogining;                                                            // 是否处于登录状态
        private DelegateCommonObject.DelegateMethod DelegateInitAllDictList = null;       // 异步加载字典
        // private IAsyncResult delegateResult = null;

        /// <summary>
        /// 登录账户
        /// </summary>
        public string StrLoginName
        {
            get { return this.strLoginName; }
            set
            {
                this.strLoginName = value;
                this.RaisePropertyChanged("StrLoginName");
            }
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string StrLoginPwd
        {
            get { return this.strLoginPwd; }
            set
            {
                this.strLoginPwd = value;
                this.RaisePropertyChanged("StrLoginPwd");
            }
        }

        /// <summary>
        /// 登录成功或者失败的提示信息
        /// </summary>
        public string StrMessage
        {
            get { return this.strMessage; }
            set
            {
                this.strMessage = value;
                this.RaisePropertyChanged("StrMessage");
            }
        }

        /// <summary>
        /// 是否正在登录
        /// </summary>
        public bool IsLogining
        {
            get { return this.isLogining; }
            set
            {
                this.isLogining = value;
                this.RaisePropertyChanged("IsLogining");
            }
        }

        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.Login(data);
                });
            }
        }

        /// <summary>
        /// 构造方法：进行网络连接测试
        /// </summary>
        public LoginViewModel()
        {
            //this.InitAllDictList();
            //new TestViewModel();
        }

        /// <summary>
        /// 登录界面初始化时进行网络连接测试同时初始化字典列表
        /// </summary>
        public void InitAllDictList()
        {
            try
            {
                // 先使用异步初始化字典
                if (null == this.DelegateInitAllDictList)
                {
                    this.DelegateInitAllDictList = new DelegateCommonObject.DelegateMethod(this.InitApiDicts);
                }

                this.DelegateInitAllDictList.BeginInvoke(null, null);
            }
            catch (Exception ex)
            {
                Logger.Error("获取字典异常！\r\n" + ex.Message);
                this.ShowMessageBox("获取字典异常,退出当前登录！", MessageBoxButton.OK, MessageBoxImage.Error);
                Messenger.Default.Send<object>(this, "ExitLogin");
            }
        }

        /// <summary>
        /// 初始化Api获取的字典
        /// </summary>
        private void InitApiDicts()
        {
            try
            {
                this.InitDictAsync().Wait();
            }
            catch (Exception ex)
            {
                Logger.Error("初始化Api字典数据失败！", ex);
                return;
            }
        }

        public async Task InitDictAsync()
        {
            var t1 = DictService.Instance.GetCheckProjectStatusDict();
            var t2 = DictService.Instance.GetDownFlagDict();
            var t3 = DictService.Instance.GetHotpitalInfo();
            var t4 = DictService.Instance.GetProductDict();
            var t5 = DictService.Instance.GetSexDict();
            var t6 = DictService.Instance.GetSubmitDoctorDict();
            var t7 = DictService.Instance.GetExperimentStatusDict();
            await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7).ConfigureAwait(false);
            Database.ExtendApiDict.Instance.CheckProjectStatusDict = t1.Result;
            Database.ExtendApiDict.Instance.DownFlagDict = t2.Result;
            Database.ExtendApiDict.Instance.HotpitalInfo = t3.Result;
            Database.ExtendApiDict.Instance.ProductDict = t4.Result;
            Database.ExtendApiDict.Instance.SexDict = t5.Result;
            Database.ExtendApiDict.Instance.SubmitDoctorDict = t6.Result;
            Database.ExtendApiDict.Instance.ExperimentStatusDict = t7.Result;
        }

        /// <summary>
        /// 校验用户登录
        /// </summary>
        public bool VerifyLogin()
        {
            bool result = this.CheckLoginData();
            if (result)
            {
                try
                {
                    Database.ExtendAppContext.Current.LoginModel = LoginService.Instance.Login(new QueryLoginModel { username = this.StrLoginName, password = this.StrLoginPwd });
                }
                catch (Exception ex)
                {
                    result = false;
                    Logger.Error("查询登录用户信息错误", ex);
                    ConfirmMessageBox.Show("提示信息", ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            return result;
        }

        /// <summary>
        /// 检查登录信息
        /// </summary>
        private bool CheckLoginData()
        {
            bool result = true;
            if (string.IsNullOrEmpty(this.StrLoginName))
            {
                this.ShowMessageBox("用户名为空，请重新输入！");
                result = false;
            }
            else if (string.IsNullOrEmpty(this.StrLoginPwd))
            {
                this.ShowMessageBox("密码为空，请重新输入！");
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 密码回车键触发登录功能
        /// </summary>
        public void TextPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LoginCommand.Execute(sender);
            }

            this.SwitchMoveFocus(e);
        }

        /// <summary>
        /// 用户名文本框回车键触发切换焦点
        /// </summary>
        public void TextUserName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                (e.OriginalSource as UIElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            this.SwitchMoveFocus(e);
        }

        /// <summary>
        /// 登录按键回车键触发切换焦点
        /// </summary>
        public void LoginButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LoginCommand.Execute(sender);
            }

            this.SwitchMoveFocus(e);
        }

        /// <summary>
        /// 响应方向键切换焦点
        /// </summary>
        private void SwitchMoveFocus(KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                (e.OriginalSource as UIElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                // 获取焦点的控件
                UIElement nextUIElement = Keyboard.FocusedElement as UIElement;
                if (nextUIElement != null)
                {
                    Keyboard.Focus(nextUIElement);
                    if (nextUIElement is TextBox)
                    {
                        (nextUIElement as TextBox).SelectAll();
                    }
                    else if (nextUIElement is PasswordBox)
                    {
                        (nextUIElement as PasswordBox).SelectAll();
                    }
                }
            }
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                (e.OriginalSource as UIElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));

                UIElement nextUIElement = Keyboard.FocusedElement as UIElement;
                if (nextUIElement != null)
                {
                    Keyboard.Focus(nextUIElement);
                    if (nextUIElement is TextBox)
                    {
                        (nextUIElement as TextBox).SelectAll();
                    }
                    else if (nextUIElement is PasswordBox)
                    {
                        (nextUIElement as PasswordBox).SelectAll();
                    }
                }
            }
        }

        /// <summary>
        /// 登录操作
        /// </summary>
        private void Login(object data)
        {
            if (null != data && data is PasswordBox)
            {
                this.IsLogining = true;
                PasswordBox passBox = data as PasswordBox;
                this.StrLoginPwd = passBox.Password;

                System.Threading.Thread loginThread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    if (this.VerifyLogin())
                    {
                        this.InitApiDicts(); // 不可异步，登录完成后获取token才能查询
                        Messenger.Default.Send<object>(this, EnumMessageKey.CloseLogin);
                    }
                    else
                    {
                        this.IsLogining = false;
                    }
                }));

                loginThread.SetApartmentState(System.Threading.ApartmentState.STA);
                loginThread.Start();
            }
            else
            {
                this.IsLogining = false;
            }
        }
    }
}
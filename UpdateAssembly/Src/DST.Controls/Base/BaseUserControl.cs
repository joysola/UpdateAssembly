using DST.Common.Logger;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DST.Controls.Base
{
    public class BaseUserControl : UserControl, IDisposable
    {
        public ImageSource Icon { get; set; }

        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register(
            "Result", typeof(object), typeof(BaseUserControl),
            new PropertyMetadata((s1, e1) => (s1 as BaseUserControl).Result = e1.NewValue));

        /// <summary>
        /// 回调参数
        /// 当Control使用ContentWindow打开后，ContentWindow关闭时回调CallBackCommand时传入该参数
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 回调命令
        /// 使用ContentWindow打开时如果给ContentWindow赋值了CallBackCommand会自动进行赋值
        /// 窗口关闭时会自动触发，可以在窗口打开过程中手动触发
        /// </summary>
        public Action<object> CallBack { get; set; }

        /// <summary>
        /// 关闭，如果显示在ContentWindow内时关闭容器窗口
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// 容器窗口
        /// </summary>
        public Window ParentWindow { get; set; }

        /// <summary>
        /// 无参构造：声明加载事件和按键事件
        /// </summary>
        public BaseUserControl()
        {
            this.Loaded += this.BaseUserControl_Loaded;
            this.PreviewKeyDown += this.BaseUserControl_KeyDown;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BaseViewModel;
            if (vm != null)//ViewModel基类统一加载
            {
                if (this.Close != null)
                    vm.CloseContentWindowDelegate = this.Close;

                try
                {
                    //if (!(this is MedicalSystem.AnesWorkStation.View.ToolBar.BottomMenu) &&
                    //    !(this is MedicalSystem.AnesWorkStation.View.OperationInformation.OperationInterfaceControl))
                    WhirlingControlManager.ShowWaitingForm();
                    vm.RegisterKeyBoardMessage();
                    this.Unloaded += (s1, e1) => vm.OnViewUnLoaded();
                    vm.OnViewLoaded();
                    vm.LoadData();
                    if (ParentWindow != null)
                    {
                        if (ParentWindow is ContentWindow)
                        {
                            ParentWindow.Closing += (s1, e1) =>
                            {
                                if (!(ParentWindow as ContentWindow).IsAnimationCloseWindow)
                                    vm.OnPreviewViewUnLoaded(e1);

                                if ((ParentWindow as ContentWindow).ClosingAction != null)
                                    (ParentWindow as ContentWindow).ClosingAction(s1, e1);
                            };
                        }
                        else
                        {
                            ParentWindow.Closing += (s1, e1) => vm.OnPreviewViewUnLoaded(e1);
                        }

                        ParentWindow.Deactivated += Control_LostFocus;//响应点击其他地方关闭窗口操作
                    }
                }
                catch (Exception ex)
                {
                    WhirlingControlManager.CloseWaitingForm();
                    Logger.Error("程序异常", ex);
                    vm.ShowMessageBox(ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
            if (this.ParentWindow != null)
                this.ParentWindow.Activate();
        }

        /// <summary>
        /// 控件响应KeyDown，将实际处理流程定位到BaseViewModel里
        /// </summary>
        private void BaseUserControl_KeyDown(object sender, KeyEventArgs e)
        {
            var vm = this.DataContext as BaseViewModel;
            if (vm != null)
            {
                vm.KeyDown(sender, e);
            }
        }

        /// <summary>
        /// 失去焦点
        /// </summary>
        private void Control_LostFocus(object sender, EventArgs e)
        {
            var vm = this.DataContext as BaseViewModel;
            vm.Control_LostFocus(sender, e);
        }

        /// <summary>
        /// 实现释放资源接口
        /// </summary>
        public virtual void Dispose()
        {
            this.Loaded -= BaseUserControl_Loaded;
            this.PreviewKeyDown -= BaseUserControl_KeyDown;
            Messenger.Default.Unregister<dynamic>(this);
        }

        ~BaseUserControl()
        {
            Icon = null;
            Result = null;
            CallBack = null;
            Close = null;
            ParentWindow = null;
        }
    }
}
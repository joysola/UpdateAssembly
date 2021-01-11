using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace DST.Controls
{
    public class ConfirmMessageBox : Window
    {
        #region 依赖属性

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ConfirmMessageBox), new PropertyMetadata(""));

        public static readonly DependencyProperty SubMessageProperty =
            DependencyProperty.Register("SubMessage", typeof(string), typeof(ConfirmMessageBox), new PropertyMetadata(""));

        public static readonly DependencyProperty MessageBoxResultProperty =
           DependencyProperty.Register("MessageBoxResult", typeof(MessageBoxResult), typeof(ConfirmMessageBox), new PropertyMetadata(MessageBoxResult.None));

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(MessageBoxImage), typeof(ConfirmMessageBox), new PropertyMetadata(MessageBoxImage.Asterisk));

        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(ConfirmMessageBox), new PropertyMetadata(MessageBoxButton.OK));

        #endregion 依赖属性

        #region 属性

        private bool isAutoClose;

        public bool IsAutoClose
        {
            get { return isAutoClose; }
            set { isAutoClose = value; }
        }

        private int autoCloseTime;

        public int AutoCloseTime
        {
            get { return autoCloseTime; }
            set { autoCloseTime = value; }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public string SubMessage
        {
            get { return (string)GetValue(SubMessageProperty); }
            set { SetValue(SubMessageProperty, value); }
        }

        public MessageBoxResult MessageBoxResult
        {
            get { return (MessageBoxResult)GetValue(MessageBoxResultProperty); }
            set { SetValue(MessageBoxResultProperty, value); }
        }

        public MessageBoxImage Image
        {
            get { return (MessageBoxImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public MessageBoxButton MessageBoxButton
        {
            get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
            set { SetValue(MessageBoxButtonProperty, value); }
        }

        #endregion 属性

        #region 构造函数

        static ConfirmMessageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConfirmMessageBox), new FrameworkPropertyMetadata(typeof(ConfirmMessageBox)));
        }

        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private ConfirmMessageBox()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.AllowsTransparency = true;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ShowInTaskbar = false;
            this.Topmost = true;
            this.Loaded += ConfirmMessageBox_Loaded;
        }

        private void ConfirmMessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }

        #endregion 构造函数

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var temp = GetTemplateChild("PART_Btn_OK");

            var buttonOK = GetTemplateChild("PART_Btn_OK") as Border;
            var buttonCacel = GetTemplateChild("PART_Btn_Cancel") as Border;

            var buttonNO = GetTemplateChild("PART_Btn_NO") as Border;

            if (buttonOK != null)
            {
                buttonOK.MouseLeftButtonUp += (s1, e1) =>
                {
                    this.MessageBoxResult = System.Windows.MessageBoxResult.OK;
                    this.Close();
                };
            }

            if (buttonCacel != null)
            {
                buttonCacel.MouseLeftButtonUp += (s1, e1) =>
                {
                    this.MessageBoxResult = System.Windows.MessageBoxResult.Cancel;
                    this.Close();
                };
            }

            if (buttonNO != null)
            {
                buttonNO.MouseLeftButtonUp += (s1, e1) =>
                {
                    this.MessageBoxResult = System.Windows.MessageBoxResult.No;
                    this.Close();
                };
            }

            if (IsAutoClose)
            {
                DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
                //设置事件处理函数
                dTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                //定时器时间间隔1s
                if (dTimer.Interval != null)
                {
                    dTimer.Interval = new TimeSpan(0, 0, AutoCloseTime / 1000);
                    dTimer.Start();
                }
            }
        }

        /// <summary>
        /// 定时关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 加载弹出窗体

        /// <summary>
        /// 显示一个消息框，该消息框包含消息并返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText)
        {
            return Show(null, messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息和标题栏标题，并且返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string subMessage)
        {
            return Show(null, messageBoxText, subMessage, MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息并返回结果。
        /// </summary>
        /// <param name="owner">一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return Show(owner, messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题和按钮，并且返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string subMessage, MessageBoxButton button)
        {
            return Show(null, messageBoxText, subMessage, button, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息和标题栏标题，并且返回结果。
        /// </summary>
        /// <param name="owner">一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string subMessage)
        {
            return Show(owner, messageBoxText, subMessage, MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题、按钮和图标，并且返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <param name="icon">一个 System.Windows.MessageBoxImage 值，用于指定要显示的图标。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string subMessage, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(null, messageBoxText, subMessage, button, icon);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题、按钮和图标，是否是自动关闭,并且返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <param name="icon">一个 System.Windows.MessageBoxImage 值，用于指定要显示的图标。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string subMessage, MessageBoxButton button, MessageBoxImage icon, bool isAutoClose, int autoCloseTime)
        {
            return Show(null, messageBoxText, subMessage, button, icon, MessageBoxResult.None, isAutoClose, autoCloseTime);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息、标题栏标题和按钮，并且返回结果。
        /// </summary>
        /// <param name="owner">一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string subMessage, MessageBoxButton button)
        {
            return Show(owner, messageBoxText, subMessage, button, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题、按钮和图标，并接受默认消息框结果和返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <param name="icon">一个 System.Windows.MessageBoxImage 值，用于指定要显示的图标。</param>
        /// <param name="defaultResult">一个 System.Windows.MessageBoxResult 值，用于指定消息框的默认结果。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string subMessage, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            return Show(null, messageBoxText, subMessage, button, icon, defaultResult);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息、标题栏标题、按钮和图标，并且返回结果。
        /// </summary>
        /// <param name="owner">一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <param name="icon">一个 System.Windows.MessageBoxImage 值，用于指定要显示的图标。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string subMessage, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(owner, messageBoxText, subMessage, button, icon, MessageBoxResult.None);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息、标题栏标题、按钮和图标，并接受默认消息框结果和返回结果。
        /// </summary>
        /// <param name="owner">一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本</param>
        /// <param name="subMessage">一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <param name="icon">一个 System.Windows.MessageBoxImage 值，用于指定要显示的图标。</param>
        /// <param name="defaultResult">一个 System.Windows.MessageBoxResult 值，用于指定消息框的默认结果。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string subMessage = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Asterisk, MessageBoxResult defaultResult = MessageBoxResult.None,
            bool isAutoClose = false, int autoCloseTime = 3000)
        {
            WhirlingControlManager.CloseWaitingForm();

            var mbox = new ConfirmMessageBox();
            mbox.Message = messageBoxText;
            mbox.SubMessage = subMessage;
            mbox.Image = icon;
            mbox.MessageBoxButton = button;
            mbox.IsAutoClose = isAutoClose;
            mbox.AutoCloseTime = autoCloseTime;

            if (owner != null)
            {
                mbox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            mbox.PreviewKeyDown += mbox_PreviewKeyDown;
            mbox.ShowDialog();
            return mbox.MessageBoxResult;
        }

        private static void mbox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var curSender = (sender as ConfirmMessageBox);
            if (e.IsDown)
            {
                switch (e.Key)
                {
                    case Key.Y:
                    case Key.Return:
                        if (curSender.MessageBoxButton == MessageBoxButton.OKCancel)
                            curSender.MessageBoxResult = MessageBoxResult.OK;
                        else if (curSender.MessageBoxButton == MessageBoxButton.YesNo)
                            curSender.MessageBoxResult = MessageBoxResult.Yes;
                        else if (curSender.MessageBoxButton == MessageBoxButton.YesNoCancel)
                            curSender.MessageBoxResult = MessageBoxResult.Yes;
                        else if (curSender.MessageBoxButton == MessageBoxButton.OK)
                            curSender.MessageBoxResult = MessageBoxResult.OK;
                        curSender.Close();
                        break;

                    case Key.N:
                        curSender.MessageBoxResult = MessageBoxResult.No;
                        curSender.Close();
                        break;

                    case Key.C:
                        curSender.MessageBoxResult = MessageBoxResult.Cancel;
                        curSender.Close();
                        break;
                }
            }
            e.Handled = true;
        }

        #endregion 加载弹出窗体
    }
}
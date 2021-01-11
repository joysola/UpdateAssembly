using System;
using System.Windows;

namespace DST.Controls
{
    /// <summary>
    /// 弹出MessageBox消息
    /// </summary>
    public class ShowMessageBoxMessage
    {
        public string Text { get; set; }

        public string SubMessage { get; set; }

        public MessageBoxButton Button { get; set; }

        public MessageBoxImage Icon { get; set; }

        public bool IsAutoClose { get; set; }

        public bool IsAsyncShow { get; set; }

        public Action<MessageBoxResult> CallBack { get; set; }

        public int AutoCloseTime { get; set; }

        public ShowMessageBoxMessage(string text, string subMessage = "",
            MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
            Action<MessageBoxResult> callBack = null, int autoCloseTime = 3000, bool isAsyncShow = false)
        {
            Text = text;
            SubMessage = subMessage;
            Button = button;
            Icon = icon;
            CallBack = callBack;
            AutoCloseTime = autoCloseTime;
            IsAsyncShow = isAsyncShow;
        }

        public ShowMessageBoxMessage(string text, bool isAutoClose, string subMessage = "",
            MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
            Action<MessageBoxResult> callBack = null, int autoCloseTime = 3000, bool isAsyncShow = false)
        {
            Text = text;
            SubMessage = subMessage;
            Button = button;
            Icon = icon;
            CallBack = callBack;
            IsAutoClose = isAutoClose;
            AutoCloseTime = autoCloseTime;
            IsAsyncShow = isAsyncShow;
        }
    }
}
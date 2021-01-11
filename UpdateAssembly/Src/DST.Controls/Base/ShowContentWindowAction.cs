using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DST.Controls.Base
{
    /// <summary>
    /// 弹出ContentWindow
    /// Content和Title必须赋值，其他为可选项
    /// </summary>
    public class ShowContentWindowAction : TriggerAction<DependencyObject>, IShowContentWindowMessage
    {
        #region DependencyProperty

        public static readonly DependencyProperty ArgsProperty = DependencyProperty.Register(
            "Args", typeof(IEnumerable<object>), typeof(ShowContentWindowAction),
            new PropertyMetadata((s1, e1) => (s1 as ShowContentWindowAction).Args = e1.NewValue as IEnumerable<object>));

        public static readonly DependencyProperty CallBackCommandProperty = DependencyProperty.Register(
            "CallBackCommand", typeof(ICommand), typeof(ShowContentWindowAction),
            new PropertyMetadata((s1, e1) => (s1 as ShowContentWindowAction).CallBackCommand = e1.NewValue as ICommand));

        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
            "Owner", typeof(Window), typeof(ShowContentWindowAction),
            new PropertyMetadata((s1, e1) => (s1 as ShowContentWindowAction).Owner = e1.NewValue as Window));

        private ShowContentWindowMessage ContentMessage { get; set; }

        /// <summary>
        /// 内容控件构造方法的参数
        /// </summary>
        public IEnumerable<object> Args { get; set; }

        /// <summary>
        /// ContentWindow关闭时的回调命令
        /// </summary>
        public ICommand CallBackCommand { get; set; }

        /// <summary>
        /// 父窗口
        /// </summary>
        public Window Owner { get; set; }

        #endregion DependencyProperty

        /// <summary>
        /// 当前打开所有窗体
        /// </summary>
        private WindowCollection _OpenedWindows;

        /// <summary>
        /// 显示在ContentWindow中的内容控件的Type
        /// </summary>
        public Type Content { get; set; }

        /// <summary>
        /// 显示在ContentWindow中的内容控件的Name
        /// </summary>
        public string ContentName { get; set; }

        /// <summary>
        /// ContentWindow标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 按相对大小显示，与父窗口尺寸的差值
        /// </summary>
        public Thickness BorderMargin { get; set; }

        /// <summary>
        /// 内容控件在ContentWindow内的边距
        /// </summary>
        public Thickness ContentMargin { get; set; }

        /// <summary>
        /// 窗口大小调整模式
        /// </summary>
        public ResizeMode ResizeMode { get; set; }

        /// <summary>
        /// 窗口的起始位置
        /// </summary>
        public WindowStartupLocation StartupLocation { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// x
        /// </summary>
        public double PositionX { get; set; }

        /// <summary>
        /// y
        /// </summary>
        public double PositionY { get; set; }

        /// <summary>
        /// 最小高度
        /// </summary>
        public double MinHeight { get; set; }

        /// <summary>
        /// 最小宽度
        /// </summary>
        public double MinWidth { get; set; }

        /// <summary>
        /// 是否以模态弹出窗口
        /// </summary>
        public bool IsModal { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource Icon { get; set; }

        /// <summary>
        /// 窗体状态
        /// </summary>
        public WindowState WindowState { get; set; }

        /// <summary>
        /// 弹窗类型枚举：普通弹窗、工具栏弹窗、文书弹窗
        /// </summary>
        public ContentWindowType WindowType { get; set; }

        /// <summary>
        /// 标题栏的背景色
        /// </summary>
        public Brush TileBackground { get; set; }

        /// <summary>
        /// 窗口动画
        /// </summary>
        public ContentWindowAnimation WindowAnimation { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public ShowContentWindowAction()
        {
            Title = "Title";
            BorderMargin = new Thickness(-1);
            ResizeMode = ResizeMode.CanResize;
            Height = Double.NaN;
            Width = Double.NaN;
            MinHeight = 150;
            MinWidth = 300;
            WindowType = ContentWindowType.Common;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        public ShowContentWindowAction(ShowContentWindowMessage message)
            : this()
        {
            ContentMessage = message;
            Args = message.Args;
            CallBackCommand = message.CallBackCommand;
            Content = message.Content;
            Title = message.Title;
            Icon = message.Icon;
            ContentMargin = message.ContentMargin;
            Height = message.Height;
            Width = message.Width;
            ContentName = message.ContentName;
            IsModal = true;
            WindowType = message.WindowType;
            WindowAnimation = message.WindowAnimation;
            PositionX = message.PositionX;
            PositionY = message.PositionY;
            if (message.TileBackground == null)
            {
                BrushConverter brushConverter = new BrushConverter();
                Brush brush = (Brush)brushConverter.ConvertFromString("#006699");
                message.TileBackground = brush;
            }

            TileBackground = message.TileBackground;

            switch (WindowType)
            {
                case ContentWindowType.Document:
                    WindowState = WindowState.Maximized;
                    IsModal = message.IsModal;
                    break;

                case ContentWindowType.Common:
                    WindowState = WindowState.Maximized;
                    BorderMargin = message.BorderMargin;
                    ContentMargin = message.ContentMargin;
                    ResizeMode = message.ResizeMode;
                    MinHeight = message.MinHeight;
                    MinWidth = message.MinWidth;
                    IsModal = message.IsModal;
                    WindowState = message.WindowState;
                    break;

                case ContentWindowType.ToolBox:
                    IsModal = message.IsModal;
                    break;
            }
        }

        /// <summary>
        /// 生成和显示弹窗方法
        /// </summary>
        protected override void Invoke(object parameter)
        {
            if (Content == null)
            {
                throw new NotFiniteNumberException("Content is null");
            }

            if (Title == null)
            {
                throw new NotFiniteNumberException("Title is null");
            }

            object[] args = null;
            if (Args != null)
            {
                args = Args.ToArray();
            }

            // 调取有参构造失败后调取无参构造，待修改
            IContentWindow win = null;
            BaseUserControl content = null;
            ContentWindow contentWindow = null;     //当前打开的窗体

            try
            {
                content = Content.Assembly.CreateInstance(Content.FullName,
                                                          true,
                                                          BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                                          null,
                                                          args,
                                                          null,
                                                          null) as BaseUserControl;
            }
            catch (Exception)
            {
                content = Content.Assembly.CreateInstance(Content.FullName) as BaseUserControl;
            }

            // 根据弹窗类型反向生成对应的的窗口类
            switch (WindowType)
            {
                case ContentWindowType.ToolBox:
                    contentWindow = HasOpenedWindow(Title);
                    if (contentWindow != null)
                    {
                        // 若已打开，则使当前窗口弹出至最上层
                        contentWindow.Topmost = true;
                        return;
                    }
                    else
                    {
                        win = ToolBoxContentWindow.Create(content, Title)
                            .SetContentMargin(ContentMargin)
                            .SetHeight(Height)
                            .SetWidth(Width)
                            .SetTileBackground(TileBackground)
                            .SetIcon(Icon);
                    }
                    break;

                case ContentWindowType.Document:
                    contentWindow = HasOpenedWindow(Title);
                    if (contentWindow != null)
                    {
                        // 若已打开，则使当前窗口弹出至最上层
                        contentWindow.Topmost = true;
                        return;
                    }
                    else
                    {
                        win = ConfirmContentWindow.Create(content, Title)
                            .SetContentMargin(ContentMargin)
                            .SetHeight(Height)
                            .SetTileBackground(TileBackground)
                            .SetWidth(Width);
                    }
                    break;

                default:
                    contentWindow = HasOpenedWindow(Title);
                    if (contentWindow != null)
                    {
                        // 若已打开，则使当前窗口弹出至最上层
                        contentWindow.Topmost = true;
                        return;
                    }
                    else
                    {
                        if (Title == "麻醉评分")
                        {
                            win = ContentWindow.Create(content, Title, false)
                          .SetContentMargin(ContentMargin)
                          .SetResizeMode(ResizeMode)
                          .SetMinHeight(MinHeight)
                          .SetMinWidth(MinWidth)
                          .SetHeight(Height)
                          .SetWidth(Width)
                          .SetBorderMargin(BorderMargin)
                          .SetTileBackground(TileBackground)
                          .SetIcon(content.Icon);
                        }
                        else
                        {
                            win = ContentWindow.Create(content, Title)
                           .SetContentMargin(ContentMargin)
                           .SetResizeMode(ResizeMode)
                           .SetMinHeight(MinHeight)
                           .SetMinWidth(MinWidth)
                           .SetHeight(Height)
                           .SetWidth(Width)
                           .SetBorderMargin(BorderMargin)
                           .SetTileBackground(TileBackground)
                           .SetIcon(content.Icon);
                        }
                    }
                    break;
            }

            //设置内容控制窗体大小
            if (!(Height > 0 & Width > 0))
            {
                (win as Window).SizeToContent = SizeToContent.WidthAndHeight;
            }

            //边界控制
            if (PositionX > 0 || PositionY > 0)
            {
                win.SetStartupLocation(WindowStartupLocation.Manual);
                //防止窗体显示不全
                //获取屏幕的边界
                double maxWidth = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
                double maxHeight = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度
                if (PositionX + Width > maxWidth)
                {
                    PositionX = maxWidth - Width - 20;
                }
                if (PositionY + Height > maxHeight & PositionY - Height > 0)
                {
                    PositionY = PositionY - Height;
                }
                else
                {
                    if (ContentName.Equals("OperationInterfaceControl"))
                    {
                    }
                    else
                    {
                        PositionY = maxHeight - Height - 5;
                    }
                }

                win.SetX(PositionX);
                win.SetY(PositionY);
            }
            else
            {
                win.SetStartupLocation(WindowStartupLocation.CenterScreen);
            }

            content.CallBack = obj => { if (CallBackCommand != null) CallBackCommand.Execute(obj); };
            content.Close = () => (win as Window).Close();
            content.ParentWindow = (win as Window);

            var vm = content.DataContext as BaseViewModel;
            if (vm != null)
            {
                vm.Args = args;
                vm.CloseContentWindowDelegate = () => (win as Window).Close();
            }

            (win as Window).Loaded += ShowContentWindowAction_Loaded;
            (win as Window).Closed += (s1, e1) =>
            {
                content.Dispose();
                object result = null;
                if (vm != null)
                {
                    if (vm.Result != null)
                    {
                        result = vm.Result;
                    }
                }
                else
                {
                    result = content.Result;
                }

                // 出发回调函数
                if (CallBackCommand != null)
                {
                    CallBackCommand.Execute(result);
                }

                // 设置返回值
                if (ContentMessage != null)
                {
                    ContentMessage.Result = result;
                }
                //content.DataContext = null;
                //content = null;
                //(win as Window).Content = null;
                //win = null;
            };

            if (win is ContentWindow)
            {
                ContentWindow contentWin = win as ContentWindow;
                contentWin.ClosingAction = (s1, e1) =>
                {
                    if (e1.Cancel)
                        return;
                    if (!contentWin.IsAnimationCloseWindow)
                    {
                        double durationTime = 0.2;
                        switch (WindowAnimation)
                        {
                            case ContentWindowAnimation.FadeIn:
                                e1.Cancel = true;
                                DoubleAnimation daShow = new DoubleAnimation();
                                daShow.From = 1;
                                daShow.To = 0.4;
                                daShow.Duration = TimeSpan.FromSeconds(durationTime);
                                contentWin.IsAnimationCloseWindow = true;
                                daShow.Completed += (sender, e) =>
                                {
                                    (s1 as Window).Close();
                                };
                                (s1 as Window).BeginAnimation(Window.OpacityProperty, daShow);
                                break;

                            case ContentWindowAnimation.VerticalFloating:
                                TranslateTransform tt = new TranslateTransform();
                                DoubleAnimation da = new DoubleAnimation();
                                Duration duration = new Duration(TimeSpan.FromSeconds(durationTime));
                                (s1 as Window).RenderTransform = tt;
                                tt.Y = 0;
                                da.To = (s1 as Window).ActualHeight;
                                da.Duration = duration;
                                contentWin.IsAnimationCloseWindow = true;
                                da.Completed += (sender, e) => { (s1 as Window).Close(); };
                                tt.BeginAnimation(TranslateTransform.YProperty, da);
                                e1.Cancel = true;
                                break;

                            case ContentWindowAnimation.HorizontalFloating:
                                TranslateTransform ttHorizontal = new TranslateTransform();
                                DoubleAnimation daHorizontal = new DoubleAnimation();
                                Duration durationHorizontal = new Duration(TimeSpan.FromSeconds(durationTime));
                                (s1 as Window).RenderTransform = ttHorizontal;
                                daHorizontal.From = 0;
                                daHorizontal.To = -(s1 as Window).ActualWidth;
                                daHorizontal.Duration = durationHorizontal;
                                contentWin.IsAnimationCloseWindow = true;
                                daHorizontal.Completed += (sender, e) => { (s1 as Window).Close(); };
                                ttHorizontal.BeginAnimation(TranslateTransform.XProperty, daHorizontal);
                                e1.Cancel = true;
                                break;
                        }
                    }
                    else
                    {
                        e1.Cancel = false;
                    }
                };
            }

            if (this.Owner != null)
            {
                win.SetOwner(Owner);
            }

            if (IsModal)
            {
                win.ShowDialog();
            }
            else
            {
                win.Show();
            }
        }

        /// <summary>
        /// 加载窗体动画效果
        /// </summary>
        private void ShowContentWindowAction_Loaded(object sender, RoutedEventArgs e)
        {
            double durationTime = 0.2;
            switch (WindowAnimation)
            {
                case ContentWindowAnimation.FadeIn:
                    DoubleAnimation daShow = new DoubleAnimation();
                    daShow.From = 0;
                    daShow.To = 1;
                    daShow.Duration = TimeSpan.FromSeconds(durationTime);
                    (sender as Window).BeginAnimation(Window.OpacityProperty, daShow);
                    break;

                case ContentWindowAnimation.VerticalFloating:
                    TranslateTransform tt = new TranslateTransform();
                    DoubleAnimation da = new DoubleAnimation();
                    Duration duration = new Duration(TimeSpan.FromSeconds(durationTime));
                    (sender as Window).RenderTransform = tt;
                    tt.Y = (sender as Window).ActualHeight;
                    da.To = 0;
                    da.Duration = duration;
                    tt.BeginAnimation(TranslateTransform.YProperty, da);
                    break;

                case ContentWindowAnimation.HorizontalFloating:
                    TranslateTransform ttHorizontal = new TranslateTransform();
                    DoubleAnimation daHorizontal = new DoubleAnimation();
                    Duration durationHorizontal = new Duration(TimeSpan.FromSeconds(durationTime));
                    (sender as Window).RenderTransform = ttHorizontal;
                    daHorizontal.From = -(sender as Window).ActualWidth;
                    daHorizontal.To = 0;
                    daHorizontal.Duration = durationHorizontal;
                    ttHorizontal.BeginAnimation(TranslateTransform.XProperty, daHorizontal);
                    break;
            }
        }

        /// <summary>
        /// 公开的生成弹窗的接口方法
        /// </summary>
        public void CallInvoke()
        {
            Invoke(null);
        }

        /// <summary>
        /// 判断当前是否已经打开此窗体，如果已经打开则返回该窗体，否则返回空
        /// </summary>
        private ContentWindow HasOpenedWindow(string windowName)
        {
            ContentWindow hasOpenedWindow = null;

            _OpenedWindows = System.Windows.Application.Current.Windows;

            foreach (Window window in _OpenedWindows)
            {
                if (window is ContentWindow && window.Title == windowName)
                {
                    hasOpenedWindow = window as ContentWindow;
                }
            }

            return hasOpenedWindow;
        }
    }
}
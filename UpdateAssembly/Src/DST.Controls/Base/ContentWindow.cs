//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//文件名称(File Name)：      ContentWindow.cs
//功能描述(Description)：    创建窗口基类，用于设置图标，标题，
//                          大小，弹出位置等
//数据表(Tables)：		    无
//作者(Author)：             DST-
//日期(Create Date)：        2017/12/26 14:58
//R1:
//    修改作者:
//    修改日期:
//    修改理由:
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DST.Controls.Base
{
    /// <summary>
    /// 弹出窗口
    /// 可以将界面元素作为其内容进行弹出
    /// </summary>
    public class ContentWindow : Window, IContentWindow
    {
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(ContentWindow));
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(ContentWindow));

        public static readonly DependencyProperty TileBackgroundProperty =
           DependencyProperty.Register("TileBackground", typeof(Brush), typeof(ContentWindow));

        /// <summary>
        /// 内部宽度
        /// </summary>
        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        /// <summary>
        /// 内部高度
        /// </summary>
        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        /// <summary>
        /// 标题栏的背景色
        /// </summary>
        public Brush TileBackground
        {
            get { return (Brush)GetValue(TileBackgroundProperty); }
            set { SetValue(TileBackgroundProperty, value); }
        }

        /// <summary>
        /// 是否是动画执行关闭窗口
        /// </summary>
        public bool IsAnimationCloseWindow { get; set; }

        /// <summary>
        /// 关闭弹窗时的事件
        /// </summary>
        public Action<object, System.ComponentModel.CancelEventArgs> ClosingAction;

        /// <summary>
        /// 控件内容实体
        /// </summary>
        private ContentPresenter _contentPresenter;

        /// <summary>
        /// 边框的粗细
        /// </summary>
        private Thickness _contentMargin;

        /// <summary>
        /// 无参构造
        /// </summary>
        static ContentWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentWindow), new FrameworkPropertyMetadata(typeof(ContentWindow)));
        }

        /// <summary>
        /// 创建窗口
        /// </summary>
        public static IContentWindow Create(UIElement content, string title)
        {
            var win = new ContentWindow();
            win.Content = content;
            win.Title = title;
            return win;
        }

        /// <summary>
        /// 创建窗口
        /// </summary>
        public static IContentWindow Create(UIElement content, string title, bool isAllowsTransparency)
        {
            var win = new ContentWindow();
            win.Content = content;
            win.Title = title;
            win.AllowsTransparency = isAllowsTransparency;
            return win;
        }

        /// <summary>
        /// 内部无参构造，用于被页面中的对象生成
        /// </summary>
        internal ContentWindow()
        {
            WindowStyle = System.Windows.WindowStyle.None;
            MinHeight = 150;
            MinWidth = 300;
            this.Loaded += ContentWindow_Loaded;
        }

        /// <summary>
        /// 加载事件，注册关闭事件
        /// </summary>
        private void ContentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Messenger.Default.Register<CloseContentWindowMessage>(this, message => this.Close());
        }

        /// <summary>
        /// 重写关闭事件的方法体
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // Messenger.Default.Unregister<CloseContentWindowMessage>(this);
        }

        /// <summary>
        /// 应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._contentPresenter = GetTemplateChild("PART_ContentPresenter") as ContentPresenter;
            this._contentPresenter.Margin = this._contentMargin;
        }

        /// <summary>
        /// 设置弹窗的父窗体
        /// </summary>
        public IContentWindow SetOwner(Window owner)
        {
            this.Owner = owner;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            return this;
        }

        /// <summary>
        /// 设置弹窗的父窗体
        /// </summary>
        public IContentWindow SetOwner(Window owner, Thickness borderMargin)
        {
            SetOwner(owner);
            return this;
        }

        /// <summary>
        /// 设置弹窗的边框样式
        /// </summary>
        public IContentWindow SetContentMargin(Thickness margin)
        {
            this._contentMargin = margin;
            return this;
        }

        /// <summary>
        /// 设置弹窗的内容显示的区域大小
        /// </summary>
        public IContentWindow SetBorderMargin(Thickness margin)
        {
            if (margin.Left != -1)
            {
                this.ContentWidth = SystemParameters.PrimaryScreenWidth - margin.Left * 2;
            }

            if (margin.Top != -1)
            {
                this.ContentHeight = SystemParameters.PrimaryScreenHeight - margin.Top * 2;
            }

            return this;
        }

        /// <summary>
        /// 设置弹窗的大小更改枚举
        /// </summary>
        public IContentWindow SetResizeMode(ResizeMode resizeMode)
        {
            this.ResizeMode = resizeMode;
            return this;
        }

        /// <summary>
        /// 设置弹窗高度
        /// </summary>
        public IContentWindow SetHeight(double height)
        {
            this.Height = height;
            this.ContentHeight = height;
            return this;
        }

        /// <summary>
        /// 设置弹窗的宽度
        /// </summary>
        public IContentWindow SetWidth(double width)
        {
            this.Width = width;
            this.ContentWidth = width;
            return this;
        }

        /// <summary>
        /// 设置弹窗标题栏的背景色
        /// </summary>
        public IContentWindow SetTileBackground(Brush tileBackground)
        {
            this.TileBackground = tileBackground;
            return this;
        }

        /// <summary>
        /// 设置最小高度
        /// </summary>
        public IContentWindow SetMinHeight(double minHeight)
        {
            this.MinHeight = MinHeight;
            return this;
        }

        /// <summary>
        /// 设置最小宽度
        /// </summary>
        public IContentWindow SetMinWidth(double minWidth)
        {
            this.MinWidth = minWidth;
            return this;
        }

        /// <summary>
        /// 设置弹窗的初始样式：最大、最小
        /// </summary>
        public IContentWindow SetWindowState(WindowState windowState)
        {
            this.WindowState = windowState;
            return this;
        }

        /// <summary>
        /// 设置弹窗的Icon
        /// </summary>
        public IContentWindow SetIcon(ImageSource img)
        {
            this.Icon = img;
            return this;
        }

        /// <summary>
        /// 设置弹窗的初始显示位置
        /// </summary>
        public IContentWindow SetStartupLocation(WindowStartupLocation location)
        {
            this.WindowStartupLocation = location;
            return this;
        }

        /// <summary>
        /// 设置弹出的位置：左
        /// </summary>
        public IContentWindow SetX(double x)
        {
            this.Left = x;
            return this;
        }

        /// <summary>
        /// 设置弹出的位置：顶
        /// </summary>
        public IContentWindow SetY(double y)
        {
            this.Top = y;
            return this;
        }
    }
}
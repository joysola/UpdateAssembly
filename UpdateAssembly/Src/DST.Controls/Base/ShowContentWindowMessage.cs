using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DST.Controls.Base
{
    /// <summary>
    /// 弹窗类型枚举
    /// </summary>
    public enum ContentWindowType
    {
        /// <summary>
        /// 普通弹窗
        /// </summary>
        [Description("普通弹窗")]
        Common = 1,

        /// <summary>
        /// 工具栏弹窗
        /// </summary>
        [Description("工具栏弹窗")]
        ToolBox = 2,

        /// <summary>
        /// 文书弹窗
        /// </summary>
        [Description("文书弹窗")]
        Document = 3
    }

    /// <summary>
    /// 内容窗口动画
    /// </summary>
    public enum ContentWindowAnimation
    {
        /// <summary>
        /// 无动画
        /// </summary>
        None = 0,

        /// <summary>
        /// 垂直浮出动画（下到上）
        /// </summary>
        VerticalFloating,

        /// <summary>
        /// 水平浮出动画（左到右）
        /// </summary>
        HorizontalFloating,

        /// <summary>
        /// 淡入
        /// </summary>
        FadeIn
    }

    /// <summary>
    /// 弹出ContentWindow消息
    /// </summary>
    public class ShowContentWindowMessage : IShowContentWindowMessage
    {
        public Brush TileBackground { get; set; }

        /// <summary>
        /// ContentWindow类型
        /// </summary>
        public ContentWindowType WindowType { get; set; }

        /// <summary>
        /// 窗口动画
        /// </summary>
        public ContentWindowAnimation WindowAnimation { get; set; }

        /// <summary>
        /// 内容控件构造方法的参数
        /// </summary>
        public IEnumerable<object> Args { get; set; }

        /// <summary>
        /// ContentWindow关闭时的回调命令
        /// </summary>
        public ICommand CallBackCommand { get; set; }

        /// <summary>
        /// 显示在ContentWindow中的内容控件的类型名
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
        /// 高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }

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

        public object Result { get; set; }

        /// <summary>
        /// x
        /// </summary>
        public double PositionX { get; set; }

        /// <summary>
        /// y
        /// </summary>
        public double PositionY { get; set; }

        public ShowContentWindowMessage(string contentType, string title, ContentWindowAnimation WindowAnimation = ContentWindowAnimation.FadeIn)
        {
            ContentName = contentType;
            Title = title;
            IsModal = true;
            BorderMargin = new Thickness(-1);
            ResizeMode = ResizeMode.CanResize;
            Height = Double.NaN;
            Width = Double.NaN;
            MinHeight = 150;
            MinWidth = 300;

            WindowType = ContentWindowType.Common;
            this.WindowAnimation = WindowAnimation;
        }

        public ShowContentWindowMessage(string contentType, string title, ContentWindowType windowType)
            : this(contentType, title)
        {
            WindowType = windowType;
        }
    }
}
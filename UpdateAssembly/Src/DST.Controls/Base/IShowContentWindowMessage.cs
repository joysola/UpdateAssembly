using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DST.Controls.Base
{
    public interface IShowContentWindowMessage
    {
        /// <summary>
        /// 标题的背景色
        /// </summary>
        Brush TileBackground { get; set; }

        /// <summary>
        /// ContentWindow类型
        /// </summary>
        ContentWindowType WindowType { get; set; }

        /// <summary>
        /// 内容控件构造方法的参数
        /// </summary>
        IEnumerable<object> Args { get; set; }

        /// <summary>
        /// ContentWindow关闭时的回调命令
        /// </summary>
        ICommand CallBackCommand { get; set; }

        /// <summary>
        /// 显示在ContentWindow中的内容控件的Type
        /// </summary>
        Type Content { get; set; }

        /// <summary>
        /// 显示在ContentWindow中的内容控件的Name
        /// </summary>
        string ContentName { get; set; }

        /// <summary>
        /// ContentWindow标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 内容控件在ContentWindow内的边距
        /// </summary>
        Thickness ContentMargin { get; set; }

        /// <summary>
        /// 按相对大小显示，与父窗口尺寸的差值
        /// </summary>
        Thickness BorderMargin { get; set; }

        /// <summary>
        /// 窗口大小调整模式
        /// </summary>
        ResizeMode ResizeMode { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// x
        /// </summary>
        double PositionX { get; set; }

        /// <summary>
        /// y
        /// </summary>
        double PositionY { get; set; }

        /// <summary>
        /// 最小高度
        /// </summary>
        double MinHeight { get; set; }

        /// <summary>
        /// 最小宽度
        /// </summary>
        double MinWidth { get; set; }

        /// <summary>
        /// 是否以模态弹出窗口
        /// </summary>
        bool IsModal { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        ImageSource Icon { get; set; }

        /// <summary>
        /// 窗体状态
        /// </summary>
        WindowState WindowState { get; set; }
    }
}
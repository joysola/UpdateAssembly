//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//文件名称(File Name)：      ToolBoxContentWindow.cs
//功能描述(Description)：    工具菜单窗口弹出
//数据表(Tables)：		    无
//作者(Author)：             DST-
//日期(Create Date)：        2017/12/26 14:58
//R1:
//    修改作者:
//    修改日期:
//    修改理由:
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
using System.Windows;

namespace DST.Controls.Base
{
    public class ToolBoxContentWindow : ContentWindow
    {
        static ToolBoxContentWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBoxContentWindow), new FrameworkPropertyMetadata(typeof(ToolBoxContentWindow)));
        }

        public static new IContentWindow Create(UIElement content, string title)
        {
            var win = new ToolBoxContentWindow();
            win.Content = content;
            win.Title = title;
            return win;
        }

        internal ToolBoxContentWindow()
        {
        }
    }
}
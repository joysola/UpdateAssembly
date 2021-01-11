//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//文件名称(File Name)：      ConfirmContentWindow.cs
//功能描述(Description)：    文书窗口弹出
//数据表(Tables)：		    无
//作者(Author)：             DST-
//日期(Create Date)：        2017/12/26 14:58
//R1:
//    修改作者:
//    修改日期:
//    修改理由:
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
using System.Windows;
using System.Windows.Controls;

namespace DST.Controls.Base
{
    public class ConfirmContentWindow : ContentWindow
    {
        static ConfirmContentWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConfirmContentWindow), new FrameworkPropertyMetadata(typeof(ConfirmContentWindow)));
        }

        public static new IContentWindow Create(UIElement content, string title)
        {
            var win = new ConfirmContentWindow();
            win.Content = content;
            win.Title = title;
            return win;
        }

        internal ConfirmContentWindow()
        {
            this.ContentHeight = SystemParameters.PrimaryScreenHeight - 115;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var buttonCacel = GetTemplateChild("PART_Btn_Cancel") as Button;
            if (buttonCacel != null)
            {
                buttonCacel.Click += (s1, e1) => this.Close();
            }
        }
    }
}
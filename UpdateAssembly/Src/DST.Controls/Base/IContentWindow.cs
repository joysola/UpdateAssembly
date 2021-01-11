//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//文件名称(File Name)：      IContentWindow.cs
//功能描述(Description)：    创建窗口基类接口，用于设置图标，标题，
//                          大小，弹出位置等
//数据表(Tables)：		    无
//作者(Author)：             DST-
//日期(Create Date)：        2017/12/26 14:58
//R1:
//    修改作者:
//    修改日期:
//    修改理由:
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
using System.Windows;
using System.Windows.Media;

namespace DST.Controls.Base
{
    public interface IContentWindow
    {
        IContentWindow SetStartupLocation(WindowStartupLocation location);

        IContentWindow SetOwner(Window owner);

        IContentWindow SetOwner(Window owner, Thickness borderMargin);

        IContentWindow SetContentMargin(Thickness margin);

        IContentWindow SetIcon(ImageSource img);

        IContentWindow SetX(double x);

        IContentWindow SetY(double y);

        IContentWindow SetBorderMargin(Thickness margin);

        IContentWindow SetResizeMode(ResizeMode resizeMode);

        IContentWindow SetHeight(double height);

        IContentWindow SetWidth(double width);

        IContentWindow SetMinHeight(double minHeight);

        IContentWindow SetMinWidth(double minWidth);

        IContentWindow SetWindowState(WindowState windowState);

        IContentWindow SetTileBackground(Brush tileBackground);

        void Show();

        bool? ShowDialog();
    }
}
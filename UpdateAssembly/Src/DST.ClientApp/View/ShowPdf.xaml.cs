using DST.Common.Model;
using DST.Controls.Base;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DST.ClientApp.View
{
    /// <summary>
    /// ShowPdf.xaml 的交互逻辑
    /// </summary>
    public partial class ShowPdf : BaseUserControl
    {
        private List<string> pdfPathList = new List<string>();

        /// <summary>
        /// 无参构造
        /// </summary>
        public ShowPdf()
        {
            InitializeComponent();
            this.Loaded += ShowPdf_Loaded;
        }

        /// <summary>
        /// 设置打印按钮的位置
        /// </summary>
        private void ShowPdf_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(this.btnPrint, this.mainCanvas.ActualWidth - this.btnPrint.Width - 50);
            Canvas.SetTop(this.btnPrint, this.mainCanvas.ActualHeight - this.btnPrint.Height - 10);
        }

        /// <summary>
        /// 显示PDF文件
        /// </summary>
        /// <param name="pdfPath">pdf完整路径</param>
        public void OpenPdf(List<string> pdfPath)
        {
            this.pdfPathList.Clear();
            this.pdfPathList = null;
            this.pdfPathList = new List<string>(pdfPath);
            this.ResetTabControl();
        }

        /// <summary>
        /// 重置TabItem信息
        /// </summary>
        private void ResetTabControl()
        {
            this.tabControl.Items.Clear();
            for (int i = 0; i < this.pdfPathList.Count; i++)
            {
                TabItem item = new TabItem() { Header = System.IO.Path.GetFileName(this.pdfPathList[i]) };
                item.Height = 35;
                this.tabControl.Items.Add(item);
            }

            this.tabControl.SelectedIndex = 0;
            this.btnPrint.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打印报告
        /// </summary>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>(this.pdfPathList[this.tabControl.SelectedIndex], EnumMessageKey.PrintReport);
        }

        /// <summary>
        /// 切换选项卡
        /// </summary>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tabControl.SelectedIndex > -1)
            {
                this.moonPdfPanel.OpenFile(this.pdfPathList[this.tabControl.SelectedIndex]);
                this.moonPdfPanel.ZoomStep = 1.0;
                this.moonPdfPanel.ZoomIn();
            }
        }
    }
}
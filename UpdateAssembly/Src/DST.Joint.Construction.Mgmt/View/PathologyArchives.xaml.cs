using DST.Controls;
using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// PathologyArchivesView.xaml 的交互逻辑
    /// </summary>
    public partial class PathologyArchives : BaseUserControl
    {
        private PathologyArchivesViewModel pathologyArchivesViewModel = null;

        public PathologyArchives()
        {
            InitializeComponent();
            this.pathologyArchivesViewModel = new PathologyArchivesViewModel();
            this.DataContext = this.pathologyArchivesViewModel;
            this.IsVisibleChanged += PathologyArchives_IsVisibleChanged;
            RegisterMessage();
        }

        private void RegisterMessage()
        {
        }

        private void PathologyArchives_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                this.pathologyArchivesViewModel.LoadCustomDict();
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog floder = new System.Windows.Forms.FolderBrowserDialog();
            var result = floder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string path = floder.SelectedPath;
                string lastFloderName = System.IO.Path.GetFileNameWithoutExtension(path);
                DateTime selectDt;
                IFormatProvider ifp = new CultureInfo("zh-CN", true);
                if (DateTime.TryParseExact(lastFloderName, "yyyyMMdd", ifp, System.Globalization.DateTimeStyles.None, out selectDt))
                {
                    this.pathologyArchivesViewModel.MatchSample.Execute(floder.SelectedPath);
                }
                else
                {
                    ConfirmMessageBox.Show("", "选中的文件夹非日期目录，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// 显示表格序号
        /// </summary>
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
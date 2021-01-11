using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;
using System.Windows;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// IAM_ImportData.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_ImportData : BaseUserControl
    {
        private IAM_ImportDataViewModel importDataViewModel = null;

        public IAM_ImportData()
        {
            InitializeComponent();
            importDataViewModel = new IAM_ImportDataViewModel();
            this.DataContext = importDataViewModel;
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            this.OKBtn.IsEnabled = false; // 打开excel文件时，禁用确认按钮
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xls,*.xlsx,*.csv)|*.xls;*.xlsx;*.csv"; // 组合
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var examResult = this.importDataViewModel.ImportExcel(openFileDialog.FileName);
                if (examResult) // 检验成功启用确认按钮
                {
                    this.OKBtn.IsEnabled = true;
                }
            }
        }
    }
}
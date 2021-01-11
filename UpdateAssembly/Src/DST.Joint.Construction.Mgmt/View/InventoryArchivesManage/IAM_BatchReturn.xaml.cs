using DST.Controls.Base;
using DST.Database.Model.ApiModels_old;
using DST.Joint.Construction.Mgmt.ViewModel.ViewModel;
using System.Windows.Controls;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// IAM_BatchReturn.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_BatchReturn : BaseUserControl
    {
        private IAM_BatchReturnViewModel batchReturnViewModel = null;

        public IAM_BatchReturn()
        {
            InitializeComponent();
            this.batchReturnViewModel = new IAM_BatchReturnViewModel();
            this.DataContext = this.batchReturnViewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.dg.CurrentItem as SlideLendOutInfo).Back_Status = (sender as RadioButton).Tag.ToString();
        }
    }
}
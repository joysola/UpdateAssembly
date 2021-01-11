using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;
using System.Windows.Controls;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// IAM_SliceAddEditCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_SliceAddEditCtrl : BaseUserControl
    {
        public IAM_SliceAddEditCtrl()
        {
            InitializeComponent();
            this.DataContext = new IAM_SliceAddEditCtrlViewModel();
        }

        private void GiveGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
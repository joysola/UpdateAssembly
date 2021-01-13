using DST.Controls.Base;

using System.Windows;
using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.ClientApp.View
{
    /// <summary>
    /// IAM_SliceReturn.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_SliceReturn : BaseUserControl
    {
        public IAM_SliceReturn()
        {
            InitializeComponent();
            this.DataContext = new IAM_SliceReturnViewModel();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
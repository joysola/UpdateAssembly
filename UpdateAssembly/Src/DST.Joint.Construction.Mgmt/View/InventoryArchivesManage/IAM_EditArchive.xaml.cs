using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;
using System.Windows;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// IAM_EditArchive.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_EditArchive : BaseUserControl
    {
        public IAM_EditArchive()
        {
            InitializeComponent();
            this.DataContext = new IAM_EditArchiveViewModel();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
using DST.Controls.Base;

using System.Text.RegularExpressions;
using System.Windows;
using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.ClientApp.View
{
    /// <summary>
    /// IAM_SliceLoan.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_SliceLoan : BaseUserControl
    {
        public IAM_SliceLoan()
        {
            InitializeComponent();
            this.DataContext = new IAM_SliceLoanViewModel();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");

            e.Handled = re.IsMatch(e.Text);
        }
    }
}
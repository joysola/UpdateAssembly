using DST.Controls.Base;

using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.ClientApp.View
{
    /// <summary>
    /// FailedScanArchives.xaml 的交互逻辑
    /// </summary>
    public partial class FailedScanArchives : BaseUserControl
    {
        private FailedScanArchivesViewModel failedScanArchivesViewModel = null;

        public FailedScanArchives()
        {
            InitializeComponent();
            this.failedScanArchivesViewModel = new FailedScanArchivesViewModel();
            this.DataContext = this.failedScanArchivesViewModel;
            this.IsVisibleChanged += FailedScanArchives_IsVisibleChanged;
        }

        private void FailedScanArchives_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                this.failedScanArchivesViewModel.LoadCustomDict();
            }
        }
    }
}
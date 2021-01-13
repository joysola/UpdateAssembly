using DST.Controls.Base;

using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.ClientApp.View
{
    /// <summary>
    /// InventoryArchivesManage.xaml 的交互逻辑
    /// </summary>
    public partial class InventoryArchivesManage : BaseUserControl
    {
        private InventoryArchivesManageViewModel inventoryArchivesManageViewModel = null;

        public InventoryArchivesManage()
        {
            InitializeComponent();
            this.inventoryArchivesManageViewModel = new InventoryArchivesManageViewModel();
            this.DataContext = this.inventoryArchivesManageViewModel;
            this.IsVisibleChanged += InventoryArchivesManage_IsVisibleChanged;
        }

        private void InventoryArchivesManage_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                this.inventoryArchivesManageViewModel.LoadCustomDict();
            }
        }

        private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
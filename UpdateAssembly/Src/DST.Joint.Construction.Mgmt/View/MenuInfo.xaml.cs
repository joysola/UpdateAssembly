using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// MenuInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MenuInfo : BaseUserControl
    {
        public MenuInfoViewModel menuInfoViewModel = null;

        public MenuInfo()
        {
            InitializeComponent();
            this.menuInfoViewModel = new MenuInfoViewModel();
            this.DataContext = this.menuInfoViewModel;
        }
    }
}
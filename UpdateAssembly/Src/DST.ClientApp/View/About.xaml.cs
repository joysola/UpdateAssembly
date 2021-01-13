using DST.Controls.Base;

using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.ClientApp.View
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : BaseUserControl
    {
        public About()
        {
            InitializeComponent();
            this.DataContext = new AboutViewModel();
        }
    }
}
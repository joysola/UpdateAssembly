using DST.Controls.Base;
using DST.PathologyArchivesSystem.ViewModel.ViewModel;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// MainTop.xaml 的交互逻辑
    /// </summary>
    public partial class MainTop : BaseUserControl
    {
        public MainTop()
        {
            InitializeComponent();
            this.DataContext = new MainTopViewModel();
        }
    }
}
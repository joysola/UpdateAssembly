using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// DictionaryConfig.xaml 的交互逻辑
    /// </summary>
    public partial class DC_AddItem : BaseUserControl
    {
        public DC_AddItem()
        {
            InitializeComponent();
            this.DataContext = new DC_AddItemViewModel();
        }
    }
}
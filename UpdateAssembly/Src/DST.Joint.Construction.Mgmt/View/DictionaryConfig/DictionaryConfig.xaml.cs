using DST.Controls.Base;
using DST.Joint.Construction.Mgmt.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// DictionaryConfig.xaml 的交互逻辑
    /// </summary>
    public partial class DictionaryConfig : BaseUserControl
    {
        private DictionaryConfigViewModel dictionaryConfigViewModel = null;

        public DictionaryConfig()
        {
            InitializeComponent();
            this.dictionaryConfigViewModel = new DictionaryConfigViewModel();
            this.DataContext = this.dictionaryConfigViewModel;
            this.IsVisibleChanged += DictionaryConfig_IsVisibleChanged;
        }

        private void DictionaryConfig_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                this.dictionaryConfigViewModel.LoadCustomDict();
            }
        }

        /// <summary>
        /// 新增诊断类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDiagnosisType_Click(object sender, RoutedEventArgs e)
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("DC_AddDiagnosisType", "新增诊断类型");
            msg.Width = 450;
            msg.Height = 250;
            msg.CallBackCommand = this.dictionaryConfigViewModel.QueryCommand;
            Messenger.Default.Send<ShowContentWindowMessage>(msg);
        }

        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("DC_AddItem", "新增检查项目");
            msg.Width = 450;
            msg.Height = 180;
            msg.CallBackCommand = this.dictionaryConfigViewModel.QueryCommand;
            Messenger.Default.Send<ShowContentWindowMessage>(msg);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("DC_AddDiagnosisType", "编辑检查项目");
            msg.Width = 450;
            msg.Height = 250;
            msg.Args = new object[] { this.dgDict.SelectedItem };
            msg.CallBackCommand = this.dictionaryConfigViewModel.QueryCommand;
            Messenger.Default.Send<ShowContentWindowMessage>(msg);
        }

        private void DgDict_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
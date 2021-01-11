using DST.Controls.Base;
using DST.Database;
using DST.Joint.Construction.Mgmt.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DST.Joint.Construction.Mgmt.View
{
    /// <summary>
    /// InventoryArchivesManage.xaml 的交互逻辑
    /// </summary>
    public partial class DC_AddDiagnosisType : BaseUserControl
    {
        private DC_AddDiagnosisTypeViewModel dc_AddDiagnosisTypeViewModel = null;

        public DC_AddDiagnosisType()
        {
            InitializeComponent();
            this.dc_AddDiagnosisTypeViewModel = new DC_AddDiagnosisTypeViewModel();
            this.DataContext = this.dc_AddDiagnosisTypeViewModel;
            this.Loaded += DC_AddDiagnosisType_Loaded;
        }

        private void DC_AddDiagnosisType_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // 接收传递过来的数据
            if (this.dc_AddDiagnosisTypeViewModel.Args != null && this.dc_AddDiagnosisTypeViewModel.Args.Length == 1)
            {
                string diaType = (this.dc_AddDiagnosisTypeViewModel.Args[0] as DST_DICT).DICT_CODE;
                this.rtbDiaType.Document.Blocks.Clear();
                Run run = new Run(diaType);
                Paragraph p = new Paragraph();
                p.Inlines.Add(run);
                this.rtbDiaType.Document.Blocks.Add(p);
            }
        }

        /// <summary>
        /// 注册下拉框的textChanged事件，实现下拉框可直接编辑功能
        /// </summary>
        private void RegisterComboboxTextChanged()
        {
            this.cbDictClass.IsReadOnly = false;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.cbDictClass); i++)
            {
                DependencyObject o1 = VisualTreeHelper.GetChild(this.cbDictClass, i);

                for (int j = 0; j < VisualTreeHelper.GetChildrenCount(o1); j++)
                {
                    DependencyObject o2 = VisualTreeHelper.GetChild(o1, j);
                    if (o2 is TextBox)
                    {
                        (o2 as TextBox).TextChanged += DC_AddDiagnosisType_TextChanged;
                        break;
                    }
                }
            }
        }

        private void DC_AddDiagnosisType_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
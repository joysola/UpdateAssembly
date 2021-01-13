using DST.Common.Model;
using DST.Controls.Base;
using DST.Database.WPFCommonModels;
using DST.PathologyArchivesSystem.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace DST.ClientApp.View
{
    /// <summary>
    /// IAM_AddEditSample.xaml 的交互逻辑
    /// </summary>
    public partial class IAM_AddEditSample : BaseUserControl
    {
        private IAM_AddEditSampleViewModel viewModel;

        public IAM_AddEditSample()
        {
            InitializeComponent();
            this.viewModel = new IAM_AddEditSampleViewModel();
            this.DataContext = this.viewModel;
            this.RegisterMessage();
        }

        private void RegisterMessage()
        {
            // 性别勾选
            Messenger.Default.Register<string>(this, EnumMessageKey.IDCardChange, sex =>
            {
                if (sex == "1")
                {
                    this.rbMale.IsChecked = true;
                }
                else
                {
                    this.rbFemale.IsChecked = true;
                }
            });
        }

        //private void ComboBoxItem_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var xx = sender as ListBoxItem;
        //    var yy = xx.Content as ProductModel;
        //    if (yy != null && yy.productTypes != null && yy.productTypes.Count > 0)
        //    {
        //        if (xx.ContextMenu.ItemsSource == null)
        //        {
        //            xx.ContextMenu.ItemsSource = yy.productTypes;

        //            xx.ContextMenu.PlacementTarget = xx;
        //            xx.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Relative;
        //            xx.ContextMenu.HorizontalOffset = xx.ActualWidth;
        //            xx.ContextMenu.IsOpen = true;
        //            //xx.Focus();
        //            //this.cbContextMenu.IsOpen = true;
        //        }
        //    }
        //    else
        //    {
        //        xx.ContextMenu.ItemsSource = null;
        //    }
        //}

        /// <summary>
        /// 级联下拉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var types = this.viewModel.ProductDict[this.cbProduct.SelectedIndex].productTypes;
            if (types != null)
            {
                this.viewModel.ProductTypes = types;
                this.cbType.SelectedIndex = -1;
                this.cbType.Visibility = Visibility.Visible;
                this.tbItemType.Visibility = Visibility.Visible;
                if (!this.viewModel.IsAdd) // 编辑
                {
                    this.cbType.IsEnabled = false;
                }
                else // 新增
                {
                    this.cbType.IsDropDownOpen = true;
                }
            }
            else
            {
                this.viewModel.MBPSample.productType = null;
                this.cbType.Visibility = Visibility.Collapsed;
                this.tbItemType.Visibility = Visibility.Collapsed;
            }
        }
    }
}
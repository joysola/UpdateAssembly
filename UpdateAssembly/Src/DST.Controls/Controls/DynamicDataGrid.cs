using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace DST.Controls
{
    public partial class DynamicDataGrid : DataGrid
    {
        public DynamicDataGrid()
        {
            #region 特殊处理

            //数据源监视
            //DependencyPropertyDescriptor descriptorItemSource = DependencyPropertyDescriptor.FromProperty(ItemsSourceProperty, typeof(DynamicDataGrid));
            //if (descriptorItemSource != null)
            //{
            //    descriptorItemSource.AddValueChanged(this, Actual_ValueChanged);
            //}
            //防止内存泄漏 - protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)

            #endregion 特殊处理

            this.AddHandler(DataGridCell.PreviewKeyUpEvent, new RoutedEventHandler(columnCellPreviewKeyUp));

            //监听点击事件
            //columnHeader
            this.AddHandler(DataGrid.PreviewMouseLeftButtonUpEvent, new RoutedEventHandler(columnHeaderClick));
            //  this.AddHandler(DataGrid.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(columnHeaderDownClick));
            //datagridcell
            this.AddHandler(DataGridCell.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(dataGridCellClick));
        }

        public IEnumerable SelectColumsItems
        {
            get { return (IEnumerable)GetValue(SelectColumsItemsProperty); }
            set { SetValue(SelectColumsItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectColumsItemsProperty =
            DependencyProperty.Register("SelectColumsItems", typeof(IEnumerable), typeof(DynamicDataGrid), new PropertyMetadata(null));

        public void dataGridCellClick(object sender, RoutedEventArgs e)
        {
            //设置选中样式
            this.SelectionUnit = DataGridSelectionUnit.FullRow;
        }

        public void columnCellPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
        }

        public void columnHeaderUpClick(object sender, RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            var columnHeader = dep as DataGridColumnHeader;
            if (columnHeader != null)
            {
                //当前的列名
                //string columnName = columnHeader.Column.Header.ToString();
                DataGridColumn column = columnHeader.Column;
                string columnName = column.GetValue(ControlAttachProperty.TagProperty).ToString();

                this.SelectionUnit = DataGridSelectionUnit.Cell;
                this.SelectedCells.Clear();
                (SelectColumsItems as IList).Clear();
                foreach (dynamic item in this.Items)
                {
                    //设置选中
                    this.SelectedCells.Add(new DataGridCellInfo(item, columnHeader.Column));
                    (SelectColumsItems as IList).Add(item.GetMemberValue(columnName));
                }
                //强制通知待优化
                SelectColumsItems = SelectColumsItems;
            }
        }

        public void columnHeaderDownClick(object sender, RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.Source;
            var columnHeader = dep as DataGridColumnHeader;
            if (columnHeader != null)
            {
                //当前的列名
                //string columnName = columnHeader.Column.Header.ToString();
                DataGridColumn column = columnHeader.Column;
                string columnName = column.GetValue(ControlAttachProperty.TagProperty).ToString();

                this.SelectionUnit = DataGridSelectionUnit.Cell;
                this.SelectedCells.Clear();
                (SelectColumsItems as IList).Clear();
                foreach (dynamic item in this.Items)
                {
                    //设置选中
                    this.SelectedCells.Add(new DataGridCellInfo(item, columnHeader.Column));
                    //设置数据
                    if ((SelectColumsItems as IList).Contains(item.GetMemberValue(columnName)))
                    {
                        (SelectColumsItems as IList).Remove(item.GetMemberValue(columnName));
                    }
                    else
                        (SelectColumsItems as IList).Add(item.GetMemberValue(columnName));
                }
                //强制通知待优化
                SelectColumsItems = SelectColumsItems;
            }
        }

        public void columnHeaderClick(object sender, RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            var columnHeader = dep as DataGridColumnHeader;
            if (columnHeader != null)
            {
                //当前的列名
                //string columnName = columnHeader.Column.Header.ToString();
                DataGridColumn column = columnHeader.Column;
                string columnName = column.GetValue(ControlAttachProperty.TagProperty).ToString();

                this.SelectionUnit = DataGridSelectionUnit.Cell;
                this.SelectedCells.Clear();
                (SelectColumsItems as IList).Clear();
                foreach (dynamic item in this.Items)
                {
                    this.SelectedCells.Add(new DataGridCellInfo(item, columnHeader.Column));
                    //设置选中
                    //设置数据
                    if ((SelectColumsItems as IList).Contains(item.GetMemberValue(columnName)))
                    {
                        //   this.SelectedCells.Remove(new DataGridCellInfo(item, columnHeader.Column));
                        (SelectColumsItems as IList).Remove(item.GetMemberValue(columnName));
                    }
                    else
                    {
                        (SelectColumsItems as IList).Add(item.GetMemberValue(columnName));
                    }
                }
                //强制通知待优化
                SelectColumsItems = SelectColumsItems;
            }
        }

        /// <summary>
        /// 防止内存泄漏
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.Actual_ValueChanged(this, null);
        }

        /// <summary>
        /// 变换时生成列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Actual_ValueChanged(object sender, EventArgs args)
        {
            if (this.ItemsSource != null)
            {
                ObservableCollection<dynamic> ld = this.ItemsSource as ObservableCollection<dynamic>;
                if (ld != null && ld.Count > 0)
                {
                    this.Columns.Clear();

                    IEnumerable<string> cols = ld[0].GetDynamicMemberNames();

                    foreach (var propName in cols)
                    {
                        // MED_VITAL_SIGN mvs = ld[0].GetMemberValue(propName) as MED_VITAL_SIGN;
                        DataGridTextColumn newColumn = new DataGridTextColumn();
                        //设置Key值
                        newColumn.SetValue(ControlAttachProperty.TagProperty, propName);

                        DateTime dt = DateTime.Now;
                        bool result = DateTime.TryParse(propName, out dt);
                        if (result)
                        {
                            newColumn.Header = dt.ToString("HH:mm");
                        }
                        else
                        {
                            newColumn.Header = propName;
                        }

                        Binding bind = new Binding();
                        bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        bind.Path = new PropertyPath(propName + ".ITEM_VALUE");
                        newColumn.Binding = bind;
                        newColumn.Width = DataGridLength.Auto;
                        newColumn.MinWidth = 80;

                        //if (mvs.IsShowColumn == false)
                        //{
                        //    newColumn.Visibility = Visibility.Collapsed;
                        //}
                        //if (mvs.IsReadOnlyColumn)
                        //{
                        //    newColumn.IsReadOnly = true;
                        //}

                        this.Columns.Add(newColumn);
                    }
                    //this.FrozenColumnCount = 2;
                }
            }
        }
    }
}
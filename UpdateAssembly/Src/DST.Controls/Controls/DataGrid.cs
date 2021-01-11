using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DST.Controls
{
    partial class DataGridStyle : ResourceDictionary
    {
        /// <summary>
        /// 修改为单击进入编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = (DataGridCell)sender;
            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                if (!cell.IsFocused)
                {
                    cell.Focus();
                }

                DataGrid grdData = FindVisualParent<DataGrid>(cell);
                if (grdData.SelectionUnit != DataGridSelectionUnit.FullRow)
                {
                    if (!cell.IsSelected)
                    {
                        cell.IsSelected = true;
                    }
                }
                else
                {
                    DataGridRow row = FindVisualParent<DataGridRow>(cell);
                    if (row != null && !row.IsSelected)
                    {
                        row.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// 获取父容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        private static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        public void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid grid = sender as DataGrid;

            if (grid != null && (grid.Tag == null || !grid.Tag.ToString().Equals("NoPreviewKeyDown")))
            {
                //获取当前的单元格
                DataGridCellInfo dgci = grid.CurrentCell;
                if (dgci == null || dgci.Column == null || grid.SelectedIndex < 0)
                {
                    return;
                }

                int column = dgci.Column.DisplayIndex;
                int row = grid.SelectedIndex;
                int totalColums = grid.Columns.Count;
                int totalRows = grid.Items.Count;

                if (e.Key == Key.Right)
                {
                    if (column + 1 >= totalColums)
                    {
                        return;
                    }

                    // 存在列隐藏时，跳转到单元格失败
                    while (grid.Columns[column + 1].Visibility != Visibility.Visible)
                    {
                        column++;
                        if (column + 1 >= totalColums)
                        {
                            return;
                        }
                    }

                    DataGridCell newCell = GetCell(grid, row, column + 1);
                    if (null != newCell)
                    {
                        grid.CurrentCell = new DataGridCellInfo(newCell);
                        grid.BeginEdit();
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.Up)
                {
                    if (row <= 0)
                    {
                        return;
                    }

                    DataGridCell newCell = GetCell(grid, row - 1, column);
                    if (null != newCell)
                    {
                        grid.CurrentCell = new DataGridCellInfo(newCell);
                        grid.SelectedIndex = row - 1;
                        grid.BeginEdit();
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.Down)
                {
                    if (row + 1 >= totalRows)
                    {
                        return;
                    }

                    DataGridCell newCell = GetCell(grid, row + 1, column);
                    if (null != newCell)
                    {
                        grid.CurrentCell = new DataGridCellInfo(newCell);
                        grid.SelectedIndex = row + 1;
                        grid.BeginEdit();
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.Left)
                {
                    if (column <= 0)
                    {
                        return;
                    }

                    // 存在列隐藏时，跳转到单元格失败
                    while (grid.Columns[column - 1].Visibility != Visibility.Visible)
                    {
                        column--;
                        if (column <= 0)
                        {
                            return;
                        }
                    }

                    DataGridCell newCell = GetCell(grid, row, column - 1);
                    if (null != newCell)
                    {
                        grid.CurrentCell = new DataGridCellInfo(newCell);
                        grid.BeginEdit();
                        e.Handled = true;
                    }
                }
            }
        }

        public static DataGridCell GetCell(DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            DataGridCell cell = null;
            if (rowIndex > -1 && columnIndex > -1)
            {
                DataGridRow rowContainer = GetRow(dataGrid, rowIndex);
                if (rowContainer != null)
                {
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                    if (null != presenter)
                    {
                        cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                        if (cell == null)
                        {
                            dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[columnIndex]);
                            cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                        }
                    }
                }
            }

            return cell;
        }

        public static DataGridRow GetRow(DataGrid dataGrid, int rowIndex)
        {
            DataGridRow rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (rowContainer == null)
            {
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            }
            return rowContainer;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
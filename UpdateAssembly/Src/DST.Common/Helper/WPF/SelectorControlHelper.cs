using System.Windows;
using System.Windows.Controls.Primitives;

namespace DST.Common.Helper
{
    /// <summary>
    /// 列表控件帮助类：ListBox，ListView，ComboBox帮助类
    /// </summary>
    public class SelectorControlHelper
    {
        /// <summary>
        /// 设置列表选中项获得焦点
        /// </summary>
        /// <param name="selector">列表控件</param>
        /// <param name="selectItem">选中项</param>
        /// <returns>焦点是否获得</returns>
        public static bool SetItemFocus(Selector selector, object selectItem)
        {
            bool result = false;
            if (null != selector)
            {
                selector.UpdateLayout();
                DependencyObject tempItem = selector.ItemContainerGenerator.ContainerFromItem(selectItem);
                if (null != tempItem && tempItem is UIElement)
                {
                    result = ((UIElement)tempItem).Focus();
                }
            }

            return result;
        }
    }
}
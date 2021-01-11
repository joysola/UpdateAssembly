using System.Collections.Generic;
using System.Windows.Forms;

namespace DST.Common.Helper
{
    /// <summary>
    /// 用于Winform中获取本控件中某种类型的所有控件
    /// </summary>
    public static class GetControlsbyWinform
    {
        /// <summary>
        /// 获取控件中的某个类型的所有子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<T> GetControls<T>(this Control parent) where T : Control
        {
            var list = new List<T>();
            GetControls<T>(parent, list);
            return list;
        }

        /// <summary>
        /// 递归获取parent的所有T类型的子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="list"></param>
        private static void GetControls<T>(Control parent, List<T> list) where T : Control
        {
            if (parent is T)
            {
                list.Add((T)parent);
            }
            foreach (Control item in parent.Controls)
            {
                GetControls<T>(item, list);
            }
        }
    }
}
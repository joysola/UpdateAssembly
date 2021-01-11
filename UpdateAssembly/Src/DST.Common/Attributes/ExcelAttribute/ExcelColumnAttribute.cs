using System;

namespace DST.Common.Helper
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed public class ExcelColumnAttribute : Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        private readonly string _columnName;

        private readonly bool _isNecessary;

        /// <summary>
        ///
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="isNecessary">是否是必需填的</param>
        public ExcelColumnAttribute(string columnName, bool isNecessary = false)
        {
            this._columnName = columnName;
            this._isNecessary = isNecessary;
            // TODO: Implement code here
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string TableColumnName
        {
            get { return _columnName; }
        }

        /// <summary>
        /// 是否是必须填的
        /// </summary>
        public bool IsNecessary
        {
            get { return _isNecessary; }
        }

        /// <summary>
        /// 表格列的类型（可以随意写）
        /// </summary>
        public Type TableColumnType { get; set; }
    }
}
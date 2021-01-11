using System;

namespace DST.Common.Attributes
{
    /// <summary>
    /// 实体属性的特性，用于格式化属性（目前进支持DateTime类型）
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed public class DSTUrlParamAttribute : System.Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        private readonly string _format;

        /// <summary>
        /// 默认格式字符串
        /// </summary>
        private const string _defaultDateTimeFormatStr = "yyyy-MM-dd";

        public DSTUrlParamAttribute(string format = _defaultDateTimeFormatStr)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = _defaultDateTimeFormatStr;
            }
            this._format = format;
        }

        /// <summary>
        /// 实体属性格式化字符串
        /// </summary>
        public string Format
        {
            get { return _format; }
        }
    }
}
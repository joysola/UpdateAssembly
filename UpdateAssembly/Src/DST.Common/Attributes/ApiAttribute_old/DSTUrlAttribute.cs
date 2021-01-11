using System;

namespace DST.Common.Attributes
{
    /// <summary>
    /// 查询实体类属性，属性用于写入Controller和Action
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed public class DSTUrlAttribute : System.Attribute
    {
        private readonly string _dstUrl;

        // This is a positional argument
        public DSTUrlAttribute(string dstUrl)
        {
            this._dstUrl = dstUrl;
        }

        /// <summary>
        /// Url中的Controller和Action
        /// </summary>
        public string DSTUrl
        {
            get { return _dstUrl; }
        }
    }
}
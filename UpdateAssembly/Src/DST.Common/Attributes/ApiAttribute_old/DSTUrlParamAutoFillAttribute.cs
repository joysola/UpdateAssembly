using System;

namespace DST.Common.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DSTUrlParamAutoFillAttribute : Attribute
    {
        /// <summary>
        /// DSTUrl自动填充构造器
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="end">结束</param>
        public DSTUrlParamAutoFillAttribute(string start, string end)
        {
            Strat = start;
            End = end;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public string Strat { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public string End { get; set; }
    }
}
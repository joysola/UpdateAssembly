using System;

namespace DST.Common.Attributes
{
    /// <summary>
    /// 合成Url时忽略此属性
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DSTUrlParamIgnoreAttribute : Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        // This is a positional argument
    }
}
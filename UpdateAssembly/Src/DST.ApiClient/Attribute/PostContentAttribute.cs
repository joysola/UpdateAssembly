using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.ApiClient.Attribute
{
    /// <summary>
    /// 标记post请求需要发送的内容的特性
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public sealed class PostContentAttribute : System.Attribute
    {
    }
}

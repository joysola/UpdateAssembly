using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.ApiClient.Attribute
{
    /// <summary>
    /// url特性用于获取api地址
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class UrlAttribute : System.Attribute
    {

        readonly string _url;

        public UrlAttribute(string url)
        {
            this._url = url;
        }

        public string Url
        {
            get { return _url; }
        }
    }
}

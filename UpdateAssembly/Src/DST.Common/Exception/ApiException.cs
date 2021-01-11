using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common
{
    public class ApiException : Exception
    {
        public ApiException(string msg) : base(msg)
        {

        }
    }
}

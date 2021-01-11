using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Model
{
    public class ExtendAppContext
    {
        public static ExtendAppContext Current { get; } = new ExtendAppContext();
        private ExtendAppContext() { }
        /// <summary>
        /// 登录信息
        /// </summary>
        public LoginModel LoginModel { get; set; } = new LoginModel();
    }
}

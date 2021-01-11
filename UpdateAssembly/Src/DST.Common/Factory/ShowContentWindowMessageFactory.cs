using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DST.Common.Helper.Factory
{
    /// <summary>
    /// ContentWindow的工厂类
    /// </summary>
    public class ShowContentWindowMessageFactory
    {
        /// <summary>
        /// 加载所有View类型
        /// </summary>
        private static readonly List<Type> allTypes = Assembly.Load("DST.Joint.Construction.Mgmt").GetTypes().ToList();

        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <param name="contentName"></param>
        /// <returns></returns>
        public static Type CreateContent(string contentName)
        {
            var type = allTypes.SingleOrDefault(x => x.Name == contentName);
            return type;
        }
    }
}
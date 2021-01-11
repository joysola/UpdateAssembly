using Newtonsoft.Json;
using System;

namespace DST.Common.Helper
{
    public class CopyObject
    {
        /// <summary>
        /// 根据反射复制同一类型的对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static T CopyObjectByReflection<T>(T source)
        {
            T result = Activator.CreateInstance<T>();
            Type tType = source.GetType();
            foreach (var itemOut in result.GetType().GetProperties())
            {
                var itemIn = tType.GetProperty(itemOut.Name);
                if (itemIn != null)
                {
                    itemOut.SetValue(result, itemIn.GetValue(source));
                }
            }
            return result;
        }

        /// <summary>
        /// 使用JSon复制对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static T CopyObjectByJson<T>(T source)
        {
            string strSource = JsonConvert.SerializeObject(source);
            T result = JsonConvert.DeserializeObject<T>(strSource);
            return result;
        }
    }
}
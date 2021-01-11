using System;

namespace DST.Common.Helper
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取类型的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            // 值类型创建实例，用类型为空
            var result = type.IsValueType ? Activator.CreateInstance(type) : null;
            return result;
        }

        /// <summary>
        /// 将value值转换为对应类型的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic ChangeType(this Type type, object value)
        {
            Type t = Nullable.GetUnderlyingType(type) ?? type; // 注意可空类型，不能直接使用Convert.ChangeType，需要先获取基础类型
            try
            {
                value = Convert.ChangeType(value, t); // 类型转换
            }
            catch
            {
                value = t.GetDefaultValue(); // 转型不成功则使用类型的默认值
            }
            return value;
        }

        /// <summary>
        /// 获取数组元素类型
        /// </summary>
        /// <param name="arrayType"></param>
        /// <returns></returns>
        public static Type GetArrayElementType(this Type arrayType)
        {
            if (!arrayType.IsArray)
            {
                return null;
            }
            var elementTypeStr = arrayType.FullName.Replace("[]", "");
            var elementType = arrayType.Assembly.GetType(elementTypeStr);
            return elementType;
        }
    }
}
using DST.Common.Attributes;
using DST.Common.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DST.Common.Helper
{
    /// <summary>
    /// 查询实体扩展类
    /// </summary>
    public static class DSTUrlExtension
    {
        /// <summary>
        /// 从查询实体获取Url（controller、action、查询参数）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetUrlFromModel(this IQueryModel model)
        {
            List<string> strList = new List<string>(); // 查询参数集合
            string url = string.Empty; // Url
            Type type = model.GetType();
            var urlAttr = type.GetCustomAttribute<DSTUrlAttribute>();
            url = urlAttr.DSTUrl; // url主题
            var properties = type.GetProperties(); // 获取查询实体所有属性
            foreach (var prop in properties)
            {
                // 0. 如果标记了忽略特性，则跳过
                var ignoreAttr = prop.GetCustomAttribute<DSTUrlParamIgnoreAttribute>();
                if (ignoreAttr != null)
                {
                    continue;
                }
                // 1-1. 普通属性
                var paramName = prop.Name.ToLower(); // 字段名称（即url的参数名称）
                dynamic value = prop.GetValue(model);
                if (value == null)
                {
                    continue; // 属性值为空，不需要写入查询参数
                }

                // 2. 带有特性的属性
                var paramAttr = prop.GetCustomAttribute<DSTUrlParamAttribute>();
                // 格式化字符串
                if (paramAttr != null)
                {
                    string formatStr = paramAttr.Format; // 格式化字符串

                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))// 只有一个日期
                    {
                        if (value == DateTime.MinValue) // DateTime.MinValue不需要传送
                        {
                            break;
                        }
                        //string dateStr = value?.toString(formatStr); // 日期格式化
                        //strList.Add($"{paramName}={dateStr}");
                        var str = GetDateTimeParamStr(value, formatStr, paramName);// 日期格式化
                        strList.Add(str);
                    }
                    else if (prop.PropertyType == typeof(DateTime[]) || prop.PropertyType == typeof(DateTime?[])) // 多个日期
                    {
                        string dateStr = string.Empty;
                        foreach (dynamic item in value)
                        {
                            if (item == null || item == DateTime.MinValue) // 只要有一个元素是null（或是DateTime.MinValue），则认为此日期数组违规，不处理
                            {
                                dateStr = "";
                                break;
                            }
                            //dateStr = item?.ToString(formatStr); // 日期格式化
                            //strList.Add($"{paramName}={dateStr}");
                            var str = GetDateTimeParamStr(item, formatStr, paramName);
                            strList.Add(str);
                        }
                    }
                    else // 以后可能有其他处理
                    {
                    }
                    continue; // 处理完毕进行下一个
                }

                // 3. 数组类型属性
                if (value is Array) // 数组对象
                {
                    var attr = prop.GetCustomAttribute<DSTUrlParamAutoFillAttribute>(); // 是否需要自动填充数组
                    if (attr != null)
                    {
                        value = GetAutoFillArray(value, prop, attr);
                    }
                    string paramStr = string.Empty;
                    foreach (dynamic item in value)
                    {
                        if (item == null)
                        {
                            paramStr = string.Empty;
                            break;
                        }
                        strList.Add($"{paramName}={item}");
                    }
                    continue; // 处理完毕，进行下一个
                }

                // 1-2. 衍生：普通属性（2、3都未处理的执行这里）
                strList.Add($"{paramName}={value}");
            }

            if (strList.Count > 0) // 存在查询参数的话
            {
                url += "?" + string.Join("&", strList); // 多个参数值对之间用 & 分隔
            }
            return url;
        }

        /// <summary>
        /// 默认格式化字符串
        /// </summary>
        private static readonly string _defaultDateTimeFormatStr = "yyyy-MM-dd";

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="value">日期</param>
        /// <param name="formatStr">格式化字符串</param>
        /// <param name="paramName">对应属性名</param>
        /// <returns></returns>
        private static string GetDateTimeParamStr(dynamic value, string formatStr, string paramName)
        {
            string dateStr = string.Empty;
            try
            {
                dateStr = value?.ToString(formatStr); // 日期格式化
            }
            catch (Exception ex)
            {
                DST.Common.Logger.Logger.Error("查询实体格式化字符串不正确！", ex);
                formatStr = _defaultDateTimeFormatStr;
                dateStr = value?.ToString(formatStr); // 日期格式化
            }

            var result = $"{paramName}={dateStr}";
            return result;
        }

        /// <summary>
        /// 自动填充数组元素
        /// </summary>
        /// <param name="value"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static dynamic GetAutoFillArray(dynamic value, PropertyInfo prop, DSTUrlParamAutoFillAttribute attr)
        {
            var eleType = prop.PropertyType.GetArrayElementType(); // 获取数组元素类型
            if (value.Length == 2)
            {
                if (value[0] == null) // 起始
                {
                    value[0] = eleType.ChangeType(attr.Strat);
                }
                if (value[1] == null) // 结束
                {
                    value[1] = eleType.ChangeType(attr.End);
                }
            }
            return value;
        }
    }
}
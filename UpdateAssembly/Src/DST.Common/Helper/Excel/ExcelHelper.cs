using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DST.Common.Helper.ExcelHelper
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public sealed class ExcelHelper
    {
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <param name="worksheetIndex">读取工作表索引</param>
        /// <param name="firstRowIndex">首行数据索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ReadExcelFile(string filePath, int worksheetIndex = 0, int firstRowIndex = 0)
        {
            var dt = new DataTable();
            var workbook = new Workbook(filePath);
            var cells = workbook.Worksheets[worksheetIndex].Cells;
            var rows = cells.Rows.Count;
            //var colums = cells.Columns.Count;
            //for (int i = 0; i < rows; i++)
            //{
            //    for (int j = 0; j < colums; j++)
            //    {
            //        var cell = cells[i, j];
            //        if (cell.IsMerged)
            //        {
            //        }
            //    }
            //}
            dt = cells.ExportDataTable(firstRowIndex, 0, rows, 30);
            return dt;
        }

        /// <summary>
        /// 表格列
        /// </summary>
        private class Column
        {
            /// <summary>
            /// 列名
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// 是否是必填字段
            /// </summary>
            public bool IsNecessary { get; set; }

            /// <summary>
            /// 列类型
            /// </summary>
            public Type ColumnType { get; set; }

            /// <summary>
            /// 属性的属性信息
            /// </summary>
            public PropertyInfo PropInfo { get; set; }
        }

        /// <summary>
        /// Excel的实体返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ResultModelList<T>
        {
            public List<T> Result { get; set; } = new List<T>();
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// 将excel数据读入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static ResultModelList<T> ReadExcelFile<T>(string filePath, int worksheetIndex = 0, int firstRowIndex = 0) where T : new()
        {
            DataTable dataTable = ReadExcelFile(filePath, worksheetIndex, firstRowIndex); // excel表格读取
            var result = new ResultModelList<T>(); // 结果
            Dictionary<string, Column> modelPropDict = null; // 实体属性字典（属性名，表格列信息）
            Dictionary<string, int> tableTitleDict = null; // excel的表头字典（列名，列序号）
            var type = typeof(T);
            var excelModelAttr = type.GetCustomAttribute<ExcelModelAttribute>();
            if (excelModelAttr == null)
            {
                result.ErrorMessage += "此实体不是ExcelModel实体！";
                Logger.Logger.Error("导入Excel故障：" + result.ErrorMessage);
                return result;
            }

            try
            {
                // 获取所有带有ExcelColumn的属性
                modelPropDict = GetExcelColumnProperties(type);
                // 统计excel表头
                tableTitleDict = GetTitlefromExcel(dataTable);
                // excel的真实列 中 是否 缺少 必填列
                var columns = modelPropDict.Values.Where(x => x.IsNecessary).ToList(); // 必填列集合
                var isCompleted = true;
                string lackColName = string.Empty; // 缺少的列
                foreach (var col in columns)
                {
                    if (!tableTitleDict.Keys.ToList().Contains(col.ColumnName)) // excel列缺少必填列
                    {
                        isCompleted = false;
                        lackColName = col.ColumnName;
                        break;
                    }
                }
                if (!isCompleted)
                {
                    result.ErrorMessage += $"Excel文件格式不正确!缺少列:'{lackColName}'";
                    Logger.Logger.Error("导入Excel故障：" + result.ErrorMessage);
                    return result;
                }
                // 创建实体集合
                result.Result = GetModelListFromDataTable<T>(dataTable, modelPropDict, tableTitleDict);
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("解析excel文件出错！", ex);
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取所有带ExcelColumn特性的属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Dictionary<string, Column> GetExcelColumnProperties(Type type)
        {
            var modelPropDict = new Dictionary<string, Column>(); // 实体属性字典（属性名，表格列信息）
            var props = type.GetProperties();
            // 获取所有带有ExcelColumn的属性
            foreach (var pp in props)
            {
                var columnAttr = pp.GetCustomAttribute<ExcelColumnAttribute>();
                if (columnAttr == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(columnAttr.TableColumnName))
                {
                    var col = new Column
                    {
                        ColumnName = columnAttr.TableColumnName,
                        IsNecessary = columnAttr.IsNecessary,
                        ColumnType = columnAttr.TableColumnType,
                        PropInfo = pp
                    };
                    modelPropDict.Add(pp.Name, col);
                }
            }
            return modelPropDict;
        }

        /// <summary>
        /// 根据读取到的excel的datatable获取表头
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static Dictionary<string, int> GetTitlefromExcel(DataTable dataTable)
        {
            var tableTitleDict = new Dictionary<string, int>(); // excel真实的表头字典（列名，列序号）
            // 统计excel表头
            DataRow titleRow = dataTable.Rows[0]; // 第一行对应标题
            for (int i = 0; i < titleRow.ItemArray.Length; i++)
            {
                if (titleRow.ItemArray[i] != System.DBNull.Value)
                {
                    tableTitleDict.Add(titleRow.ItemArray[i].ToString(), i);
                }
            }
            return tableTitleDict;
        }

        /// <summary>
        /// 根据实体属性字典和excel表头字典将 datatable转成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="modelPropDict">实体属性字典</param>
        /// <param name="tableTitleDict">表头字段</param>
        /// <returns></returns>
        private static List<T> GetModelListFromDataTable<T>(DataTable dataTable, Dictionary<string, Column> modelPropDict, Dictionary<string, int> tableTitleDict) where T : new()
        {
            var list = new List<T>();
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                bool isPassed = false; // 是否需要跳过
                var model = new T();
                foreach (var modelP in modelPropDict)
                {
                    isPassed = false;
                    var column = modelPropDict[modelP.Key]; // 实体列具有的信息
                    var columnName = column.ColumnName; // 列名
                    var columnType = column.ColumnType; // 列类型
                    if (!tableTitleDict.ContainsKey(columnName)) // excel数据中不包含此列,跳过此属性
                    {
                        continue;
                    }
                    var value = dataTable.Rows[i][tableTitleDict[columnName]]; // 单元格的值
                    if (value == System.DBNull.Value) // 如果单元格值为空
                    {
                        if (column.IsNecessary) // 必填项为空，则忽略此"行"
                        {
                            isPassed = true; // 跳过此行
                            break;
                        }
                        value = column.PropInfo.PropertyType.GetDefaultValue(); // 获取属性默认值
                    }
                    if (columnType != null && value != null) // 表格类型不为空，说明需要转型
                    {
                        value = column.PropInfo.PropertyType.ChangeType(value);
                    }
                    column.PropInfo.SetValue(model, value); // 设置字段默认值
                }
                if (!isPassed) // 不跳过加入此行
                {
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
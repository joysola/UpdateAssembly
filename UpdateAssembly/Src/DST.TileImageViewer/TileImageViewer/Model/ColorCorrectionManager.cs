using SqlSugar;
using System;
using System.Collections.Generic;

namespace TileImageViewer.Model
{
    /// <summary>
    /// 颜色调整数据存储管理器
    /// </summary>
    internal class ColorCorrectionManager
    {
        public ColorCorrectionManager(String filePath)
        {
            string strDbPath = filePath;
            if (strDbPath.StartsWith("\\\\"))
                strDbPath = strDbPath.Replace("\\", "/");

            string connStr = @"Data Source=" + @"" + strDbPath + "\\info.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
            Db = new SqlSugarClient(
            new ConnectionConfig()
            {
                ConnectionString = connStr,
                DbType = SqlSugar.DbType.Sqlite,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
        }

        //注意：不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual List<ColorCorrection> GetList()
        {
            return Db.Queryable<ColorCorrection>().ToList();
        }

        public ColorCorrection getSettingDetail()
        {
            ColorCorrection stPer = new ColorCorrection();
            try
            {
                //根据模型建表
                Db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(ColorCorrection));
                List<ColorCorrection> stAll = this.GetList();
                if (stAll.Count > 0)
                {
                    stPer = stAll[stAll.Count - 1];
                }
                else
                {
                    stPer = ColorCorrection.DefaultSetting();
                }
            }
            catch (SqlSugarException e)
            {
            }

            return stPer;
        }

        public int UpdateToDb(ColorCorrection st) //add
        {
            int id = 0;

            try
            {
                //根据模型建表
                Db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(ColorCorrection));
                Db.Saveable(st).ExecuteReturnEntity();
            }
            catch (SqlSugarException e)
            {
            }

            return id;
        }
    }
}
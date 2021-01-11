using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TileImageViewer.Model
{
    /// <summary>
    /// 数据库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DbContext<T> where T : class, new()
    {
        public DbContext()
        {
            string connStr = @"Data Source=" + @"" + PathUtil.AppdataPath + "\\tile_image.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
            Db = new SqlSugarClient(
            new ConnectionConfig()
            {
                ConnectionString = connStr,
                DbType = SqlSugar.DbType.Sqlite,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
            //调式代码 用来打印SQL
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }

        //注意：不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作

        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }//用来处理T表的常用操作

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return CurrentDb.GetList();
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic id)
        {
            return CurrentDb.Delete(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj)
        {
            return CurrentDb.Update(obj);
        }
    }
}
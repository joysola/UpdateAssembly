/***************************************************
 *
 * 文件名称：BasePatManager.cs
 * 作    者：许文龙
 * 日    期：2020-08-21
 * 描    述：数据库模块的患者信息的基本父类
 *
 * *************************************************/

namespace DST.Database.Base
{
    public class BasePatManager<T> : BaseManager<T> where T : class, new()
    {
        /// <summary>
        /// 患者基本信息
        /// </summary>
        protected BasePatManager()
        {
            string text = System.Environment.CurrentDirectory + "\\PatientInfo.db";
            if (text.StartsWith("\\\\"))
            {
                text = text.Replace("\\", "/");
            }

            base.InitDbManger(text);
        }
    }
}
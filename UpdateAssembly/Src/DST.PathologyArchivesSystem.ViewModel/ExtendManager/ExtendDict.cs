using DST.Database;
using System.Collections.Generic;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    /// <summary>
    /// 全局字典类
    /// </summary>
    public class ExtendDict
    {
        private List<DST_DICT> dictList = null;
        private static ExtendDict instance = new ExtendDict();

        /// <summary>
        /// 私有构造，单利模式
        /// </summary>
        private ExtendDict()
        { }

        /// <summary>
        /// 单利实体
        /// </summary>
        public static ExtendDict Instance { get { return instance; } }

        /// <summary>
        /// 字典列表
        /// </summary>
        public List<DST_DICT> DictList
        {
            set { this.dictList = value; }
            get
            {
                if (dictList == null)
                {
                    this.dictList = DictDB.CreateInstance().GetList();
                }

                return this.dictList;
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace DST.Common.Model
{
    public class ExtendApiDict
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static ExtendApiDict Instance { get; } = new ExtendApiDict();

        private ExtendApiDict() { }

        /// <summary>
        /// 鳞状上皮分析结果字典
        /// </summary>
        public Dictionary<string, string> SampleTctResDict { get; set; }

        /// <summary>
        /// 腺上皮细胞分析结果字典
        /// </summary>
        public Dictionary<string, string> GlandularEpithelialCellResDict { get; set; }

        /// <summary>
        /// 炎性程度
        /// </summary>
        public Dictionary<string, string> InflammationDict { get; set; }

        /// <summary>
        /// 微生物
        /// </summary>
        public Dictionary<string, string> LabelTypeDict { get; set; }

        /// <summary>
        /// 检验项目
        /// </summary>
        public Dictionary<string, string> GlassSlideTestItemDDict { get; set; }

        /// <summary>
        /// 省份字典
        /// </summary>
        public List<ProvinceModel> ProvinceDict { get; set; }

        /// <summary>
        /// 将初始化数据作为一条插入dictionary
        /// </summary>
        public Dictionary<string, string> InitDict(Dictionary<string, string> dict, string desc)
        {
            var list = new List<KeyValuePair<string, string>>(dict);
            list.Insert(0, new KeyValuePair<string, string>("", desc));
            return list.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

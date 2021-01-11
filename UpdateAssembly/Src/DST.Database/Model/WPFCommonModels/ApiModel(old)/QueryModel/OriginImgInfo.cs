
using DST.Common.Attributes;
using DST.Common.Helper;
using DST.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DST.Common.Model
{
    [DSTUrl("tctfile/select/1/files/")]
    public class OriginImgInfo: IQueryPageModel
    {
        /// <summary>
        /// 扫描时间yyyy-MM-dd 
        /// 需要两个参数 参考示例代码 scan_date=开始⽇期&scan_date=结束⽇期
        /// </summary>
        [DSTUrlParam]
        public DateTime?[] Scan_Date { get; set; } = new DateTime?[2];
        /// <summary>
        /// 条数
        /// 显示⼀⻚数据条数
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 页数
        /// 显示第⼏⻚数据
        /// </summary>
        public string Page_Number { get; set; }
        /// <summary>
        /// 制片时间区间
        /// 需要两个参数 make_date=开始⽇期&make_date=结束⽇期
        /// </summary>
        [DSTUrlParam]
        public DateTime?[] Make_Date { get; set; } = new DateTime?[2];
        /// <summary>
        /// 地域
        /// 各省份代号<两位> 河南=41, 江苏=32, 安徽=34, 上海=31, ⼭东=37 参 考：https://zhidao.baidu.com/question/2058202758226342267.html
        /// </summary>
        public int? Area { get; set; }
        /// <summary>
        /// 年龄区间
        /// 需要两个参数 age=20&age=50
        /// </summary>
        public int?[] Age { get; set; } = new int?[2];
        /// <summary>
        /// 样本编码
        /// 允许模糊查询
        /// </summary>
        public string Sample_Code { get; set; }
        /// <summary>
        /// 腺上⽪细胞分析结果
        /// 可使⽤多个参数, ⽆=-1,未⻅腺上⽪内病变及恶性病变=0,⾮典型腺细胞（无指定）=1，原位腺癌=2，非典型腺细胞（倾向瘤变）=3，腺癌=4
        /// </summary>
        public string Gec_Result { get; set; }
        /// <summary>
        /// 鳞状上⽪细胞分析结果
        /// 可使⽤多个参数, NILM=0, ASC-US=1, ASC-H=2,LSIL=3, HSIL=4, 鳞状细胞癌=5，无=6
        /// </summary>
        public string Sec_Result { get; set; }
        /// <summary>
        /// 炎性程度
        /// 可使⽤多个参数， ⽆=-1, 轻度=0，中度=1，重度=2
        /// </summary>
        public string Inflammation { get; set; }
        /// <summary>
        /// 微生物项目
        /// 可使用多个参数，阴道滴虫=0，真菌， 形态学上符合念珠菌属=1，提示细菌性阴道炎=2，细菌，形态学上符合放线菌=3，细胞变化，符合单纯胞疹病毒感染=4
        /// /// </summary>
        public string Microorganism { get; set; }
    }
}

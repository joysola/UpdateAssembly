using DST.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Model
{
    [DSTUrl("glassslide/lent/out/")]
    public class QuerySlideAllRecords : IQueryModel
    {
        /// <summary>
        /// 样本编号
        /// </summary>
        public string Sample_Code { get; set; }
    }
}

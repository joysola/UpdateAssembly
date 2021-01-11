using DST.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Model
{
    [DSTUrl("glassslide/lent/select/")]
    public class QuerySlidesbyCodes : IQueryPageModel
    {
        public string Count { get; set; }
        public string Page_Number { get; set; }
    }
}

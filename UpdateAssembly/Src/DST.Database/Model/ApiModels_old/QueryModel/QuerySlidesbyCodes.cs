using DST.Common.Attributes;

namespace DST.Database.Model.ApiModels_old
{
    [DSTUrl("glassslide/lent/select/")]
    public class QuerySlidesbyCodes : IQueryPageModel
    {
        public string Count { get; set; }
        public string Page_Number { get; set; }
    }
}
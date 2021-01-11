using GalaSoft.MvvmLight;

namespace DST.Database.WPFCommonModels
{
    /// <summary>
    /// 自定义分页类型
    /// </summary>
    public class CustomPageModel : ObservableObject
    {
        private int pageIndex = 1;
        private int pageSize = 15;
        private int totalNum = 0;
        private int totalPage = 0;

        /// <summary>
        /// 页码序号，从1开始
        /// </summary>
        public int PageIndex
        {
            get { return this.pageIndex; }
            set
            {
                this.pageIndex = value;
                this.RaisePropertyChanged("PageIndex");
            }
        }

        /// <summary>
        /// 页码大小，每页包含多少条数据
        /// </summary>
        public int PageSize
        {
            get { return this.pageSize; }
            set
            {
                this.pageSize = value;
                this.RaisePropertyChanged("PageSize");
            }
        }

        /// <summary>
        /// 查询出来的总记录数
        /// </summary>
        public int TotalNum
        {
            get { return this.totalNum; }
            set
            {
                this.totalNum = value;
                this.RaisePropertyChanged("TotalNum");
            }
        }

        /// <summary>
        /// 查询出来的总页数
        /// </summary>
        public int TotalPage
        {
            get { return this.totalPage; }
            set
            {
                this.totalPage = value;
                this.RaisePropertyChanged("TotalPage");
            }
        }

        /// <summary>
        /// 默认PageIndex = 1, pageSize = 15
        /// </summary>
        public CustomPageModel()
        {
            this.PageIndex = 1;
            this.PageSize = 15;
            this.TotalNum = 0;
            this.TotalPage = 0;
        }
    }
}
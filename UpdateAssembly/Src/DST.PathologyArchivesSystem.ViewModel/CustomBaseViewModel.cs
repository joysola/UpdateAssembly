/********************************************************************************
 *
 * 文件名称：CustomBaseViewModel.cs
 * 作    者：许文龙
 * 日    期：2020-08-21
 * 描    述：各个界面对应的ViewModel的父类，不让界面ViewMode直接继承BaseViewModel
 *           因为BaseViewModel是和BaseControl配套使用，是基础类型。
 *           而各个界面的ViewModel的共用逻辑，不适合放在BaseViemModel，所以抽象出
 *           CustomBaseViewModel
 *
 * ******************************************************************************/

using DST.Common.Model;
using DST.Controls.Base;
using DST.Database.Model.ApiModels_old;
using DST.Database.Model.ExtendContext.old;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    /// <summary>
    /// 各个界面对应的ViewModel的父类，不让界面ViewMode直接继承BaseViewModel
    /// </summary>
    public class CustomBaseViewModel : BaseViewModel
    {
        private CustomPageModel pageModel = new CustomPageModel();
        private ObservableCollection<string> scanList = new ObservableCollection<string>();
        private ObservableCollection<string> dictList = new ObservableCollection<string>();

        /// <summary>
        /// 地域
        /// </summary>
        public List<ProvinceModel> ProvinceDict => ExtendApiDict.Instance.ProvinceDict;

        /// <summary>
        /// 鳞状上皮分析结果字典
        /// </summary>
        public Dictionary<string, string> SampleTctResDict => ExtendApiDict.Instance.SampleTctResDict;

        /// <summary>
        /// 腺上皮细胞分析结果字典
        /// </summary>
        public Dictionary<string, string> GlandularEpithelialCellResDict => ExtendApiDict.Instance.GlandularEpithelialCellResDict;

        /// <summary>
        /// 炎性程度
        /// </summary>
        public Dictionary<string, string> InflammationDict => ExtendApiDict.Instance.InflammationDict;

        /// <summary>
        /// 微生物
        /// </summary>
        public Dictionary<string, string> LabelTypeDict => ExtendApiDict.Instance.LabelTypeDict;

        /// <summary>
        /// 检验项目
        /// </summary>
        public Dictionary<string, string> GlassSlideTestItemDDict => ExtendApiDict.Instance.GlassSlideTestItemDDict;

        /// <summary>
        /// 借片状态
        /// </summary>
        public Dictionary<string, string> LendStateDict => new Dictionary<string, string> { { "", "请选择是否外借" }, { "0", "未外借" }, { "1", "已外借" } };

        /// <summary>
        /// 玻片完整度
        /// </summary>
        public Dictionary<string, string> SlideIntegrityDict => new Dictionary<string, string> { { "", "请选择玻片完整度" }, { "0", "完好" }, { "1", "损坏" }, { "2", "丢失" } };

        /// <summary>
        /// 分页信息对象
        /// </summary>
        public CustomPageModel PageModel
        {
            get { return this.pageModel; }
            set
            {
                this.pageModel = value;
                this.RaisePropertyChanged("PageModel");
            }
        }

        /// <summary>
        /// 扫描结果列表
        /// </summary>
        public ObservableCollection<string> ScanList
        {
            get { return this.scanList; }
            set
            {
                this.scanList = value;
                this.RaisePropertyChanged("ScanList");
            }
        }

        /// <summary>
        /// 检查项目列表
        /// </summary>
        public ObservableCollection<string> DictList
        {
            get { return this.dictList; }
            set
            {
                this.dictList = value;
                this.RaisePropertyChanged("DictList");
            }
        }

        /// <summary>
        /// 分页导航栏：首页
        /// </summary>
        public ICommand FirstPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.PageModel.PageIndex = 1;
                    this.PaginationFirstPage();
                });
            }
        }

        /// <summary>
        /// 分页导航栏：上一页
        /// </summary>
        public ICommand PreviousPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    if (this.PageModel.PageIndex > 1)
                    {
                        this.PageModel.PageIndex = this.PageModel.PageIndex - 1;
                        this.PaginationPreviousPage();
                    }
                });
            }
        }

        /// <summary>
        /// 分页导航栏：下一页
        /// </summary>
        public ICommand NextPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    if (this.PageModel.PageIndex < this.PageModel.TotalPage)
                    {
                        this.PageModel.PageIndex = this.PageModel.PageIndex + 1;
                        this.PaginationNextPage();
                    }
                });
            }
        }

        /// <summary>
        /// 分页导航栏：末页
        /// </summary>
        public ICommand LastPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    if (this.PageModel.PageIndex != this.PageModel.TotalPage)
                    {
                        this.PageModel.PageIndex = this.PageModel.TotalPage;
                        this.PaginationLastPagePage();
                    }
                });
            }
        }

        /// <summary>
        /// 分页导航栏：每页显示的条数
        /// </summary>
        public ICommand PaginPageChangedCommand
        {
            get
            {
                return new RelayCommand<int>(data =>
                {
                    this.PageModel.PageIndex = 1;
                    this.PageModel.PageSize = data;
                    this.PaginPageChanged();
                });
            }
        }

        /// <summary>
        /// 无参构造
        /// </summary>
        public CustomBaseViewModel() : base()
        {
        }

        /// <summary>
        /// 自定义加载字典
        /// </summary>
        public virtual void LoadCustomDict()
        {
            List<string> tmp = ExtendDict.Instance.DictList.Where(x => x.DICT_CLASS.Equals("检查项目")).Select(x => x.DICT_NAME).Distinct().ToList();
            tmp.Insert(0, "请选择检查项目");
            this.DictList = new ObservableCollection<string>(tmp);

            tmp = null;
            tmp = new List<string>();
            tmp.Add("请选择扫描结果");
            tmp.Add("已扫描");
            tmp.Add("未扫描");
            this.ScanList = new ObservableCollection<string>(tmp);
        }

        /// <summary>
        /// 分页导航栏：首页
        /// </summary>
        public virtual void PaginationFirstPage()
        {
        }

        /// <summary>
        /// 分页导航栏：上一页
        /// </summary>
        public virtual void PaginationPreviousPage()
        {
        }

        /// <summary>
        /// 分页导航栏：下一页
        /// </summary>
        public virtual void PaginationNextPage()
        {
        }

        /// <summary>
        /// 分页导航栏：最后一页
        /// </summary>
        public virtual void PaginationLastPagePage()
        {
        }

        /// <summary>
        /// 分页导航栏：切换每页条数
        /// </summary>
        public virtual void PaginPageChanged()
        {
        }
    }
}
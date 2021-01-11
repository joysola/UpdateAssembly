using DST.Database;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class DictionaryConfigViewModel : CustomBaseViewModel
    {
        private string selectDictClass = string.Empty;
        private ObservableCollection<DST_DICT> allDictList = new ObservableCollection<DST_DICT>();

        public string SelectDictClass
        {
            get { return this.selectDictClass; }
            set
            {
                this.selectDictClass = value;
                this.RaisePropertyChanged("SelectDictClass");
            }
        }

        /// <summary>
        /// 检查项目列表
        /// </summary>
        public ObservableCollection<DST_DICT> AllDictList
        {
            get { return this.allDictList; }
            set
            {
                this.allDictList = value;
                this.RaisePropertyChanged("AllDictList");
            }
        }

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand QueryCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.PageModel.PageIndex = 1;
                    this.RefreshData();
                });
            }
        }

        /// <summary>
        /// 无参构造
        /// </summary>
        public DictionaryConfigViewModel()
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void LoadData()
        {
            // 设置检查项目下拉框数据源
            this.LoadCustomDict();
            this.RefreshData();
            this.SelectDictClass = this.DictList[0];
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            ExtendDict.Instance.DictList = DictDB.CreateInstance().GetList();
            string itemName = string.IsNullOrEmpty(this.SelectDictClass) || this.SelectDictClass.Equals("请选择检查项目") ? "" : this.SelectDictClass;
            List<DST_DICT> tmpList = DictDB.CreateInstance().GetDictByClassAndName("检查项目", itemName, this.PageModel);

            // 设置检查项目下拉框数据源
            this.LoadCustomDict();
            this.AllDictList.Clear();
            this.AllDictList = null;
            this.AllDictList = new ObservableCollection<DST_DICT>(tmpList);
        }

        /// <summary>
        /// 首页
        /// </summary>
        public override void PaginationFirstPage()
        {
            this.QueryCommand.Execute(null);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public override void PaginationPreviousPage()
        {
            this.RefreshData();
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public override void PaginationNextPage()
        {
            this.RefreshData();
        }

        /// <summary>
        /// 最后页
        /// </summary>
        public override void PaginationLastPagePage()
        {
            this.RefreshData();
        }

        /// <summary>
        /// 分页导航栏：切换每页条数
        /// </summary>
        public override void PaginPageChanged()
        {
            this.RefreshData();
        }
    }
}
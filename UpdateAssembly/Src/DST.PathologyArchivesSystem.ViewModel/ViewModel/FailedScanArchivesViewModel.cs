using DST.Common.Helper;
using DST.Controls;
using DST.Database;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class FailedScanArchivesViewModel : CustomBaseViewModel
    {
        private bool selectAll = false;
        private DST_PATIENT_INFO patInfoCondition = new DST_PATIENT_INFO();
        private ObservableCollection<DST_PATIENT_INFO> patientInfoList = new ObservableCollection<DST_PATIENT_INFO>();

        /// <summary>
        /// 全选标志
        /// </summary>
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                this.PatientInfoList.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        /// <summary>
        /// 查询条件实体
        /// </summary>
        public DST_PATIENT_INFO PatInfoCondition
        {
            get { return this.patInfoCondition; }
            set
            {
                this.patInfoCondition = value;
                this.RaisePropertyChanged("PatInfoCondition");
            }
        }

        /// <summary>
        /// 患者信息列表，主体数据源
        /// </summary>
        public ObservableCollection<DST_PATIENT_INFO> PatientInfoList
        {
            get { return this.patientInfoList; }
            set
            {
                this.patientInfoList = value;
                this.RaisePropertyChanged("PatientInfoList");
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
        /// 重置查询条件命令
        /// </summary>
        public ICommand ResetCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.ResetPatInfoCondition();
                });
            }
        }

        /// <summary>
        /// 删除勾选行数据命令
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.DeleteData();
                });
            }
        }

        /// <summary>
        /// 无参构造
        /// </summary>
        public FailedScanArchivesViewModel()
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void LoadData()
        {
            this.LoadCustomDict();
            this.PatInfoCondition.ITEM_NAME = this.DictList[0];
            this.PatInfoCondition.SCAN_RESULT = this.ScanList[0];

            // 根据查询条件获取最新数据
            this.RefreshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (this.CheckData())
            {
                string itemName = this.PatInfoCondition.ITEM_NAME;
                string scanResult = this.PatInfoCondition.SCAN_RESULT;
                DST_PATIENT_INFO newObj = CopyObject.CopyObjectByReflection<DST_PATIENT_INFO>(this.PatInfoCondition);
                newObj.ITEM_NAME = itemName.Equals("请选择检查项目") ? "" : itemName;
                newObj.SCAN_RESULT = scanResult.Equals("请选择扫描结果") ? "" : scanResult;
                this.PatientInfoList = new ObservableCollection<DST_PATIENT_INFO>(PatientInfoDB.CreateInstance().GetPageListByCondition(newObj, this.PageModel));
            }
        }

        /// <summary>
        /// 删除勾选的数据
        /// </summary>
        protected override bool DeleteData()
        {
            // 判断是否有勾选
            if (this.PatientInfoList.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("请先勾选数据行！", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                this.ShowMessageBox("确认删除数据？", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question,
                (r =>
                {
                    if (r == MessageBoxResult.Yes || r == MessageBoxResult.OK)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            PatientInfoDB.CreateInstance().Delete(this.PatientInfoList.Where(x => x.IsSelected).ToList());
                            this.RefreshData();
                            this.SelectAll = false;
                        }
                        finally
                        {
                            WhirlingControlManager.CloseWaitingForm();
                        }
                    }
                }));
            }

            return true;
        }

        /// <summary>
        /// 检查数据是否正确
        /// </summary>
        protected override bool CheckData()
        {
            bool result = base.CheckData();
            if (this.PatInfoCondition.MinAge.HasValue && this.PatInfoCondition.MaxAge.HasValue &&
                this.PatInfoCondition.MinAge.Value > this.PatInfoCondition.MaxAge.Value)
            {
                result = false;
                this.ShowMessageBox("年龄范围错误，最小值和最大值错误！");
            }

            return result;
        }

        /// <summary>
        /// 重置查询条件
        /// </summary>
        private void ResetPatInfoCondition()
        {
            this.SelectAll = false;
            this.PatInfoCondition = null;
            this.PatInfoCondition = new DST_PATIENT_INFO() { ITEM_NAME = this.DictList[0], SCAN_RESULT = this.ScanList[0] };
            this.PageModel.PageIndex = 1;
            this.RefreshData();
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
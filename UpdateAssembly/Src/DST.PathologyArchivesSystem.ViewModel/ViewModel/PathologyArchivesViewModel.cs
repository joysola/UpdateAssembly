using DST.Common.Helper;
using DST.Common.Logger;
using DST.Common.Model;
using DST.Controls;
using DST.Database;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class PathologyArchivesViewModel : CustomBaseViewModel
    {
        private DST_PATIENT_INFO _selectedItem;
        private DST_PATIENT_INFO patInfoCondition = new DST_PATIENT_INFO();
        private ObservableCollection<DST_PATIENT_INFO> patientInfoList = new ObservableCollection<DST_PATIENT_INFO>();

        /// <summary>
        /// 查询条件对象
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
        /// 患者信息列表
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
        /// 选中行
        /// </summary>
        public DST_PATIENT_INFO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
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
                    this.PatInfoCondition = null;
                    this.PatInfoCondition = new DST_PATIENT_INFO() { ITEM_NAME = this.DictList[0], SCAN_RESULT = this.ScanList[0] };
                    this.PageModel.PageIndex = 1;
                    this.RefreshData();
                });
            }
        }

        /// <summary>
        /// 查看病例命令
        /// </summary>
        public ICommand ViewCommand
        {
            get
            {
                return new RelayCommand<DST_PATIENT_INFO>(data =>
                {
                    if (data != null && !string.IsNullOrEmpty(data.SAMPLE_IMAGE_PATH))
                    {
                        // 发送图片路径
                        Messenger.Default.Send<string>(data.SAMPLE_IMAGE_PATH, EnumMessageKey.ViewFilePath);
                    }
                    else
                    {
                        this.ShowMessageBox("当前患者未能匹配到样本图片！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    }
                });
            }
        }

        /// <summary>
        /// 匹配患者和样本图片命令
        /// </summary>
        public ICommand MatchSample
        {
            get
            {
                return new RelayCommand<string>(path =>
                {
                    this.MatchSampleImagePath(path);
                });
            }
        }

        /// <summary>
        /// 病理档案查看
        /// </summary>
        public PathologyArchivesViewModel()
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void LoadData()
        {
            base.LoadData();
            this.LoadCustomDict();
            this.PatInfoCondition.ITEM_NAME = this.DictList[0];
            this.PatInfoCondition.SCAN_RESULT = this.ScanList[0];
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
        /// 匹配患者信息和样本图片路径
        /// </summary>
        /// <param name="path">路径</param>
        private void MatchSampleImagePath(string path)
        {
            DirectoryInfo[] dics = new DirectoryInfo(path).GetDirectories();
            List<DST_PATIENT_INFO> tmpList = PatientInfoDB.CreateInstance().GetList().
                                                            Where(x => string.IsNullOrEmpty(x.SAMPLE_IMAGE_PATH)).ToList();
            if (tmpList.Count > 0)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    tmpList.ForEach(x =>
                    {
                        dics.ToList().ForEach(p =>
                        {
                            if (x.SLIDE_ID.Equals(p.Name))
                            {
                                x.SAMPLE_IMAGE_PATH = p.FullName;
                                x.SCAN_RESULT = "已扫描";
                            }
                        });
                    });

                    PatientInfoDB.CreateInstance().Save(tmpList);
                    this.RefreshData();
                }
                catch (Exception e)
                {
                    Logger.Error("匹配样本图片失败！" + e.Message);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
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
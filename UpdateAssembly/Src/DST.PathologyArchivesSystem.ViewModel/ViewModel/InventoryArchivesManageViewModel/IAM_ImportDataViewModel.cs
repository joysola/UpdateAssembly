using DST.Common.Helper.ExcelHelper;
using DST.Common.Logger;
using DST.Common.Model;
using DST.Controls;
using DST.Database;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static DST.Common.Helper.ExcelHelper.ExcelHelper;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class IAM_ImportDataViewModel : CustomBaseViewModel
    {
        private ICommand _saveCommand;
        private ICommand _cancelCommand;
        private string _excelPath;
        private DateTime? scanDatetime = DateTime.Now;
        private List<DST_PATIENT_INFO> patientInfoList = new List<DST_PATIENT_INFO>(); // 导入的患者信息

        public IAM_ImportDataViewModel()
        {
            this.RegisterCommand();
        }

        /// <summary>
        /// 外送扫描日期
        /// </summary>
        public DateTime? ScanDatetime
        {
            get { return scanDatetime; }
            set
            {
                scanDatetime = value;
                RaisePropertyChanged("ScanDatetime");
            }
        }

        /// <summary>
        /// 切片信息
        /// </summary>
        public List<ExcelSlideInfo> SlideInfos { get; set; }

        /// <summary>
        /// 读取到的Excel路径
        /// </summary>
        public string ExcelPath
        {
            get { return _excelPath; }
            set
            {
                _excelPath = value;
                RaisePropertyChanged("ExcelPath");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveCommand
        {
            get { return _saveCommand; }
            set { _saveCommand = value; }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value; }
        }

        private void RegisterCommand()
        {
            SaveCommand = new RelayCommand(async () =>
            {
                if (this.ScanDatetime == null || this.ScanDatetime.Value == DateTime.MinValue)
                {
                    this.ShowMessageBox("请确认制片日期信息！");
                    return;
                }
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var now = new DateTime(this.ScanDatetime.Value.Year, this.ScanDatetime.Value.Month, this.ScanDatetime.Value.Day, this.ScanDatetime.Value.Hour, this.ScanDatetime.Value.Second, 0);
                    SlideInfos.ForEach(s =>
                    {
                        if (s.Make_Time == DateTime.MinValue)
                        {
                            s.Make_Time = now;
                        }
                        if (s.Test_Item.ToUpper().Contains("TCT")) // TCT是1
                        {
                            s.Test_Item = "1";
                        }
                    });
                    var result = true;//await InventoryArchivesManageService.Client.PutSildesinStore(SlideInfos); // excel玻片入库
                    if (result)
                    {
                        ShowMessageBox("数据导入成功！", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.CloseContentWindow();
                    }
                    else
                    {
                        ShowMessageBox("数据导入失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            });

            CancelCommand = new RelayCommand(() =>
            {
                this.CloseContentWindow();
            });
        }

        /// <summary>
        /// 导入验证excel文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool ImportExcel(string path)
        {
            this.ExcelPath = path;
            var result = false;
            ResultModelList<ExcelSlideInfo> resultModel = null;
            resultModel = ExcelHelper.ReadExcelFile<ExcelSlideInfo>(path); // 读取excel
            if (string.IsNullOrEmpty(resultModel.ErrorMessage)) // 成功
            {
                result = true;
                this.SlideInfos = resultModel.Result;
            }
            else
            {
                Logger.Info($"导入excel文件出现问题，原因:{resultModel.ErrorMessage}");
                this.ShowMessageBox($"{resultModel.ErrorMessage}", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return result;
        }

        #region 弃用方法

        /// <summary>
        /// 检验导入的excel信息
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public bool ExamExcelData(DataTable dataTable)
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var examData = this.ImportExcelData(dataTable);
                if (examData != null)
                {
                    this.patientInfoList = examData;
                }
                return examData != null;
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        /// <summary>
        /// 根据读到的excel的DataTable数据转换成对应实体
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>返回null 表示导入失败</returns>
        private List<DST_PATIENT_INFO> ImportExcelData(DataTable dataTable)
        {
            var patInfoList = new List<DST_PATIENT_INFO>();
            var title = dataTable.Rows[0]; // 第一行对应标题
            // 检验excel的标题是否正确
            if (title[1].ToString().Trim().Equals("病历号") &&
                title[2].ToString().Trim().Equals("检查项目") &&
                title[3].ToString().Trim().Equals("患者姓名") &&
                title[4].ToString().Trim().Equals("年龄") &&
                title[7].ToString().Trim().Equals("取样日期"))
            {
                // 第0行标题
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    var row = dataTable.Rows[i];

                    var slideID = row[1].ToString().Trim();
                    var itemName = row[2].ToString().Trim();
                    var name = row[3].ToString().Trim();
                    var age = row[4].ToString().Trim();
                    var samplingDate = row[7].ToString().Trim();
                    // 检验必填项
                    if (!string.IsNullOrWhiteSpace(slideID) &&
                        !string.IsNullOrWhiteSpace(itemName) &&
                        !string.IsNullOrWhiteSpace(name) &&
                        !string.IsNullOrWhiteSpace(age) &&
                        !string.IsNullOrWhiteSpace(row[7].ToString()))
                    {
                        try
                        {
                            var patInfo = new DST_PATIENT_INFO
                            {
                                SLIDE_ID = slideID,
                                ITEM_NAME = itemName,
                                NAME = name,
                                AGE = Convert.ToInt32(age),
                                SAMPLING_DATE = Convert.ToDateTime(samplingDate),
                                SCAN_RESULT = "未扫描"
                            };
                            patInfoList.Add(patInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("导入excel文件，生成实体时出错！", ex);
                            ShowMessageBox($"患者：{name} 信息填写错误！", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                    }
                    else // 缺少必填项目
                    {
                        // 判断当前行的数据是否为空
                        if (string.IsNullOrEmpty(slideID) &&
                            string.IsNullOrEmpty(itemName) &&
                            string.IsNullOrEmpty(name) &&
                            string.IsNullOrEmpty(age) &&
                            string.IsNullOrEmpty(samplingDate))
                        {
                        }
                        else
                        {
                            ShowMessageBox($"样本：{samplingDate} 的数据不完整，请补充！", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                    }
                }
            }
            else
            {
                //ConfirmMessageBox.Show("系统提示", "Excel文件格式不正确！", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowMessageBox("Excel文件格式不正确！", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return patInfoList;
        }

        /// <summary>
        /// 工具病历号获取需要更新的数据
        /// </summary>
        /// <param name="excelData"></param>
        /// <returns></returns>
        private List<DST_PATIENT_INFO> GetUpdatePatInfoList(List<DST_PATIENT_INFO> excelData)
        {
            string sql = "select * from DST_PATIENT_INFO where ";
            StringBuilder sb = new StringBuilder();
            sb.Append(sql);

            for (int i = 0; i < excelData.Count; i++)
            {
                if (i < excelData.Count - 1)
                {
                    sb.Append("SLIDE_ID = '");
                    sb.Append(excelData[i].SLIDE_ID);
                    sb.Append("'");
                    sb.Append(" or ");
                }
                else
                {
                    sb.Append("SLIDE_ID = '");
                    sb.Append(excelData[i].SLIDE_ID);
                    sb.Append("'");
                }
            }
            sql = sb.ToString();
            var oldData = PatientInfoDB.CreateInstance().Query(sql); // 查询已经存在的数据
            var result = excelData.Where(e => !oldData.Exists(o => e.SLIDE_ID == o.SLIDE_ID)).ToList(); // 数据库理已经存在的不需要更新
            return result;
        }

        #endregion 弃用方法
    }
}
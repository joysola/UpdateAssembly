using DST.ApiClient.Service;
using DST.Common.Logger;
using DST.Common.MinioHelper;
using DST.Common.Model;
using DST.Common.ZipHelper;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class InventoryArchivesManageViewModel : CustomBaseViewModel
    {
        private string BaseReportPath = ConfigurationManager.AppSettings["PdfPath"];
        private bool selectAll = false;
        private int _cbPaginationStr;
        private QueryMBPSampleList curQueryMBPSampleList = new QueryMBPSampleList();
        private ObservableCollection<MBPSampleModel> sampleModelList = new ObservableCollection<MBPSampleModel>();

        /// <summary>
        /// 查询实例
        /// </summary>
        public QueryMBPSampleList CurQueryMBPSampleList
        {
            get { return this.curQueryMBPSampleList; }
            set
            {
                this.curQueryMBPSampleList = value;
                this.RaisePropertyChanged("CurQueryMBPSampleList");
            }
        }

        /// <summary>
        /// 患者列表
        /// </summary>

        public ObservableCollection<MBPSampleModel> SampleModelList
        {
            get { return this.sampleModelList; }
            set
            {
                this.sampleModelList = value;
                this.RaisePropertyChanged("SampleModelList");
            }
        }

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
                this.SampleModelList.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        /// <summary>
        /// 更新分页下拉选中项
        /// </summary>
        public int CbPaginationStr
        {
            get { return _cbPaginationStr; }
            set
            {
                _cbPaginationStr = value;
                this.RaisePropertyChanged("CbPaginationStr");
            }
        }

        /// <summary>
        /// 检验项目字典
        /// </summary>
        public List<ProductModel> ProductDict { get; set; } = new List<ProductModel>();

        /// <summary>
        /// 检查项目状态状态字典
        /// </summary>
        public List<DictItem> CheckProjectStatusDict { get; set; } = new List<DictItem>();

        #region 命令

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand QueryCommand { get; set; }

        /// <summary>
        /// 重置命令
        /// </summary>
        public ICommand ResetCommand { get; set; }

        /// <summary>
        /// 编辑患者信息命令
        /// </summary>
        public ICommand EditRecordCommand { get; set; }

        /// <summary>
        /// 预览报告
        /// </summary>
        public ICommand PreviewReportCommand { get; set; }

        /// <summary>
        /// 打印报告命令
        /// </summary>
        public ICommand PrintReportCommand { get; set; }

        /// <summary>
        /// 删除患者记录命令
        /// </summary>
        public ICommand DeleteRecordCommand { get; set; }

        /// <summary>
        /// 下载报告文件
        /// </summary>
        public ICommand DownloadReportCommand { get; set; }

        /// </summary>
        /// 新增
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// 改变性别
        /// </summary>
        public ICommand ChangeSexCommand { get; set; }

        #endregion 命令

        /// <summary>
        /// 无参构造
        /// </summary>
        public InventoryArchivesManageViewModel()
        {
            PageModel.PageSize = 30;
            this.InitDict();
            this.RegisterCommand();
            this.RefreshData();
            System.IO.Directory.CreateDirectory(this.BaseReportPath);
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            this.QueryCommand = new RelayCommand(() =>
            {
                // 调整日期格式
                if (CurQueryMBPSampleList.gatherTimeStart.HasValue)
                {
                    curQueryMBPSampleList.gatherTimeStart = Convert.ToDateTime(curQueryMBPSampleList.gatherTimeStart?.ToString("yyyy-MM-dd"));
                }
                if (curQueryMBPSampleList.gatherTimeEnd.HasValue)
                {
                    curQueryMBPSampleList.gatherTimeEnd = Convert.ToDateTime(curQueryMBPSampleList.gatherTimeEnd?.ToString("yyyy-MM-dd")).AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                if (CurQueryMBPSampleList.reportTimeStart.HasValue)
                {
                    curQueryMBPSampleList.reportTimeStart = Convert.ToDateTime(curQueryMBPSampleList.reportTimeStart?.ToString("yyyy-MM-dd"));
                }
                if (curQueryMBPSampleList.reportTimeEnd.HasValue)
                {
                    curQueryMBPSampleList.reportTimeEnd = Convert.ToDateTime(curQueryMBPSampleList.reportTimeEnd?.ToString("yyyy-MM-dd")).AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                //this.PageModel.PageIndex = 1;
                this.RefreshData();
            });

            // 重置搜索条件
            this.ResetCommand = new RelayCommand(() =>
            {
                this.CurQueryMBPSampleList = new QueryMBPSampleList();
            });

            // 预览报告
            this.PreviewReportCommand = new RelayCommand<MBPSampleModel>(data =>
            {
                if (data.status == "2")
                {
                    this.PreviewReportShow(data);
                }
                else
                {
                    ShowMessageBox("样本还未处理好！", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            // 编辑患者信息
            this.EditRecordCommand = new RelayCommand<MBPSampleModel>(data =>
            {
                ShowAddEditControl(data, "编辑", false);
            });

            // 打印报告命令
            this.PrintReportCommand = new RelayCommand<MBPSampleModel>(data =>
            {
                if (data.status == "2")
                {
                    Task.Run(() =>
                    {
                        this.PrintSampleReport(data);
                    });
                }
                else
                {
                    ShowMessageBox("样本还未处理好！", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            // 删除患者信息
            this.DeleteRecordCommand = new RelayCommand<MBPSampleModel>(data =>
            {
                ShowMessageBox("请问是否需要删除该数据？", MessageBoxButton.OKCancel, MessageBoxImage.Question, msg =>
                {
                    if (msg == MessageBoxResult.OK)
                    {
                        var backMBP = new BackMBPSample
                        {
                            chargeBackCause = $"C端退单调用,操作医生：{Database.ExtendAppContext.Current.LoginModel.userId}",
                            id = data.id
                        };
                        var result = MBPSampleService.Instance.BackMBPSample(backMBP);
                        if (!result)
                        {
                            ShowMessageBox("删除失败！", MessageBoxButton.OK, MessageBoxImage.Error, null, true, 3000);
                        }
                        else
                        {
                            RefreshData();
                        }
                    }
                });
            });

            // 下载报告文件
            this.DownloadReportCommand = new RelayCommand(() =>
            {
                this.DownloadReprt();
            });

            /// 接收预览报告界面，发送的打印命令
            Messenger.Default.Register<string>(this, EnumMessageKey.PrintReport, pdfPath =>
            {
                Task.Run(() =>
                {
                    this.PrintPDF(pdfPath);
                });
            });

            // 新增
            this.AddCommand = new RelayCommand(() =>
            {
                var sample = new MBPSampleModel
                {
                    patientSex = "1",
                    hospitalId = Database.ExtendApiDict.Instance.HotpitalInfo.id,
                    gatherTime = DateTime.Now // 取样时间默认今天
                };
                ShowAddEditControl(sample, "新增", true);
            });
            // 改变性别
            this.ChangeSexCommand = new RelayCommand<string>(data =>
            {
                //CurQueryMBPSampleList.patientSex = data;
            });
        }

        private void ShowAddEditControl(MBPSampleModel sampleModel, string title, bool isAdd)
        {
            var args = new object[] { sampleModel, isAdd };
            ShowContentWindowMessage msg = new ShowContentWindowMessage("IAM_AddEditSample", title);
            msg.Width = 800;
            msg.Height = 750;
            msg.Args = args;
            msg.CallBackCommand = new RelayCommand(() =>
            {
                RefreshData();
            });
            Messenger.Default.Send<ShowContentWindowMessage>(msg);
        }

        /// <summary>
        /// 下载选中的文件
        /// </summary>
        private void DownloadReprt()
        {
            if (this.SampleModelList.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("请选中要下载报告的患者！");
                return;
            }

            string zipPath = string.Empty;
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (DialogResult.OK == folderDlg.ShowDialog())
            {
                zipPath = folderDlg.SelectedPath;
            }

            if (!string.IsNullOrEmpty(zipPath))
            {
                List<string> localPathList = new List<string>();
                this.SampleModelList.Where(x => x.IsSelected).ToList().ForEach(x =>
                {
                    List<string> urlList = ReportService.Instance.GetReport(x.id, x.productId);
                    urlList.ForEach(url =>
                    {
                        localPathList.Add(this.DownloadReportPDF(url));
                    });
                });

                if (localPathList.Count == 0)
                {
                    this.ShowMessageBox("患者报告还未生成，请稍后再试！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 将文件复制到文件夹
                string zipName = DateTime.Now.ToString("yyyyMMddHHmmss");
                string tempPath = this.BaseReportPath + $"\\{zipName}";
                System.IO.Directory.CreateDirectory(tempPath);
                localPathList.ForEach(x =>
                {
                    System.IO.File.Copy(x, tempPath + $"\\{System.IO.Path.GetFileName(x)}", true);
                });
                bool res = ZipHelper.ZipDirectory(tempPath, zipPath + $"\\{zipName}.zip").Result;
                if (res)
                {
                    this.ShowMessageBox("报告下载完成！", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    this.ShowMessageBox("报告下载失败，请查看日志信息！");
                }
            }
        }

        /// <summary>
        /// 下载minio的PDF报告，返回PDF的保存路径
        /// </summary>
        /// <param name="url">minio链接</param>
        /// <returns>pdf本地路径</returns>
        private string DownloadReportPDF(string url)
        {
            int index = url.LastIndexOf(@"/");
            string localPath = this.BaseReportPath + @"\" + url.Substring(index + 1);
            bool res = MinioHelper.Client.DownloadFileByUrl(url, localPath).Result;
            return localPath;
        }

        /// <summary>
        /// 打印报告
        /// </summary>
        private void PrintSampleReport(MBPSampleModel sample)
        {
            try
            {
                List<string> urlList = ReportService.Instance.GetReport(sample.id, sample.productId);
                if (urlList.Count == 0)
                {
                    this.ShowMessageBox("患者报告还未生成，请稍后再试！", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                List<string> localPathList = new List<string>();
                urlList.ForEach(x =>
                {
                    localPathList.Add(this.DownloadReportPDF(x));
                });

                localPathList.ForEach(x =>
                {
                    this.PrintPDF(x);
                });
            }
            catch (Exception ex)
            {
                Logger.Error("打印报告异常：" + ex.Message);
                this.ShowMessageBox("打印报告异常，请查看日志信息！", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 打印PDF
        /// </summary>
        private void PrintPDF(string pdfPath)
        {
            try
            {
                if (System.IO.File.Exists(pdfPath))
                {
                    using (Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument())
                    {
                        doc.LoadFromFile(pdfPath);
                        //使用默认打印机打印文档所有页面
                        doc.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"打印 {pdfPath} 文档异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 报告预览
        /// </summary>
        private void PreviewReportShow(MBPSampleModel sample)
        {
            List<string> urlList = ReportService.Instance.GetReport(sample.id, sample.productId);
            List<string> localPathList = new List<string>();
            urlList.ForEach(x =>
            {
                localPathList.Add(this.DownloadReportPDF(x));
            });

            if (localPathList.Count > 0)
            {
                Messenger.Default.Send<List<string>>(localPathList, EnumMessageKey.ShowReportPdf);
            }
            else
            {
                this.ShowMessageBox("患者报告还未生成，请稍后预览！", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        protected override bool CheckData()
        {
            var result = true;

            return result;
        }

        /// <summary>
        /// 根据条件查询患者数据
        /// </summary>
        private void RefreshData()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (!CheckData()) // 验证失败，则不查询数据
                {
                    return;
                }

                this.SelectAll = false;
                var result = MBPSampleService.Instance.GetMBPSamples(this.PageModel, CurQueryMBPSampleList);
                //List<MBPSampleModel> tmpList = new List<MBPSampleModel>()
                //{
                //    new MBPSampleModel(){ patentNumber = "0001", hospitalId = "092020", code = "202045451", patientName= "张三", patientAge = 25, patientSex = "女", idCard = "1020202020202020", productName = "TCT/HPV", gatherTime = new DateTime(2020,12,8), reportTime =new DateTime(2020,12,9)}
                //};
                this.SampleModelList = new ObservableCollection<MBPSampleModel>(result);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
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

        private void InitDict()
        {
            // 初始化字典
            ProductDict.Add(new ProductModel { id = null, name = "请选择检验项目" });
            ProductDict.AddRange(Database.ExtendApiDict.Instance.ProductDict);
            CheckProjectStatusDict.Add(new DictItem { dictKey = null, dictValue = "请选择检查项目状态" });
            CheckProjectStatusDict.AddRange(Database.ExtendApiDict.Instance.CheckProjectStatusDict);
        }
    }
}
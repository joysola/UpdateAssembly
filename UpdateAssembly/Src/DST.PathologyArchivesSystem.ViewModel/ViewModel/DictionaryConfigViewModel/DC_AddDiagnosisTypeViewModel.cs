using DST.Common.Model;
using DST.Database;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class DC_AddDiagnosisTypeViewModel : CustomBaseViewModel
    {
        private bool isEnabled = true;
        private string newDiaType = string.Empty;
        private string selectDictClass = string.Empty;
        private DST_DICT curArgs = null;

        public DST_DICT CurArgs { get; }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                this.isEnabled = value;
                this.RaisePropertyChanged("IsEnabled");
            }
        }

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
        /// 确认保存命令
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    System.Windows.Controls.RichTextBox rtb = data as System.Windows.Controls.RichTextBox;
                    if (null != rtb)
                    {
                        TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                        this.newDiaType = textRange.Text.Replace("\r\n", "").Trim();
                        this.SaveData();
                    }
                });
            }
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.CloseContentWindow();
                });
            }
        }

        public DC_AddDiagnosisTypeViewModel()
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void LoadData()
        {
            base.LoadData();
            ExtendDict.Instance.DictList = DictDB.CreateInstance().GetList();
            IEnumerable<DST_DICT> tmpDict = ExtendDict.Instance.DictList.Where(x => x.DICT_CLASS.Equals("检查项目"));

            this.LoadCustomDict();
            this.SelectDictClass = this.DictList[0];

            // 编辑模式，只能编辑诊断信息，不能修改检查项目信息
            if (this.Args != null && this.Args.Length == 1)
            {
                this.IsEnabled = false;
                this.curArgs = this.Args[0] as DST_DICT;
                this.SelectDictClass = this.curArgs.DICT_NAME;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected override SaveResult SaveData()
        {
            SaveResult result = SaveResult.Success;
            if (this.CheckData())
            {
                if (this.curArgs == null)
                {
                    // 新增数据
                    DST_DICT newDict = ExtendDict.Instance.DictList.FirstOrDefault(x => x.DICT_CLASS.Equals("检查项目") && x.DICT_NAME.Equals(this.SelectDictClass) && string.IsNullOrEmpty(x.DICT_CODE));
                    if (newDict == null)
                    {
                        newDict = new DST_DICT() { DICT_CLASS = "检查项目", DICT_NAME = this.SelectDictClass, DICT_CODE = this.newDiaType };
                    }
                    else
                    {
                        newDict.DICT_CODE = this.newDiaType;
                    }

                    DictDB.CreateInstance().Save(newDict);
                }
                else
                {
                    // 更新数据
                    this.curArgs.DICT_CODE = this.newDiaType;
                    DictDB.CreateInstance().Save(this.curArgs);
                }
                this.CloseContentWindow();
            }

            return result;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        protected override bool CheckData()
        {
            bool result = true;
            ExtendDict.Instance.DictList = DictDB.CreateInstance().GetList();
            if (string.IsNullOrEmpty(this.SelectDictClass) || this.SelectDictClass.Equals("请选择检查项目"))
            {
                this.ShowMessageBox("请选择具体检查项目！");
                result = false;
            }
            else if (string.IsNullOrEmpty(this.newDiaType) || this.newDiaType.Equals("\r\n"))
            {
                this.ShowMessageBox("请录入诊断类型信息！");
                result = false;
            }
            else if (null != ExtendDict.Instance.DictList.FirstOrDefault(x => !string.IsNullOrEmpty(x.DICT_CLASS) && x.DICT_CLASS.Equals("检查项目") &&
                                                                              !string.IsNullOrEmpty(x.DICT_NAME) && x.DICT_NAME.Equals(this.SelectDictClass) &&
                                                                              !string.IsNullOrEmpty(x.DICT_CODE) && x.DICT_CODE.Equals(this.newDiaType)))
            {
                this.ShowMessageBox("该检查项目已包含此诊断类型！");
                result = false;
            }

            return result;
        }
    }
}
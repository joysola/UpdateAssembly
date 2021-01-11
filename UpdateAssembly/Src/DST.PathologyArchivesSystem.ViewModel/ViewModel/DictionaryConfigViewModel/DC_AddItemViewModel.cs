using DST.Common.Model;
using DST.Database;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class DC_AddItemViewModel : CustomBaseViewModel
    {
        private string newDictName = string.Empty;

        public string NewDictName
        {
            get { return this.newDictName; }
            set
            {
                this.newDictName = value.Trim();
                this.RaisePropertyChanged("NewDictName");
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
                    this.SaveData();
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

        public DC_AddItemViewModel()
        {
        }

        protected override SaveResult SaveData()
        {
            SaveResult result = SaveResult.Success;
            if (this.CheckData())
            {
                DST_DICT newDict = new DST_DICT() { DICT_CLASS = "检查项目", DICT_NAME = this.NewDictName };
                DictDB.CreateInstance().Save(newDict);
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
            if (result && string.IsNullOrEmpty(this.NewDictName))
            {
                this.ShowMessageBox("新增检查项目名称不能为空！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                result = false;
            }
            else if (result)
            {
                ExtendDict.Instance.DictList = DictDB.CreateInstance().GetList();
                if (null != ExtendDict.Instance.DictList.FirstOrDefault(x => x.DICT_CLASS.Equals("检查项目") && x.DICT_NAME.Equals(this.NewDictName)))
                {
                    this.ShowMessageBox("该检查项目已存在，请重新录入！");
                    result = false;
                }
            }

            return result;
        }
    }
}
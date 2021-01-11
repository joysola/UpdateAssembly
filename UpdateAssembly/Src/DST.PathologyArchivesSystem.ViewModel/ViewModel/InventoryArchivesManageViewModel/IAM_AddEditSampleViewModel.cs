using DST.ApiClient.Service;
using DST.Database;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using DST.Joint.Construction.Mgmt.ViewModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DST.PathologyArchivesSystem.ViewModel
{
    public class IAM_AddEditSampleViewModel : CustomBaseViewModel
    {
        private bool _isAdd;
        private MBPSampleModel _MBPSample;
        private List<ProductType> _ProductTypes = new List<ProductType>();
        public bool IsAdd { get => _isAdd; set { _isAdd = value; RaisePropertyChanged("IsAdd"); } }
        public MBPSampleModel MBPSample { get => _MBPSample; set { _MBPSample = value; RaisePropertyChanged("MBPSample"); } }
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;
        public List<ProductType> ProductTypes { get => _ProductTypes; set { _ProductTypes = value; RaisePropertyChanged("ProductTypes"); } }

        public ICommand SaveCommand { get; set; }
        public ICommand ChangeSexCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ProductTypeChangeCommand { get; set; }
        public ICommand IDCardChangeCommand { get; set; }

        public IAM_AddEditSampleViewModel()
        {
            this.RegisterCommand();
        }

        public override void OnViewLoaded()
        {
            this.MBPSample = this.Args[0] as MBPSampleModel;
            this.IsAdd = (bool)this.Args[1];
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            this.SaveCommand = new RelayCommand(() =>
            {
                if (CheckData())
                {
                    MBPSample.doctorId = ExtendAppContext.Current.LoginModel.userId; // 默认送检医生是本操作人
                    if (MBPSampleService.Instance.SaveMBPSample(MBPSample))
                    {
                        this.CloseContentWindow();
                    }
                    else
                    {
                        this.ShowMessageBox("保存失败！");
                    }
                }
            });

            this.ChangeSexCommand = new RelayCommand<string>(sex =>
            {
                MBPSample.patientSex = sex;
            });

            this.CancelCommand = new RelayCommand(() => this.CloseContentWindow());

            //this.ProductTypeChangeCommand = new RelayCommand<string>(data =>
            //{
            //    MBPSample.productType = data;
            //});

            // 输入身份证后的操作
            this.IDCardChangeCommand = new RelayCommand(() =>
            {
                if (!string.IsNullOrEmpty(MBPSample.idCard) && MBPSample.idCard.Length == 18)
                {
                    // 计算性别
                    var sexCode = MBPSample.idCard[16];
                    var sex = sexCode % 2;
                    if (sex == 1)
                    {
                        MBPSample.patientSex = "1";
                    }
                    else
                    {
                        MBPSample.patientSex = "2";
                    }
                    Messenger.Default.Send(MBPSample.patientSex, EnumMessageKey.IDCardChange);
                    // 计算年龄
                    var yearCode = MBPSample.idCard.Substring(6, 8);
                    DateTime.TryParseExact(yearCode, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime year);
                    if (year != DateTime.MinValue)
                    {
                        MBPSample.patientAge = GetAgeByBirthdate(year);
                    }
                }
                else
                {
                    MBPSample.patientAge = null;
                }
            });
        }

        /// <summary>
        /// 年龄算法
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        private int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        protected override bool CheckData()
        {
            var result = true;
            string msg = string.Empty;
            //if (string.IsNullOrEmpty(MBPSample.barCode))
            //{
            //    result = false;
            //    msg += "请填写条码号！";
            //}
            if (!MBPSample.gatherTime.HasValue)
            {
                result = false;
                msg += "请填写取样时间！";
            }
            //if (string.IsNullOrEmpty(MBPSample.idCard) || MBPSample.idCard.Length != 18)
            //{
            //    result = false;
            //    msg += "请填写正确的身份证号码！";
            //}
            if (string.IsNullOrEmpty(MBPSample.patientName))
            {
                result = false;
                msg += "请填写患者姓名！";
            }
            if (!MBPSample.patientAge.HasValue)
            {
                result = false;
                msg += "请填写年龄！";
            }
            if (string.IsNullOrEmpty(MBPSample.patientSex))
            {
                result = false;
                msg += "请填写患者性别！";
            }
            if (string.IsNullOrEmpty(MBPSample.productId))
            {
                result = false;
                msg += "请填写检验项目！";
            }
            else
            {
                // 存在类型，且没有选类型时
                var product = ExtendApiDict.Instance.ProductDict.FirstOrDefault(x => x.id == MBPSample.productId);
                if (product != null && product.productTypes != null && string.IsNullOrEmpty(MBPSample.productType))
                {
                    result = false;
                    msg += "请填写检验类型！";
                }
            }
            if (!result)
            {
                ShowMessageBox(msg, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return result;
        }
    }
}
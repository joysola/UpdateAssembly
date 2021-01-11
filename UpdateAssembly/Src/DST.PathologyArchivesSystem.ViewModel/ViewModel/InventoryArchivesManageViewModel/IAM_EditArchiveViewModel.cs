using DST.Database;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class IAM_EditArchiveViewModel : CustomBaseViewModel
    {
        private DST_PATIENT_INFO _patInfo;

        public DST_PATIENT_INFO PatientInfo
        {
            get { return _patInfo; }
            set
            {
                _patInfo = value;
                RaisePropertyChanged("PatientInfo");
            }
        }

        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand SaveCommand { get; set; }

        public IAM_EditArchiveViewModel()
        {
            this.RegisterCommand();
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
        }

        private void RegisterCommand()
        {
            SaveCommand = new RelayCommand(() =>
            {
                var result = PatientInfoDB.CreateInstance().Update(PatientInfo);
                if (result)
                {
                    this.CloseContentWindow();
                }
                else
                {
                    ShowMessageBox("保存信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public override void OnViewLoaded()
        {
            //base.OnViewLoaded();
            PatientInfo = (DST_PATIENT_INFO)this.Args[0];
        }
    }
}
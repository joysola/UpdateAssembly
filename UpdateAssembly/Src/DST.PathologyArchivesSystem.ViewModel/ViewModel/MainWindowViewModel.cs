namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class MainWindowViewModel : CustomBaseViewModel
    {
        private string _filePath;
        /// <summary>
        /// 病例切片图片的路径
        /// </summary>

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        public MainWindowViewModel()
        {
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
        }
    }
}
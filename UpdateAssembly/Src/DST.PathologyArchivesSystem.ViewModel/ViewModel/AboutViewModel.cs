namespace DST.Joint.Construction.Mgmt.ViewModel
{
    public class AboutViewModel : CustomBaseViewModel
    {
        private string name;

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public AboutViewModel()
        {
            this.Result = "AboutViewModel";
        }

        public override void LoadData()
        {
            base.LoadData();
        }
    }
}
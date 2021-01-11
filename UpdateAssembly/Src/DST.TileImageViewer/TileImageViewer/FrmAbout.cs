using System;
using System.Windows.Forms;

namespace TileImageViewer
{
    /// <summary>
    /// 关于窗体
    /// </summary>
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
            this.Text = GlobalData.GlobalLanguage.Text("App_Name");
            this.lblLeft1.Text = GlobalData.GlobalLanguage.Text("App_Name");
            this.lblLeft3.Text = $"CopyRight © 2018 - {DateTime.Now.Year} DeepSight Ltd.";
            this.lblLeft4.Text = GlobalData.GlobalLanguage.Text("version") + " : " + System.Windows.Forms.Application.ProductVersion.ToString();
        }
    }
}
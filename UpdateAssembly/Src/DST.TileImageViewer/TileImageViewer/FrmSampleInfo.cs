using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TileImageViewer.Forms;

namespace TileImageViewer
{
    /// <summary>
    /// 样本信息窗体
    /// </summary>
    public partial class FrmSampleInfo : FrmWithTitle
    {
        private string baseFilePath = "";
        private string sectionName = "LayerMag";

        public FrmSampleInfo(String baseFilePath)
        {
            InitializeComponent();
            this.baseFilePath = baseFilePath;
            this.loadInitData();
        }

        private void loadInitData()
        {
            String file = this.baseFilePath + "\\Slide.dat";
            if (File.Exists(file))
            {
                StringBuilder sc = new StringBuilder();
                List<string> sectionArr = FileUtils.ReadSections(file);
                StringBuilder scValue = new StringBuilder();
                foreach (string sectionValue in sectionArr)
                {
                    this.listBox1.Items.Add(sectionValue);

                    List<string> keyArr = FileUtils.ReadKeys(sectionValue, file);

                    {
                        foreach (string key in keyArr)
                        {
                            scValue = new StringBuilder();
                            FileUtils.GetPrivateProfileString(sectionValue, key, "0", scValue, 255, file);
                            string lngKey = GlobalData.GlobalLanguage.Text(key);
                            //非必要的信息不显示
                            if (!lngKey.Equals(key))
                            {
                                string rowValue = "  " + lngKey + "：" + "\t" + scValue.ToString();
                                this.listBox1.Items.Add(rowValue);
                            }
                        }
                    }
                }
            }
        }
    }
}
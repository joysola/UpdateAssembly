using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileImageViewer.Controls;
using TileImageViewer.Model;

namespace TileImageViewer
{
    /// <summary>
    /// 设置窗体
    /// </summary>
    public partial class FrmSettings : Form
    {
        private Setting st = new Setting();
        private ImgCtrl imgCtrl;
        private FrmMain _frmMain;

        public FrmSettings(FrmMain frmMain, ImgCtrl imgCtrl)
        {
            InitializeComponent();
            //SettingManager smObj = SettingManager.getInstance();
            //st = smObj.getSettingDetail();
            _frmMain = frmMain;
            this.imgCtrl = imgCtrl;
            st = Constants.SettingDetail;
            BindLangComboBox();
            this.loadData();
            tabControl1.ItemSize = new Size(0, 1);
            SetLanguage();
        }

        /// <summary>
        /// 语言选择控件
        /// </summary>
        private void BindLangComboBox()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("id");
            DataColumn dc2 = new DataColumn("name");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);

            DataRow dr1 = dt.NewRow();
            dr1["id"] = "English";
            dr1["name"] = "English";

            DataRow dr2 = dt.NewRow();
            dr2["id"] = "Chinese";
            dr2["name"] = "中文";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);

            cbLanguage.DataSource = dt;
            cbLanguage.ValueMember = "id";
            cbLanguage.DisplayMember = "name";
        }

        /// <summary>
        /// 设置多语言
        /// </summary>
        private void SetLanguage()
        {
            this.label11.Text = GlobalData.GlobalLanguage.Text("Enable_smooth_navigation");
            this.label9.Text = GlobalData.GlobalLanguage.Text("Label_reverse");
            this.label8.Text = GlobalData.GlobalLanguage.Text("Enable_zoom_limit");
            this.label6.Text = GlobalData.GlobalLanguage.Text("enlarge");
            this.radioButton2.Text = GlobalData.GlobalLanguage.Text("absolutely");
            this.radioButton1.Text = GlobalData.GlobalLanguage.Text("relevant");
            this.label17.Text = GlobalData.GlobalLanguage.Text("the_measure_of_area");
            this.label18.Text = GlobalData.GlobalLanguage.Text("diameter");
            this.label19.Text = GlobalData.GlobalLanguage.Text("Language");

            this.label16.Text = GlobalData.GlobalLanguage.Text("Fixed_circle");
            this.label15.Text = GlobalData.GlobalLanguage.Text("the_measure_of_area");
            this.label14.Text = GlobalData.GlobalLanguage.Text("height");
            this.label13.Text = GlobalData.GlobalLanguage.Text("width");
            this.label12.Text = GlobalData.GlobalLanguage.Text("Fixed_rectangle");
            this.label33.Text = GlobalData.GlobalLanguage.Text("fluorescence");
            this.label32.Text = GlobalData.GlobalLanguage.Text("Mingchang");
            this.label31.Text = GlobalData.GlobalLanguage.Text("Default_color");
            this.label30.Text = GlobalData.GlobalLanguage.Text("Comment_name");
            this.label29.Text = GlobalData.GlobalLanguage.Text("Slice_name");
            this.label28.Text = GlobalData.GlobalLanguage.Text("Set_font_size");
            this.label27.Text = GlobalData.GlobalLanguage.Text("Enter_preset");
            this.label26.Text = GlobalData.GlobalLanguage.Text("Show_comment_size");
            this.label25.Text = GlobalData.GlobalLanguage.Text("auto_number");
            this.label24.Text = GlobalData.GlobalLanguage.Text("Automatic_naming");
            this.label23.Text = GlobalData.GlobalLanguage.Text("Display_name");
            this.label10.Text = GlobalData.GlobalLanguage.Text("Reverse_mouse_zoom");
            this.label7.Text = GlobalData.GlobalLanguage.Text("Use_memory_as_cache");
            this.label5.Text = GlobalData.GlobalLanguage.Text("Service_mode");
            this.label1.Text = GlobalData.GlobalLanguage.Text("debug_information");
            this.Text = GlobalData.GlobalLanguage.Text("browse");

            this.magSetting.TipsText = GlobalData.GlobalLanguage.Text("browse");
            this.seniorSetting.TipsText = GlobalData.GlobalLanguage.Text("advanced_setting"); ;
            this.saveSetting.TipsText = GlobalData.GlobalLanguage.Text("save");

            this.annotationSetting.TipsText = GlobalData.GlobalLanguage.Text("Note_requirements");
            this.textSetting.TipsText = GlobalData.GlobalLanguage.Text("Note_size");
            this.ucBtnExt2.BtnText = GlobalData.GlobalLanguage.Text("Reset");
        }

        /// <summary>
        /// 根据设置值设置各控件初始化状态
        /// </summary>
        private void loadData()
        {
            if (st == null)
            {
                return;
            }
            // 浏览Tab
            this.radioButton1.Checked = st.EnlargeSelect == 1 ? true : false;
            this.radioButton2.Checked = st.EnlargeSelect == 0 ? true : false;
            this.relationTxt.Text = st.EnlargeRelationVlaue + "x";
            this.relationBar.Value = st.EnlargeRelationVlaue;
            this.absoluteTxt.Text = "x" + st.EnlargeAbsoluteVlaue;
            this.absoluteBar.Value = st.EnlargeAbsoluteVlaue;
            this.ucSwitch1.Checked = st.ApplyZoomLimitSwitch == 1 ? true : false;
            this.ucSwitch2.Checked = st.SmoothSlideNavigationSwitch == 1 ? true : false;
            this.ucSwitch3.Checked = st.LabelOrientationSwitch == 1 ? true : false;
            //this.ucSwitch4.Checked = st.ScaleBarSwitch == 1 ? true : false;
            //this.ucBtnExt1.FillColor = ColorTranslator.FromHtml(st.ScaleBarColor);

            // 注释尺寸Tab
            this.ucNumTextBox1.Num = st.RectAngleWidth.ToDecimal();
            this.ucNumTextBox2.Num = st.RectAngleHeight.ToDecimal();
            //st.RectAngleUnit = 1;
            this.ucNumTextBox3.Num = st.CircularRadius.ToDecimal();
            //st.CircularUnit = 1;
            //this.textBox3.Text = st.CircularTwentyRadius.ToString();
            //this.trackBar1.Value = st.CircularTwentyRadius.ToInt();
            //this.textBox4.Text = st.CircularFortyRadius.ToString();
            //this.trackBar2.Value = st.CircularFortyRadius.ToInt();

            // 注释属性Tab
            this.ucSwitch5.Checked = st.ShowNameSwitch == 1 ? true : false;
            this.ucSwitch6.Checked = st.AutoNameSwitch == 1 ? true : false;
            this.ucSwitch7.Checked = st.AutoNumSwitch == 1 ? true : false;
            this.textBox5.Text = st.PrefixStr;
            //st.TMAPrefixStr = "TMA";
            this.ucSwitch8.Checked = st.ShowAnnotationSizeSwitch == 1 ? true : false;
            this.ucNumTextBox4.Num = st.SlicesFontSize.ToDecimal();
            this.ucNumTextBox5.Num = st.AnnotationLabelSize.ToDecimal();
            this.ucBtnExt14.FillColor = ColorTranslator.FromHtml(st.BrightfieldColor);
            this.ucBtnExt15.FillColor = ColorTranslator.FromHtml(st.FluorescentColor);

            // 高级设置Tab
            this.debugInfoSwitch.Checked = st.ShowDebugInfoSwitch == 1 ? true : false;
            this.memCachedSwitch.Checked = st.MemCachedSwitch == 1 ? true : false;
            this.reverseMouseSwitch.Checked = st.ReverseMouseSwitch == 1 ? true : false;
            this.cbLanguage.SelectedValue = st.Language;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void relationBar_Scroll(object sender, EventArgs e)
        {
            relationTxt.Text = relationBar.Value.ToString() + "x";
        }

        private void absoluteBar_Scroll(object sender, EventArgs e)
        {
            absoluteTxt.Text = "x" + absoluteBar.Value.ToString();
        }

        //private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        //{
        //    if (colorDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        //获取所选择的颜色
        //        Color colorChoosed = colorDialog1.Color;
        //        //改变panel的背景色
        //        ucBtnExt1.FillColor = colorChoosed;
        //    }

        //}

        private void ucBtnExt2_BtnClick(object sender, EventArgs e)
        {
            relationBar.Value = 2;
            absoluteBar.Value = 1;
            relationTxt.Text = "x2";
            absoluteTxt.Text = "1x";
        }

        private void ucBtnExt3_BtnClick(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
            this.Text = "浏览";
            this.setBtnActive(this.magSetting);
        }

        private void ucBtnExt4_BtnClick(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
            this.Text = "注释尺寸";
            this.setBtnActive(this.textSetting);
        }

        private void ucBtnExt5_BtnClick(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 2;
            this.Text = "注释属性";
            this.setBtnActive(this.annotationSetting);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.White, 3);
            pen.DashStyle = DashStyle.Solid;
            e.Graphics.DrawRectangle(pen, panel11.DisplayRectangle);
        }

        /**
         * 固定矩形面积
         * */

        private void ucNumTextBox1_NumChanged(object sender, EventArgs e)
        {
            textBox1.Text = Math.Round(ucNumTextBox1.Num * ucNumTextBox2.Num, 2).ToString();
        }

        /**
         * 固定矩形面积
         * */

        private void ucNumTextBox2_NumChanged(object sender, EventArgs e)
        {
        }

        /**
         * 固定圆形面积
         * */

        private void ucNumTextBox3_NumChanged(object sender, EventArgs e)
        {
            // πr²
            textBox2.Text = Math.Round(Math.Pow(ucNumTextBox3.Num.ToDouble() / 2, 2.00) * Math.PI, 2).ToString();
        }

        /**
         * 切片名称字体大小
         * */

        private void ucNumTextBox4_NumChanged(object sender, EventArgs e)
        {
            Font _btnFont = new System.Drawing.Font("宋体", float.Parse(ucNumTextBox4.Num.ToString()), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            ucBtnExt12.BtnFont = _btnFont;
        }

        /**
         * 注释名称字体大小
         * */

        private void ucNumTextBox5_NumChanged(object sender, EventArgs e)
        {
            Font _btnFont = new System.Drawing.Font("宋体", float.Parse(ucNumTextBox5.Num.ToString()), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            ucBtnExt13.BtnFont = _btnFont;
        }

        private void ucBtnExt14_BtnClick(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                //获取所选择的颜色
                Color colorChoosed = colorDialog2.Color;
                //改变panel的背景色
                ucBtnExt14.FillColor = colorChoosed;
            }
        }

        private void ucBtnExt15_BtnClick(object sender, EventArgs e)
        {
            if (colorDialog3.ShowDialog() == DialogResult.OK)
            {
                //获取所选择的颜色
                Color colorChoosed = colorDialog3.Color;
                //改变panel的背景色
                ucBtnExt15.FillColor = colorChoosed;
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }

        private void ucBtnExt6_BtnClick(object sender, EventArgs e)
        {
            this.setBtnActive(this.saveSetting);
            this.BeginInvoke((Action)(async () =>
            {
                //代码1
                SettingManager sm = new SettingManager();
                Setting obj = new Setting();
                if (st != null && st.Id > 0)
                {
                    obj.Id = st.Id;
                }

                // 浏览Tab
                obj.EnlargeSelect = radioButton1.Checked ? 1 : 0;
                obj.EnlargeRelationVlaue = relationTxt.Text.Replace("x", "").ToInt();
                obj.EnlargeAbsoluteVlaue = absoluteTxt.Text.Replace("x", "").ToInt();
                obj.ApplyZoomLimitSwitch = ucSwitch1.Checked ? 1 : 0;

                obj.SmoothSlideNavigationSwitch = ucSwitch2.Checked ? 1 : 0;
                obj.LabelOrientationSwitch = ucSwitch3.Checked ? 1 : 0;
                //obj.ScaleBarSwitch = ucSwitch4.Checked ? 1 : 0;
                //obj.ScaleBarColor = ColorTranslator.ToHtml(ucBtnExt1.FillColor);

                obj.ScaleBarSwitch = 1;
                obj.ScaleBarColor = "Yellow";

                // 注释尺寸Tab
                obj.RectAngleWidth = ucNumTextBox1.Num;
                obj.RectAngleHeight = ucNumTextBox2.Num;
                obj.RectAngleUnit = 1;
                obj.CircularRadius = ucNumTextBox3.Num;
                obj.CircularUnit = 1;
                //obj.CircularTwentyRadius = int.Parse(textBox3.Text);
                //obj.CircularFortyRadius = int.Parse(textBox4.Text);
                obj.CircularTwentyRadius = 2.50m;
                obj.CircularFortyRadius = 2.50m;

                // 注释属性Tab
                obj.ShowNameSwitch = ucSwitch5.Checked ? 1 : 0;
                obj.AutoNameSwitch = ucSwitch6.Checked ? 1 : 0;
                obj.AutoNumSwitch = ucSwitch7.Checked ? 1 : 0;
                obj.PrefixStr = textBox5.Text;
                obj.TMAPrefixStr = "TMA";
                obj.ShowAnnotationSizeSwitch = ucSwitch8.Checked ? 1 : 0;
                obj.SlicesFontSize = ucNumTextBox4.Num.ToInt();
                obj.AnnotationLabelSize = ucNumTextBox5.Num.ToInt();
                obj.BrightfieldColor = ColorTranslator.ToHtml(ucBtnExt14.FillColor);
                obj.FluorescentColor = ColorTranslator.ToHtml(ucBtnExt15.FillColor);

                // 高级设置Tab
                obj.ShowDebugInfoSwitch = debugInfoSwitch.Checked ? 1 : 0;
                obj.MemCachedSwitch = memCachedSwitch.Checked ? 1 : 0;
                obj.ReverseMouseSwitch = reverseMouseSwitch.Checked ? 1 : 0;
                obj.Language = cbLanguage.SelectedValue.ToString();

                await Task.Run(() =>
                {
                    sm.UpdateToDb(obj);
                });

                _frmMain.setBtnActive(_frmMain.tsbSettingsBtn, false);
                Constants.LoadSettingInfo();
                this.Close();
            }));
            //Task.Run(() =>
            //{
            //}).ContinueWith<int>((t) =>
            //{
            //    //Thread.Sleep(100);
            //    return 1;
            //});
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void seniorSetting_BtnClick(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 3;
            this.Text = "高级设置";
            this.setBtnActive(this.seniorSetting);
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void FrmSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frmMain.setBtnActive(_frmMain.tsbSettingsBtn, false);
        }

        /// <summary>
        /// 设置按钮的选中样式
        /// </summary>
        /// <param name="btn">按钮</param>
        public void setBtnActive(UCBtnExt btn)
        {
            string btnName = btn.Name;
            switch (btnName)
            {
                case "magSetting":
                    btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_browser_press;
                    this.textSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_text;
                    this.annotationSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_annotation;
                    this.seniorSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_senior_setting;
                    break;

                case "textSetting":
                    btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_text_press;
                    this.magSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_browser;
                    this.annotationSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_annotation;
                    this.seniorSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_senior_setting;
                    break;

                case "annotationSetting":
                    btn.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_annotation_press;
                    this.magSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_browser;
                    this.textSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_text;
                    this.seniorSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_senior_setting;
                    break;

                case "seniorSetting":
                    this.seniorSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_senior_setting_press;
                    this.magSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_browser;
                    this.textSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_text;
                    this.annotationSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_annotation;
                    break;

                case "saveSetting":
                    this.saveSetting.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_setting_save_press;
                    break;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
        }
    }
}
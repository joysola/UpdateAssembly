using System;
using System.Drawing;
using System.Windows.Forms;
using TileImageViewer.Model;

namespace TileImageViewer
{
    public partial class FrmColorSet : Form
    {
        private String filePath;
        private Bitmap _image;
        private ColorCorrection colorCor = new ColorCorrection();
        private FrmMain _frmMain;

        public FrmColorSet(FrmMain frmMain, String filePath)
        {
            InitializeComponent();
            this._frmMain = frmMain;
            this.filePath = filePath;
            this.loadData();
        }

        public void setPreviewImg(Bitmap previewImage)
        {
            _image = previewImage;
            this.pictureBox1.Image =
                ImgHelper.BrightnessAndContrastOpenCV(previewImage, colorCor);
        }

        private void loadData()
        {
            colorCor = Constants.ColorCorrectionDetail;
            if (colorCor == null)
            {
                return;
            }

            this.setColor(colorCor);
        }

        private void ucTrackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBoxEx2.Text = ucTrackBar2.Value.ToString();
            //this.saveColorInfo();
            if (
                ucTrackBar2.Value.ToDoubleOrNull() != colorCor.gamma
                )
            {
                this.previewImageColor();
            }
        }

        private void ucTrackBar3_ValueChanged(object sender, EventArgs e)
        {
            textBoxEx3.Text = ucTrackBar3.Value.ToString();
            //this.saveColorInfo();
            if (
                ucTrackBar3.Value.ToDecimalOrNull() != colorCor.white
                )
            {
                this.previewImageColor();
            }
        }

        private void ucTrackBar4_ValueChanged(object sender, EventArgs e)
        {
            textBoxEx4.Text = ucTrackBar4.Value.ToString();
            if (
                ucTrackBar4.Value.ToDecimalOrNull() != colorCor.red
                )
            {
                this.previewImageColor();
            }
        }

        private void ucTrackBar5_ValueChanged(object sender, EventArgs e)
        {
            textBoxEx5.Text = ucTrackBar5.Value.ToString();
            if (
                ucTrackBar5.Value.ToDecimalOrNull() != colorCor.green
                )
            {
                this.previewImageColor();
            }
        }

        private void ucTrackBar6_ValueChanged(object sender, EventArgs e)
        {
            textBoxEx6.Text = ucTrackBar6.Value.ToString();
            if (
                ucTrackBar6.Value.ToDecimalOrNull() != colorCor.blue
                )
            {
                this.previewImageColor();
            }
        }

        private void saveColorInfo(Boolean saveFlag = false)
        {
            ColorCorrectionManager cm = new ColorCorrectionManager(filePath);
            ColorCorrection obj = new ColorCorrection();
            // 校验值是否变更，未变更部做处理
            if (
                // ucTrackBar1.Value == colorCor.black &&
                ucTrackBar2.Value.ToDoubleOrNull() == colorCor.gamma &&
                ucTrackBar3.Value.ToDecimalOrNull() == colorCor.white &&
                ucTrackBar4.Value.ToDecimalOrNull() == colorCor.red &&
                ucTrackBar5.Value.ToDecimalOrNull() == colorCor.green &&
                ucTrackBar6.Value.ToDecimalOrNull() == colorCor.blue
                )
            {
                if (saveFlag)
                {
                    this.Close();
                }
                return;
            }

            if (saveFlag)
            {
                if (colorCor != null && colorCor.Id > 0)
                {
                    obj.Id = colorCor.Id;
                }

                // obj.black = ucTrackBar1.Value.ToInt();
                obj.gamma = ucTrackBar2.Value.ToDouble();
                obj.white = ucTrackBar3.Value.ToInt();

                obj.red = ucTrackBar4.Value.ToInt();
                obj.green = ucTrackBar5.Value.ToInt();
                obj.blue = ucTrackBar6.Value.ToInt();

                cm.UpdateToDb(obj);
                Constants.LoadColorInfo(filePath);
                this.Close();
            }
            else
            {
                Constants.LoadColorInfo(filePath);
            }
        }

        /**
         * 保存到切片按钮
         * **/

        private void ucBtnSaveColor_BtnClick(object sender, EventArgs e)
        {
            _frmMain.setBtnActive(_frmMain.tsbColorCorrecionBtn, false);
            this.saveColorInfo(true);
        }

        private void previewImageColor()
        {
            ColorCorrection color = new ColorCorrection
            {
                // black = ucTrackBar1.Value.ToInt(),
                gamma = ucTrackBar2.Value.ToDouble(),
                white = ucTrackBar3.Value.ToInt(),
                red = ucTrackBar4.Value.ToInt(),
                green = ucTrackBar5.Value.ToInt(),
                blue = ucTrackBar6.Value.ToInt()
            };
            // pictureBox1.Image = ImgHelper.ImgColorGradation((Bitmap)_image.Clone(), color.red, color.green, color.blue);
            pictureBox1.Image = ImgHelper.BrightnessAndContrastOpenCV((Bitmap)_image.Clone(), color);
        }

        private void ucBtnReset_BtnClick(object sender, EventArgs e)
        {
            ColorCorrection color = ColorCorrection.DefaultSetting();
            this.setColor(color);
            pictureBox1.Image = ImgHelper.BrightnessAndContrastOpenCV((Bitmap)_image.Clone(), color);
        }

        private void setColor(ColorCorrection color)
        {
            //伽马
            this.ucTrackBar2.Value = (float)color.gamma;
            textBoxEx2.Text = color.gamma.ToString();
            //白色
            this.ucTrackBar3.Value = color.white;
            textBoxEx3.Text = color.white.ToString();

            //R
            this.ucTrackBar4.Value = color.red;
            textBoxEx4.Text = color.red.ToString();
            //G
            this.ucTrackBar5.Value = color.green;
            textBoxEx5.Text = color.green.ToString();
            //B
            this.ucTrackBar6.Value = color.blue;
            textBoxEx6.Text = color.blue.ToString();
        }

        private void FrmColorSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frmMain.setBtnActive(_frmMain.tsbColorCorrecionBtn, false);
        }
    }
}
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TileImageViewer
{
    public partial class SlicesCtrl : UserControl
    {
        private FrmFolderSelect frmFolder;
        private string strFilePath;
        private string strSlicesImg;
        private string strBarcodeImg;

        public SlicesCtrl(FrmFolderSelect fs, string strPath, string slicesImg, string barcodeImg)
        {
            InitializeComponent();
            frmFolder = fs;
            strFilePath = strPath;
            strSlicesImg = slicesImg;
            strBarcodeImg = barcodeImg;
        }

        private void SlicesCtrl_Load(object sender, EventArgs e)
        {
            this.slicesPicturerbox.Image = Image.FromFile(strSlicesImg);
            Image img2 = null;
            if (!File.Exists(strBarcodeImg))
                img2 = DST.TileImageViewer.Properties.Resources.barcode;
            else
                img2 = Image.FromFile(strBarcodeImg);

            this.barCodePicturebox.Image = img2;
            this.textBox1.Text = strFilePath.Remove(0, strFilePath.LastIndexOf("\\") + 1);

            //this.FileName.Text = strFilePath.Remove(0, strFilePath.LastIndexOf("\\") + 1);
        }

        private void SlicesCtrl_Click(object sender, EventArgs e)
        {
            frmFolder.ViewSlices(strFilePath);
        }

        private void slicesPicturerbox_Click(object sender, EventArgs e)
        {
            frmFolder.ViewSlices(strFilePath);
        }

        private void barCodePicturebox_Click(object sender, EventArgs e)
        {
            frmFolder.ViewSlices(strFilePath);
        }

        private void RotatePictureBox_Click(object sender, EventArgs e)
        {
            this.RotatePictureBox.Enabled = false;

            Image oldImage = barCodePicturebox.Image;
            barCodePicturebox.Image = RotateImage(oldImage, 180);

            if (oldImage != null)
                oldImage.Dispose();

            this.RotatePictureBox.Enabled = true;
        }

        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            float dx = image.Width / 2.0f;
            float dy = image.Height / 2.0f;

            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            Graphics g = Graphics.FromImage(rotatedBmp);
            g.TranslateTransform(dx, dy);
            g.RotateTransform(angle);
            g.TranslateTransform(-dx, -dy);
            g.DrawImage(image, new PointF(0, 0));
            g.Dispose(); // joysola
            return rotatedBmp;
        }
    }
}
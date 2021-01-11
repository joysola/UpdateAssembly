using System;
using System.Drawing;
using System.Windows.Forms;

namespace TileImageViewer
{
    public partial class FrmPageMap : Form
    {
        private FrmMain.GetImgCtrlDelegate Delegate;
        private ImgCtrl imgCtrl;
        private ImgCtrlPage floorPage;

        public FrmPageMap(FrmMain.GetImgCtrlDelegate getImgCtrlDelegate)
        {
            InitializeComponent();
            this.Delegate = getImgCtrlDelegate;
            imgCtrl = Delegate();
            floorPage = imgCtrl.Page.FloorPage();
            Reload();
        }

        public void Reload()
        {
            pbox.Image = floorPage.JoinImageByLevel();
            float w = (float)(2);
            Pen pen = new Pen(Color.Yellow, w);
            floorPage.DrawMRectOnAbs((Bitmap)pbox.Image, pen, imgCtrl.VisableAbsRect());
        }

        private void pbox_SizeChanged(object sender, EventArgs e)
        {
            //this.Size = new Size(pbox.Image.Size.Width + 10, pbox.Image.Size.Height + 10);
        }
    }
}
using System;
using System.Drawing;
using TileImageViewer.Controls;

namespace TileImageViewer
{
    public partial class SlideInfoCtrl : UCPanelParent
    {
        public SlideInfoCtrl()
        {
            InitializeComponent();
        }

        public bool Update(Point absPoint, float scale, MapRectangle visMapR, Color color)
        {
            if (absPoint.IsEmpty() || absPoint.X <= 0 || absPoint.Y <= 0)
            {
                lbltp1.Hide();
                lbltp2.Hide();
                lbltp3.Hide();
                lbltp4.Hide();
                lbltp5.Hide();
                lblRGB.Hide();
                lblPos.Hide();
                lblAbsPoint.Hide();

                lbltp1.Text = "暂无可用信息";
                lbltp1.Show();
            }
            else
            {
                lbltp1.Text = "坐标";

                lbltp1.Show();
                lbltp2.Show();
                lbltp3.Show();
                lbltp4.Show();
                lbltp5.Show();
                lblRGB.Text = color.R + "," + color.G + "," + color.B;
                lblRGB.Show();

                lblPos.Text = visMapR.Level + " (" + Math.Round(scale, 2) + ") / " + visMapR.ColStart + "," + visMapR.RowStart;
                lblPos.Show();

                lblAbsPoint.Text = absPoint.X + "," + absPoint.Y;
                lblAbsPoint.Show();
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void ucBtnImg1_BtnClick(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
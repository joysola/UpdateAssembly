using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TileImageViewer
{
    /// <summary>
    /// 标注窗体
    /// </summary>
    public partial class FrmAnnotationSetting : Form
    {
        private ImgCtrl iCtrl;

        public FrmAnnotationSetting(ImgCtrl i)
        {
            InitializeComponent();
            this.iCtrl = i;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void FrmAnnotationSetting_Load(object sender, EventArgs e)
        {
            this.AnnotationsListView.BeginUpdate();
            AnnotationsListView.Items.Clear();
            foreach (Annotation an in iCtrl.Page.ScanPage.Annotations)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = an.Title;
                AnnotationsListView.Items.Add(lvi);
            }

            this.ColorButton.Enabled = false;
            this.SaveButton.Enabled = false;
            this.ExportButton.Enabled = false;
            this.DeleteButton.Enabled = false;
            this.HideButton.Enabled = true;

            this.AnnotationsListView.EndUpdate();
        }

        private void ExportButton_Click(object sender, EventArgs e) //导出
        {
            if (AnnotationsListView.FocusedItem != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = AnnotationsListView.FocusedItem.SubItems[0].Text + ".json";//设置默认文件名为111
                saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory.ToString() + "\\Jsons";//设置默认目录为本程序目录
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(saveFileDialog.FileName))
                    {
                        FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite);
                        fs.Close();
                    }
                    int i = AnnotationsListView.FocusedItem.Index;
                    if (i < iCtrl.Page.ScanPage.Annotations.Count)
                    {
                        Stock st = Stock.MakeStock(iCtrl.Page.ScanPage.Annotations[i]);

                        File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(st));
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem it in AnnotationsListView.Items)
            {
                int index = it.Index;
                if (index >= 0 && index < iCtrl.Page.ScanPage.Annotations.Count)
                {
                    if (iCtrl.Page.ScanPage.Annotations[index].Title != it.Text)
                    {
                        iCtrl.Page.ScanPage.Annotations[index].Title = it.Text;
                        iCtrl.Page.ScanPage.UpdateAnnotationToDb(iCtrl.Page.ScanPage.Annotations[index]);
                        using (var gg = Graphics.FromImage(iCtrl.Image)) // joysola
                        {
                            iCtrl.Page.DrawAnnotationDetail(gg, iCtrl.Page.ScanPage.Annotations[index], iCtrl.Page.ImgOffset);
                        }
                        //iCtrl.Page.DrawAnnotationDetail(Graphics.FromImage(iCtrl.Image), iCtrl.Page.ScanPage.Annotations[index], iCtrl.Page.ImgOffset);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            bool bDelete = false;
            List<Annotation> ls = new List<Annotation>();
            foreach (ListViewItem it in AnnotationsListView.SelectedItems)
            {
                int index = it.Index;
                ls.Add(iCtrl.Page.ScanPage.Annotations[index]);
            }

            foreach (ListViewItem it in AnnotationsListView.SelectedItems)
            {
                AnnotationsListView.Items.Remove(it);
            }

            foreach (Annotation an in ls)
            {
                if (iCtrl.Page.ScanPage.DeleteAnnotationFromDb(an) > 0)
                {
                    iCtrl.Page.ScanPage.Annotations.Remove(an);
                    bDelete = true;
                }
            }

            //if(bDelete)
            //    iCtrl.Image = iCtrl.Page.JoinImage(iCtrl.GetImgTag());
            if (bDelete)
                iCtrl.ReloadImage();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //this.Hide();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            if (AnnotationsListView.FocusedItem != null)
            {
                ColorDialog ColorForm = new ColorDialog();
                if (ColorForm.ShowDialog() == DialogResult.OK)
                {
                    int i = AnnotationsListView.FocusedItem.Index;
                    if (i >= 0 && i < iCtrl.Page.ScanPage.Annotations.Count)
                    {
                        iCtrl.Page.ScanPage.Annotations[i].Graph.PenColor = ColorForm.Color;
                        iCtrl.Page.ScanPage.UpdateAnnotationToDb(iCtrl.Page.ScanPage.Annotations[i]);
                        using (var gg = Graphics.FromImage(iCtrl.Image))
                        {
                            iCtrl.Page.DrawAnnotationGraph(gg, iCtrl.Page.ScanPage.Annotations[i], iCtrl.Page.ImgOffset);
                        }
                        //iCtrl.Page.DrawAnnotationGraph(Graphics.FromImage(iCtrl.Image), iCtrl.Page.ScanPage.Annotations[i], iCtrl.Page.ImgOffset);
                    }
                }
            }
        }

        private void AnnotationsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AnnotationsListView.SelectedItems.Count > 1) //多选
            {
                this.ColorButton.Enabled = false;
                this.SaveButton.Enabled = true;
                this.ExportButton.Enabled = false;
                this.DeleteButton.Enabled = true;
            }
            else
            {
                this.ColorButton.Enabled = true;
                this.SaveButton.Enabled = true;
                this.ExportButton.Enabled = true;
                this.DeleteButton.Enabled = true;

                if (this.AnnotationsListView.FocusedItem != null)
                {
                    int i = AnnotationsListView.FocusedItem.Index;
                    if (i >= 0 && i < iCtrl.Page.ScanPage.Annotations.Count)
                        DetailLabel.Text = iCtrl.Page.ScanPage.Annotations[i].Graph.Detail();
                }
            }
        }

        private static bool bShow = true;

        private void HideButton_Click(object sender, EventArgs e)
        {
            if (bShow)
                bShow = false;
            else
                bShow = true;

            iCtrl.Page.ShowAnnotations(bShow);
            //iCtrl.Image = iCtrl.Page.JoinImage(iCtrl.GetImgTag());
            iCtrl.ReloadImage();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
        }

        private bool isLeftMousePressed = false;
        private Point selfPoint;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isLeftMousePressed = true;
                selfPoint = new Point(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isLeftMousePressed)
            {
                isLeftMousePressed = false;
                int x = this.Location.X + e.X - selfPoint.X;
                int y = this.Location.Y + e.Y - selfPoint.Y;
                this.Location = new Point(x, y);
            }
        }
    }
}
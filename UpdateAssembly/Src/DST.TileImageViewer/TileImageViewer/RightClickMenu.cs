using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TileImageViewer
{
    /// <summary>
    /// 标注右键菜单
    /// </summary>
    internal class RightClickMenu : ContextMenuStrip
    {
        //imgctrl控件引用
        private ImgCtrl iCtrl;

        //标注引用
        private Annotation anno;

        public RightClickMenu(ImgCtrl i, Annotation an)
        {
            this.iCtrl = i;
            anno = an;

            Items.Add("设置名称");
            Items.Add("设置颜色");
            Items.Add("导出标注");
            Items.Add("删除标注");
            Items.Add("调整大小");

            Items[0].Click += new EventHandler(SetTitle);
            Items[1].Click += new EventHandler(SetColor);
            Items[2].Click += new EventHandler(Export);
            Items[3].Click += new EventHandler(Delete);
            Items[4].Click += new EventHandler(Adjust);
            if (anno != null && anno.Graph.Type == AnnotationType.FixCircle || anno.Graph.Type == AnnotationType.FixRectangle)
            {
                Items[4].Enabled = false;
            }
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetColor(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK && anno != null)
            {
                anno.Graph.PenColor = ColorForm.Color;
                try
                {
                    iCtrl.Page.ScanPage.UpdateAnnotationToDb(anno);
                    using (var gg = Graphics.FromImage(iCtrl.Image)) // joysola
                    {
                        iCtrl.Page.DrawAnnotationGraph(gg, anno, iCtrl.Page.ImgOffset);
                    }
                    //iCtrl.Page.DrawAnnotationGraph(Graphics.FromImage(iCtrl.Image), anno, iCtrl.Page.ImgOffset);
                }
                catch
                {
                }
            }
        }

        private TextBox txtInput = null;

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTitle(object sender, EventArgs e)
        {
            if (anno != null)
            {
                this.txtInput = new TextBox();
                this.txtInput.Parent = iCtrl;
                this.txtInput.Multiline = true;
                this.txtInput.Bounds = new Rectangle(new Point(anno.DetailRectangle.X - iCtrl.Page.ImgOffset.X,
                                                     anno.DetailRectangle.Y - iCtrl.Page.ImgOffset.Y), anno.DetailRectangle.Size);

                this.txtInput.Text = anno.Title;
                this.txtInput.KeyPress += new KeyPressEventHandler(txtInput_KeyPress);

                this.txtInput.Focus();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete(object sender, EventArgs e)
        {
            if (anno != null && iCtrl.Page.ScanPage.DeleteAnnotationFromDb(anno) > 0)
            {
                iCtrl.Page.ScanPage.Annotations.Remove(anno);

                //iCtrl.Image = iCtrl.Page.JoinImage(iCtrl.GetImgTag());
                iCtrl.ReloadImage();
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = anno.Title + ".json";//设置默认文件名为111
            saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory.ToString() + "\\Jsons";//设置默认目录为本程序目录
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(saveFileDialog.FileName))
                {
                    FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite);
                    fs.Close();
                }

                Stock st = Stock.MakeStock(anno);

                File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(st));
            }
        }

        /// <summary>
        /// 调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adjust(object sender, EventArgs e)
        {
            if (anno != null)
                this.iCtrl.StartAdjust(anno);
        }

        /// <summary>
        /// 回车保存内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13 && this.txtInput != null)
                {
                    anno.Title = this.txtInput.Text;
                    using (var gg = Graphics.FromImage(iCtrl.Image)) // joysola
                    {
                        iCtrl.Page.DrawAnnotationDetail(gg, anno, iCtrl.Page.ImgOffset);
                    }
                    //iCtrl.Page.DrawAnnotationDetail(Graphics.FromImage(iCtrl.Image), anno, iCtrl.Page.ImgOffset);
                    iCtrl.Page.ScanPage.UpdateAnnotationToDb(anno);

                    this.txtInput.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
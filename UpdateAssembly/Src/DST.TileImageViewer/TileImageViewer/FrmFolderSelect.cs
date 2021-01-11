using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace TileImageViewer
{
    /// <summary>
    /// 切片选择界面
    /// </summary>
    public partial class FrmFolderSelect : Form
    {
        public FrmFolderSelect()
        {
            InitializeComponent();
            expandedNodes = new Dictionary<IntPtr, bool>();
            favoriteNode = null;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            StartPosition = FormStartPosition.CenterScreen;
            if (IsDateBeforeOrToday("09102020"))
            {
                if (MessageBox.Show(
                    "您的版本过旧，请使用新版本",
                    GlobalData.GlobalLanguage.Text("close"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    ) == DialogResult.OK)
                {
                    try
                    {
                        System.Environment.Exit(0);
                    }
                    catch (Exception ree)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 判断日期是否在今天之前
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDateBeforeOrToday(string input)
        {
            bool result = true;

            if (input != null)
            {
                DateTime dTCurrent = DateTime.Now;
                int currentDateValues = Convert.ToInt32(dTCurrent.ToString("MMddyyyy"));
                int inputDateValues = Convert.ToInt32(input.Replace("/", ""));

                result = inputDateValues <= currentDateValues;
            }
            else
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// IconIndexs类 对应ImageList中5张图片的序列
        /// </summary>
        private class IconIndexes
        {
            public const int MyComputer = 0;      //我的电脑
            public const int ClosedFolder = 1;    //文件夹关闭
            public const int OpenFolder = 2;      //文件夹打开
            public const int FixedDrive = 3;      //磁盘盘符
            public const int MyDocuments = 4;     //我的文档
            public const int MyPictures = 5;     //我的图片
            public const int MyFavorites = 6;    //收藏夹
            public const int Desktop = 7;        //桌面
        }

        private Dictionary<IntPtr, bool> expandedNodes;
        private TreeNode favoriteNode;

        /// <summary>
        /// 窗体加载Load事件 初始化
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.LoadDirectoryTree(); //new
        }

        private void LoadDirectoryTree() //new
        {
            //实例化特殊文件夹
            TreeNode SpecialFolderNode = new TreeNode("特殊文件夹", IconIndexes.MyComputer, IconIndexes.MyComputer);
            SpecialFolderNode.Tag = "特殊文件夹";
            SpecialFolderNode.Text = GlobalData.GlobalLanguage.Text("Special_Folder");
            SpecialFolderNode.Name = "特殊文件夹";
            this.directoryTree.Nodes.Add(SpecialFolderNode);

            //显示特殊文件夹(我的文档)结点
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //获取我的文档文件夹
            TreeNode DocNode = new TreeNode(myDocuments, IconIndexes.MyDocuments, IconIndexes.MyDocuments);
            DocNode.Tag = "我的文档";                            //设置结点名称
            DocNode.Name = myDocuments;
            DocNode.Text = GlobalData.GlobalLanguage.Text("My_Documents");
            SpecialFolderNode.Nodes.Add(DocNode);                          //specialfolder目录下加载节点
            DocNode.Nodes.Add("");

            //显示特殊文件夹(我的图片)结点
            var myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);//获取我的图片文件夹
            TreeNode myPictureNode = new TreeNode(myPictures, IconIndexes.MyPictures, IconIndexes.MyPictures);
            myPictureNode.Tag = "我的图片";                            //设置结点名称
            myPictureNode.Name = myPictures;
            myPictureNode.Text = GlobalData.GlobalLanguage.Text("My_Picture");
            SpecialFolderNode.Nodes.Add(myPictureNode);                          //specialfolder目录下加载节点
            myPictureNode.Nodes.Add("");

            //显示特殊文件夹(桌面)结点
            var myDeskTop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//获取桌面文件夹
            TreeNode deskTopNode = new TreeNode(myDeskTop, IconIndexes.Desktop, IconIndexes.Desktop);
            deskTopNode.Tag = "桌面";                            //设置结点名称
            deskTopNode.Name = myDeskTop;
            deskTopNode.Text = GlobalData.GlobalLanguage.Text("Desktop");
            SpecialFolderNode.Nodes.Add(deskTopNode);                          //specialfolder目录下加载节点
            deskTopNode.Nodes.Add("");

            //显示特殊文件夹(收藏夹)节点
            var favorites = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);    //获取收藏夹
            favoriteNode = new TreeNode(favorites, IconIndexes.MyFavorites, IconIndexes.MyFavorites);
            favoriteNode.Tag = "收藏夹";
            favoriteNode.Text = GlobalData.GlobalLanguage.Text("Favorite");
            favoriteNode.Name = "收藏夹";
            //favoriteNode.Nodes.Add("");
            this.directoryTree.Nodes.Add(favoriteNode);

            //实例化TreeNode类 TreeNode(string text,int imageIndex,int selectImageIndex)
            TreeNode MyComputerNode = new TreeNode("我的电脑", IconIndexes.MyComputer, IconIndexes.MyComputer);  //载入显示 选择显示
            MyComputerNode.Tag = "我的电脑";                            //树节点数据
            MyComputerNode.Text = GlobalData.GlobalLanguage.Text("My_Computer");                           //树节点标签内容
            MyComputerNode.Name = "我的电脑";
            this.directoryTree.Nodes.Add(MyComputerNode);               //树中添加根目录

            //循环遍历计算机所有逻辑驱动器名称(盘符)
            foreach (string drive in Environment.GetLogicalDrives())
            {
                //实例化DriveInfo对象 命名空间System.IO
                var dir = new DriveInfo(drive);
                switch (dir.DriveType)           //判断驱动器类型
                {
                    case DriveType.Network:
                    case DriveType.Fixed:        //仅取固定磁盘盘符 Removable-U盘
                        {
                            //Split仅获取盘符字母
                            TreeNode tNode = new TreeNode(dir.Name.Split(':')[0]);
                            tNode.Name = dir.Name;
                            tNode.Tag = tNode.Name;
                            tNode.ImageIndex = IconIndexes.FixedDrive;         //设置获取结点显示图片
                            tNode.SelectedImageIndex = IconIndexes.FixedDrive; //设置选择显示图片
                            MyComputerNode.Nodes.Add(tNode);                    //加载驱动节点
                            tNode.Nodes.Add("");
                        }
                        break;
                }
            }

            //网络
            TreeNode NetWorkNode = new TreeNode("网络", IconIndexes.MyComputer, IconIndexes.MyComputer);
            NetWorkNode.Tag = "网络";
            NetWorkNode.Text = GlobalData.GlobalLanguage.Text("Network"); ;
            NetWorkNode.Name = "网络";
            this.directoryTree.Nodes.Add(NetWorkNode);
            NetWorkNode.Nodes.Add("");

            XmlNodeList eNodeList = XMLHelper.ReadElementsByName("Favorites");//加载收藏夹内容
            for (int i = 0; i < eNodeList.Count; i++)
            {
                TreeViewItems.Add(favoriteNode, eNodeList[i].InnerText);
            }

            string strLastPath = XMLHelper.ReadLastDirectory();
            if (!strLastPath.StartsWith("网络"))
            {
                TreeNode tn = TreeViewItems.ExpandByPath(directoryTree.Nodes, strLastPath);   //展开上次路径
                if (tn != null)
                    directoryTree.SelectedNode = tn;
            }

            SetLanguage();
        }

        /// <summary>
        /// 设置app语言
        /// </summary>
        private void SetLanguage()
        {
            this.Text = GlobalData.GlobalLanguage.Text("App_Name");
        }

        /// <summary>
        /// 在结点展开后发生 展开子结点
        /// </summary>
        private void directoryTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
            if (!e.Node.FullPath.StartsWith("网络"))
            {
                XMLHelper.WriteExpandDirectory(e.Node.FullPath);
            }
        }

        /// <summary>
        /// 在将要展开结点时发生 加载子结点
        /// </summary>
        private void directoryTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!expandedNodes.ContainsKey(e.Node.Handle))
            {
                expandedNodes.Add(e.Node.Handle, true);
                TreeViewItems.Add(e.Node);
            }
        }

        /// <summary>
        /// 自定义类TreeViewItems 调用其Add(TreeNode e)方法加载子目录
        /// </summary>
        public static class TreeViewItems
        {
            public static void Add(TreeNode e)
            {
                //try..catch异常处理
                try
                {
                    //判断"我的电脑"Tag 上面加载的该结点没指定其路径
                    if ((e.Tag.ToString() == "特殊文件夹") || (e.Tag.ToString() == "我的电脑") || (e.Tag.ToString() == "收藏夹"))
                    {
                        e.Expand();
                    }
                    else if (e.Tag.ToString() == "网络")
                    {
                        e.Nodes.Clear();
                        DirectoryEntry root = new DirectoryEntry("WinNT:");
                        DirectoryEntries domains = root.Children;
                        domains.SchemaFilter.Add("domain");
                        foreach (DirectoryEntry domain in domains)
                        {
                            DirectoryEntries computers = domain.Children;
                            computers.SchemaFilter.Add("computer");
                            foreach (DirectoryEntry computer in computers)
                            {
                                TreeNode subNode = new TreeNode(computer.Name); //实例化
                                subNode.Name = @"\\" + computer.Name;               //完整目录
                                subNode.Tag = "网络计算机";
                                subNode.ImageIndex = IconIndexes.MyComputer;       //设置获取节点显示图片
                                subNode.SelectedImageIndex = IconIndexes.MyComputer; //设置选择节点显示图片
                                e.Nodes.Add(subNode);
                                subNode.Nodes.Add("");
                            }
                        }
                    }
                    else if (e.Tag.ToString() == "网络计算机")
                    {
                        e.Nodes.Clear();
                        string[] ShareList = TileImageViewer.Helpers.NetShareFolderEnmu.NetShareList(e.Name);
                        if (ShareList != null)
                        {
                            foreach (string shareDir in ShareList)
                            {
                                TreeNode subNode1 = new TreeNode(shareDir); //实例化
                                subNode1.Name = e.Name + "\\" + shareDir;               //完整目录
                                subNode1.Tag = "共享";
                                subNode1.ImageIndex = IconIndexes.ClosedFolder;       //设置获取节点显示图片
                                subNode1.SelectedImageIndex = IconIndexes.OpenFolder; //设置选择节点显示图片
                                e.Nodes.Add(subNode1);
                                subNode1.Nodes.Add("");                               //加载空节点 实现+号
                            }
                        }
                    }
                    else
                    {
                        e.Nodes.Clear();                               //清除空节点再加载子节点
                        TreeNode tNode = e;                            //获取选中\展开\折叠结点
                        string path = tNode.Name;                      //路径

                        //获取"我的文档"路径
                        if (e.Tag.ToString() == "我的文档")
                        {
                            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);           //获取计算机我的文档文件夹
                        }

                        if (e.Tag.ToString() == "我的图片")
                        {
                            path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        }

                        if (e.Tag.ToString() == "桌面")
                        {
                            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        }

                        //获取指定目录中的子目录名称并加载结点
                        string[] dics = Directory.GetDirectories(path);
                        foreach (string dic in dics)
                        {
                            if ((new FileInfo(dic).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                            {
                                TreeNode subNode = new TreeNode(new DirectoryInfo(dic).Name); //实例化
                                subNode.Name = new DirectoryInfo(dic).FullName;               //完整目录
                                subNode.Tag = subNode.Name;
                                subNode.ImageIndex = IconIndexes.ClosedFolder;       //设置获取节点显示图片
                                subNode.SelectedImageIndex = IconIndexes.OpenFolder; //设置选择节点显示图片
                                tNode.Nodes.Add(subNode);
                                subNode.Nodes.Add("");                               //加载空节点 实现+号
                            }
                        }
                    }
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message);                   //异常处理
                }
            }

            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="tn1">待添加的节点</param>
            /// <param name="strName">名称</param>
            /// <returns></returns>
            public static String Add(TreeNode tn1, string strName)
            {
                TreeNode subNode = new TreeNode(new DirectoryInfo(strName).Name);
                subNode.Name = new DirectoryInfo(strName).FullName;               //完整目录
                subNode.Tag = subNode.Name;
                subNode.ImageIndex = IconIndexes.ClosedFolder;       //设置获取节点显示图片
                subNode.SelectedImageIndex = IconIndexes.OpenFolder; //设置选择节点显示图片
                tn1.Nodes.Add(subNode);
                subNode.Nodes.Add("");                               //加载空节点 实现+号

                return subNode.FullPath;
            }

            /// <summary>
            /// 查找结点
            /// </summary>
            /// <param name="tnNode">父节点</param>
            /// <param name="strValue">名称</param>
            /// <returns></returns>
            public static TreeNode FindNode(TreeNode tnNode, string strValue)
            {
                if (tnNode == null)
                {
                    return null;
                }

                if (tnNode.Text.Contains(strValue))
                {
                    return tnNode;
                }

                TreeNode tnRet = null;
                foreach (TreeNode tn in tnNode.Nodes)
                {
                    tnRet = TreeViewItems.FindNode(tn, strValue);
                    if (tnRet != null)
                    {
                        break;
                    }
                }

                return tnRet;
            }

            /// <summary>
            /// 根据节点展开
            /// </summary>
            /// <param name="tNodes"></param>
            /// <param name="strPath"></param>
            /// <returns></returns>
            public static TreeNode ExpandByPath(TreeNodeCollection tNodes, String strPath)
            {
                string[] strPathArr = strPath.Split('\\');
                TreeNodeCollection tNodeCollection = tNodes;
                TreeNode tnRt = null;
                for (int i = 0; i < strPathArr.Count(); i++)
                {
                    foreach (TreeNode tn in tNodeCollection)
                    {
                        if (tn.Text == strPathArr[i])
                        {
                            tnRt = tn;
                            tnRt.Expand();
                            break;
                        }
                    }

                    if ((tnRt != null) && (tnRt.Nodes.Count > 0))
                    {
                        tNodeCollection = tnRt.Nodes;
                    }
                    else
                    {
                        tNodeCollection = null;
                        break;
                    }
                }

                return tnRt;
            }
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private void directoryTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path = e.Node.Name;
            filelistFlowLayoutPanel.Controls.Clear();

            if (!Directory.Exists(path))
            {
                return;
            }

            string[] directories = Directory.GetDirectories(path);
            if (directories != null)
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    try
                    {
                        string sname = new DirectoryInfo(directories[i]).FullName;
                        PageException pse = ScanPage.IsValidScanPage(sname);
                        PageException pde = ScanPage.IsValidScanPage(path);

                        if (pse == null || pse.ErrCode != PageException.ERR_NON_BASE_FILE)
                        {
                            SlicesCtrl slices = new SlicesCtrl(this, sname, sname + "\\1\\0\\0.jpg", sname + Constants.ScanPageBarcodeFilePath);
                            filelistFlowLayoutPanel.Controls.Add(slices);
                        }
                        else if (pde == null || pde.ErrCode != PageException.ERR_NON_BASE_FILE)
                        {
                            SlicesCtrl slices = new SlicesCtrl(this, path, path + "\\1\\0\\0.jpg", path + Constants.ScanPageBarcodeFilePath);
                            filelistFlowLayoutPanel.Controls.Add(slices);
                            break;
                        }
                    }
                    catch (Exception exp)
                    {
                        //  MessageBox.Show(exp.Message);
                    }
                }
            }
        }

        private void directoryTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if ((e.Node.Tag.ToString() == "特殊文件夹") || (e.Node.Tag.ToString() == "我的电脑") || (e.Node.Tag.ToString() == "收藏夹"))
            {
                if (favoritesButton.Enabled)
                {
                    favoritesButton.Enabled = false;
                    favoritesButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_favorite_press;
                }
            }
            else
            {
                if (!favoritesButton.Enabled)
                {
                    favoritesButton.Enabled = true;
                    favoritesButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_favorite;
                }
            }
        }

        /// <summary>
        /// 合并图像
        /// </summary>
        /// <param name="sectionImage">底图路径</param>
        /// <param name="barcodeImage">条形码路径</param>
        /// <returns></returns>
        private Image CombinImage(string sectionImage, string barcodeImage)
        {
            Image img1 = Image.FromFile(sectionImage);
            Image img2 = null;
            if (!File.Exists(barcodeImage))
            {
                img2 = DST.TileImageViewer.Properties.Resources.barcode;
            }
            else
            {
                img2 = Image.FromFile(barcodeImage);
            }

            var width = filesIcons.ImageSize.Width;
            var height = filesIcons.ImageSize.Height;
            Bitmap bitMap = new Bitmap(width, height); // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            Pen p = new Pen(Color.Black, 4);

            //图一
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height * 3 / 4));
            g1.DrawImage(img1, 4 + (width - 2 * 4 - img1.Width / 4) / 2, (height / 2 - 8) / 2, img1.Width / 4, img1.Height / 4);
            g1.DrawRectangle(p, new Rectangle(0, 0, width, height * 3 / 4));

            g1.DrawImage(img2, 4, height * 3 / 4 + 10, width - 10, height / 4 - 14);//在图一往下10像素处画上图二
            g1.DrawRectangle(p, new Rectangle(0, height * 3 / 4 + 6, width, height / 4 - 6));

            Image img = bitMap;

            g1.Dispose();// joysola
            return img;
        }

        private FrmMain frmMain;

        /// <summary>
        /// 查看切片
        /// </summary>
        /// <param name="strPath">切片路径</param>
        public void ViewSlices(string strPath)
        {
            if (ScanPage.IsValidScanPage(strPath) != null)
            {
                MessageBox.Show(ScanPage.IsValidScanPage(strPath).Message);
                return;
            }

            if (frmMain == null || (frmMain != null && frmMain.GetCurrentFilePath() != strPath))
                frmMain = new FrmMain(strPath, this.Size);

            //frmMain = new FrmMain(strPath);
            this.CloseStartWindow.Visible = true;
            this.Hide();
            frmMain.WindowState = this.WindowState;
            frmMain.ShowDialog();

            this.Show();

            return;
        }

        /// <summary>
        /// 搜索按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_Click(object sender, EventArgs e)
        {
            if (searchText.Text.Count() == 0)
                return;

            searchButton.Enabled = false;
            bool bFind = false;
            foreach (TreeNode n in directoryTree.Nodes)
            {
                TreeNode temp = TreeViewItems.FindNode(n, searchText.Text);
                if (temp != null)
                {
                    TreeViewItems.ExpandByPath(directoryTree.Nodes, temp.FullPath);
                    directoryTree.SelectedNode = temp;

                    bFind = true;
                    break;
                }
            }

            searchButton.Enabled = true;

            if (!bFind)
            {
                MessageBox.Show("抱歉，未发现匹配项！");
            }

            return;
        }

        private void directoryTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Node.Bounds);
            if (e.State == TreeNodeStates.Selected)
            {
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, new Rectangle(e.Node.Bounds.Left, e.Node.Bounds.Top, e.Node.Bounds.Width, e.Node.Bounds.Height));//背景色为蓝色
                RectangleF drawRect = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height);
                e.Graphics.DrawString(e.Node.Text, directoryTree.Font, Brushes.White, drawRect);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void searchText_Enter(object sender, EventArgs e)
        {
            if (!searchButton.Enabled && (searchText.Text.Count() > 0))
            {
                searchButton.Enabled = true;
                searchButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_search;
            }
        }

        private void searchText_Leave(object sender, EventArgs e)
        {
            if ((searchText.Text.Count() == 0) && searchButton.Enabled)
            {
                searchButton.Enabled = false;
                searchButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_search_press;
            }
        }

        private void searchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.search_Click(sender, e);
            }
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            if (searchText.Text.Count() > 0)
            {
                if (!searchButton.Enabled)
                {
                    searchButton.Enabled = true;
                    searchButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_search;
                }
            }
            else
            {
                if (searchButton.Enabled)
                {
                    searchButton.Enabled = false;
                    searchButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_search_press;
                }
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmFolderSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show(
                    GlobalData.GlobalLanguage.Text("Close_Confirm"),
                    GlobalData.GlobalLanguage.Text("close"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    ) == DialogResult.Yes)
                {
                    if (frmMain != null)
                    {
                        frmMain.Close();
                        frmMain = null;
                    }
                    try
                    {
                        if (IsDisposed || !this.Parent.IsHandleCreated) return;
                        System.Environment.Exit(0);
                    }
                    catch (Exception ree)
                    {
                    }

                    //this.DialogResult = DialogResult.OK;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadButton_Click(object sender, EventArgs e) //new
        {
            this.ReloadButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_reload_press;
            this.ReloadButton.Enabled = false;

            directoryTree.Nodes.Clear();
            expandedNodes.Clear();

            System.Threading.Thread.Sleep(500);
            this.LoadDirectoryTree();

            this.ReloadButton.Enabled = true;
            this.ReloadButton.BackgroundImage = DST.TileImageViewer.Properties.Resources.icon_reload;
        }

        private void favoritesButton_Click(object sender, EventArgs e)
        {
            string strCurrentPath = directoryTree.SelectedNode.Name;    //当前选中节点路径
            if (favoriteNode != null)
            {
                if (!XMLHelper.WriteFavoritesPath("Favorites", strCurrentPath)) //加收藏
                {
                    string strPath = TreeViewItems.Add(favoriteNode, directoryTree.SelectedNode.Name);
                    TreeViewItems.ExpandByPath(directoryTree.Nodes, strPath);
                }
                else    //去掉收藏
                {
                    TreeNode tNode = null;
                    foreach (TreeNode tn in favoriteNode.Nodes)
                    {
                        if (strCurrentPath.Contains(tn.Name))
                        {
                            tNode = tn;
                            break;
                        }
                    }

                    if (tNode != null)
                    {
                        expandedNodes.Remove(tNode.Handle);
                        favoriteNode.Nodes.Remove(tNode);
                    }
                }
            }

            TreeViewItems.ExpandByPath(directoryTree.Nodes, directoryTree.SelectedNode.FullPath);
        }

        public static class XMLHelper
        {
            private static string configxmlpath = Path.Combine(PathUtil.AppdataPath, "config.xml");

            public static XmlDocument LoadConfigXml()
            {
                XmlDocument configXml = new XmlDocument();
                if (File.Exists(configxmlpath))    //读取配置文件
                {
                    configXml.Load(configxmlpath);
                }
                else //文件不存在
                {
                    XmlDeclaration dec = configXml.CreateXmlDeclaration("1.0", "utf-8", null);
                    configXml.AppendChild(dec);

                    //创建根节点
                    XmlElement path = configXml.CreateElement("Path");
                    configXml.AppendChild(path);
                    path.SetAttribute("Value", "收藏夹");
                    configXml.Save(configxmlpath);
                }

                return configXml;
            }

            public static string ReadLastDirectory()
            {
                String strPath = "收藏夹";
                try
                {
                    XmlDocument configXml = XMLHelper.LoadConfigXml();
                    if (configXml != null)
                    {
                        XmlElement rootElement = configXml.DocumentElement;
                        strPath = rootElement.GetAttribute("Value");
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    MessageBox.Show(e.Message);
#endif
                }

                return strPath;
            }

            public static XmlNodeList ReadElementsByName(string strName)
            {
                XmlNodeList eList = null;
                try
                {
                    XmlDocument configXml = XMLHelper.LoadConfigXml();
                    if (configXml != null)
                    {
                        eList = configXml.GetElementsByTagName(strName);
                    }
                }
                catch (Exception e)
                {
#if DEUBG
                   // MessageBox.Show(e.Message);
#endif
                }

                return eList;
            }

            public static void WriteExpandDirectory(string strValue)
            {
                try
                {
                    XmlDocument configXml = XMLHelper.LoadConfigXml();
                    if (configXml != null)
                    {
                        XmlElement root = configXml.DocumentElement;
                        root.SetAttribute("Value", strValue);
                        configXml.Save(configxmlpath);
                    }
                }
                catch (Exception msg)
                {
#if DEBUG
                    MessageBox.Show(msg.Message);
#endif
                }
            }

            public static bool WriteFavoritesPath(string strName, string strValue)
            {
                //写入配置文件
                bool bFind = false;
                try
                {
                    XmlDocument configXml = XMLHelper.LoadConfigXml();
                    if (configXml != null)
                    {
                        XmlElement root = configXml.DocumentElement;
                        XmlNodeList elemList = configXml.GetElementsByTagName(strName);

                        for (int i = 0; i < elemList.Count; i++)
                        {
                            if (strValue.Contains(elemList[i].InnerText))
                            {
                                bFind = true;
                                root.RemoveChild(elemList[i]);
                                break;
                            }
                        }

                        if (!bFind)
                        {
                            XmlNode newElement = configXml.CreateNode(XmlNodeType.Element, strName, null);
                            newElement.InnerText = strValue;
                            root.AppendChild(newElement);
                        }

                        configXml.Save(configxmlpath);
                    }
                }
                catch (Exception msg)
                {
#if DEBUG
                    MessageBox.Show(msg.Message);
#endif
                }

                return bFind;
            }
        }

        private void CloseStartWindow_Click(object sender, EventArgs e)
        {
            if (frmMain != null)
            {
                this.Hide();
                frmMain.ShowDialog();
            }

            this.Show();
        }

        private void filelistFlowLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            this.filelistFlowLayoutPanel.Select();
        }

        private void verticalSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int movex = e.SplitX - this.DirectoryTreeBarPictureBox.Right;
            this.DirectoryTreeBarPictureBox.Width += movex;
            this.searchText.Width += movex;
            this.favoritesButton.Location = new Point(this.favoritesButton.Location.X + movex, this.favoritesButton.Location.Y);
            this.searchButton.Location = new Point(this.searchButton.Location.X + movex, this.searchButton.Location.Y);
            this.directoryTree.Width += movex;
            this.directoryTree.Height += this.verticalSplitContainer.Bottom - this.directoryTree.Bottom;
        }
    }
}
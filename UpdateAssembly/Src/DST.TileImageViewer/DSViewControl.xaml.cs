using DST.Controls;
using DST.Controls.Base;
using DST.Database.WPFCommonModels;
using DST.Joint.Construction.Mgmt.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using TileImageViewer;

namespace DST.TileImageViewer
{
    /// <summary>
    /// DSViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class DSViewControl : BaseUserControl
    {
        /// <summary>
        /// 控件的实际宽度
        /// </summary>
        private double ctrlWidth;

        /// <summary>
        /// 控件实际高度
        /// </summary>
        private double ctrlHight;

        /// <summary>
        /// 切片控件
        /// </summary>
        public FrmMain frmMain = null;

        /// <summary>
        /// 外部可以访问WindowsFormsHost
        /// </summary>
        public WindowsFormsHost WindowsFormsHost => this.dsvHostMain;

        /// <summary>
        /// 新增依赖属性 文件路径
        /// </summary>
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(DSViewControl), new PropertyMetadata("", FilePathChanged));

        /// <summary>
        /// 文件路径属性
        /// </summary>
        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set
            {
                SetValue(FilePathProperty, value); // 来自于MainWindow的赋值
                ShowFrmMain(value); // 显示看片程序
            }
        }

        /// <summary>
        /// 静态构造器，用于第一次加载时操作
        /// </summary>
        static DSViewControl()
        {
            try
            {
                // 第一次加载
                Constants.LoadSettingInfo();
                GlobalData.InitLanguage();
            }
            catch (Exception ex)
            {
                DST.Common.Logger.Logger.Error("初始化看片winform环境失败！", ex);
            }
        }

        /// <summary>
        /// 实例构造器
        /// </summary>
        public DSViewControl()
        {
            InitializeComponent();
            this.DataContext = new DSViewControlViewModel();
            // 第一次加载完成 记录 控件高度和宽度，防止第二次重新计算之前ActualHeight和ActualWidth=0
            this.Loaded += (sender, e) =>
            {
                ctrlHight = this.ActualHeight;
                ctrlWidth = this.ActualWidth;
            };
            RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            // 切换焦点至看图
            Messenger.Default.Register<bool>(this, EnumMessageKey.UnSelectPathology, data =>
            {
                if (data)
                {
                    this.dsvHostMain.Focus();
                }
            });
        }

        private static void FilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// 显示看片程序
        /// </summary>
        /// <param name="value"></param>
        private void ShowFrmMain(string value)
        {
            this.Focus();
            WhirlingControlManager.ShowWaitingForm();
            this.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    var size = new System.Drawing.Size(Convert.ToInt32(ctrlWidth), Convert.ToInt32(ctrlHight)); // 大小需要修改
                    frmMain = new FrmMain(value, size);
                    // 不可以是顶级窗体
                    frmMain.TopLevel = false;
                    // 如果不使用，那么Host中的控件会出现变形的问题。而且放大的大小比放大之后来的更加的大(因为DPI不一样)
                    frmMain.AutoScaleMode = AutoScaleMode.Inherit;
                    frmMain.BackgroundImageLayout = ImageLayout.Stretch;
                    this.dsvHostMain.Child = frmMain;
                }
                catch (Exception ex)
                {
                    DST.Common.Logger.Logger.Error("看片模块报错！", ex);
                    ConfirmMessageBox.Show("", $"看片模块异常，请确认样本路径正确或者联系管理员查看日志。", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }

                // 模拟鼠标滚路事件
                // await Test.Test.TestMouseWheel(frmMain.ImgBox_MouseWheel);
            });
        }
    }
}
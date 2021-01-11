using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TileImageViewer
{
    public partial class SplashScreen : Form
    {
        private static Thread _splashLauncher;
        private static SplashScreen _splashScreen;

        public SplashScreen()
        {
            InitializeComponent();
            this.lblVersion.Parent = this.pictureBox1;
            this.lblVersion.BackColor = Color.Transparent;
            //Content.Text = "程序集版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n";
            //Content.Text += "文件版本：" + Application.ProductVersion.ToString() + "\n";
            //Content.Text += "部署版本：" + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            this.lblVersion.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void ShowSplash()
        {
            //Show the form in a new thread
            _splashLauncher = new Thread(new ThreadStart(LaunchSplash));
            _splashLauncher.IsBackground = true;
            _splashLauncher.Start();
        }

        private static void LaunchSplash()
        {
            _splashScreen = new SplashScreen();
            _splashScreen.StartPosition = FormStartPosition.CenterScreen;

            //Create new message pump
            Application.Run(_splashScreen);
        }

        private static void CloseSplashDown()
        {
            Application.ExitThread();
        }

        public static void CloseSplash()
        {
            //Need to get the thread that launched the form, so
            //we need to use invoke.
            MethodInvoker mi = new MethodInvoker(CloseSplashDown);
            _splashScreen.Invoke(mi);
        }
    }
}
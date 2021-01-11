using System;
using System.Windows.Forms;

namespace TileImageViewer
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Constants.LoadSettingInfo();

            GlobalData.InitLanguage();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreen.ShowSplash();
            System.Threading.Thread.Sleep(3000);
            SplashScreen.CloseSplash();

            Application.Run(new FrmFolderSelect());
            //Application.Run(new FrmMain());
        }
    }
}
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DST.Common.WindowsAPI
{
    public class WindowsAPI
    {
        /// <summary>
        /// API：切换窗口
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        /// API：根据窗口名称查找窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string cls, string win);

        /// <summary>
        /// 根据进程名称杀掉进程
        /// </summary>
        /// <param name="processName"></param>
        public static void KillWindow(string processName)
        {
            Process[] pros = Process.GetProcessesByName(processName);
            foreach (Process item in pros)
            {
                if (IntPtr.Zero != item.MainWindowHandle)
                {
                    item.Kill();
                }
            }
        }
    }
}
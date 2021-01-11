using System;
using System.Collections.Generic;
using System.Threading;

namespace DST.Controls
{
    public static class WhirlingControlManager
    {
        /// <summary>
        /// 弹出窗体的子线程
        /// </summary>
        private static readonly Thread showWhirlingControlThread = null;

        private static object loadLock = new object();

        private static Queue<bool> WaitingForClose = new Queue<bool>();

        static WhirlingControlManager()
        {
            showWhirlingControlThread = new Thread(new ThreadStart(() =>
            {
                WhirlingControl instance = new WhirlingControl();
                EventHandler tickEvent = (s, e) =>
                {
                    if (instance.IsVisible)
                    {
                        if (WaitingForClose.Count > 0 && !WaitingForClose.Dequeue())
                        {
                            instance.Hide();
                        }
                    }
                };
                instance.SetTimerTick(tickEvent);

                while (true)
                {
                    if (WaitingForClose.Count > 0 && WaitingForClose.Dequeue())
                    {
                        instance.ShowDialog();
                    }
                    else
                    {
                        Thread.Sleep(2);
                    }
                }
            }));

            showWhirlingControlThread.Name = "进度条";
            showWhirlingControlThread.IsBackground = true;
            showWhirlingControlThread.SetApartmentState(ApartmentState.STA);
            showWhirlingControlThread.Start();
        }

        /// <summary>
        /// 显示等待对话框
        /// </summary>
        public static void ShowWaitingForm()
        {
            lock (loadLock)
            {
                WaitingForClose.Enqueue(true);
            }
        }

        /// <summary>
        /// 关闭等待窗体线程
        /// </summary>
        public static void CloseWaitingForm()
        {
            lock (loadLock)
            {
                WaitingForClose.Enqueue(false);
            }
        }
    }
}
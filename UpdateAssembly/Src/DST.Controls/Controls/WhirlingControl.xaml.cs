using System;
using System.Windows;
using System.Windows.Threading;

namespace DST.Controls
{
    /// <summary>
    /// WhirlingControl.xaml 的交互逻辑
    /// </summary>
    public partial class WhirlingControl : Window
    {
        private DispatcherTimer timer = null;             // 定时器
        private EventHandler tickEvent = null;

        public WhirlingControl()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Closing += WhirlingControl_Closing;
        }

        public void SetTimerTick(EventHandler e)
        {
            if (timer == null)
            {
                tickEvent = e;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(2);
                timer.Tick += tickEvent;
                timer.Start();
            }
        }

        private void WhirlingControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (timer != null)
            {
                if (tickEvent != null)
                {
                    timer.Tick -= tickEvent;
                    tickEvent = null;
                }
                timer.Stop();
                timer = null;
            }
        }
    }
}
using System;
using System.Drawing;

namespace TileImageViewer.Animation
{
    /// <summary>
    /// 动画抽象类
    /// </summary>
    public abstract class Animate : IDisposable
    {
        public long StartAt { get; set; } = 0;
        public double ContinueAtPercent { get; set; } = 0;

        /// <summary>
        /// 动画开始时中心点的绝对坐标
        /// </summary>
        public Point CenterAbsPoint { get; set; }

        /// <summary>
        /// 动画开始时中心点的控件坐标
        /// </summary>
        public Point CenterCtrlPoint { get; set; }

        public Boolean Finished { get; set; } = false;

        /// <summary>
        /// 动画开始时执行
        /// </summary>
        /// <param name="imgCtrl"></param>
        public abstract void ExecStartWork(ImgCtrl imgCtrl);

        /// <summary>
        /// 动画在某一时间节点时执行
        /// </summary>
        /// <param name="timestamp">时间点</param>
        /// <param name="imgCtrl"></param>
        public abstract void ExecAnimateAt(long timestamp, ImgCtrl imgCtrl);

        /// <summary>
        /// 动画结束时执行
        /// </summary>
        /// <param name="imgCtrl"></param>
        public abstract void ExecFinishWork(ImgCtrl imgCtrl);

        // 虚方法释放资源
        public virtual void Dispose()
        {
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;

namespace DST.Controls
{
    /// <summary>
    /// DatePickerRange.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerRange : UserControl
    {
        #region 依赖属性

        /// <summary>
        /// 选择开始日期
        /// </summary>
        public DateTime? SelectedStartDate
        {
            get { return (DateTime?)GetValue(SelectedStartDateProperty); }
            set { SetValue(SelectedStartDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedStartDateProperty =
            DependencyProperty.Register("SelectedStartDate", typeof(DateTime?), typeof(DatePickerRange),
            new PropertyMetadata(null));

        /// <summary>
        /// 选择结束日期
        /// </summary>
        public DateTime? SelectedEndDate
        {
            get { return (DateTime?)GetValue(SelectedEndDateProperty); }
            set { SetValue(SelectedEndDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedEndDateProperty =
            DependencyProperty.Register("SelectedEndDate", typeof(DateTime?), typeof(DatePickerRange),
            new PropertyMetadata(null));

        /// <summary>
        /// 格式化样式
        /// </summary>
        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(DatePickerRange),
            new PropertyMetadata("yyyy-MM-dd HH:mm:ss", new PropertyChangedCallback((d, e) =>
            {
                DatePickerRange dp = d as DatePickerRange;
                //string propName = e.Property.Name;
                dp.startDate.StringFormat = dp.StringFormat;
                dp.endDate.StringFormat = dp.StringFormat;
            })));

        /// <summary>
        /// 两个时间控件中间的连接文字
        /// </summary>
        public string InnerTextStr
        {
            get { return (string)GetValue(InnerTextStrProperty); }
            set { SetValue(InnerTextStrProperty, value); }
        }

        public static readonly DependencyProperty InnerTextStrProperty =
            DependencyProperty.Register("InnerTextStr", typeof(string), typeof(DatePickerRange),
            new PropertyMetadata("至", new PropertyChangedCallback((d, e) =>
            {
                DatePickerRange dp = d as DatePickerRange;
                //string propName = e.Property.Name;
                dp.innerTxt.Text = dp.InnerTextStr;
            })));

        #endregion 依赖属性

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var value = e.NewValue as DateTime?;
            //if (value.HasValue && value != DateTime.MinValue)
            //{
            //    DatePickerRange datePickerRange = d as DatePickerRange;
            //    datePickerRange.startDate.popup.IsOpen = true;
            //    datePickerRange.startDate.popup.StaysOpen = true;
            //    datePickerRange.endDate.popup.IsOpen = true;
            //    datePickerRange.endDate.popup.StaysOpen = true;
            //}
        }

        public DatePickerRange()
        {
            InitializeComponent();
            this.startDate.DateChangedEvent += CheckStartEndDate;
            this.endDate.DateChangedEvent += CheckStartEndDate;
        }

        private bool startChecked; // 是否改了start的日期
        private bool endChecked; // ...end....

        /// <summary>
        /// 检查是否需要自动弹出日期控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckStartEndDate(object sender, EventArgs e)
        {
            if (sender.Equals(this.startDate))
            {
                this.startChecked = true;
                var isShow = startChecked ^ endChecked; // 异或，相异为true，相同为false
                PopDatePicker(this.endDate, isShow);
            }
            else
            {
                this.endChecked = true;
                var isShow = startChecked ^ endChecked;
                PopDatePicker(this.startDate, isShow);
            }
        }

        /// <summary>
        /// 是否需要弹出对应的第二个日期控件
        /// </summary>
        /// <param name="datePickerPro"></param>
        /// <param name="isShow"></param>
        private void PopDatePicker(DatePickerPro datePickerPro, bool isShow)
        {
            if (isShow)
            {
                datePickerPro.imgDate_MouseLeftButtonUp(null, null); // 模拟点击日期图标
                datePickerPro.popup.IsOpen = true;
                datePickerPro.popup.Focus();
            }
            else // 两个都点击过后重置状态
            {
                if (this.startDate.SelectedDate > this.endDate.SelectedDate)
                {
                    this.popup.IsOpen = true;
                    this.popup.StaysOpen = true;
                }
                else
                {
                    this.popup.IsOpen = false;
                    this.popup.StaysOpen = false;
                }
                startChecked = false;
                endChecked = false;
            }
        }
    }
}
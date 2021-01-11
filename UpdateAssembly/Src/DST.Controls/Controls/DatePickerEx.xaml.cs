using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DST.Controls
{
    /// <summary>
    /// DatePickerEx.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerEx : UserControl
    {
        public DatePickerEx()
        {
            InitializeComponent();

            this.Loaded += DatePickerEx_Loaded;
        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        public void ResetFocus()
        {
            this.tbSplitSpecial.Focus();
        }

        private void DatePickerEx_Loaded(object sender, RoutedEventArgs e)
        {
            //对DataGridCell的特殊处理
            DataGridCell dgc = GetParentObject<DataGridCell>(this, null);
            if (dgc != null)
            {
                dgc.Focusable = false;
            }
            //设置在简易模式下设置宽度
            this.tbHourSpecial.MinWidth = InputTextWidth;
            this.tbMinuteSpecial.MinWidth = InputTextWidth;
            if (SpecialMode)
            {
                this.tbSplitSpecial.Text = ":";
                this.tbSplitSpecial.MinWidth = 10;
            }

            //设置默认值
            if (this.SelectedDate == null)
            {
                if (!IsEndTime)
                {
                    this.btnReset_Click(null, null);
                    //this.tbHour.Text = DateTime.Now.Hour.ToString().PadLeft(2, '0');
                    //this.tbMinute.Text = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                    //this.tbHourSpecial.Text = DateTime.Now.Hour.ToString().PadLeft(2, '0'); ;
                    //this.tbMinuteSpecial.Text = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                    //this.tbSecond.Text = DateTime.Now.Second.ToString().PadLeft(2, '0');
                    //this.calendar.SelectedDate = DateTime.Now;
                }
                else
                {
                    this.tbSplitSpecial.Text = "";
                }
            }
        }

        /// <summary>
        /// 确认回掉命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DatePickerEx), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DatePickerEx), new PropertyMetadata(null));

        /// <summary>
        /// 是否启用简易模式
        /// </summary>
        public bool SpecialMode
        {
            get { return (bool)GetValue(SpecialModeProperty); }
            set { SetValue(SpecialModeProperty, value); }
        }

        public static readonly DependencyProperty SpecialModeProperty =
            DependencyProperty.Register("SpecialMode", typeof(bool), typeof(DatePickerEx),
            new PropertyMetadata(false, new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 是否为结束时间
        /// </summary>
        public bool IsEndTime
        {
            get { return (bool)GetValue(IsEndTimeProperty); }
            set { SetValue(IsEndTimeProperty, value); }
        }

        public static readonly DependencyProperty IsEndTimeProperty =
            DependencyProperty.Register("IsEndTime", typeof(bool), typeof(DatePickerEx), new PropertyMetadata(false));

        /// <summary>
        /// 用户字体大小
        /// </summary>
        public int ConsumerFontSize
        {
            get { return (int)GetValue(ConsumerFontSizeProperty); }
            set { SetValue(ConsumerFontSizeProperty, value); }
        }

        public static readonly DependencyProperty ConsumerFontSizeProperty =
            DependencyProperty.Register("ConsumerFontSize", typeof(int), typeof(DatePickerEx), new PropertyMetadata(12, new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 输入框宽度
        /// </summary>
        public int InputTextWidth
        {
            get { return (int)GetValue(InputTextWidthProperty); }
            set { SetValue(InputTextWidthProperty, value); }
        }

        public static readonly DependencyProperty InputTextWidthProperty =
            DependencyProperty.Register("InputTextWidth", typeof(int), typeof(DatePickerEx), new PropertyMetadata(60));

        /// <summary>
        /// 用户前置颜色
        /// </summary>
        public Brush ConsumerForeground
        {
            get { return (Brush)GetValue(ConsumerForegroundProperty); }
            set { SetValue(ConsumerForegroundProperty, value); }
        }

        public static readonly DependencyProperty ConsumerForegroundProperty =
            DependencyProperty.Register("ConsumerForeground", typeof(Brush), typeof(DatePickerEx), new PropertyMetadata(Brushes.Black,
            new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 选择日期
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePickerEx),
            new PropertyMetadata(null, new PropertyChangedCallback(PropertyChangedCallback)));

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DatePickerEx dp = d as DatePickerEx;
            string propName = e.Property.Name;

            if (propName == "SelectedDate")
            {
                if (e.NewValue == null)
                {
                    dp.btnReset_Click(null, null);
                    return;
                }
                DateTime dtValue = e.NewValue != null ? (DateTime)e.NewValue : DateTime.Now;
                dp.tbHour.Text = dtValue.Hour.ToString().PadLeft(2, '0');
                dp.tbMinute.Text = dtValue.Minute.ToString().PadLeft(2, '0');
                dp.tbHourSpecial.Text = dtValue.Hour.ToString().PadLeft(2, '0'); ;
                dp.tbMinuteSpecial.Text = dtValue.Minute.ToString().PadLeft(2, '0');
                dp.tbSecond.Text = dtValue.Second.ToString().PadLeft(2, '0');
                dp.calendar.SelectedDate = dtValue;
                dp.tbInput.Text = e.NewValue != null ? dtValue.ToString(dp.StringFormat) : "";

                dp.gridMain.ToolTip = dtValue.ToString("yyyy-MM-dd HH:mm");
            }
            else if (propName == "StringFormat")
            {
                if (e.NewValue.ToString() == "HH:mm")
                {
                    dp.splitSecond.Visibility = Visibility.Collapsed;
                    dp.tbSecond.Visibility = Visibility.Collapsed;
                }
                else if (e.NewValue.ToString() == "yyyy-MM-dd")
                {
                    dp.inputControlPanel.Visibility = Visibility.Collapsed;
                }
                else if (e.NewValue.ToString() == "yyyy-MM-dd HH:mm")
                {
                    dp.splitSecond.Visibility = Visibility.Collapsed;
                    dp.tbSecond.Visibility = Visibility.Collapsed;
                }
            }
            else if (propName == "ConsumerFontSize")
            {
                int fontSize = (int)e.NewValue;
                dp.tbInput.FontSize = fontSize;
                dp.tbHourSpecial.FontSize = fontSize;
                dp.tbMinuteSpecial.FontSize = fontSize;
                dp.tbSplitSpecial.FontSize = fontSize;
            }
            else if (propName == "ConsumerForeground")
            {
                Brush brush = (Brush)e.NewValue;
                dp.tbInput.Foreground = brush;
            }
            else if (propName == "SpecialMode")
            {
                bool result = (bool)e.NewValue;
                if (result)
                {
                    dp.tbInput.Visibility = Visibility.Collapsed;
                    dp.spInputSpecial.Visibility = Visibility.Visible;
                    dp.inputControlPanel.Visibility = Visibility.Collapsed;
                    dp.popup.PlacementTarget = dp.spInputSpecial;
                    dp.popup.Placement = PlacementMode.Bottom;
                    dp.popup.VerticalOffset = 3;

                    dp.btnReset.Visibility = Visibility.Collapsed;
                    dp.btnConform.Width = 110;
                    dp.btnCancel.Width = 110;
                    dp.imgCancle.Visibility = Visibility.Hidden;
                    dp.tbHourSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, false);
                    dp.tbMinuteSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, false);
                }
            }
        }

        /// <summary>
        /// 格式化样式
        /// </summary>
        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(DatePickerEx),
            new PropertyMetadata("yyyy-MM-dd HH:mm:ss", new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 点击打开POPUP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbInput_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SpecialMode)
            {
                this.tbSplitSpecial.Text = ":";
                this.imgCancle.Visibility = Visibility.Visible;
                this.tbHourSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, true);
                this.tbMinuteSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, true);
            }
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                this.tbHour.Focus();
                this.tbHour.SelectAll();
                this.popup.IsOpen = true;
                if (SelectedDate == DateTime.MinValue)
                    SelectedDate = DateTime.Now;
                this.popup.StaysOpen = true;
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.popup.IsOpen = false;
            this.popup.StaysOpen = false;

            if (SpecialMode)  //隐藏按钮和下划线
            {
                this.imgCancle.Visibility = Visibility.Hidden;
                this.tbHourSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, false);
                this.tbMinuteSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, false);
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConform_Click(object sender, RoutedEventArgs e)
        {
            if (calendar.SelectedDate == null)
            {
                calendar.SelectedDate = DateTime.Now;
            }

            if (SpecialMode)
            {
                //组装时间
                DateTime selectData = DateTime.Parse(calendar.SelectedDate.ToString());
                DateTime dt = new DateTime(selectData.Year, selectData.Month, selectData.Day,
                                           int.Parse(string.IsNullOrEmpty(this.tbHourSpecial.Text) ? DateTime.Now.Hour.ToString() : this.tbHourSpecial.Text),
                                           int.Parse(string.IsNullOrEmpty(this.tbMinuteSpecial.Text) ? DateTime.Now.Minute.ToString() : this.tbMinuteSpecial.Text),
                                           0);
                //通知
                SelectedDate = dt;
            }
            else
            {
                //组装时间
                DateTime selectData = DateTime.Parse(calendar.SelectedDate.ToString());
                DateTime dt = new DateTime(selectData.Year, selectData.Month, selectData.Day,
                                           int.Parse(string.IsNullOrEmpty(this.tbHour.Text) ? DateTime.Now.Hour.ToString() : this.tbHour.Text),
                                           int.Parse(string.IsNullOrEmpty(this.tbMinute.Text) ? DateTime.Now.Minute.ToString() : this.tbMinute.Text),
                                           int.Parse(string.IsNullOrEmpty(this.tbSecond.Text) ? DateTime.Now.Second.ToString() : this.tbSecond.Text));
                //通知
                SelectedDate = dt;
                //在文本框上显示
                this.tbInput.Text = dt.ToString(StringFormat);
            }

            gridMain.ToolTip = DateTime.Parse(SelectedDate.ToString()).ToString("yyyy-MM-dd HH:mm");

            this.popup.IsOpen = false;
            this.popup.StaysOpen = false;

            if (SpecialMode)  //隐藏按钮和下划线
            {
                this.imgCancle.Visibility = Visibility.Hidden;
                this.tbHourSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, false);
                this.tbMinuteSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, false);
            }

            //触发回掉命令
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }

        /// <summary>
        /// 防止非数字输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                TextBox tb = sender as TextBox;
                if (tb.Text.Length == 0)
                {
                    //重新定位光标
                    if (tb.Name == "tbHour" && this.tbMinute.Text.Length > 0)
                    {
                        tbMinute.Focus();
                        tbMinute.SelectionStart = tbMinute.Text.Length;
                    }
                    else if (tb.Name == "tbMinuteSpecial" && this.tbHour.Text.Length > 0)
                    {
                        tbHour.Focus();
                        tbHour.SelectionStart = tbHour.Text.Length;
                    }
                }
            }
            else if (e.Key == Key.Left)
            {
                TextBox tb = sender as TextBox;
                if (tb.Name == "tbMinute" && tb.SelectionStart == 0)
                {
                    this.tbHour.Focus();
                    this.tbHour.SelectAll();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Right)
            {
                TextBox tb = sender as TextBox;
                if (tb.Name == "tbHour" && tb.SelectionStart == tb.Text.Length)
                {
                    this.tbMinute.Focus();
                    this.tbMinute.SelectAll();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Enter)
            {
                btnConform_Click(sender, e);
            }
        }

        /// <summary>
        /// 防止非数字输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!isNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// isDigit是否是数字
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static bool isNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;

            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (string.IsNullOrEmpty(tb.Text))
            {
                return;
            }

            int willText = int.Parse(tb.Text);
            int maxText = int.Parse(tb.Tag.ToString());

            if (willText > maxText)
            {
                tb.Text = maxText.ToString();
                tb.SelectionStart = tb.Text.Length;
            }

            //判断是否需要跳转到下一格子
            if (tb.Name == "tbHour")
            {
                int length = tb.Text.Length;
                if (length == 1 && int.Parse(tb.Text) > 2)
                {
                    this.tbMinute.Focus();
                    this.tbMinute.SelectAll();
                }
                else if (length == 2)
                {
                    this.tbMinute.Focus();
                    this.tbMinute.SelectAll();
                }
            }
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            btnConform_Click(sender, null);
            Mouse.Capture(null);
        }

        public T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }
                // 在上一级父控件中没有找到指定名字的控件，就再往上一级找
                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);

            if (SpecialMode)
            {
                TextBox tb = e.NewFocus as TextBox;
                if (tb != null && (tb.Name == "tbHourSpecial" || tb.Name == "tbMinuteSpecial"))
                {
                    return;
                }
            }
            else
            {
                TextBox tb = e.NewFocus as TextBox;
                if (tb != null && (tb.Name == "tbHour" || tb.Name == "tbMinute"))
                {
                    return;
                }
                if (!ContainsMouse(this.mainGrid))
                {
                    this.popup.IsOpen = false;
                    this.popup.StaysOpen = false;
                }
            }
        }

        public bool ContainsMouse(FrameworkElement grid)
        {
            Point pt = Mouse.GetPosition(grid);
            return pt.X >= 0 && pt.X <= grid.ActualWidth && pt.Y >= 0 && pt.Y <= grid.ActualHeight;
        }

        private void imgCancle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedDate = null;
            this.tbHourSpecial.Text = "";
            this.tbMinuteSpecial.Text = "";
            this.popup.IsOpen = false;
            this.popup.StaysOpen = false;

            if (SpecialMode)  //隐藏按钮和下划线
            {
                this.imgCancle.Visibility = Visibility.Hidden;
                this.tbHourSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, true);
                this.tbMinuteSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, true);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = DateTime.MinValue;
            this.tbInput.Text = "请选择日期";
            this.popup.IsOpen = false;
            this.popup.StaysOpen = false;
        }

        private void tb_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.tbHour.Text = DateTime.Now.Hour.ToString();
            this.tbHourSpecial.Text = DateTime.Now.Hour.ToString();

            this.tbMinute.Text = DateTime.Now.Minute.ToString().PadLeft(2, '0');
            this.tbMinuteSpecial.Text = DateTime.Now.Minute.ToString().PadLeft(2, '0');

            this.tbSecond.Text = DateTime.Now.Second.ToString().PadLeft(2, '0');

            this.calendar.SelectedDate = DateTime.Now;
        }

        private void tbInput_GotFocus(object sender, RoutedEventArgs e)
        {
            tbInput_PreviewMouseLeftButtonDown(sender, null);
        }
    }
}
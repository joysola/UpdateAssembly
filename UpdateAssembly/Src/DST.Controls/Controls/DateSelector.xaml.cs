using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DST.Controls
{
    /// <summary>
    /// DateSelector.xaml 的交互逻辑
    /// </summary>
    public partial class DateSelector : UserControl
    {
        /// <summary>
        /// 判断控件是否加载完毕
        /// </summary>
        private bool controlLoaded = false;

        private bool CanPopu = false;

        public DateSelector()
        {
            InitializeComponent();
            this.Loaded += DateSelector_Loaded;
        }

        #region prop

        /// <summary>
        /// 选择日期
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DateSelector),
            new PropertyMetadata(null, new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 默认是否置空值
        /// </summary>
        public bool IsDefaultValue
        {
            get { return (bool)GetValue(IsDefaultValueProperty); }
            set { SetValue(IsDefaultValueProperty, value); }
        }

        public static readonly DependencyProperty IsDefaultValueProperty =
            DependencyProperty.Register("IsDefaultValue", typeof(bool), typeof(DateSelector),
            new PropertyMetadata(true, new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 输入框宽度
        /// </summary>
        public int InputTextWidth
        {
            get { return (int)GetValue(InputTextWidthProperty); }
            set { SetValue(InputTextWidthProperty, value); }
        }

        public static readonly DependencyProperty InputTextWidthProperty =
            DependencyProperty.Register("InputTextWidth", typeof(int), typeof(DateSelector), new PropertyMetadata(60));

        /// <summary>
        /// 用户字体大小
        /// </summary>
        public int ConsumerFontSize
        {
            get { return (int)GetValue(ConsumerFontSizeProperty); }
            set { SetValue(ConsumerFontSizeProperty, value); }
        }

        public static readonly DependencyProperty ConsumerFontSizeProperty =
            DependencyProperty.Register("ConsumerFontSize", typeof(int), typeof(DateSelector), new PropertyMetadata(12, new PropertyChangedCallback(PropertyChangedCallback)));

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateSelector dp = d as DateSelector;
            string propName = e.Property.Name;

            if (propName == "ConsumerFontSize")
            {
                int fontSize = (int)e.NewValue;
                dp.tbHourSpecial.FontSize = fontSize;
                dp.tbMinuteSpecial.FontSize = fontSize;
                dp.tbSplitSpecial.FontSize = fontSize;
            }
            else if (propName == "SelectedDate" && null == dp.SelectedDate)
            {
                dp.SelectedDate = null;
                dp.tbHourSpecial.Text = string.Empty;
                dp.tbMinuteSpecial.Text = string.Empty;
            }
            else if (propName == "SelectedDate" && null != dp.SelectedDate &&
                     dp.SelectedDate != DateTime.MinValue && dp.SelectedDate != DateTime.MaxValue &&
                     string.IsNullOrEmpty(dp.tbHourSpecial.Text) && string.IsNullOrEmpty(dp.tbMinuteSpecial.Text))
            {
                DateTime dtValue = DateTime.Parse(dp.SelectedDate.ToString());
                dp.tbHourSpecial.Text = dtValue.Hour.ToString().PadLeft(2, '0');
                if (dp.tbHourSpecial.IsEnabled)
                {
                    bool tt = dp.tbHourSpecial.Focus();
                    tt = dp.tbHourSpecial.Focusable;
                }
                dp.tbMinuteSpecial.Text = dtValue.Minute.ToString().PadLeft(2, '0');
                dp.calendar.SelectedDate = dtValue;
                dp.gridMain.ToolTip = dtValue.ToString("yyyy-MM-dd HH:mm");
            }
            //else if (propName == "IsDefaultValue")
            //{
            //    bool result = (bool)e.NewValue;
            //    if (!result && null == dp.SelectedDate)
            //    {
            //        dp.SelectedDate = null;
            //        dp.tbHourSpecial.Text = string.Empty;
            //        dp.tbHourSpecial.Focus();
            //        dp.tbMinuteSpecial.Text = string.Empty;
            //    }
            //}
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
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DateSelector), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DateSelector), new PropertyMetadata(null));

        #endregion prop

        #region event

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            // this.imgCalendar.Visibility = Visibility.Visible;
            this.imgDelete.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            // this.imgCalendar.Visibility = Visibility.Collapsed;
            this.imgDelete.Visibility = Visibility.Collapsed;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void DateSelector_Loaded(object sender, RoutedEventArgs e)
        {
            this.FocusableChanged += DateSelector_FocusableChanged;
            //对DataGridCell的特殊处理
            DataGridCell dgc = GetParentObject<DataGridCell>(this, null);
            if (dgc != null)
            {
                dgc.Focusable = false;
            }

            //获取顶层空间
            DependencyObject topParent = GetTopLevelControl(this);
            UIElement uiParent = topParent as UIElement;
            if (uiParent != null)
            {
                //  uiParent.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(MouseClick));
            }

            if (SelectedDate != null && SelectedDate != DateTime.MinValue)
            {
                DateTime dtValue = DateTime.Parse(SelectedDate.ToString());
                this.tbHourSpecial.Text = dtValue.Hour.ToString().PadLeft(2, '0');
                if (this.tbHourSpecial.IsEnabled)
                {
                    this.tbHourSpecial.Focus();
                }
                this.tbMinuteSpecial.Text = dtValue.Minute.ToString().PadLeft(2, '0');
                this.calendar.SelectedDate = dtValue;
                this.gridMain.ToolTip = dtValue.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                if (!IsDefaultValue && SelectedDate == DateTime.MinValue)
                {
                    SelectedDate = null;
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        tbHourSpecial.Focus();
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }

            this.tbHourSpecial.MinWidth = InputTextWidth;
            this.tbMinuteSpecial.MinWidth = InputTextWidth;

            controlLoaded = true;
        }

        private void DateSelector_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                this.tbHourSpecial.Focus();
                this.tbHourSpecial.SelectAll();
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        //public void Focus()
        //{
        //    this.Dispatcher.BeginInvoke((Action)delegate
        //    {
        //        this.tbHourSpecial.Focus();
        //    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        //}

        public void MouseClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is DateSelector)
            {
                // this.ReleaseMouseCapture(); || this.CaptureMouse()
                return;
            }
            // this.ReleaseMouseCapture();
            if (!CanPopu)
            {
                this.popup.IsOpen = false;
                this.popup.StaysOpen = false;
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    tbHourSpecial.Focusable = true;
                    tbMinuteSpecial.Focusable = true;
                    tbHourSpecial.Focus();
                    tbHourSpecial.SelectAll();
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                CanPopu = true;
            }

            e.Handled = false;
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
                    if (tb.Name == "tbHourSpecial" && this.tbMinuteSpecial.Text.Length > 0)
                    {
                        tbMinuteSpecial.Focus();
                        tbMinuteSpecial.SelectionStart = tbMinuteSpecial.Text.Length;
                    }
                    else if (tb.Name == "tbMinuteSpecial" && this.tbHourSpecial.Text.Length > 0)
                    {
                        tbHourSpecial.Focus();
                        tbHourSpecial.SelectionStart = tbHourSpecial.Text.Length;
                    }
                }
            }
            //增加左右键add by@cc
            if (e.Key == Key.Left)
            {
                TextBox tb = sender as TextBox;
                if (tb.Name == "tbMinuteSpecial" && tb.SelectionStart == 0)
                {
                    this.tbHourSpecial.Focus();
                    this.tbHourSpecial.SelectAll();
                    e.Handled = true;
                }
            }
            if (e.Key == Key.Right)
            {
                TextBox tb = sender as TextBox;
                if (tb.Name == "tbHourSpecial" && tb.SelectionStart == tb.Text.Length)
                {
                    this.tbMinuteSpecial.Focus();
                    this.tbMinuteSpecial.SelectAll();
                    e.Handled = true;
                }
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
            if (controlLoaded == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.tbHourSpecial.Text) && string.IsNullOrEmpty(this.tbMinuteSpecial.Text))
            {
                SelectedDate = null;
                this.gridMain.ToolTip = null;
                return;
            }

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
                //控制光标
                tb.SelectionStart = tb.Text.Length;
            }
            if (calendar.SelectedDate != null)
            {
                //组装时间
                DateTime selectData = DateTime.Parse(calendar.SelectedDate.ToString());
                //DateTime dt = new DateTime(selectData.Year, selectData.Month, selectData.Day,
                //                           int.Parse(string.IsNullOrEmpty(this.tbHourSpecial.Text) ? DateTime.Now.Hour.ToString() : this.tbHourSpecial.Text),
                //                           int.Parse(string.IsNullOrEmpty(this.tbMinuteSpecial.Text) ? DateTime.Now.Minute.ToString() : this.tbMinuteSpecial.Text),
                //                           DateTime.Now.Second);
                //所有时间秒为零repair by@cc
                DateTime dt = selectData.Date
                    .AddHours(int.Parse(string.IsNullOrEmpty(this.tbHourSpecial.Text) ? DateTime.Now.Hour.ToString() : this.tbHourSpecial.Text))
                    .AddMinutes(int.Parse(string.IsNullOrEmpty(this.tbMinuteSpecial.Text) ? DateTime.Now.Minute.ToString() : this.tbMinuteSpecial.Text));
                //通知
                SelectedDate = dt;
                this.gridMain.ToolTip = dt.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                calendar.SelectedDate = DateTime.Now;
            }

            //判断是否需要跳转到下一格子
            if (tb.Name == "tbHourSpecial")
            {
                int length = tb.Text.Length;
                if (length == 1 && int.Parse(tb.Text) > 2)
                {
                    this.tbMinuteSpecial.Focus();
                    this.tbMinuteSpecial.SelectAll();
                }
                else if (length == 2)
                {
                    this.tbMinuteSpecial.Focus();
                    this.tbMinuteSpecial.SelectAll();
                }
            }

            //触发回掉命令
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }

        /// <summary>
        /// 点击打开POPUP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbInput_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedDate == null)
            {
                this.tbHourSpecial.Text = DateTime.Now.Hour.ToString().PadLeft(2, '0');
                this.tbMinuteSpecial.Text = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                this.calendar.SelectedDate = DateTime.Now;
                SelectedDate = DateTime.Now;
            }

            this.tbSplitSpecial.Text = ":";
            this.tbHourSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, true);
            this.tbMinuteSpecial.SetValue(ControlAttachProperty.ShowUnderlineProperty, true);

            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }

        private void tb_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.tbHourSpecial.Text = DateTime.Now.Hour.ToString();
            this.tbMinuteSpecial.Text = DateTime.Now.Minute.ToString().PadLeft(2, '0');
        }

        private void imgCalendar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tbHourSpecial.Focusable = this.popup.IsOpen;
            this.tbMinuteSpecial.Focusable = this.popup.IsOpen;
            this.popup.StaysOpen = !this.popup.IsOpen;
            this.popup.IsOpen = !this.popup.IsOpen;
        }

        private void imgDelete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectedDate = null;
            this.tbHourSpecial.Text = string.Empty;
            this.tbMinuteSpecial.Text = string.Empty;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (controlLoaded == false)
            {
                return;
            }
            //组装时间
            if (calendar.SelectedDate != null)
            {
                DateTime selectData = DateTime.Parse(calendar.SelectedDate.ToString());
                //DateTime dt = new DateTime(selectData.Year, selectData.Month, selectData.Day,
                //                           int.Parse(string.IsNullOrEmpty(this.tbHourSpecial.Text) ? DateTime.Now.Hour.ToString() : this.tbHourSpecial.Text),
                //                           int.Parse(string.IsNullOrEmpty(this.tbMinuteSpecial.Text) ? DateTime.Now.Minute.ToString() : this.tbMinuteSpecial.Text),
                //                           DateTime.Now.Second);
                //所有时间秒为零repair by@cc
                DateTime dt = selectData.Date
                    .AddHours(int.Parse(string.IsNullOrEmpty(this.tbHourSpecial.Text) ? DateTime.Now.Hour.ToString() : this.tbHourSpecial.Text))
                    .AddMinutes(int.Parse(string.IsNullOrEmpty(this.tbMinuteSpecial.Text) ? DateTime.Now.Minute.ToString() : this.tbMinuteSpecial.Text));
                //通知
                SelectedDate = dt;
                this.gridMain.ToolTip = dt.ToString("yyyy-MM-dd HH:mm");
                popup.IsOpen = false;
                popup.StaysOpen = false;
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    tbHourSpecial.Focusable = true;
                    tbMinuteSpecial.Focusable = true;
                    tbHourSpecial.Focus();
                    if (tbHourSpecial.Text.Length == 1)
                    {
                        tbHourSpecial.SelectionStart = 1;
                    }
                    else if (string.IsNullOrEmpty(tbHourSpecial.Text))
                    {
                        tbHourSpecial.SelectionStart = 0;
                    }
                    else
                        tbHourSpecial.SelectAll();
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                //触发回掉命令
                if (Command != null)
                {
                    Command.Execute(CommandParameter);
                }
            }

            Mouse.Capture(null);
        }

        private void tbHourSpecial_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectAll();
            }
        }

        private void calendar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Path)
            {
                this.popup.IsOpen = false;
                this.popup.StaysOpen = false;
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    tbHourSpecial.Focusable = true;
                    tbMinuteSpecial.Focusable = true;
                    tbHourSpecial.Focus();
                    tbHourSpecial.SelectAll();
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
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

        private DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                parent = tmp;
            }
            return parent;
        }

        #endregion event

        private void calendar_MouseLeave(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this.calendar);
            IInputElement input = calendar.InputHitTest(point);
            if (point.X < 0 || point.Y < 0 || input == null)
            {
                DependencyObject topParent = GetTopLevelControl(this);
                UIElement uiParent = topParent as UIElement;
                if (uiParent != null)
                {
                    uiParent.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(MouseClick));
                }
                CanPopu = false;
            }
            else
            {
                CanPopu = true;
            }
            //this.popup.IsOpen = false;
            //this.popup.StaysOpen = false;
        }

        private void calendar_MouseEnter(object sender, MouseEventArgs e)
        {
            CanPopu = true;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DST.Controls
{
    /// <summary>
    /// 可搜索和可输入的下拉列表
    /// </summary>
    public class ComboBoxEx : ComboBox
    {
        private bool hasSelectedChanged = false;

        static ComboBoxEx()
        {
            //预防下面写法的内存泄漏
            TextProperty.OverrideMetadata(typeof(ComboBoxEx),
                        new FrameworkPropertyMetadata(
                                String.Empty,
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                                new PropertyChangedCallback(OnTextChanged)));
        }

        //DependencyPropertyDescriptor descriptorText = DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(ComboBoxEx));
        private TextBox editTextBox = null;

        public ComboBoxEx()
        {
            this.IsTextSearchEnabled = false;
            this.IsKeyDown = false;
            this.IsTextChanged = false;
            this.IsSelectedChanged = false;
            this.SelectionChanged += ComboBoxEx_SelectionChanged;
            this.LostFocus += ComboBoxEx_LostFocus;
            this.DropDownClosed += ComboBoxEx_DropDownClosed;
            this.DropDownOpened += ComboBoxEx_DropDownOpened;// 用于显示工号和姓名等
        }

        private List<object> displayMemberPathValueList = new List<object>();// 存放DisplayMemberPath的值

        /// <summary>
        /// 下拉选项中显示DisplayMemberPath和OtherDisplayMemberPath对应的组合值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxEx_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                this.editTextBox.IsReadOnly = false;

                if (!string.IsNullOrEmpty(this.DisplayMemberPath) &&
                    !string.IsNullOrEmpty(this.OtherDisplayMemberPath))
                {
                    // 可以存放多个属性，用逗号隔开
                    string[] disPath = this.OtherDisplayMemberPath.Split(new string[] { "," },
                                                                    StringSplitOptions.RemoveEmptyEntries);
                    // 例如MED_HIS_USERS需要显示NAME、USER_JOB_ID
                    if (disPath.Length > 0)
                    {
                        foreach (var item in this.OriginalItemSource)
                        {
                            string temp = string.Empty;
                            // 以DisplayMemberPath绑定的字段为主
                            PropertyInfo mainPro = item.GetType().GetProperty(this.DisplayMemberPath);
                            object mainProValue = mainPro.GetValue(item, null);
                            if (null != mainProValue)
                            {
                                temp = mainProValue.ToString();
                            }

                            // 获取OtherDisplayMemberPath字段对应的实际值
                            for (int i = 0; i < disPath.Length; i++)
                            {
                                PropertyInfo tempPro = item.GetType().GetProperty(disPath[i]);
                                if (null != tempPro.GetValue(item, null))
                                {
                                    temp += " " + tempPro.GetValue(item, null).ToString();// DisplayMemberPath和OtherDisplayMemberPath的组合
                                }
                            }

                            mainPro.SetValue(item, temp);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void ComboBoxEx_DropDownClosed(object sender, EventArgs e)
        {
            this.editTextBox.IsReadOnly = this.CustomReadOnly;// 关闭下拉，直接不可编辑
            // 当窗口关闭时，检查当前下拉选项的数据源，以备鼠标点击下拉。
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                if (SelectedIndex >= 0 && !((this.OriginalItemSource as IList).Count == (this.ItemsSource as IList).Count || (this.ItemsSource as IList).Count >= MaxSourceCount))
                {
                    List<object> list = new List<object>();
                    list.Add(this.SelectedItem);
                    foreach (var item in this.OriginalItemSource)
                    {
                        if (item != SelectedItem)
                        {
                            list.Add(item);
                        }
                    }
                    this.ItemsSource = GetTopData(list);
                    this.SelectedIndex = 0;
                }
                //
                try
                {
                    if (!string.IsNullOrEmpty(this.DisplayMemberPath) &&
                        !string.IsNullOrEmpty(this.OtherDisplayMemberPath))
                    {
                        foreach (var item in this.OriginalItemSource)
                        {
                            PropertyInfo mainPro = item.GetType().GetProperty(this.DisplayMemberPath);
                            object mainValue = mainPro.GetValue(item, null);

                            PropertyInfo otherPro = item.GetType().GetProperty(this.OtherDisplayMemberPath);
                            object otherValue = otherPro.GetValue(item, null);

                            if (mainValue != null && otherValue != null)
                            {
                                string strMainValue = mainValue.ToString();
                                string strOtherValue = otherValue.ToString();
                                strMainValue = strMainValue.Replace(strOtherValue, "").Trim();
                                mainPro.SetValue(item, strMainValue);
                            }
                        }

                        if (null != this.SelectedValueEx && this.hasSelectedChanged)
                        {
                            this.Text = this.Text.Replace(this.SelectedValueEx.ToString(), "").Trim();// 界面上选中后，只显示DisplayMemberPath对应的值
                        }

                        if (string.IsNullOrEmpty(this.SelectedValueEx) && this.CustomReadOnly)
                        {
                            this.Text = null;
                            this.IsKeyDown = true;
                            UpdateDataSources();
                        }
                    }
                }
                catch
                {
                }
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void ComboBoxEx_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = false;
            this.editTextBox.IsReadOnly = this.CustomReadOnly;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (editTextBox == null)
            {
                editTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            }
            if (editTextBox != null)
            {
                if (TextCenter)
                    editTextBox.TextAlignment = TextAlignment.Center;
                editTextBox.PreviewKeyDown -= editTextBox_PreviewKeyDown;
                editTextBox.PreviewKeyDown += editTextBox_PreviewKeyDown;
                editTextBox.TextChanged -= editTextBox_TextChanged;
                editTextBox.TextChanged += editTextBox_TextChanged;
                editTextBox.IsReadOnly = this.CustomReadOnly;
            }
            //if (descriptorText != null)
            //{
            //    descriptorText.RemoveValueChanged(this, Actual_ValueChanged);
            //    descriptorText.AddValueChanged(this, Actual_ValueChanged);
            //}
        }

        // When the Text Property changes, search for an item exactly
        // matching the new text and set the selected index to that item
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxEx cb = (ComboBoxEx)d;

            cb.Dispatcher.BeginInvoke((Action)delegate
            {
                try
                {
                    if (cb.SelectedIndex == -1 && cb.SelectedValue == null)
                    {
                        cb.IsSelectedChanged = true;
                        if (!cb.CustomReadOnly)
                        {
                            cb.SelectedValueEx = cb.Text;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(cb.Text))
                            {
                                cb.SelectedValueEx = null;
                            }
                        }
                    }
                }
                finally
                {
                    cb.IsSelectedChanged = false;
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        //private void Actual_ValueChanged(object sender, EventArgs args)
        //{
        //    this.Dispatcher.BeginInvoke((Action)delegate
        //    {
        //        try
        //        {
        //            if (this.SelectedIndex == -1 && this.SelectedValue == null)
        //            {
        //                this.IsSelectedChanged = true;
        //                this.SelectedValueEx = this.Text;
        //            }
        //        }
        //        finally
        //        {
        //            this.IsSelectedChanged = false;
        //        }
        //    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        //}

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            KeyDownOrUp = true;
            if (this.IsDropDownOpen && e.Key == Key.Enter)
            {
                if (this.Items.Count == 1)
                {
                    this.SelectedIndex = 0;
                }
            }
            base.OnPreviewKeyDown(e);
        }

        private void editTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyDownOrUp = true;

            if (this.CustomReadOnly && !this.IsDropDownOpen && e.Key == Key.Back && !this.hasSelectedChanged)
            {
                this.editTextBox.IsReadOnly = false;
            }
            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                case Key.F4:
                case Key.Escape:
                case Key.Enter:
                case Key.Home:
                case Key.End:
                case Key.Right:
                case Key.Left:
                case Key.PageUp:
                case Key.PageDown:
                case Key.Oem5:
                    break;

                default:
                    this.IsKeyDown = true;
                    this.IsTextChanged = false;
                    UpdateDataSources();
                    break;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                KeyDownOrUp = false;
            }
            base.OnMouseDown(e);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                KeyDownOrUp = false;
            }
            base.OnPreviewMouseDown(e);
        }

        private void editTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.IsTextChanged = true;
            UpdateDataSources();
        }

        private void UpdateDataSources()
        {
            if (this.IsKeyDown && this.IsTextChanged)
            {
                this.IsKeyDown = false;
                this.IsTextChanged = false;

                this.hasSelectedChanged = false;

                // show the drop down when user starts typing then stops
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    List<object> lo = new List<object>();

                    string searchText = this.Text;

                    if (string.IsNullOrEmpty(searchText))
                    {
                        this.IsDropDownOpen = false;
                        this.editTextBox.IsReadOnly = this.CustomReadOnly;

                        if (this.OriginalItemSource != null)
                        {
                            //清除对TextChanged事件的变化。
                            editTextBox.TextChanged -= editTextBox_TextChanged;
                            //如果原来的选中值还没有清空，则清空他。
                            if (this.SelectedIndex >= 0)
                            {
                                this.SelectedIndex = -1;
                            }
                            this.ItemsSource = GetTopData(this.OriginalItemSource);
                            editTextBox.TextChanged += editTextBox_TextChanged;
                        }
                    }
                    else if (this.OriginalItemSource != null && !string.IsNullOrEmpty(SearchFieldName))
                    {
                        int i = 0;
                        //对ItemSource进行筛选
                        foreach (var item in this.OriginalItemSource)
                        {
                            PropertyInfo pi = item.GetType().GetProperty(SearchFieldName);
                            object pyValue = pi.GetValue(item, null);
                            var val = Convert.ToString(pyValue);
                            if (pyValue != null && val.ToUpper().IndexOf(searchText.ToUpper()) >= 0)
                            {
                                // 对于检索数据长度较短的内容，则优先显示。
                                if (searchText.Length == val.Length)
                                {
                                    lo.Insert(i++, item);
                                }
                                else
                                {
                                    lo.Add(item);
                                }
                            }
                        }

                        //清除对TextChanged事件的变化。
                        editTextBox.TextChanged -= editTextBox_TextChanged;
                        //如果原来的选中值还没有清空，则清空他。
                        if (this.SelectedIndex >= 0)
                        {
                            this.SelectedIndex = -1;
                            //如果之前有选中值，再次绑定数据源时，必须重新下拉，否则无法通过方向键选中
                            this.IsDropDownOpen = false;
                            this.editTextBox.IsReadOnly = this.CustomReadOnly;
                        }
                        this.ItemsSource = GetTopData(lo);
                        if (this.Text != searchText)
                        {
                            this.Text = searchText;
                            this.editTextBox.CaretIndex = searchText.Length;
                        }
                        if (lo.Count > 0 && !this.IsDropDownOpen)
                        {
                            this.IsEditable = false;

                            this.IsDropDownOpen = true;
                            if (this.Text != searchText)
                            {
                                this.Text = searchText;
                            }

                            this.IsEditable = true;
                        }
                        else if (lo.Count == 0)
                        {
                            this.IsDropDownOpen = false;
                            this.editTextBox.IsReadOnly = this.CustomReadOnly;

                            if (this.OriginalItemSource != null)
                            {
                                this.ItemsSource = GetTopData(this.OriginalItemSource);
                            }
                        }

                        editTextBox.TextChanged += editTextBox_TextChanged;
                    }
                }, System.Windows.Threading.DispatcherPriority.Send);
            }
        }

        private bool KeyDownOrUp = false;

        private void ComboBoxEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.editTextBox.IsReadOnly = this.CustomReadOnly;

            this.IsSelectedChanged = true;

            this.hasSelectedChanged = true;
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                try
                {
                    if (this.SelectedIndex == -1 && this.SelectedValue == null)
                    {
                        if (!this.CustomReadOnly)
                        {
                            this.SelectedValueEx = this.Text;
                        }
                    }
                    else
                    {
                        this.SelectedValueEx = Convert.ToString(this.SelectedValue);
                        this.editTextBox.SelectAll();
                    }

                    if (this.ClickedSelectionChanged != null && !this.KeyDownOrUp)
                    {
                        this.ClickedSelectionChanged(sender, e);
                    }
                    KeyDownOrUp = false;
                }
                finally
                {
                    this.IsSelectedChanged = false;
                }
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        /// <summary>
        /// 作为搜索的字段
        /// </summary>
        public string SearchFieldName
        {
            get { return (string)GetValue(SearchFieldNameProperty); }
            set { SetValue(SearchFieldNameProperty, value); }
        }

        public bool TextCenter
        {
            get { return (bool)GetValue(TextCenterProperty); }
            set { SetValue(TextCenterProperty, value); }
        }

        public static readonly DependencyProperty SearchFieldNameProperty =
            DependencyProperty.Register("SearchFieldName", typeof(string), typeof(ComboBoxEx), new PropertyMetadata(null));

        public static readonly DependencyProperty TextCenterProperty =
            DependencyProperty.Register("TextCenter", typeof(bool), typeof(ComboBoxEx), new PropertyMetadata(null));

        /// <summary>
        /// 下拉框显示其他信息，例如MED_HIS_USERS表需要同时显示USER_NAME和USER_JOB_ID
        /// 则DisplayMemberPath 设置 USER_NAME， OtherDisplayMemberPath 设置 USER_JOB_ID
        /// 移植自河南省人民医院
        /// </summary>
        public string OtherDisplayMemberPath
        {
            get { return (string)GetValue(OtherDisplayMemberPathProperty); }
            set { SetValue(OtherDisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty OtherDisplayMemberPathProperty =
            DependencyProperty.Register("OtherDisplayMemberPath", typeof(string), typeof(ComboBoxEx),
            new PropertyMetadata(null));

        /// <summary>
        /// 作为原始数据源
        /// </summary>
        public IEnumerable OriginalItemSource
        {
            get { return (IEnumerable)GetValue(OriginalItemSourceProperty); }
            set { SetValue(OriginalItemSourceProperty, value); }
        }

        public static readonly DependencyProperty OriginalItemSourceProperty =
            DependencyProperty.Register("OriginalItemSource", typeof(IEnumerable), typeof(ComboBoxEx), new PropertyMetadata(null, new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 只读不可编辑标识。默认为false
        /// </summary>
        public bool CustomReadOnly
        {
            get { return (bool)GetValue(CustomReadOnlyProperty); }
            set { SetValue(CustomReadOnlyProperty, value); }
        }

        /// <summary>
        /// 只读不可编辑标识。默认为false
        /// </summary>
        public static readonly DependencyProperty CustomReadOnlyProperty =
            DependencyProperty.Register("CustomReadOnly", typeof(bool), typeof(ComboBoxEx), new PropertyMetadata(false));

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxEx cbe = d as ComboBoxEx;
            if (e.Property.Name == "OriginalItemSource")
            {
                var val = cbe.SelectedValueEx;

                cbe.ItemsSource = GetTopData(((IList)e.NewValue));

                if (val != cbe.SelectedValueEx)
                {
                    cbe.SelectedValueEx = val;
                }
            }
            else if (e.Property.Name == "SelectedValueEx")
            {
                if (!cbe.IsSelectedChanged)
                {
                    cbe.Dispatcher.BeginInvoke((Action)delegate
                    {
                        //修复绑定值不在下拉列表的情况下。
                        string path = string.IsNullOrEmpty(cbe.SelectedValuePath) ? cbe.DisplayMemberPath : cbe.SelectedValuePath;
                        if (!string.IsNullOrEmpty(path) && cbe.OriginalItemSource != null && cbe.editTextBox != null)
                        {
                            cbe.editTextBox.TextChanged -= cbe.editTextBox_TextChanged;
                            bool isFinded = false;
                            string text = Convert.ToString(e.NewValue) ?? "";
                            object selectItem = null;
                            foreach (var item in cbe.OriginalItemSource)
                            {
                                PropertyInfo pi = item.GetType().GetProperty(path);
                                if (pi == null) break;
                                object pyValue = pi.GetValue(item, null);
                                var val = Convert.ToString(pyValue);
                                if (pyValue != null && val == text)
                                {
                                    isFinded = true;
                                    selectItem = item;
                                    break;
                                }
                            }
                            if (!isFinded)
                            {
                                cbe.Text = text;
                            }
                            else
                            {
                                List<object> list = new List<object>();
                                list.Add(selectItem);
                                foreach (var item in cbe.OriginalItemSource)
                                {
                                    if (item != selectItem)
                                    {
                                        list.Add(item);
                                    }
                                }
                                cbe.ItemsSource = GetTopData(list);
                                cbe.SelectedIndex = 0;
                            }

                            cbe.editTextBox.TextChanged += cbe.editTextBox_TextChanged;
                        }
                        else
                        {
                            cbe.SelectedValue = e.NewValue;
                            if (e.NewValue == null || string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue)))
                            {
                                cbe.Text = "";
                            }
                            else if (e.NewValue != cbe.SelectedValue)
                            {
                                if (e.NewValue == null || e.NewValue.GetType() == typeof(string))
                                {
                                    cbe.Text = Convert.ToString(e.NewValue) ?? "";
                                }
                                else
                                {
                                }
                            }
                        }
                    }, System.Windows.Threading.DispatcherPriority.Send);
                }
                cbe.Dispatcher.BeginInvoke((Action)delegate
                {
                    if (cbe.ValueExChanged != null)
                    {
                        ValueExArgs args = new ValueExArgs(cbe.SelectedValueEx);
                        cbe.ValueExChanged(cbe, args);
                    }
                }, System.Windows.Threading.DispatcherPriority.Send);
            }
        }

        public string SelectedValueEx
        {
            get { return (string)GetValue(SelectedValueExProperty); }
            set { SetValue(SelectedValueExProperty, value); }
        }

        #region Events and Delegates

        /// <summary>
        /// Used to pass arguments for autocomplete control
        /// </summary>
        public class ValueExArgs : EventArgs
        {
            private string _valueEx;

            public string ValueEx
            {
                get
                {
                    return this._valueEx;
                }
            }

            public ValueExArgs(string valueEx)
            {
                this._valueEx = valueEx;
            }
        }

        public delegate void ValueExChangedHandler(ComboBox sender, ValueExArgs args);

        public event ValueExChangedHandler ValueExChanged;

        /// <summary>
        /// 鼠标点击选中选项事件
        /// </summary>
        public event SelectionChangedEventHandler ClickedSelectionChanged;

        #endregion Events and Delegates

        public static readonly DependencyProperty SelectedValueExProperty =
            DependencyProperty.Register("SelectedValueEx", typeof(object), typeof(ComboBoxEx), new PropertyMetadata(null, new PropertyChangedCallback(PropertyChangedCallback)));

        private const int MaxSourceCount = 100;

        /// <summary>
        /// 超过100条数据只获取前100条数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static IList GetTopData(IEnumerable dataSource)
        {
            List<object> lo = new List<object>();

            if (dataSource != null)
            {
                int count = 1;
                foreach (var item in dataSource)
                {
                    if (count > MaxSourceCount)
                    {
                        break;
                    }
                    lo.Add(item);
                    count++;
                }
            }

            return lo;
        }

        private bool IsKeyDown
        {
            get { return _cacheValid[(int)CacheBits.IsKeyDown]; }
            set { _cacheValid[(int)CacheBits.IsKeyDown] = value; }
        }

        private bool IsTextChanged
        {
            get { return _cacheValid[(int)CacheBits.IsTextChanged]; }
            set { _cacheValid[(int)CacheBits.IsTextChanged] = value; }
        }

        private bool IsSelectedChanged
        {
            get { return _cacheValid[(int)CacheBits.IsSelectedChanged]; }
            set { _cacheValid[(int)CacheBits.IsSelectedChanged] = value; }
        }

        private BitVector32 _cacheValid = new BitVector32(0);   // Condense boolean bits

        private enum CacheBits
        {
            IsKeyDown = 0x01,
            IsTextChanged = 0x02,
            IsSelectedChanged = 0x04,
            //AAA = 0x08,
            //BBB = 0x10,
            //CCC = 0x20,
        }
    }
}
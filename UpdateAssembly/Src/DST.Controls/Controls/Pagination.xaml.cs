using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DST.Controls
{
    /// <summary>
    /// Pagination.xaml 的交互逻辑
    /// </summary>
    public partial class Pagination : UserControl
    {
        private bool needChange = true; // 是否需要触发PaginPageChangedEvent

        public static RoutedEvent FirstPageEvent;
        public static RoutedEvent PreviousPageEvent;
        public static RoutedEvent NextPageEvent;
        public static RoutedEvent LastPageEvent;
        public static RoutedEvent PaginPageChangedEvent;

        public static readonly DependencyProperty CurrentPageProperty;
        public static readonly DependencyProperty TotalPageProperty;

        public static readonly DependencyProperty PageSizeProperty;
        public static readonly DependencyProperty CBSelectedItemProperty;

        public string CurrentPage
        {
            get { return (string)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public string TotalPage
        {
            get { return (string)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public int CBSelectedItem
        {
            get { return (int)GetValue(CBSelectedItemProperty); }
            set { SetValue(CBSelectedItemProperty, value); }
        }

        public Pagination()
        {
            InitializeComponent();
            this.Loaded += Pagination_Loaded;
        }

        private void Pagination_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbPaginPage.SelectionChanged += this.CbPaginPage_SelectionChanged;
        }

        static Pagination()
        {
            FirstPageEvent = EventManager.RegisterRoutedEvent("FirstPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));
            PreviousPageEvent = EventManager.RegisterRoutedEvent("PreviousPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));
            NextPageEvent = EventManager.RegisterRoutedEvent("NextPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));
            LastPageEvent = EventManager.RegisterRoutedEvent("LastPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));
            PaginPageChangedEvent = EventManager.RegisterRoutedEvent("PaginPageChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));
            CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(string), typeof(Pagination), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnCurrentPageChanged)));
            TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(string), typeof(Pagination), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTotalPageChanged)));
            PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(Pagination), new PropertyMetadata(15));
            CBSelectedItemProperty = DependencyProperty.Register("CBSelectedItem", typeof(int), typeof(Pagination), new PropertyMetadata(0,
                (dp, e) =>
                {
                    var pn = (Pagination)dp;
                    var newValue = (int)(e.NewValue);
                    if (newValue != 0 && pn.PageSize != newValue)
                    {
                        var cbSelectedItem = pn.cbPaginPage.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString().Contains(newValue.ToString()));
                        if (cbSelectedItem != null)
                        {
                            pn.needChange = false;
                            pn.cbPaginPage.SelectedItem = cbSelectedItem;
                            pn.needChange = true;
                        }
                    }
                }));
        }

        public event RoutedEventHandler FirstPage
        {
            add { AddHandler(FirstPageEvent, value); }
            remove { RemoveHandler(FirstPageEvent, value); }
        }

        public event RoutedEventHandler PreviousPage
        {
            add { AddHandler(PreviousPageEvent, value); }
            remove { RemoveHandler(PreviousPageEvent, value); }
        }

        public event RoutedEventHandler NextPage
        {
            add { AddHandler(NextPageEvent, value); }
            remove { RemoveHandler(NextPageEvent, value); }
        }

        public event RoutedEventHandler LastPage
        {
            add { AddHandler(LastPageEvent, value); }
            remove { RemoveHandler(LastPageEvent, value); }
        }

        public event RoutedEventHandler PaginPageChanged
        {
            add { AddHandler(PaginPageChangedEvent, value); }
            remove { RemoveHandler(PaginPageChangedEvent, value); }
        }

        public static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination p = d as Pagination;

            if (p != null)
            {
                Run rTotal = (Run)p.FindName("rTotal");

                rTotal.Text = (string)e.NewValue;
            }
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination p = d as Pagination;

            if (p != null)
            {
                Run rCurrrent = (Run)p.FindName("rCurrent");

                rCurrrent.Text = (string)e.NewValue;
            }
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FirstPageEvent, this));
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PreviousPageEvent, this));
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NextPageEvent, this));
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LastPageEvent, this));
        }

        /// <summary>
        /// 每页条数切换
        /// </summary>
        private void CbPaginPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBoxItem cbi = this.cbPaginPage.SelectedItem as System.Windows.Controls.ComboBoxItem;
            this.PageSize = int.Parse(cbi.Content.ToString().Replace("条/页", "").Trim());
            if (needChange)
            {
                RaiseEvent(new RoutedEventArgs(PaginPageChangedEvent, this));
            }
            this.FirstPageButton.Focus();
        }
    }
}
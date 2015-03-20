using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WpfCommon.Controls
{
    /// <summary>
    ///     Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public static readonly DependencyProperty ApplicationNameAndVersionProperty =
            DependencyProperty.Register("ApplicationNameAndVersion", typeof (object), typeof (AboutDialog),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty CopyrightProperty = DependencyProperty.Register("Copyright",
            typeof (object), typeof (AboutDialog), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading",
            typeof (bool),
            typeof (AboutDialog), new PropertyMetadata(false));

        private bool _isLoading;

        public AboutDialog()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            ApplicationNameAndVersion = fvi.ProductName + " v" + fvi.FileVersion;
            Copyright = fvi.LegalCopyright;

            var icon = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName);
            Icon = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                BitmapSizeOptions.FromEmptyOptions());

            InitializeComponent();
            Loaded += (sender, args) => { WebBrowser.DocumentText = HtmlDescription; };
        }

        public object ApplicationNameAndVersion
        {
            get { return GetValue(ApplicationNameAndVersionProperty); }
            set { SetValue(ApplicationNameAndVersionProperty, value); }
        }

        public string HtmlDescription { get; set; }

        public object Copyright
        {
            get { return GetValue(CopyrightProperty); }
            set { SetValue(CopyrightProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        private void WebBrowser_OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var wb = (WebBrowser) sender;
            wb.ScriptErrorsSuppressed = true;
            IsLoading = false;
        }

        private void WebBrowser_OnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            IsLoading = true;
        }
    }
}
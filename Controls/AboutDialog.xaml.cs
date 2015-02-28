using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using WebBrowser = System.Windows.Controls.WebBrowser;

//For Icon

namespace WpfCommon.Controls
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public static readonly DependencyProperty ApplicationNameAndVersionProperty = DependencyProperty.Register("ApplicationNameAndVersion", typeof (object), typeof (AboutDialog), new PropertyMetadata(default(object)));
        public static readonly DependencyProperty CopyrightProperty = DependencyProperty.Register("Copyright", typeof (object), typeof (AboutDialog), new PropertyMetadata(default(object)));

        public AboutDialog()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            ApplicationNameAndVersion = fvi.ProductName + " v" + fvi.FileVersion;
            Copyright = fvi.LegalCopyright;

            var icon = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName);
            this.Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                BitmapSizeOptions.FromEmptyOptions());

            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                WebBrowser.DocumentText = HtmlDescription;
            };
        }

        public object ApplicationNameAndVersion
        {
            get { return (object) GetValue(ApplicationNameAndVersionProperty); }
            set { SetValue(ApplicationNameAndVersionProperty, value); }
        }

        public string HtmlDescription { get; set; }

        public object Copyright
        {
            get { return (object) GetValue(CopyrightProperty); }
            set { SetValue(CopyrightProperty, value); }
        }

        private void WebBrowser_OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var wb = (System.Windows.Forms.WebBrowser)sender;
            wb.ScriptErrorsSuppressed = true;
        }

        private void WebBrowser_OnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!e.Url.ToString().StartsWith("about:blank"))
            {
                e.Cancel = true;
                Process.Start(e.Url.ToString());
            }
        }
    }
}

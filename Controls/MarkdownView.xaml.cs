using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Nicologies.WpfCommon.Controls
{
    /// <summary>
    ///     Interaction logic for MarkdownView.xaml
    /// </summary>
    public partial class MarkdownView
    {
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading",
            typeof (bool),
            typeof(MarkdownView), new PropertyMetadata(false));


        public MarkdownView()
        {
            InitializeComponent();
        }

        private string _HtmlContent;

        public string HtmlContent
        {
            get
            {
                return _HtmlContent;
            }
            set
            {
                _HtmlContent = value;
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, 
                    new Action(() => WebBrowser.DocumentText = HtmlContent));
            }
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
            if (OpenLinkInExternalBrowser && !e.Url.ToString().StartsWith("about:blank"))
            {
                e.Cancel = true;
                Process.Start(e.Url.ToString());
                IsLoading = false;
                return;
            }
            IsLoading = true;
        }

        public bool OpenLinkInExternalBrowser = false;
    }
}
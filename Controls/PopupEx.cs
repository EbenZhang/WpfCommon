using System;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using WpfCommon.Utils;

namespace WpfCommon.Controls
{
    public class PopupEx : Popup
    {
        protected override void OnOpened(EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, 
                new Action(() => ZOrderHelper.SetNonTopMost(this.Child)));
        }
    }
}

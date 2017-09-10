using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Nicologies.WpfCommon.Controls
{
    [ComVisible(true), Guid("D81F90A3-8156-44F7-AD28-5ABB87003274"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IProtectFocus
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        int AllowFocusChange([In] [Out] ref bool pfAllow);
    }

    [ComVisible(true), Guid("6d5140c1-7436-11ce-8034-00aa006009fa"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IServiceProvider
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        int QueryService([In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);
    }

    public class WebBrowserCtrl : WebBrowser
    {
        protected sealed class WebBrowserControlSite : WebBrowser.WebBrowserSite, IServiceProvider, IProtectFocus
        {
            public WebBrowserControlSite(WebBrowser host) : base(host)
            {
            }
            public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
            {
                var b = new Guid("D81F90A3-8156-44F7-AD28-5ABB87003274");
                ppvObject = IntPtr.Zero;
                var result = -2147467262; /* E_NOINTERFACE */
                if (riid == b)
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IProtectFocus));
                    result = 0; /* S_OK */
                }
                return result;
            }
            public int AllowFocusChange(ref bool pfAllow)
            {
                pfAllow = false;
                return 0; /* S_OK */
            }
        }
        
        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            return new WebBrowserCtrl.WebBrowserControlSite(this);
        }

        //private static readonly int WM_KEYDOWN = 256;
        //public override bool PreProcessMessage(ref Message msg)
        //{
        //    if (msg.Msg == WebBrowserControl.WM_KEYDOWN)
        //    {
        //        if (msg.WParam.ToInt32() == 116)
        //        {
        //            return true;
        //        }
        //        if (msg.WParam.ToInt32() == 70 && Control.ModifierKeys == Keys.Control)
        //        {
        //            this.OnCtrlFPressed?.Invoke();
        //            return true;
        //        }
        //        if (msg.WParam.ToInt32() == 83 && Control.ModifierKeys == Keys.Control)
        //        {
        //            this.OnCtrlSPressed?.Invoke();
        //            return true;
        //        }
        //    }
        //    return base.PreProcessMessage(ref msg);
        //}
    }
}

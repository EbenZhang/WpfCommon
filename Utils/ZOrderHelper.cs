using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WpfCommon.Utils
{
    class ZOrderHelper
    {
        private const int SWP_NOMOVE = 2;
        private const int SWP_NOREDRAW = 8;
        private const int SWP_NOSIZE = 1;
        private static int HWND_NOTOPMOST = -2;

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, 
            int x, int y, int cx, int cy, int wFlags);

        public static void SetNonTopMost(Visual ctrl)
        {
            // ReSharper disable once PossibleNullReferenceException
            SetWindowPos(((HwndSource)PresentationSource.FromVisual(ctrl)).Handle,
                    HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOREDRAW | SWP_NOSIZE);
        }
    }
}

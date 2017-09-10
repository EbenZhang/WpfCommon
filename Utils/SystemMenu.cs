/**
 * Copied from
 * http://www.codeguru.com/csharp/csharp/cs_misc/userinterface/article.php/c9327/Manipulating-the-System-Menu-Using-C.htm
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Nicologies.WpfCommon.Utils
{
    public class NoSystemMenuException : Exception
    {
    }

    // Values taken from MSDN.
    public enum ItemFlags
    {
        // The item ...
        MfUnchecked = 0x00000000, // ... is not checked
        MfString = 0x00000000, // ... contains a string as label
        MfDisabled = 0x00000002, // ... is disabled
        MfGrayed = 0x00000001, // ... is grayed
        MfChecked = 0x00000008, // ... is checked
        MfPopup = 0x00000010, // ... Is a popup menu. Pass the menu handle 
        //     of the popup menu into the ID parameter.
        MfBarBreak = 0x00000020, // ... is a bar break
        MfBreak = 0x00000040, // ... is a break
        MfByPosition = 0x00000400, // ... is identified by the position
        MfByCommand = 0x00000000, // ... is identified by it's ID
        MfSeparator = 0x00000800 // ... is a seperator (String and ID parameters
        //     are ignored).
    }

    public enum WindowMessages
    {
        WmSysCommand = 0x0112
    }

    /// <summary>
    ///     A class that helps to manipulate the system menu
    ///     of a passed form.
    ///     Written by Florian "nohero" Stinglmayr
    /// </summary>
    public class SystemMenu
    {
        private static readonly Dictionary<IntPtr, Func<int, bool>> _menuHandlers = new Dictionary<IntPtr, Func<int, bool>>(); 

        private IntPtr _sysMenu = IntPtr.Zero; // Handle to the System Menu
        // I havn't found any other solution than using plain old
        // WinAPI to get what I want.
        // If you need further information on these functions, their
        // parameters and their meanings you should look them up in
        // the MSDN.

        // All parameters in the [DllImport] should be self explaining.
        // NOTICE: Use never stdcall as calling convention, since Winapi is used.
        // If the underlying structure changes, your program might causing errors
        // that are hard to find.

        // At first we need the GetSystemMenu() function. 
        // This function does not have an Unicode brother
        [DllImport("USER32", EntryPoint = "GetSystemMenu", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr apiGetSystemMenu(IntPtr windowHandle, int bReset);

        // And we need the AppendMenu() function. Since .NET uses Unicode
        // we pick the unicode solution.
        [DllImport("USER32", EntryPoint = "AppendMenuW", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.Winapi)]
        private static extern int apiAppendMenu(IntPtr menuHandle, int Flags, int NewID, String Item);

        // And we may also need the InsertMenu() function.
        [DllImport("USER32", EntryPoint = "InsertMenuW", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.Winapi)]
        private static extern int apiInsertMenu(IntPtr hMenu, int position, int Flags, int NewId, String Item);

        // Insert a separator at the given position index starting at zero.
        public bool InsertSeparator(int pos)
        {
            return (InsertMenu(pos, ItemFlags.MfSeparator | ItemFlags.MfByPosition, 0, ""));
        }

        // Simplified InsertMenu(), that assumes that Pos is relative
        // position index starting at zero
        public bool InsertMenu(int pos, int id, String item)
        {
            return (InsertMenu(pos, ItemFlags.MfByPosition | ItemFlags.MfString, id, item));
        }

        // Insert a menu at the given position. The value of the position depends
        // on the value of Flags. See in the article for a detail description.
        public bool InsertMenu(int pos, ItemFlags flags, int id, String item)
        {
            return (apiInsertMenu(_sysMenu, pos, (Int32) flags, id, item) == 0);
        }

        // Appends a seperator
        public bool AppendSeparator()
        {
            return AppendMenu(0, "", ItemFlags.MfSeparator);
        }

        // This uses the ItemFlags.mfString as default value
        public bool AppendMenu(int id, String item)
        {
            return AppendMenu(id, item, ItemFlags.MfString);
        }

        // Superseded function.
        public bool AppendMenu(int id, String item, ItemFlags flags)
        {
            return (apiAppendMenu(_sysMenu, (int) flags, id, item) == 0);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="onMenuClicked">Accepts MenuId, returns true if handled</param>
        /// <returns></returns>
        public static SystemMenu FromWnd(Window wnd, Func<int, bool> onMenuClicked)
        {
            var windowHandle = new WindowInteropHelper(wnd).Handle;
            var cSysMenu = new SystemMenu {_sysMenu = apiGetSystemMenu(windowHandle, 0)};

            if (cSysMenu._sysMenu == IntPtr.Zero)
            {
                // Throw an exception on failure
                throw new NoSystemMenuException();
            }
            
            var src = HwndSource.FromHwnd(windowHandle);
            src.AddHook(WndProc);

            _menuHandlers.Add(windowHandle, onMenuClicked);

            wnd.Closed += (sender, args) => RemoveHook(windowHandle);

            return cSysMenu;
        }

        private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // address the messages you are receiving using msg, wParam, lParam
            if (msg == (int)WindowMessages.WmSysCommand)
            {
                Func<int, bool> callback;
                if (_menuHandlers.TryGetValue(hWnd, out callback))
                {
                    handled = callback(wParam.ToInt32());
                }
            }
            return IntPtr.Zero;
        }

        // Reset's the window menu to it's default
        public static void ResetSystemMenu(Window wnd)
        {
            var windowHandle = new WindowInteropHelper(wnd).Handle;
            apiGetSystemMenu(windowHandle, 1);

            RemoveHook(windowHandle);
        }

        private static void RemoveHook(IntPtr windowHandle)
        {
            var src = HwndSource.FromHwnd(windowHandle);
            src.RemoveHook(WndProc);

            _menuHandlers.Remove(windowHandle);
        }
    }
}
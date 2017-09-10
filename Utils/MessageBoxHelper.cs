using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Nicologies.WpfCommon.Utils
{
    public static class MessageBoxHelper
    {
        public static void ShowError(Window parent, string msg, string title="Error")
        {
            MessageBox.Show(parent, msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static MessageBoxResult ShowConfirmation(Window parent, string msg, string title = "Confirm")
        {
            return MessageBox.Show(parent, msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        public static void ShowInfo(Window parent, string msg, string title = "Info")
        {
            MessageBox.Show(parent, msg, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

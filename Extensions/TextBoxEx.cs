using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Nicologies.WpfCommon.Validations;

namespace Nicologies.WpfCommon.Extensions
{
    public static class TextBoxEx
    {
        public static bool Valid(this TextBox tb)
        {
            return BindingValidator.Valid(tb, TextBox.TextProperty);
        }
    }
}

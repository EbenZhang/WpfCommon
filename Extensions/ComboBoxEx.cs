﻿using System.Windows;
using System.Windows.Controls;
using Nicologies.WpfCommon.Validations;

namespace Nicologies.WpfCommon.Extensions
{
    public static class ComboBoxEx
    {
        public static bool Valid(this ComboBox combo, DependencyProperty property)
        {
            return BindingValidator.Valid(combo, property);
        }
        public static bool Valid(this ComboBox combo)
        {
            return BindingValidator.Valid(combo, ComboBox.SelectedItemProperty);
        }
    }
}

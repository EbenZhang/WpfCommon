using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Nicologies.WpfCommon.Validations
{
    public class HasSelectionValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Please select an item");
            }
            return new ValidationResult(true, null);
        }
    }
}

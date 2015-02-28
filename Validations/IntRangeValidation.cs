using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfCommon.Validations
{
    public class IntRangeValidator : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public IntRangeValidator()
        {
            MinValue = 0;
            MaxValue = int.MaxValue;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Integer required.");
            }

            var intValue = 0;
            if (!int.TryParse(value.ToString(), out intValue))
            {
                return new ValidationResult(false, "Integer required.");
            }
            if (intValue <= MaxValue && intValue >= MinValue)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, string.Format("Input must in range [{0}, {1}]", MinValue, MaxValue));
            }
        }
    }
}

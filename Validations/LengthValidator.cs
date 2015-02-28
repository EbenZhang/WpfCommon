using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfCommon.Validations
{
    public class MinLengthValidator : ValidationRule
    {
        public int? MinLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (MinLength == null)
            {
                throw new NullReferenceException("Min Length Is Required");
            }

            var str = (string)value ?? "";
            return str.Length < MinLength ? new ValidationResult(false, string.Format("Minimal Length Is {0}", MinLength))
                : new ValidationResult(true, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WpfCommon.Extensions
{
    public static class TextBoxEx
    {
        public static bool Valid(this TextBox tb)
        {
            if (!tb.IsEnabled || !tb.IsVisible)
            {
                return true;
            }
			var binding = BindingOperations.GetBinding(tb, TextBox.TextProperty);
            if (binding == null) return true;

            var bingdExp = tb.GetBindingExpression(TextBox.TextProperty);
            if (bingdExp == null) return true;

            Validation.ClearInvalid(bingdExp);
            foreach(var rule in binding.ValidationRules)
            {
                var tempRuleForClosureCapture = rule;
                    var validResult = rule.Validate(tb.Text, CultureInfo.CurrentCulture);

                if (validResult.IsValid) continue;

                tb.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    tb.BringIntoView();
                    var validationError = new ValidationError(tempRuleForClosureCapture, binding, validResult.ErrorContent, null);
                    Validation.MarkInvalid(bingdExp, validationError);
                    tb.Focus();
                }));
                return false;
            }
            return true;
        }
    }
}

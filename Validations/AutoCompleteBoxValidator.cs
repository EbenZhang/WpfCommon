using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WpfCommon.Validations
{
    public static class AutoCompleteBoxValidator
    {
        public static bool Valid(this AutoCompleteBox box)
        {
            return ValidText(box) && ValidSelectedItem(box);
        }

        private static bool ValidText(AutoCompleteBox box)
        {
            var binding = BindingOperations.GetBinding(box, AutoCompleteBox.TextProperty);
            if (binding == null) return true;

            var bingdExp = box.GetBindingExpression(AutoCompleteBox.TextProperty);
            if (bingdExp == null) return true;

            Validation.ClearInvalid(bingdExp);

            if (!box.IsEnabled || !box.IsVisible)
            {
                return true;
            }
            foreach (var rule in binding.ValidationRules)
            {
                var tempRuleForClosureCapture = rule;
                var validResult = rule.Validate(box.Text, CultureInfo.CurrentCulture);

                if (validResult.IsValid) continue;

                box.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    box.BringIntoView();
                    var validationError = new ValidationError(tempRuleForClosureCapture, binding, validResult.ErrorContent, null);
                    Validation.MarkInvalid(bingdExp, validationError);
                    box.Focus();
                }));
                return false;
            }
            return true;
        }

        private static bool ValidSelectedItem(AutoCompleteBox box)
        {
            var binding = BindingOperations.GetBinding(box, AutoCompleteBox.TextProperty);
            if (binding == null) return true;

            var bingdExp = box.GetBindingExpression(AutoCompleteBox.TextProperty);
            if (bingdExp == null) return true;

            if (!box.IsEnabled || !box.IsVisible)
            {
                return true;
            }

            Validation.ClearInvalid(bingdExp);
            foreach (var rule in binding.ValidationRules)
            {
                var tempRuleForClosureCapture = rule;
                var validResult = rule.Validate(box.Text, CultureInfo.CurrentCulture);

                if (validResult.IsValid) continue;

                box.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    box.BringIntoView();
                    var validationError = new ValidationError(tempRuleForClosureCapture, binding, validResult.ErrorContent, null);
                    Validation.MarkInvalid(bingdExp, validationError);
                    box.Focus();
                }));
                return false;
            }
            return true;
        }
    }
}

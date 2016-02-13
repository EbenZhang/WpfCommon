using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WpfCommon.Validations
{
    public static class BindingValidator
    {
        public static bool Valid(FrameworkElement obj, DependencyProperty property)
        {
            var binding = BindingOperations.GetBinding(obj, property);
            if (binding == null) return true;

            var bingdExp = obj.GetBindingExpression(property);
            if (bingdExp == null) return true;

            Validation.ClearInvalid(bingdExp);

            if (!obj.IsEnabled || !obj.IsVisible)
            {
                return true;
            }
            foreach(var rule in binding.ValidationRules)
            {
                var tempRuleForClosureCapture = rule;
                var validResult = rule.Validate(obj.GetValue(property), CultureInfo.CurrentCulture);

                if (validResult.IsValid) continue;

                obj.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    obj.BringIntoView();
                    var validationError = new ValidationError(tempRuleForClosureCapture, binding,
                        validResult.ErrorContent, null);
                    Validation.MarkInvalid(bingdExp, validationError);
                    obj.Focus();
                }));
                return false;
            }
            return true;
        }
    }
}

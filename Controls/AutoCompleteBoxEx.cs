using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfCommon.Controls
{
    public class AutoCompleteBoxEx : AutoCompleteBox
    {
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(AutoCompleteBoxEx));

        public AutoCompleteBoxEx()
        {
            this.Focusable = true;
            this.GotFocus += new RoutedEventHandler(AutoCompleteBox_GotFocus);
            this.SetResourceReference(StyleProperty, typeof(AutoCompleteBox));
        }

        /// <summary>
        /// Set the max length of the textbox when it got focus (visible)
        /// </summary>
        void AutoCompleteBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textbox = Template.FindName("Text", this) as TextBox;
            if (textbox != null)
            {
                if (!textbox.IsFocused)
                {
                    Dispatcher.BeginInvoke(new Action(() => textbox.Focus()));
                }
                textbox.MaxLength = MaxLength;
            }
        }

        /// <summary>
        /// Rewrite this method to focus to the inner textbox.
        /// </summary>
        public new void Focus()
        {
            var textbox = Template.FindName("Text", this) as TextBox;
            if (textbox != null)
            {
                textbox.Focus();
            }
        }

        /// <summary>
        /// Length limit of the inner text box.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return (int)this.GetValue(MaxLengthProperty);
            }
            set
            {
                this.SetValue(MaxLengthProperty, value);
            }
        }

        /// <summary>
        /// Override this method to enable the enter, esc and etc key.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!IsDropDownOpen)
            {
                base.OnKeyDown(e);
                e.Handled = false;
                return;
            }
            base.OnKeyDown(e);
        }

        public bool Valid()
        {
            return ValidText() && ValidSelectedItem();
        }

        private bool ValidText()
        {
            var box = this;
            var binding = BindingOperations.GetBinding(box, TextProperty);
            if (binding == null) return true;

            var bingdExp = box.GetBindingExpression(TextProperty);
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

        private bool ValidSelectedItem()
        {
            var box = this;
            var binding = BindingOperations.GetBinding(box, TextProperty);
            if (binding == null) return true;

            var bingdExp = box.GetBindingExpression(TextProperty);
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

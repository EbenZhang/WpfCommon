using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfCommon.Extensions;

namespace WpfCommon.Utils
{
    public static class UiTreeHelper
    {
        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
            {
                return default(T);
            }
            var t = parent as T;
            return t ?? FindVisualParent<T>(parent);
        }

        public static T FindVisualChild<T>(DependencyObject container) where T : Visual
        {
            var t = default(T);
            var childrenCount = VisualTreeHelper.GetChildrenCount(container);
            for (var i = 0; i < childrenCount; i++)
            {
                var visual = VisualTreeHelper.GetChild(container, i);
                t = (visual as T) ?? FindVisualChild<T>(visual);
                if (t != null)
                {
                    break;
                }
            }
            return t;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject container) where T : DependencyObject
        {
            if (container == null) yield break;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(container); i++)
            {
                var child = VisualTreeHelper.GetChild(container, i);
                if (child != null && child is T)
                {
                    yield return (T) ((object) child);
                }
                foreach (var current in FindVisualChildren<T>(child))
                {
                    yield return current;
                }
            }
            yield break;
        }

        public static IEnumerable<T> FindLogicChildren<T>(DependencyObject container) where T : DependencyObject
        {
            if (container == null) yield break;
            foreach (var current in LogicalTreeHelper.GetChildren(container))
            {
                if (current != null && current is T)
                {
                    yield return (T) ((object) current);
                }
                if (!(current is DependencyObject)) continue;
                foreach (T current2 in FindLogicChildren<T>(current as DependencyObject))
                {
                    yield return current2;
                }
            }
            yield break;
        }

        public static T FindLogicChild<T>(DependencyObject container) where T : DependencyObject
        {
            return container != null
                ? (from object current in LogicalTreeHelper.GetChildren(container)
                    where current != null && current is T
                    select current as T).FirstOrDefault()
                : default(T);
        }

        public static bool IsVisualChildOf(DependencyObject container, DependencyObject child)
        {
            if (container == null || child == null)
            {
                return false;
            }
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(container); i++)
            {
                var child2 = VisualTreeHelper.GetChild(container, i);
                if (child == child2)
                {
                    return true;
                }
                if (IsVisualChildOf(child2, child))
                {
                    return true;
                }
            }
            return false;
        }

        public static T FindLogicalParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = LogicalTreeHelper.GetParent(child);
            if (parent == null)
            {
                return default(T);
            }
            var t = parent as T;
            return t ?? FindLogicalParent<T>(parent);
        }

        public static bool ValidateTextBoxes(this DependencyObject container)
        {
            var tbs = FindVisualChildren<TextBox>(container);
            return tbs.All(tb => tb.Valid());
        }
    }
}

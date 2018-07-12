using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

/// <summary>
/// 
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Sets the selected value.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="selectedValue">The selected value.</param>
    /// <returns></returns>
    public static IEnumerable<SelectListItem> kSetSelectedValue(this IEnumerable<SelectListItem> list, object selectedValue)
    {
        return kSetSelectedValue(list, selectedValue, null);
    }

    /// <summary>
    /// Sets the selected value.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="selectedValue">The selected value.</param>
    /// <param name="includeBlankOption">The include blank option.</param>
    /// <returns></returns>
    public static IEnumerable<SelectListItem> kSetSelectedValue(this IEnumerable<SelectListItem> list, object selectedValue, string includeBlankOption)
    {
        if (list == null)
            throw new ArgumentNullException("list");

        var codes = new List<SelectListItem>();

        if (includeBlankOption != null)
        {
            if (selectedValue != null)
            {
                codes.Add(new SelectListItem { Text = includeBlankOption, Value = "-1", Selected = selectedValue.Equals("-1") });
            }
            else
            {
                codes.Add(new SelectListItem { Text = includeBlankOption, Value = "-1", Selected = true });
            }
        }

        string selValue = Convert.ToString(selectedValue);

        foreach (SelectListItem enumValue in list)
        {
            codes.Add(new SelectListItem
            {
                Text = enumValue.Text,
                Value = enumValue.Value,
                Selected = selValue.Equals(enumValue.Value, StringComparison.OrdinalIgnoreCase)
            });
        }
        return codes;
    }

    /// <summary>
    /// Sets the selected text in a listof select items.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="selectedText">The selected text.</param>
    /// <returns></returns>
    public static IEnumerable<SelectListItem> kSetSelectedText(this IEnumerable<SelectListItem> list, object selectedText)
    {
        return kSetSelectedText(list, selectedText, null);
    }

    /// <summary>
    /// Sets the selected text.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="selectedText">The selected text.</param>
    /// <param name="includeBlankOption">The include blank option.</param>
    /// <returns></returns>
    public static IEnumerable<SelectListItem> kSetSelectedText(this IEnumerable<SelectListItem> list, object selectedText, string includeBlankOption)
    {
        if (list == null) throw new ArgumentNullException("list");
        var codes = new List<SelectListItem>();

        if (includeBlankOption != null)
        {
            if (selectedText != null)
            {
                codes.Add(new SelectListItem { Text = includeBlankOption, Value = "-1", Selected = selectedText.Equals("-1") });
            }
            else
            {
                codes.Add(new SelectListItem { Text = includeBlankOption, Value = "-1", Selected = true });
            }
        }
        string selText = Convert.ToString(selectedText);
        foreach (SelectListItem enumValue in list)
        {
            codes.Add(new SelectListItem
            {
                Text = enumValue.Text,
                Value = enumValue.Value,
                Selected = selText.Equals(enumValue.Text, StringComparison.OrdinalIgnoreCase)
            });
        }
        return codes;
    }

    /// <summary>
    /// Gets distinct values from a specified list of generic objects.
    /// </summary>
    /// <typeparam name="T">The type of the T.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="uniqueCheckerMethod">The unique checker method.</param>
    /// <returns></returns>
    public static IEnumerable<T> kDistinct<T>(this IEnumerable<T> source, Func<T, object> uniqueCheckerMethod)
    {
        return source.Distinct(new GenericComparer<T>(uniqueCheckerMethod));
    }

    /// <summary>
    /// Internal funcion used to compare objects for DISTINCT operation
    /// </summary>
    /// <typeparam name="T">The type of the T.</typeparam>
    class GenericComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="uniqueCheckerMethod">The unique checker method.</param>
        public GenericComparer(Func<T, object> uniqueCheckerMethod)
        {
            this.uniqueCheckerMethod = uniqueCheckerMethod;
        }

        private Func<T, object> uniqueCheckerMethod;

        /// <summary>
        /// compares the object
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return this.uniqueCheckerMethod(x).Equals(this.uniqueCheckerMethod(y));
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns></returns>
        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return this.uniqueCheckerMethod(obj).GetHashCode();
        }
    }
}

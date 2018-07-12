using System;
using System.Linq;
using System.Web.Mvc;


/// <summary>
/// View Data attribute extensions
/// </summary>
public static class ViewDataAttributes
{
    /// <summary>
    /// Gets the model attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the T attribute.</typeparam>
    /// <param name="viewData">The view data.</param>
    /// <param name="inherit">The inherit.</param>
    /// <returns></returns>
    public static TAttribute kGetModelAttribute<TAttribute>(this ViewDataDictionary viewData, bool inherit = false) where TAttribute : Attribute
    {
        if (viewData == null) throw new ArgumentException("ViewData");
        var containerType = viewData.ModelMetadata.ContainerType;

        return ((TAttribute[])containerType.GetProperty(viewData.ModelMetadata.PropertyName).GetCustomAttributes(typeof(TAttribute), inherit)).FirstOrDefault();
    }
}

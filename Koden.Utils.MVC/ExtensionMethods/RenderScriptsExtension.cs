using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

/// <summary>
/// Used in MVC to render scripts specific to Koden Standardization
/// </summary>
public static class RenderScriptExtensions
{
    /// <summary>
    /// Renders the scripts contained in the "DAScripts" context ViewBag.  A standard Viewbag to hold information for custom Telerik controls, etc
    /// </summary>
    /// <param name="htmlHelper">The HTML helper.</param>
    /// <returns></returns>
    public static IHtmlString kRenderScripts(this HtmlHelper htmlHelper)
    {
        var scripts = htmlHelper.ViewContext.HttpContext.Items["DAScripts"] as IList<string>;
        if (scripts != null && htmlHelper.ViewBag.IsReadOnly != true)
        {
            var builder = new StringBuilder("<script type='text/javascript'>$(document).ready(function () {{");

            foreach (var script in scripts)
            {
                builder.AppendLine(script);
            }
            builder.AppendLine("}});</script>");
            return new MvcHtmlString(builder.ToString());
        }
        return null;
    }
}

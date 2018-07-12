using System.Configuration;
using System.Web.Mvc;
using System.Web.UI;


/// <summary>
/// Extensions to manipulate/parse URLs and inject correct urls into javascript
/// </summary>
public static class UrlExtensions
{
    /// <summary>
    /// HTML Helper to retrieve directory for TRIAD image files
    /// </summary>
    /// <param name="helper">The helper.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns> relative path to image file</returns>
    public static string kImage(this UrlHelper helper, string fileName)
    {
        return string.Format("~/Content/img/{0}", fileName);
    }

    /// <summary>
    /// HTML Helper to retrieve directory for TRIAD image directory
    /// </summary>
    /// <param name="helper">The helper.</param>
    /// <returns>
    /// relative path to image directory
    /// </returns>
    public static string kImageDir(this UrlHelper helper)
    {
        return helper.Content(string.Format("~/Content/img/"));
    }

    /// <summary>
    /// HTML Helper to retrieve directory for webservices
    /// </summary>
    /// <param name="helper">The helper.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="pPage">The page.</param>
    /// <returns>relative path to webservices</returns>
    public static string kWebService(this UrlHelper helper, string fileName, Page pPage)
    {
        return pPage.ResolveClientUrl(string.Format("~/WebServices/{0}", fileName));
    }

    /// <summary>
    /// HTML Helper to retrieve directory for javascript files
    /// </summary>
    /// <param name="helper">The helper.</param>
    /// <param name="fileName">Name of the javafile.</param>
    /// <param name="pPage">The page.</param>
    /// <returns>relative path to javascript file</returns>
    public static string kJavaScript(this UrlHelper helper, string fileName, Page pPage)
    {
        return pPage.ResolveClientUrl(string.Format("~/Content/js/{0}", fileName));
    }

    /// <summary>
    /// HTML Helper to retrieve directory for telerik Theme files
    /// </summary>
    /// <param name="helper">The helper.</param>
    /// <param name="pPage">The page.</param>
    /// <returns>relative path to theme directory</returns>
    public static string kTheme(this UrlHelper helper, Page pPage)
    {
        var themeName = "flick";
        if (ConfigurationManager.AppSettings["Theme"] != null)
        {
            themeName = ConfigurationManager.AppSettings["Theme"];
        }
        return string.Format("~/Content/Themes/{0}/ui.all.css", themeName);
    }

    /// <summary>
    /// HTML Helper to find a blank icon.  Hey - it may be needed... :)
    /// </summary>
    /// <param name="helper">The helper.</param>
    /// <returns>relative path to the noIcon.png file</returns>
    public static string kNoIcon(this UrlHelper helper)
    {
        return kImage(helper, "noIcon.png");
    }

}

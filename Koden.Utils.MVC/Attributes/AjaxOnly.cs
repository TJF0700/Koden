using System;
using System.Web.Mvc;

namespace Koden.Utils.MVC.Attributes
{
    /// <summary>
    /// Force Action to be called via Ajax only - no direct call allowed.
    /// </summary>
    public class AjaxOnly : ActionFilterAttribute
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                throw new Exception("Invalid call to method. Unable to execute");
            }
        }
    }
}

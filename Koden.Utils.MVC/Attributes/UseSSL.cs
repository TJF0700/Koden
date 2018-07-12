using System.Web.Mvc;
using System.Web.Routing;

namespace Koden.Utils.MVC.Attributes
{
    /// <summary>
    /// Attribute used to ensure SSL is used during the method call
    /// </summary>
    public class UseSSL : ActionFilterAttribute
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.DataTokens["isSecure"] == null)
            {
                string controller = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
                string action = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
                var tmpRoute = new Route(controller + "/" + action, new MvcRouteHandler())
                {
                    Defaults = new RouteValueDictionary(new { controller, action }),
                    DataTokens = new RouteValueDictionary(new { isSecure = true })
                };

                RouteTable.Routes.Insert(0, tmpRoute);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller, action }));
            }
        }
    }
}
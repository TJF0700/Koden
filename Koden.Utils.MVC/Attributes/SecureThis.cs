using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Koden.Utils.MVC.Attributes
{
    /// <summary>
    /// Checks the User's authentication using FormsAuthentication
    /// and redirects to the Login Url for the application on fail
    /// </summary>
    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if not authenticated
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //use the current url for the redirect
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;

                //send them off to the login page
                string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;


                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }

    /// <summary>
    /// Secure This attribute ensures that a single Method can only be called by a certain level of authorization
    /// </summary>
    public class SecureThisAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the require authentication.
        /// </summary>
        /// <value>The require authentication.</value>
        public bool RequireAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        public string Task { get; set; }

        /// <summary>
        /// Called before an action method executes and checks the role of the user to ensure they can use the called method.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!RequireAuthentication && string.IsNullOrEmpty(Task))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            bool authorized = false;

            if ((filterContext.HttpContext.User != null) && (!String.IsNullOrEmpty(filterContext.HttpContext.User.Identity.Name)))
            {
                if (!String.IsNullOrEmpty(Task))
                {
                    if (filterContext.HttpContext.User.IsInRole(Task))
                    {
                        authorized = true;
                    }
                }
                else if (RequireAuthentication && filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    authorized = true;
                }
            }

            if (!authorized)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                        {
                            { "langCode", filterContext.RouteData.Values[ "langCode" ] },
                            { "controller", "Account" },
                            { "action", "Logon" },
                            { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                        });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
using System.Web.Mvc;

namespace Koden.Utils.MVC.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    {
        /// <summary>
        /// The key
        /// </summary>
        protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExportModelStateToTempData : ModelStateTempDataTransfer
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Only export when ModelState is not valid
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //Export if we are redirecting
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
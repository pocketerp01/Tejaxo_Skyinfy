using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorFilter());
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new SessionFilter());
        }
    }

    // In your filters folder (create this)
    public class ErrorFilter : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            System.Exception exception = filterContext.Exception;

                string controller = filterContext.RouteData.Values["controller"].ToString(); ;
            string action = filterContext.RouteData.Values["action"].ToString();
            //var model = filterContext.Controller.TempData["model"];

            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {
                // Determine the return type of the action
                string actionName = filterContext.RouteData.Values["action"].ToString();
                Type controllerType = filterContext.Controller.GetType();

                var returnType = typeof(ActionResult);

                // If the action that generated the exception returns JSON
                if (returnType.Equals(typeof(JsonResult)))
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = "DATA not returned"
                    };
                }

                // If the action that generated the exception returns a view
                if (returnType.Equals(typeof(ActionResult))
                    || (returnType).IsSubclassOf(typeof(ActionResult)))
                {
                    filterContext.Result = new ViewResult
                    {
                        //ViewName = "Error",
                        //ViewData = new ViewDataDictionary(model)
                    };
                }
            }

            // Make sure that we mark the exception as handled
            filterContext.ExceptionHandled = true;
            //filterContext.ExceptionHandled = false;
        }
    }

    public class GZipOrDeflateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting
             (ActionExecutingContext filterContext)
        {
            string acceptencoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(acceptencoding))
            {
                acceptencoding = acceptencoding.ToLower();
                var response = filterContext.HttpContext.Response;
                if (acceptencoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-Encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter,
                                          CompressionMode.Compress);
                }
                else if (acceptencoding.Contains("deflate"))
                {
                    response.AppendHeader("Content-Encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter,
                                      CompressionMode.Compress);
                }
            }
        }
    }
}

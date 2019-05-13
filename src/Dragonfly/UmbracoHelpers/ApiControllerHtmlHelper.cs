namespace Dragonfly.UmbracoHelpers
{
    using System.IO;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Dragonfly.UmbracoModels.MvcFakes;

    public static class ApiControllerHtmlHelper
    {
        private static ControllerContext ApiControllerToMvcController(HttpControllerContext ThisControllerContext, HttpContext CurrentHttpContext)
        {
            var apiRouteData = ThisControllerContext.RequestContext.RouteData;

            var routeData = new RouteData();
            routeData.Values.Add("controller", apiRouteData.Values["controller"]);  //must match your Controller name
            routeData.Values.Add("action", apiRouteData.Values["action"]);  //must match your Action name

            var fakeController = new FakeController();

            HttpContextWrapper wrapper = new HttpContextWrapper(CurrentHttpContext);

            var controllerContext = new ControllerContext(wrapper, routeData, fakeController);

            return controllerContext;
        }

        //public static HtmlHelper GenericHtmlHelper(HttpControllerContext ThisControllerContext)
        //{
        //    var apiRouteData = ThisControllerContext.RequestContext.RouteData;

        //    var routeData = new RouteData();
        //    routeData.Values.Add("controller", apiRouteData.Values["controller"]);  //must match your Controller name
        //    routeData.Values.Add("action", apiRouteData.Values["action"]);  //must match your Action name

        //    var fakeController = new FakeController();

        //    var fCC = new FakeControllerContext(fakeController);
        //    var controllerContext = new ControllerContext(fCC.HttpContext, routeData, fakeController);

        //    //HttpContextWrapper wrapper = new HttpContextWrapper(CurrentHttpContext);

        //    var vc = new ViewContext(controllerContext, new FakeView(), new ViewDataDictionary(),
        //        new TempDataDictionary(), new StringWriter());

        //    var vp = new ViewPage();

        //    var htmlHelper = new HtmlHelper(vc, vp);

        //    return htmlHelper;

        //}

        /// <summary>
        /// Gets the HTML from a partial View from a Controller
        /// </summary>
        /// <param name="ThisControllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="viewData"></param>
        /// <param name="currentContext"></param>
        /// <returns></returns>
        public static string GetPartialViewHtml(HttpControllerContext ThisControllerContext, string viewName, ViewDataDictionary viewData, HttpContext currentContext)
        {
            var mvcControllerContext = ApiControllerToMvcController(ThisControllerContext, currentContext);
            var viewResult = ViewEngines.Engines.FindView(mvcControllerContext, viewName, null);

            StringWriter stringWriter;
            if (viewData == null)
            {
                viewData = new ViewDataDictionary();
            }

            using (stringWriter = new StringWriter())
            {
                var viewContext = new ViewContext(
                    mvcControllerContext,
                    viewResult.View,
                    viewData,
                    mvcControllerContext.Controller.TempData,
                    stringWriter);

                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(mvcControllerContext, viewResult.View);
            }

            return stringWriter.ToString();
        }

    }
}

namespace Dragonfly.UmbracoHelpers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    
    public static class Mvc
    {
        private const string ThisClassName = "Dragonfly.UmbracoHelpers.Mvc";

        #region GetSafeViewData

        public static object GetSafeViewData(ViewDataDictionary VdDictionary, string Key, object DefaultNullValue = null)
        {
            if (DefaultNullValue == null)
            {
                DefaultNullValue = "";
            }
            return VdDictionary[Key] != null ? VdDictionary[Key] : DefaultNullValue;
        }

        //public static T GetSafeViewData<T>(ViewDataDictionary VdDictionary, string Key)
        //{
        //    if (DefaultNullValue == null)
        //    {
        //        DefaultNullValue = new T();
        //    }

        //    return VdDictionary[Key] != null ? VdDictionary[Key] : DefaultNullValue;
        //}

        public static string GetSafeViewDataString(ViewDataDictionary VdDictionary, string Key, string DefaultNullValue = "")
        {
            var returnValue = VdDictionary[Key] != null ? VdDictionary[Key].ToString() : DefaultNullValue;

            return returnValue;
        }

        public static int GetSafeViewDataInt(ViewDataDictionary VdDictionary, string Key, int DefaultNullValue = 0)
        {
            var val = VdDictionary[Key] != null ? VdDictionary[Key] : DefaultNullValue;

            int returnValue = DefaultNullValue;

            var test = Int32.TryParse(val.ToString(), out returnValue);

            return returnValue;
        }

        public static double GetSafeViewDataDouble(ViewDataDictionary VdDictionary, string Key, Double DefaultNullValue = 0)
        {
            var val = VdDictionary[Key] != null ? VdDictionary[Key] : DefaultNullValue;

            double returnValue = DefaultNullValue;

            var test = Double.TryParse(val.ToString(), out returnValue);

            return returnValue;
        }

        public static bool GetSafeViewDataBool(ViewDataDictionary VdDictionary, string Key, bool DefaultNullValue = false)
        {
            var val = VdDictionary[Key] != null ? VdDictionary[Key].ToString() : "";

            if (val.ToLower() == "true")
            {
                return true;
            }
            else if (val.ToLower() == "false")
            {
                return false;
            }
            else 
            {
                return DefaultNullValue;
            }
        }

        #endregion

        #region === MVC Controller Extensions ===

        public static PartialViewResult PartialView(this Controller Controller, string ViewName, object Model, ViewDataDictionary ViewData)
        {
            if (Model != null)
            {
                ViewData.Model = Model;
            }

            ViewData.ModelState.Merge(Controller.ModelState);

            return new PartialViewResult
            {
                ViewName = ViewName,
                ViewData = ViewData,
                TempData = Controller.TempData
            };
        }

        #endregion

        /// <summary>
        /// Indicates whether the current request is coming from the Umbraco back-office. 
        /// Most useful in Macros to avoid 'no current PublishedContentRequest.' error.
        /// </summary>
        /// <param name="ThisRequestUrl">Pass in "Request.Url" or "this.Request.Url"</param>
        /// <returns>TRUE if it is a back-office request, FALSE if not</returns>
        public static bool IsRenderingInBackOffice(System.Uri ThisRequestUrl)
        {
            var path = ThisRequestUrl.AbsolutePath;

            if (path.Contains("GetMacroResultAsHtmlForEditor"))
            {
                return true;
            }

            if (path.Contains("GetPartialViewResultAsHtmlForEditor"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// NOTE: This needs to be rendered (via @Html.Raw(...), etc.) since it relies upon script tags
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string RedirectToNewWindow(this HttpResponse Response, string Url)
        {
            //Requires jquery....
            //    return string.Format(
            //        "<script>$(document).ready(function () {{ window.open( '{0}' );}});</script>",
            //        url);

            return string.Format("<script> window.open( \"{0}\" );</script>", Url);
        }
    }
}

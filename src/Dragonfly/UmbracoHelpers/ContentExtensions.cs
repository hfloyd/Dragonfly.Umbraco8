namespace Dragonfly.UmbracoHelpers
{
    using Dragonfly.UmbracoModels;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;

    /// <summary>
    /// Extension methods for Base models
    /// </summary>
    public static class ContentExtensions
    {
        #region HasPropertyWithValue

        /// <summary>
        /// Checks if the model has a property and a value for the property
        /// </summary>
        /// <param name="Model">
        /// The <see cref="IPublishedContent"/> to inspect
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias on the <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// A value indicating whether or not the property exists on the <see cref="IPublishedContent"/> and has a value
        /// </returns>
        public static bool HasPropertyWithValue(this IPublishedContent Model, string PropertyAlias)
        {
            return Model.HasProperty(PropertyAlias) && Model.HasValue(PropertyAlias);
        }

        ///// <summary>
        ///// Checks if the model has a property and a value for the property
        ///// </summary>
        ///// <param name="model">The <see cref="RenderModel"/></param>
        ///// <param name="propertyAlias">The Umbraco property alias on the model</param>
        ///// <returns>A value indicating whether or not the property exists on the model and has a value</returns>
        //public static bool HasPropertyWithValue(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.HasPropertyWithValue(propertyAlias);
        //}

        #endregion

        #region String Properties

        ///// <summary>
        ///// Checks if the model has a property and a value for the property and returns either the string representation
        ///// of the property or an empty string
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property alias
        ///// </param>
        ///// <returns>
        ///// The property value as a string or an empty string
        ///// </returns>
        //public static string GetSafeString(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.GetSafeString(propertyAlias);
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or an empty string
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSafeString(this IPublishedContent Content, string PropertyAlias)
        {
            return Content.GetSafeString(PropertyAlias, string.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="DefaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSafeString(this IPublishedContent Content, string PropertyAlias, string DefaultValue)
        {
            return Content.HasPropertyWithValue(PropertyAlias) ? Content.Value<string>(PropertyAlias) : DefaultValue;
        }

        #endregion

        #region Date Properties

        /// <summary>
        /// Gets a safe date time from content
        /// </summary>
        /// <param name="Content">
        /// The content.
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetSafeDateTime(this IPublishedContent Content, string PropertyAlias)
        {
            return Content.GetSafeDateTime(PropertyAlias, DateTime.MinValue);
        }

        /// <summary>
        /// Gets a safe date time from content
        /// </summary>
        /// <param name="Content">
        /// The content.
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="DefaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetSafeDateTime(this IPublishedContent Content, string PropertyAlias, DateTime DefaultValue)
        {
            if (!Content.HasPropertyWithValue(PropertyAlias)) return DefaultValue;

            DateTime dt;

            return DateTime.TryParse(Content.Value<string>(PropertyAlias), out dt) ? dt : DefaultValue;
        }

        ///// <summary>
        ///// Gets a safe date time from content.
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>.
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The property alias.
        ///// </param>
        ///// <returns>
        ///// The <see cref="DateTime"/>.
        ///// </returns>
        //public static DateTime GetSafeDateTime(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.GetSafeDateTime(propertyAlias);
        //}

        #endregion

        #region GUID Properties

        ///// <summary>
        ///// Checks if the model has a property and a value for the property and returns either the Guid representation
        ///// of the property or the default value
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property alias.
        ///// </param>
        ///// <returns>
        ///// The <see cref="Guid"/>.
        ///// </returns>
        //public static Guid GetSafeGuid(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.GetSafeGuid(propertyAlias, Guid.Empty);
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the Guid representation
        /// of the property or the default value
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>.
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid GetSafeGuid(this IPublishedContent Content, string PropertyAlias)
        {
            return Content.GetSafeGuid(PropertyAlias, Guid.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the Guid representation
        /// of the property or the default value
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>.
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="DefaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid GetSafeGuid(this IPublishedContent Content, string PropertyAlias, Guid DefaultValue)
        {
            return Content.HasPropertyWithValue(PropertyAlias)
                ? new Guid(Content.Value<string>(PropertyAlias))
                : DefaultValue;
        }

        #endregion

        #region Integer Properties

        ///// <summary>
        ///// Checks if the model has a property and a value for the property and returns either the string representation
        ///// of the property or the default value of 0
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property alias.
        ///// </param>
        ///// <returns>
        ///// The <see cref="int"/>.
        ///// </returns>
        //public static int GetSafeInt(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.GetSafeInt(propertyAlias);
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of 0
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetSafeInt(this IPublishedContent Content, string PropertyAlias)
        {
            return Content.GetSafeInt(PropertyAlias, 0);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of 0
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="DefaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetSafeInt(this IPublishedContent Content, string PropertyAlias, int DefaultValue)
        {
            var willWork = Content.HasPropertyWithValue(PropertyAlias);
            var propVal = Content.Value<int>(PropertyAlias);
            return willWork ? propVal : DefaultValue;
        }

        #endregion

        #region Boolean Properties

        ///// <summary>
        ///// Checks if the model has a property and a value for the property and returns either the string representation
        ///// of the property or the default value of false
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property alias.
        ///// </param>
        ///// <returns>
        ///// The <see cref="bool"/>.
        ///// </returns>
        //public static bool GetSafeBool(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.GetSafeBool(propertyAlias);
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of false
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetSafeBool(this IPublishedContent Content, string PropertyAlias)
        {
            return Content.GetSafeBool(PropertyAlias, false);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of false
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="DefaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetSafeBool(this IPublishedContent Content, string PropertyAlias, bool DefaultValue)
        {
            var value = Content.GetSafeString(PropertyAlias).ToLowerInvariant();
            if (string.IsNullOrEmpty(value)) return DefaultValue;
            return value == "yes" || value == "1" || value == "true";
        }

        #endregion

        #region IHtmlString Properties

        ///// <summary>
        ///// Checks if the model has a property and a value for the property and returns either the <see cref="IHtmlString"/> representation
        ///// of the property or an empty <see cref="IHtmlString"/>
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property alias.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IHtmlString"/>.
        ///// </returns>
        //public static IHtmlString GetSafeHtmlString(this RenderModel model, string propertyAlias)
        //{
        //    return model.Content.GetSafeHtmlString(propertyAlias);
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IHtmlString"/> representation
        /// of the property or an empty <see cref="IHtmlString"/>
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString GetSafeHtmlString(this IPublishedContent Content, string PropertyAlias)
        {
            return Content.GetSafeHtmlString(PropertyAlias, string.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IHtmlString"/> representation
        /// of the property or the default <see cref="IHtmlString"/>
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias
        /// </param>
        /// <param name="DefaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString GetSafeHtmlString(this IPublishedContent Content, string PropertyAlias, string DefaultValue)
        {
            return Content.HasPropertyWithValue(PropertyAlias)
                       ? Content.Value<IHtmlString>(PropertyAlias)
                       : MvcHtmlString.Create(DefaultValue);
        }

        #endregion

        #region IPublishedContent Properties

        ///// <summary>
        ///// Gets a content Id from a content picker and renders it as <see cref="IPublishedContent"/>.
        ///// </summary>
        ///// <param name="model">
        ///// The current <see cref="RenderModel"/>.
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The property alias.
        ///// </param>
        ///// <param name="umbraco">
        ///// The <see cref="UmbracoHelper"/>.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IPublishedContent"/> from the content picker.
        ///// </returns>
        //public static IPublishedContent GetSafeContent(this RenderModel model, string propertyAlias, UmbracoHelper umbraco)
        //{
        //    return model.Content.GetSafeContent(propertyAlias, umbraco);
        //}

        /// <summary>
        /// Gets a content Id from a content picker and renders it as <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="Content">
        /// The current <see cref="IPublishedContent"/>.
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>.
        /// </param>
        /// <returns>
        /// The <see cref="IPublishedContent"/> from the content picker.
        /// </returns>
        public static IPublishedContent GetSafeContent(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco)
        {
            return Content.HasPropertyWithValue(PropertyAlias)
                       ? Umbraco.Content(Content.Value(PropertyAlias))
                       : null;
        }

        public static IEnumerable<IPublishedContent> GetSafeMultiContent(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco)
        {
            if (Content.HasPropertyWithValue(PropertyAlias))
            {
                var iPubs = Content.Value<IEnumerable<IPublishedContent>>(PropertyAlias);
                return iPubs;
            }
            else
            { return new List<IPublishedContent>(); }
        }


        #endregion

        #region Get First Matching Property Value

        public static string GetFirstMatchingPropValueString(this IPublishedContent Content, IEnumerable<string> PropsToTest)
        {
            string str = "";
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    string safeString = Content.GetSafeString(propertyAlias, "");
                    if (safeString != "")
                    {
                        str = safeString;
                        flag = false;
                    }
                }
            }
            return str;
        }

        public static DateTime GetFirstMatchingPropValueDate(this IPublishedContent Content, IEnumerable<string> PropsToTest)
        {
            var defaultVal = DateTime.MinValue;
            DateTime returnVal = defaultVal;
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeVal = Content.GetSafeDateTime(propertyAlias, defaultVal);
                    if (safeVal != defaultVal)
                    {
                        returnVal = safeVal;
                        flag = false;
                    }
                }
            }
            return returnVal;
        }

        public static Int32 GetFirstMatchingPropValueInt(this IPublishedContent Content, IEnumerable<string> PropsToTest)
        {
            var defaultVal = 0;
            Int32 returnVal = defaultVal;
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeVal = Content.GetSafeInt(propertyAlias, defaultVal);
                    if (safeVal != defaultVal)
                    {
                        returnVal = safeVal;
                        flag = false;
                    }
                }
            }
            return returnVal;
        }

        public static bool GetFirstMatchingPropValueBool(this IPublishedContent Content, IEnumerable<string> PropsToTest)
        {
            var defaultVal = false;
            bool returnVal = defaultVal;
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeVal = Content.GetSafeBool(propertyAlias, defaultVal);
                    if (safeVal != defaultVal)
                    {
                        returnVal = safeVal;
                        flag = false;
                    }
                }
            }
            return returnVal;
        }

        public static MediaImage GetFirstMatchingPropValueMediaImage(this IPublishedContent Content, IEnumerable<string> PropsToTest, UmbracoHelper Umbraco)
        {
            MediaImage mediaImg = new MediaImage();
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeMedia = Content.GetSafeImage(propertyAlias, Umbraco);
                    if (safeMedia.Url != "")
                    {
                        mediaImg = safeMedia as MediaImage;
                        flag = false;
                    }
                }
            }

            return mediaImg;
        }

        #endregion

    }
}
namespace Dragonfly.UmbracoHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Dragonfly.UmbracoModels;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;

    /// <summary>
    /// Extension methods for Base models
    /// </summary>
    public static class ContentExtensions
    {
    #region IPublishedContent
        
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

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or an empty string
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias
        /// </param>
        /// <returns>
        /// The property value as a string or an empty string
        /// </returns>
        public static string GetSafeString(this RenderModel model, string propertyAlias)
        {
            return model.Content.GetSafeString(propertyAlias);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or an empty string
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSafeString(this IPublishedContent content, string propertyAlias)
        {
            return content.GetSafeString(propertyAlias, string.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSafeString(this IPublishedContent content, string propertyAlias, string defaultValue)
        {
            return content.HasPropertyWithValue(propertyAlias) ? content.GetPropertyValue<string>(propertyAlias) : defaultValue;
        }
        
        #endregion

        #region Date Properties

        /// <summary>
        /// Gets a safe date time from content
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetSafeDateTime(this IPublishedContent content, string propertyAlias)
        {
            return content.GetSafeDateTime(propertyAlias, DateTime.MinValue);
        }

        /// <summary>
        /// Gets a safe date time from content
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetSafeDateTime(this IPublishedContent content, string propertyAlias, DateTime defaultValue)
        {
            if (!content.HasPropertyWithValue(propertyAlias)) return defaultValue;

            DateTime dt;

            return DateTime.TryParse(content.GetPropertyValue<string>(propertyAlias), out dt) ? dt : defaultValue;
        }

        /// <summary>
        /// Gets a safe date time from content.
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>.
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetSafeDateTime(this RenderModel model, string propertyAlias)
        {
            return model.Content.GetSafeDateTime(propertyAlias);
        }

        #endregion

        #region GUID Properties

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the Guid representation
        /// of the property or the default value
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid GetSafeGuid(this RenderModel model, string propertyAlias)
        {
            return model.Content.GetSafeGuid(propertyAlias, Guid.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the Guid representation
        /// of the property or the default value
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>.
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid GetSafeGuid(this IPublishedContent content, string propertyAlias)
        {
            return content.GetSafeGuid(propertyAlias, Guid.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the Guid representation
        /// of the property or the default value
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>.
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid GetSafeGuid(this IPublishedContent content, string propertyAlias, Guid defaultValue)
        {
            return content.HasPropertyWithValue(propertyAlias)
                ? new Guid(content.GetPropertyValue<string>(propertyAlias))
                : defaultValue;
        }

        #endregion

        #region Integer Properties

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of 0
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetSafeInt(this RenderModel model, string propertyAlias)
        {
            return model.Content.GetSafeInt(propertyAlias);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of 0
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetSafeInt(this IPublishedContent content, string propertyAlias)
        {
            return content.GetSafeInt(propertyAlias, 0);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of 0
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetSafeInt(this IPublishedContent content, string propertyAlias, int defaultValue)
        {
            var WillWork = content.HasPropertyWithValue(propertyAlias);
            var PropVal = content.GetPropertyValue<int>(propertyAlias);
            return WillWork ? PropVal : defaultValue;
        }

        #endregion

        #region Boolean Properties

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of false
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetSafeBool(this RenderModel model, string propertyAlias)
        {
            return model.Content.GetSafeBool(propertyAlias);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of false
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetSafeBool(this IPublishedContent content, string propertyAlias)
        {
            return content.GetSafeBool(propertyAlias, false);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the string representation
        /// of the property or the default value of false
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetSafeBool(this IPublishedContent content, string propertyAlias, bool defaultValue)
        {
            var value = content.GetSafeString(propertyAlias).ToLowerInvariant();
            if (string.IsNullOrEmpty(value)) return defaultValue;
            return value == "yes" || value == "1" || value == "true";
        }

        #endregion

        #region IHtmlString Properties

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IHtmlString"/> representation
        /// of the property or an empty <see cref="IHtmlString"/>
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString GetSafeHtmlString(this RenderModel model, string propertyAlias)
        {
            return model.Content.GetSafeHtmlString(propertyAlias);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IHtmlString"/> representation
        /// of the property or an empty <see cref="IHtmlString"/>
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString GetSafeHtmlString(this IPublishedContent content, string propertyAlias)
        {
            return content.GetSafeHtmlString(propertyAlias, string.Empty);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IHtmlString"/> representation
        /// of the property or the default <see cref="IHtmlString"/>
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> that should contain the property
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString GetSafeHtmlString(this IPublishedContent content, string propertyAlias, string defaultValue)
        {
            return content.HasPropertyWithValue(propertyAlias)
                       ? content.GetPropertyValue<IHtmlString>(propertyAlias)
                       : MvcHtmlString.Create(defaultValue);
        }

        #endregion

        #region IPublishedContent Properties

        /// <summary>
        /// Gets a content Id from a content picker and renders it as <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="model">
        /// The current <see cref="RenderModel"/>.
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>.
        /// </param>
        /// <returns>
        /// The <see cref="IPublishedContent"/> from the content picker.
        /// </returns>
        public static IPublishedContent GetSafeContent(this RenderModel model, string propertyAlias, UmbracoHelper umbraco)
        {
            return model.Content.GetSafeContent(propertyAlias, umbraco);
        }

        /// <summary>
        /// Gets a content Id from a content picker and renders it as <see cref="IPublishedContent"/>.
        /// </summary>
        /// <param name="content">
        /// The current <see cref="IPublishedContent"/>.
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>.
        /// </param>
        /// <returns>
        /// The <see cref="IPublishedContent"/> from the content picker.
        /// </returns>
        public static IPublishedContent GetSafeContent(this IPublishedContent content, string propertyAlias, UmbracoHelper umbraco)
        {
            return content.HasPropertyWithValue(propertyAlias)
                       ? umbraco.TypedContent(content.GetPropertyValue(propertyAlias))
                       : null;
        }

        public static string GetFirstMatchingPropValueString(this IPublishedContent content, IEnumerable<string> PropsToTest)
        {
            string str = "";
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    string safeString = content.GetSafeString(propertyAlias, "");
                    if (safeString != "")
                    {
                        str = safeString;
                        flag = false;
                    }
                }
            }
            return str;
        }

        public static DateTime GetFirstMatchingPropValueDate(this IPublishedContent content, IEnumerable<string> PropsToTest)
        {
            var defaultVal = DateTime.MinValue;
            DateTime returnVal = defaultVal;
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeVal = content.GetSafeDateTime(propertyAlias, defaultVal);
                    if (safeVal != defaultVal)
                    {
                        returnVal = safeVal;
                        flag = false;
                    }
                }
            }
            return returnVal;
        }

        public static Int32 GetFirstMatchingPropValueInt(this IPublishedContent content, IEnumerable<string> PropsToTest)
        {
            var defaultVal = 0;
            Int32 returnVal = defaultVal;
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeVal = content.GetSafeInt(propertyAlias, defaultVal);
                    if (safeVal != defaultVal)
                    {
                        returnVal = safeVal;
                        flag = false;
                    }
                }
            }
            return returnVal;
        }

        public static bool GetFirstMatchingPropValueBool(this IPublishedContent content, IEnumerable<string> PropsToTest)
        {
            var defaultVal = false;
            bool returnVal = defaultVal;
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeVal = content.GetSafeBool(propertyAlias, defaultVal);
                    if (safeVal != defaultVal)
                    {
                        returnVal = safeVal;
                        flag = false;
                    }
                }
            }
            return returnVal;
        }

        public static MediaImage GetFirstMatchingPropValueMediaImage(this IPublishedContent content, IEnumerable<string> PropsToTest, UmbracoHelper umbraco)
        {
            MediaImage mediaImg = new MediaImage();
            bool flag = true;
            foreach (string propertyAlias in PropsToTest)
            {
                if (flag)
                {
                    var safeMedia = content.GetSafeImage(umbraco, propertyAlias);
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

        #region General



        #endregion
    }
}
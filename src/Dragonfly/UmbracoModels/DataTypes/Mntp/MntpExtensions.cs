namespace Dragonfly.UmbracoModels.DataTypes
{
    using System.Collections.Generic;
    using System.Linq;
    using Dragonfly.UmbracoHelpers;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;

    public static class MntpExtensions
    {
        ///// <summary>
        ///// Creates a collection of <see cref="IPublishedContent"/> of either content or media based on values saved by an Umbraco MultiNodeTree Picker DataType
        ///// </summary>
        ///// <param name="model">
        ///// The <see cref="RenderModel"/>
        ///// </param>
        ///// <param name="umbraco">
        ///// The <see cref="UmbracoHelper"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property Alias.
        ///// </param>
        ///// <param name="isMedia">
        ///// True or false indicating whether or not the property is an Umbraco media item
        ///// </param>
        ///// <returns>
        ///// The collection of <see cref="IPublishedContent"/>.
        ///// </returns>
        //public static IEnumerable<IPublishedContent> GetSafeMntpContent(this RenderModel model, UmbracoHelper umbraco, string propertyAlias, bool isMedia = false)
        //{
        //    return model.Content.GetSafeMntpContent(umbraco, propertyAlias, isMedia);
        //}

        /// <summary>
        /// Creates a collection of <see cref="IPublishedContent"/> of either content or media based on values saved by an Umbraco MultiNodeTree Picker DataType
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property Alias.
        /// </param>
        /// <param name="isMedia">
        /// True or false indicating whether or not the property is an Umbraco media item
        /// </param>
        /// <returns>
        /// The collection of <see cref="IPublishedContent"/>.
        /// </returns>
        public static IEnumerable<IPublishedContent> GetSafeMntpContent(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias, bool isMedia = false)
        {
            //Check for property
            if (!content.HasPropertyWithValue(propertyAlias))
            {
                return new IPublishedContent[] { };
            }

            //check for property value
            var propValue = content.GetPropertyValue(propertyAlias);
            List<string> ids = new List<string>();

            if (propValue is string)
            {
                //string = comma-separated list of ids
                ids = propValue.ToString().Split(',').ToList();
            }
            else
            {
                //Something else... assume it's a list of IPubContents
                foreach (var item in content.GetPropertyValue<IEnumerable<IPublishedContent>>(propertyAlias))
                {
                    ids.Add(item.Id.ToString());
                }
            }

            if (!ids.Any())
            {
                return new IPublishedContent[] { };
            }

            //test for valid (non-null) data
            var returnNodes = new List<IPublishedContent>();

            if (isMedia)
            {
                var mediaNodes = umbraco.TypedMedia(ids);
                foreach (var node in mediaNodes)
                {
                    if (node != null)
                    {
                        returnNodes.Add(node);
                    }
                }
            }
            else
            {
                var contentNodes = umbraco.TypedContent(ids);
                foreach (var node in contentNodes)
                {
                    if (node != null)
                    {
                        returnNodes.Add(node);
                    }
                }

            }

            return returnNodes;
        }
    }
}

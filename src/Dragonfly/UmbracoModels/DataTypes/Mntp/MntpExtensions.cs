namespace Dragonfly.UmbracoModels.DataTypes
{
    using System.Collections.Generic;
    using System.Linq;
    using Dragonfly.UmbracoHelpers;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;

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
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="UmbHelper"></param>
        /// <param name="PropertyAlias">
        /// The Umbraco property Alias.
        /// </param>
        /// <param name="IsMedia">
        /// True or false indicating whether or not the property is an Umbraco media item
        /// </param>
        /// <returns>
        /// The collection of <see cref="IPublishedContent"/>.
        /// </returns>

        public static IEnumerable<IPublishedContent> GetSafeMntpContent(this IPublishedContent Content, UmbracoHelper UmbHelper, string PropertyAlias, bool IsMedia = false)
        {
            //Check for property
            if (!Content.HasPropertyWithValue(PropertyAlias))
            {
                return new IPublishedContent[] { };
            }

            //check for property value
            var propValue = Content.Value(PropertyAlias);
            List<string> ids = new List<string>();

            if (propValue is string)
            {
                //string = comma-separated list of ids
                ids = propValue.ToString().Split(',').ToList();
            }
            else
            {
                //Something else... assume it's a list of IPubContents
                foreach (var item in Content.Value<IEnumerable<IPublishedContent>>(PropertyAlias))
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

            if (IsMedia)
            {
                var mediaNodes = UmbHelper.Media(ids);
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
                var contentNodes = UmbHelper.Content(ids);
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

        public static IEnumerable<IPublishedContent> GetMntpContent(this IPublishedContent thisContent, string PropertyAlias)
        {
            var propData = thisContent.HasProperty(PropertyAlias) ? thisContent.Value(PropertyAlias) : null;

            if (propData != null)
            {
                //var nodesArray = ConvertDataToNodeIdArray(propData);
                //return ConvertNodeIdsToContent(nodesArray);
                return thisContent.Value<IEnumerable<IPublishedContent>>(PropertyAlias);
            }
            else
            {
                return null;
            }
        }
    }
}

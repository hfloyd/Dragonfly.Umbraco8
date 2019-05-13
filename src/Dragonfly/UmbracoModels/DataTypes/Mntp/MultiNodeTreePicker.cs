namespace Dragonfly.UmbracoModels.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;

    public static class MultiNodeTreePicker
    {
        public static IEnumerable<IPublishedContent> GetMntpContent(this IPublishedContent thisContent, string PropertyAlias)
        {
            var propData = thisContent.HasProperty(PropertyAlias) ? thisContent.GetProperty(PropertyAlias).DataValue : null;

            if (propData != null)
            {
                var nodesArray = ConvertDataToNodeIdArray(propData);

                return ConvertNodeIdsToContent(nodesArray);
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<IPublishedContent> GetContent(object SourceData)
        {
            var nodesArray = ConvertDataToNodeIdArray(SourceData);

            return ConvertNodeIdsToContent(nodesArray);
        }

        /// <summary>
        /// Convert the raw string into a nodeId integer array
        /// </summary>
        public static int[] ConvertDataToNodeIdArray(object SourceData)
        {
            var nodeIds =
                SourceData.ToString()
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            return nodeIds;
        }

        /// <summary>
        /// Convert the source nodeId into a IEnumerable of IPublishedContent (or DynamicPublishedContent)
        /// </summary>
        /// <param name="propertyType">
        /// The published property type.
        /// </param>
        /// <param name="Source">
        /// The value of the property
        /// </param>
        /// <param name="preview">
        /// The preview.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static IEnumerable<IPublishedContent> ConvertNodeIdsToContent(int[] NodeIdArray)
        {
            // Get the data type "content" or "media" setting
            /*
            var dts = ApplicationContext.Current.Services.DataTypeService;
            var startNodePreValue =
                dts.GetPreValuesCollectionByDataTypeId(propertyType.DataTypeId)
                    .PreValuesAsDictionary.FirstOrDefault(x => x.Key.ToLowerInvariant() == "startNode".ToLowerInvariant()).Value.Value;

            var startNodeObj = JsonConvert.DeserializeObject<JObject>(startNodePreValue);
            var pickerType = startNodeObj.GetValue("type").Value<string>();
            */

            if (NodeIdArray == null)
            {
                return null;
            }

            var nodeIds = (int[])NodeIdArray;

            var multiNodeTreePicker = new List<IPublishedContent>();

            if (UmbracoContext.Current != null)
            {
                var umbHelper = new UmbracoHelper(UmbracoContext.Current);

                if (nodeIds.Length > 0)
                {

                    var objectType = UmbracoObjectTypes.Unknown;

                    foreach (var nodeId in nodeIds)
                    {
                        var multiNodeTreePickerItem = GetPublishedContent(nodeId, ref objectType, UmbracoObjectTypes.Document, umbHelper.TypedContent)
                                    ?? GetPublishedContent(nodeId, ref objectType, UmbracoObjectTypes.Media, umbHelper.TypedMedia)
                                    ?? GetPublishedContent(nodeId, ref objectType, UmbracoObjectTypes.Member, umbHelper.TypedMember);

                        if (multiNodeTreePickerItem != null)
                        {
                            multiNodeTreePicker.Add(multiNodeTreePickerItem);
                        }
                    }

                }

                return multiNodeTreePicker.Where(x => x != null);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Attempt to get an IPublishedContent instance based on ID and content type
        /// </summary>
        /// <param name="nodeId">The content node ID</param>
        /// <param name="actualType">The type of content being requested</param>
        /// <param name="expectedType">The type of content expected/supported by <paramref name="contentFetcher"/></param>
        /// <param name="contentFetcher">A function to fetch content of type <paramref name="expectedType"/></param>
        /// <returns>The requested content, or null if either it does not exist or <paramref name="actualType"/> does not match <paramref name="expectedType"/></returns>
        private static IPublishedContent GetPublishedContent(int nodeId, ref UmbracoObjectTypes actualType, UmbracoObjectTypes expectedType, Func<int, IPublishedContent> contentFetcher)
        {
            // is the actual type supported by the content fetcher?
            if (actualType != UmbracoObjectTypes.Unknown && actualType != expectedType)
            {
                // no, return null
                return null;
            }

            // attempt to get the content
            var content = contentFetcher(nodeId);
            if (content != null)
            {
                // if we found the content, assign the expected type to the actual type so we don't have to keep looking for other types of content
                actualType = expectedType;
            }
            return content;
        }

    }
}

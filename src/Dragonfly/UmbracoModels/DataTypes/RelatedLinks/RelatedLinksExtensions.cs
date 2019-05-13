namespace Dragonfly.UmbracoModels.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web.Models;
    using Umbraco7Helpers;

    public static class RelatedLinksExtensions
    {

        /// <summary>
        /// Gets a collection of <see cref="ILink"/> from a related links picker.
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/> containing the related links picker.
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <returns>
        /// A collection of <see cref="ILink"/>.
        /// </returns>
        public static IEnumerable<ILink> GetSafeRelatedLinks(this RenderModel model, UmbracoHelper umbraco, string propertyAlias)
        {
            return model.Content.GetSafeRelatedLinks(umbraco, propertyAlias);
        }

        /// <summary>
        /// Gets a collection of <see cref="ILink"/> from a related links picker.
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> containing the related links picker.
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias.
        /// </param>
        /// <returns>
        /// A collection of <see cref="ILink"/>.
        /// </returns>
        public static IEnumerable<ILink> GetSafeRelatedLinks(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            var links = new List<ILink>();

            if (!content.HasPropertyWithValue(propertyAlias)) return links;


            //var propValue = content.GetPropertyValue<Newtonsoft.Json.Linq.JArray>(propertyAlias);
            var propValue = content.GetProperty(propertyAlias).DataValue.ToString();
            var relatedLinks = new RelatedLinks(propValue);


            //var relatedLinks = JsonConvert.DeserializeObject(content.GetProperty(propertyAlias).Value.ToString());
            //var relatedLinks = content.GetPropertyValue<Newtonsoft.Json.Linq.JArray>(propertyAlias);
            //var relatedLinks = JsonConvert.DeserializeObject(propValue.ToString());

            //var propValue = content.GetProperty(propertyAlias).Value;
            //if (propValue.ToString() == "Our.Umbraco.PropertyConverters.Models.RelatedLinks")
            //{
            //    //already deserialized
            //    relatedLinks = content.GetPropertyValue<IEnumerable<Our.Umbraco.PropertyConverters.Models.RelatedLink>>(propertyAlias);
            //}
            //else
            //{
            //needs to be deserialized
            //    relatedLinks = JsonConvert.DeserializeObject<IEnumerable<Our.Umbraco.PropertyConverters.Models.RelatedLink>>(content.GetProperty(propertyAlias).Value.ToString());
            //}

            foreach (var relatedLink in relatedLinks)
            {
                var rl = new Link();

                rl.Title = relatedLink.Caption;
                rl.Target = relatedLink.NewWindow ? "_blank" : "_self";

                // internal or external link
                if (relatedLink.IsInternal)
                {
                    var node = umbraco.TypedContent(relatedLink.NodeId);
                    if (node != null)
                    {
                        rl.ContentId = node.Id;
                        rl.ContentTypeAlias = node.DocumentTypeAlias;
                        rl.Url = node.Url;
                    }
                    else
                    {
                        if (rl.Url == "")
                        {
                            rl.Url = "#";
                        }
                    }
                }
                else
                {
                    rl.Url = relatedLink.Link;
                }

                links.Add(rl);
            }

            return links;
        }


    }
}

namespace Dragonfly.UmbracoModels.Helpers
{
    using System;
    using System.Collections.Generic;
    using DataTypes;
    using Newtonsoft.Json;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco7Helpers;

    public static class RelatedLinksHelper
    {
        private static UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

        public static ILink ToILink(this RelatedLink RelatedLink)
        {
            var link = new Link();

            if (RelatedLink != null)
            {
                link.Title = RelatedLink.Caption;
                link.Url = RelatedLink.Link;

                if (RelatedLink.NewWindow)
                {
                    link.Target = "_blank";
                }

                if (RelatedLink.IsInternal)
                {
                    dynamic linkdynamic = RelatedLink;
                    var nodeId = linkdynamic._linkItem.link;
                    link.ContentId = Convert.ToInt32(nodeId);

                    try
                    {

                        link.ContentNode = umbracoHelper.TypedContent(link.ContentId);
                        if (link.ContentNode != null)
                        {
                            link.ContentTypeAlias = link.ContentNode.DocumentTypeAlias;
                        }
                        else
                        {
                            //try media node
                            link.ContentNode = umbracoHelper.TypedMedia(link.ContentId);
                            if (link.ContentNode != null)
                            {
                                link.ContentTypeAlias = link.ContentNode.DocumentTypeAlias;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //skip for now
                       // throw;
                    }
                }
                else
                {
                    //external link
                    link.Url = RelatedLink.Link;
                }

            }

            return link;
        }

        public static IEnumerable<ILink> ToILinks(this RelatedLinks RelatedLinks)
        {
            var list = new List<ILink>();
            if (RelatedLinks != null)
            {
                if (RelatedLinks.Any())
                {
                    foreach (var item in RelatedLinks)
                    {
                        var x = item.ToILink();
                        list.Add(x);
                    }
                }
            }

            return list;
        }

        public static IEnumerable<RelatedLink> JObjectToRelatedLinks(string ObjValue)
        {
            var allLinks = new List<RelatedLink>();

            //var rls = (IEnumerable<RelatedLink>)JsonConvert.DeserializeObject(ObjValue);

            dynamic jsonObj = JsonConvert.DeserializeObject(ObjValue);

            foreach (var obj in jsonObj)
            {
                RelatedLink rl = new RelatedLink(obj);
                allLinks.Add(rl);
            }
            
            // JArray array = JsonConvert.DeserializeObject(ObjValue);
            //IEnumerable<RelatedLink> links = array
            return allLinks;
        }

        /// <summary>
        /// Gets a collection of <see cref="ILink"/> from a related links picker.
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/> containing the related links picker.
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <returns>
        /// A collection of <see cref="ILink"/>.
        /// </returns>
        public static IEnumerable<ILink> GetRelatedLinks(this IPublishedContent Content, UmbracoHelper Umbraco, string PropertyAlias)
        {
            var links = new List<ILink>();

            if (!Content.HasPropertyWithValue(PropertyAlias)) return links;

            //IEnumerable<Our.Umbraco.PropertyConverters.Models.RelatedLink> relatedLinks = null;

            //var propValue = content.GetPropertyValue<Newtonsoft.Json.Linq.JArray>(propertyAlias);
            var propValue = Content.GetProperty(PropertyAlias).DataValue.ToString();
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
                    var node = Umbraco.TypedContent(relatedLink.NodeId);
                    if (node != null)
                    {
                        rl.ContentNode = null;
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

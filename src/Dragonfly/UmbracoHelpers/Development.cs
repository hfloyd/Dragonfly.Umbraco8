namespace Dragonfly.UmbracoHelpers
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Umbraco.Core;
    using Umbraco.Core.Composing;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.Services;
    using Umbraco.Web;

    /// <summary>
    /// Helpers related to Templates, Node Paths, Udis, and Getting Site Pages.
    /// Also includes functions for manipulating HTML strings.
    /// </summary>
    public static class Development
    {
        private const string ThisClassName = "Dragonfly.UmbracoHelpers.Development";

        private static IFileService _umbFileService = Current.Services.FileService;
        private static IContentService _umbContentService = Current.Services.ContentService;
        private static IMediaService _umbMediaService = Current.Services.MediaService;

        /// <summary>
        /// Get the Alias of a template from its ID. If the Id is null or zero, "NONE" will be returned.
        /// </summary>
        /// <param name="TemplateId">
        /// The template id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetTemplateAlias(int? TemplateId)
        {
            //var umbFileService = ApplicationContext.Current.Services.FileService;
            string templateAlias;

            if (TemplateId == 0 | TemplateId == null)
            {
                templateAlias = "NONE";
            }
            else
            {
                var lookupTemplate = _umbFileService.GetTemplate(Convert.ToInt32(TemplateId));
                templateAlias = lookupTemplate.Alias;
            }

            return templateAlias;
        }

        /// <summary>
        /// Return the first descendant page in the site by its DocType
        /// </summary>
        /// <param name="UmbHelper">UmbracoHelper</param>
        /// <param name="SiteRootNodeId">ex: model.Site.Id</param>
        /// <param name="DoctypeAlias">Name of the Doctype to serach for</param>
        /// <returns>An IPublishedContent of the node, or NULL if not found. You can then cast to a strongly-typed model for the DocType (ex: new ContactUsPage(contactPage))</returns>
        public static IPublishedContent GetSitePage(UmbracoHelper UmbHelper, int SiteRootNodeId, string DoctypeAlias)
        {
            //var xPathExpr = $"root/{SiteRootDocTypeAlias}[@id={SiteRootNodeId}]//{DoctypeAlias}";
            //var page = UmbHelper.ContentSingleAtXPath(xPathExpr);

            var pageMatches = GetSitePages(UmbHelper, SiteRootNodeId, DoctypeAlias).ToList();
            if (pageMatches.Any())
            {
                return pageMatches.First();
            }

            //If we get here, node not found
            return null;
        }

        /// <summary>
        /// Returns all descendant pages in the site of a specified DocType
        /// </summary>
        /// <param name="UmbHelper">UmbracoHelper</param>
        /// <param name="SiteRootNodeId">ex: model.Site.Id</param>
        /// <param name="DoctypeAlias">Name of the Doctype to serach for</param>
        /// <returns>An IEnumerable&lt;IPublishedContent&gt; of the nodes, or empty list if not found.</returns>
        public static IEnumerable<IPublishedContent> GetSitePages(UmbracoHelper UmbHelper, int SiteRootNodeId, string DoctypeAlias)
        {
            //var xPathExpr = $"root/{SiteRootDocTypeAlias}[@id={SiteRootNodeId}]//{DoctypeAlias}";
            //var page = UmbHelper.ContentSingleAtXPath(xPathExpr);

            var site = UmbHelper.Content(SiteRootNodeId);
            if (site != null)
            {
                var pageMatches = site.Descendants().Where(n => n.ContentType.Alias == DoctypeAlias).ToList();
                if (pageMatches.Any())
                {
                    return pageMatches;
                }
            }

            //If we get here, nodes not found
            return new List<IPublishedContent>();
        }


        ///// <summary>
        ///// Return a list of Prevalues for a given DataType by Name
        ///// </summary>
        ///// <param name="DataTypeName"></param>
        ///// <returns></returns>
        //public static IEnumerable<PreValue> GetPrevaluesForDataType(string DataTypeName)
        //{
        //    IEnumerable<PreValue> toReturn = new List<PreValue>();

        //    IDataTypeDefinition dataType = Current.Services.DataTypeService.GetDataTypeDefinitionByName(DataTypeName);

        //    if (dataType == null)
        //    {
        //        return toReturn;
        //    }

        //    PreValueCollection preValues = Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id);

        //    if (preValues == null)
        //    {
        //        return toReturn;
        //    }

        //    IDictionary<string, PreValue> tempDictionary = preValues.FormatAsDictionary();

        //    toReturn = tempDictionary.Select(N => N.Value);

        //    return toReturn;
        //}

        #region Node Paths

        /// <summary>
        /// Return a string representation of the path to the Node
        /// </summary>
        /// <param name="UmbContentNode">Node to Get a Path for</param>
        /// <param name="Separator">String to separate parts of the path</param>
        /// <returns></returns>
        public static string NodePath(IPublishedContent UmbContentNode, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            try
            {
                string pathIdsCsv = UmbContentNode.Path;
                nodePathString = NodePathFromPathIdsCsv(pathIdsCsv, Separator);
            }
            catch (Exception ex)
            {
                var functionName = $"{ThisClassName}.NodePath";
                Current.Logger.Error<string>(ex, "ERROR in {FunctionName} for node #{NodeId} ({NodeName}).", functionName, UmbContentNode.Id.ToString(), UmbContentNode.Name);

                var returnMsg = $"Unable to generate node path. (ERROR:{ex.Message})";
                return returnMsg;
            }

            return nodePathString;
        }

        /// <summary>
        /// Return a string representation of the path to the Node
        /// </summary>
        /// <param name="UmbContentNode">Node to Get a Path for</param>
        /// <param name="Separator">String to separate parts of the path</param>
        /// <returns></returns>
        public static string NodePath(IContent UmbContentNode, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            try
            {
                string pathIdsCsv = UmbContentNode.Path;
                nodePathString = NodePathFromPathIdsCsv(pathIdsCsv, Separator);
            }
            catch (Exception ex)
            {
                var functionName = $"{ThisClassName}.NodePath";
                Current.Logger.Error<string>(ex, "ERROR in {FunctionName} for node #{NodeId} ({NodeName}).", functionName, UmbContentNode.Id.ToString(), UmbContentNode.Name);

                var returnMsg = $"Unable to generate node path. (ERROR:{ex.Message})";
                return returnMsg;
            }

            return nodePathString;
        }

        /// <summary>
        /// Return a string representation of the path to the Node
        /// </summary>
        /// <param name="PathIdsCsv">Comma-separated list of NodeIds representing the Path</param>
        /// <param name="Separator">String to separate parts of the path</param>
        /// <returns></returns>
        private static string NodePathFromPathIdsCsv(string PathIdsCsv, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            string[] pathIdsArray = PathIdsCsv.Split(',');

            foreach (var sId in pathIdsArray)
            {
                if (sId != "-1")
                {
                    IContent getNode = _umbContentService.GetById(Convert.ToInt32(sId));
                    string nodeName = getNode.Name;
                    nodePathString = String.Concat(nodePathString, Separator, nodeName);
                }
            }

            return nodePathString.TrimStart(Separator);

        }

        /// <summary>
        /// Return a string representation of the path to the Node
        /// </summary>
        /// <param name="UmbMediaNode">Node to Get a Path for</param>
        /// <param name="Separator">String to separate parts of the path</param>
        /// <returns></returns>
        public static string MediaPath(IPublishedContent UmbMediaNode, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            try
            {
                string pathIdsCsv = UmbMediaNode.Path;
                nodePathString = MediaNodePathFromPathIdsCsv(pathIdsCsv, Separator);
            }
            catch (Exception ex)
            {
                var functionName = String.Format("{0}.NodePath", ThisClassName);
                Current.Logger.Error<string>(ex, "ERROR in {FunctionName} for node #{MediaNodeId} ({MediaNodeName}).", functionName, UmbMediaNode.Id.ToString(), UmbMediaNode.Name);

                var returnMsg = $"Unable to generate node path. (ERROR:{ex.Message})";
                return returnMsg;
            }

            return nodePathString;
        }

        /// <summary>
        /// Return a string representation of the path to the Node
        /// </summary>
        /// <param name="PathIdsCsv">Comma-separated list of NodeIds representing the Path</param>
        /// <param name="Separator">String to separate parts of the path</param>
        /// <returns></returns>
        private static string MediaNodePathFromPathIdsCsv(string PathIdsCsv, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            string[] pathIdsArray = PathIdsCsv.Split(',');

            foreach (var sId in pathIdsArray)
            {
                if (sId != "-1")
                {
                    IMedia getNode = _umbMediaService.GetById(Convert.ToInt32(sId));
                    string nodeName = getNode.Name;
                    nodePathString = String.Concat(nodePathString, Separator, nodeName);
                }
            }

            return nodePathString.TrimStart(Separator);
        }

        #endregion

        #region Udi

        /// <summary>
        /// Converts a list of published content to a comma-separated string of UDI values suitable for using with the content service
        /// </summary>
        /// <param name="PubsEnum">A collection of IPublishedContent</param>
        /// <param name="UdiType">UDI Type to use (document, media, etc) (use 'Umbraco.Core.Constants.UdiEntityType.' to specify)
        /// If excluded, will try to use the DocTypeAlias to determine the UDI Type</param>
        /// <returns>A CSV string of UID values eg. umb://document/56c0f0ef0ac74b58ae1cce16db1476af,umb://document/5cbac9249ffa4f5ab4f5e0db1599a75b</returns>
        public static string ToUdiCsv(this IEnumerable<IPublishedContent> PubsEnum, string UdiType = "")
        {
            var list = new List<string>();
            if (PubsEnum != null)
            {
                foreach (var publishedContent in PubsEnum)
                {
                    if (publishedContent != null)
                    {
                        var udi = ToUdiString(publishedContent, UdiType);
                        list.Add(udi.ToString());
                    }
                }
            }
            return String.Join(",", list);
        }

        /// <summary>
        /// Converts an IPublishedContent to a UDI string suitable for using with the content service
        /// </summary>
        /// <param name="PublishedContent">Node to use</param>
        /// <param name="UdiType">UDI Type to use (document, media, etc) (use 'Umbraco.Core.Constants.UdiEntityType.' to specify)
        /// If excluded, will try to use the DocTypeAlias to determine the UDI Type</param>
        /// <returns></returns>
        public static string ToUdiString(this IPublishedContent PublishedContent, string UdiType = "")
        {
            if (PublishedContent != null)
            {
                var udiType = UdiType != "" ? UdiType : GetUdiType(PublishedContent);
                var udi = Udi.Create(udiType, PublishedContent.Key);
                return udi.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a string representation of the type for the Udi (ex: 'media' or 'document')
        /// </summary>
        /// <param name="PublishedContent">Node to get data for</param>
        /// <returns></returns>
        private static string GetUdiType(IPublishedContent PublishedContent)
        {
            var udiType = Umbraco.Core.Constants.UdiEntityType.Document;

            //if it's a media or member type, use that, otherwise, document is assumed
            switch (PublishedContent.ItemType)
            {
                case PublishedItemType.Media:
                    udiType = Umbraco.Core.Constants.UdiEntityType.Media;
                    break;
                case PublishedItemType.Member:
                    udiType = Umbraco.Core.Constants.UdiEntityType.Member;
                    break;
            }

            return udiType;
        }

        #endregion

        #region Html

        /// <summary>
        /// Validates string as html
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <returns>True if valid HTML, False if Invalid</returns>
        public static bool HtmlIsValid(this string OriginalHtml)
        {
            IEnumerable<HtmlParseError> validationErrors;
            return HtmlIsValid(OriginalHtml, out validationErrors);
        }

        /// <summary>
        /// Validates string as html, returns errors
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ValidationErrors">Variable of type IEnumerable&lt;HtmlParseError&gt;</param>
        /// <returns></returns>
        public static bool HtmlIsValid(this string OriginalHtml, out IEnumerable<HtmlParseError> ValidationErrors)
        {
            if (!OriginalHtml.IsNullOrWhiteSpace())
            {
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(OriginalHtml);

                if (doc.ParseErrors.Any())
                {
                    //Invalid HTML
                    ValidationErrors = doc.ParseErrors;
                    return false;
                }
            }
            ValidationErrors = new List<HtmlParseError>();
            return true;
        }

        /// <summary>
        /// Removes all &lt;script&gt; tags from HTML
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ReplaceWith">optional - text or HTML to replace the script tag with</param>
        /// <returns></returns>
        public static string StripScripts(this string OriginalHtml, string ReplaceWith = "")
        {
            if (!OriginalHtml.IsNullOrWhiteSpace())
            {
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(OriginalHtml);

                var badNodes = doc.DocumentNode.SelectNodes("//script");
                if (badNodes != null)
                {
                    if (ReplaceWith != "")
                    {
                        HtmlNode replacementNode = HtmlNode.CreateNode(ReplaceWith);

                        foreach (var node in badNodes)
                        {
                            doc.DocumentNode.ReplaceChild(replacementNode, node);
                        }
                    }
                    else
                    {
                        //Just remove
                        foreach (var node in badNodes)
                        {
                            node.Remove();
                        }
                    }

                    return doc.DocumentNode.InnerHtml.ToString();
                }
                else
                {
                    //No scripts, just return original
                    return OriginalHtml;
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Removes all &lt;script&gt; tags from HTML
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ReplaceWith">optional - text or HTML to replace the script tag with</param>
        /// <returns></returns>
        public static IHtmlString StripScripts(this IHtmlString OriginalHtml, string ReplaceWith = "")
        {
            var originalHtml = OriginalHtml.ToString();
            var finalHtml = originalHtml.StripScripts(ReplaceWith);

            return new HtmlString(finalHtml);
        }

        /// <summary>
        /// Removes all &lt;iframe&gt; tags from HTML
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ReplaceWith">optional - text or HTML to replace the script tag with</param>
        /// <returns></returns>
        public static string StripIframes(this string OriginalHtml, string ReplaceWith = "")
        {
            if (!OriginalHtml.IsNullOrWhiteSpace())
            {
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(OriginalHtml);

                var badNodes = doc.DocumentNode.SelectNodes("//iframe");
                if (badNodes != null)
                {
                    if (ReplaceWith != "")
                    {
                        HtmlNode replacementNode = HtmlNode.CreateNode(ReplaceWith);

                        foreach (var node in badNodes)
                        {
                            doc.DocumentNode.ReplaceChild(replacementNode, node);
                        }
                    }
                    else
                    {
                        //Just remove
                        foreach (var node in badNodes)
                        {
                            node.Remove();
                        }
                    }
                    return doc.DocumentNode.InnerHtml.ToString();
                }
                else
                {
                    //Nothing to remove, just return original
                    return OriginalHtml;
                }
            }
            else
            {
                return "";
            }
        }
        
        /// <summary>
        /// Removes all &lt;iframe&gt; tags from HTML
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ReplaceWith">optional - text or HTML to replace the script tag with</param>
        /// <returns></returns>
        public static IHtmlString StripIframes(this IHtmlString OriginalHtml, string ReplaceWith = "")
        {
            var originalHtml = OriginalHtml.ToString();
            var finalHtml = originalHtml.StripIframes(ReplaceWith);

            return new HtmlString(finalHtml);
        }

        /// <summary>
        /// Removes all &lt;p&gt; tags from HTML
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ReplaceWithBr">optional - if there are multiple paragraphs, will put a &lt;br/&gt; tag between them</param>
        /// <returns></returns>
        public static string StripParagraphTags(this string OriginalHtml, bool ReplaceWithBr = true)
        {
            if (!OriginalHtml.IsNullOrWhiteSpace())
            {
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(OriginalHtml);

                var badNodes = doc.DocumentNode.SelectNodes("//p");
                
                if (badNodes!=null && badNodes.Any())
                {
                    var totalParagraphs = badNodes.Count;

                    foreach (var node in badNodes)
                    {
                        var innerText = node.InnerText;
                        var newHtml = "";
                        var isLastParagraph = badNodes.IndexOf(node) == (totalParagraphs - 1);

                        if (ReplaceWithBr & totalParagraphs > 1 & !isLastParagraph)
                        {
                            newHtml = $"{innerText}<br/>";
                        }
                        else
                        {
                            newHtml = $"{innerText}";
                        }

                        HtmlNode replacementNode = HtmlNode.CreateNode(newHtml);
                        doc.DocumentNode.ReplaceChild(replacementNode, node);
                    }
                    return doc.DocumentNode.InnerHtml.ToString();
                }
                else
                {
                    //Nothing to remove, just return original
                    return OriginalHtml;
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Removes all &lt;p&gt; tags from HTML
        /// </summary>
        /// <param name="OriginalHtml"></param>
        /// <param name="ReplaceWithBr">optional - if there are multiple paragraphs, will put a &lt;br/&gt; tag between them</param>
        /// <returns></returns>
        public static IHtmlString StripParagraphTags(this IHtmlString OriginalHtml, bool ReplaceWithBr = true)
        {
            var originalHtml = OriginalHtml.ToString();
            var finalHtml = originalHtml.StripParagraphTags(ReplaceWithBr);

            return new HtmlString(finalHtml);
        }

        #endregion
    }

}

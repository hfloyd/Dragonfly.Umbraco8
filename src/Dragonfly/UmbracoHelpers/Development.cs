namespace Dragonfly.UmbracoHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.Services;
    
    public static class Development
    {
        private const string ThisClassName = "Dragonfly.UmbracoHelpers.Development";

        private static IFileService umbFileService = ApplicationContext.Current.Services.FileService;
        private static IContentService umbContentService = ApplicationContext.Current.Services.ContentService;
        private static IMediaService umbMediaService = ApplicationContext.Current.Services.MediaService;

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
            string TemplateAlias;

            if (TemplateId == 0 | TemplateId == null)
            {
                TemplateAlias = "NONE";
            }
            else
            {
                var LookupTemplate = umbFileService.GetTemplate(Convert.ToInt32(TemplateId));
                TemplateAlias = LookupTemplate.Alias;
            }

            return TemplateAlias;
        }

        /// <summary>
        /// Lookup a descendant page in the site by its DocType
        /// </summary>
        /// <param name="SiteRootNodeId">ex: model.Site.Id</param>
        /// <param name="DoctypeAlias">Name of the Doctype to serach for</param>
        /// <param name="SiteRootDocTypeAlias">default="Homepage"</param>
        /// <returns>An IPublishedContent of the node, or NULL if not found. You can then cast to a strongly-typed model for the DocType (ex: new ContactUsPage(contactPage))</returns>
        public static IPublishedContent GetSitePage(int SiteRootNodeId, string DoctypeAlias, string SiteRootDocTypeAlias = "Homepage")
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var xPathExpr = String.Format("root/{0}[@id={1}]//{2}", SiteRootDocTypeAlias, SiteRootNodeId, DoctypeAlias);

            var page = umbracoHelper.ContentSingleAtXPath(xPathExpr);
            if (page.Id > 0)
            {
                return page as IPublishedContent;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Return a list of Prevalues for a given DataType by Name
        /// </summary>
        /// <param name="DataTypeName"></param>
        /// <returns></returns>
        public static IEnumerable<PreValue> GetPrevaluesForDataType(string DataTypeName)
        {
            IEnumerable<PreValue> toReturn = new List<PreValue>();

            IDataTypeDefinition dataType = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByName(DataTypeName);

            if (dataType == null)
            {
                return toReturn;
            }

            PreValueCollection preValues = ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id);

            if (preValues == null)
            {
                return toReturn;
            }

            IDictionary<string, PreValue> tempDictionary = preValues.FormatAsDictionary();

            toReturn = tempDictionary.Select(n => n.Value);

            return toReturn;
        }

        #region Node Paths

        public static string NodePath(IPublishedContent UmbContentNode, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            try
            {
                string pathIdsCSV = UmbContentNode.Path;
                nodePathString = NodePathFromPathIdsCSV(pathIdsCSV, Separator);
            }
            catch (Exception ex)
            {
                var functionName = String.Format("{0}.NodePath", ThisClassName);
                var errMsg = String.Format(
                    "ERROR in {0} for node #{1} ({2}). [{3}]",
                    functionName,
                    UmbContentNode.Id.ToString(),
                    UmbContentNode.Name,
                    ex.Message);
                LogHelper.Error<string>(errMsg, ex);

                var returnMsg = String.Format("Unable to generate node path. (ERROR:{0})", ex.Message);
                return returnMsg;
            }

            return nodePathString;
        }

        public static string NodePath(IContent UmbContentNode, string Separator = " » ")
        {
            string nodePathString = String.Empty;

            try
            {
                string pathIdsCSV = UmbContentNode.Path;
                nodePathString = NodePathFromPathIdsCSV(pathIdsCSV, Separator);
            }
            catch (Exception ex)
            {
                var functionName = String.Format("{0}.NodePath", ThisClassName);
                var errMsg = String.Format(
                    "ERROR in {0} for node #{1} ({2}). [{3}]",
                    functionName,
                    UmbContentNode.Id.ToString(),
                    UmbContentNode.Name,
                    ex.Message);
                LogHelper.Error<string>(errMsg, ex);

                var returnMsg = String.Format("Unable to generate node path. (ERROR:{0})", ex.Message);
                return returnMsg;
            }

            return nodePathString;
        }

        private static string NodePathFromPathIdsCSV(string PathIdsCSV, string Separator = " » ")
        {
            string NodePathString = String.Empty;

            string[] PathIdsArray = PathIdsCSV.Split(',');

            foreach (var sId in PathIdsArray)
            {
                if (sId != "-1")
                {
                    IContent GetNode = umbContentService.GetById(Convert.ToInt32(sId));
                    string NodeName = GetNode.Name;
                    NodePathString = String.Concat(NodePathString, Separator, NodeName);
                }
            }

            return NodePathString.TrimStart(Separator);

        }

        public static string MediaPath(IPublishedContent UmbMediaNode, string Separator = " » ")
        {

            string nodePathString = String.Empty;

            try
            {
                string pathIdsCSV = UmbMediaNode.Path;
                nodePathString = MediaNodePathFromPathIdsCSV(pathIdsCSV, Separator);
            }
            catch (Exception ex)
            {
                var functionName = String.Format("{0}.NodePath", ThisClassName);
                var errMsg = String.Format(
                    "ERROR in {0} for node #{1} ({2}). [{3}]",
                    functionName,
                    UmbMediaNode.Id.ToString(),
                    UmbMediaNode.Name,
                    ex.Message);
                LogHelper.Error<string>(errMsg, ex);

                var returnMsg = String.Format("Unable to generate node path. (ERROR:{0})", ex.Message);
                return returnMsg;
            }

            return nodePathString;
        }

        private static string MediaNodePathFromPathIdsCSV(string PathIdsCSV, string Separator = " » ")
        {
            string NodePathString = String.Empty;

            string[] PathIdsArray = PathIdsCSV.Split(',');

            foreach (var sId in PathIdsArray)
            {
                if (sId != "-1")
                {
                    IMedia GetNode = umbMediaService.GetById(Convert.ToInt32(sId));
                    string NodeName = GetNode.Name;
                    NodePathString = String.Concat(NodePathString, Separator, NodeName);
                }
            }

            return NodePathString.TrimStart(Separator);
        }

        #endregion

        #region Udi

        /// <summary>
        /// Converts a list of published content to a comma-separated string of UDI values suitable for using with the content service
        /// </summary>
        /// <param name="IPubsEnum">A collection of IPublishedContent</param>
        /// <param name="UdiType">UDI Type to use (document, media, etc) (use 'Umbraco.Core.Constants.UdiEntityType.' to specify)
        /// If excluded, will try to use the DocTypeAlias to determine the UDI Type</param>
        /// <returns>A CSV string of UID values eg. umb://document/56c0f0ef0ac74b58ae1cce16db1476af,umb://document/5cbac9249ffa4f5ab4f5e0db1599a75b</returns>
        public static string ToUdiCsv(this IEnumerable<IPublishedContent> IPubsEnum, string UdiType = "")
        {
            var list = new List<string>();
            if (IPubsEnum != null)
            {
                foreach (var publishedContent in IPubsEnum)
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
        /// <param name="IPub">IPublishedContent</param>
        /// <param name="UdiType">UDI Type to use (document, media, etc) (use 'Umbraco.Core.Constants.UdiEntityType.' to specify)
        /// If excluded, will try to use the DocTypeAlias to determine the UDI Type</param>
        /// <returns></returns>
        public static string ToUdiString(this IPublishedContent IPub, string UdiType="")
        {
            if (IPub != null)
            {
                var udiType = UdiType != ""? UdiType : GetUdiType(IPub);
                var udi = Umbraco.Core.Udi.Create(udiType, IPub.GetKey());
                return udi.ToString();
            }
            else
            {
                return "";
            }
        }

        private static string GetUdiType(IPublishedContent PublishedContent)
        {
            var udiType = Umbraco.Core.Constants.UdiEntityType.Document;

            //if it's a known (default) media or member type, use that, otherwise, document is assumed
            switch (PublishedContent.DocumentTypeAlias)
            {
                case Constants.Conventions.MediaTypes.Image:
                    udiType = Umbraco.Core.Constants.UdiEntityType.Media;
                    break;
                case Constants.Conventions.MediaTypes.File:
                    udiType = Umbraco.Core.Constants.UdiEntityType.Media;
                    break;
                case Constants.Conventions.MediaTypes.Folder:
                    udiType = Umbraco.Core.Constants.UdiEntityType.Media;
                    break;
                case Constants.Conventions.MemberTypes.DefaultAlias:
                    udiType = Umbraco.Core.Constants.UdiEntityType.Member;
                    break;
            }

            return udiType;
        }

        #endregion
    }

}

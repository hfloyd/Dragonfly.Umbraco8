namespace Dragonfly.UmbracoModels
{
    using System.Web;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;

    /// <summary>
    /// Defines a hyperlink
    /// </summary>
    public interface ILink
    {
        /// <summary>
        /// Gets or sets Umbraco content id if applicable
        /// </summary>
        int ContentId { get; set; }

        /// <summary>
        /// Gets or sets Umbraco content node, if applicable
        /// </summary>
        IPublishedContent ContentNode { get; set; }

        /// <summary>
        /// Gets or sets the content type alias.
        /// </summary>
        string ContentTypeAlias { get; set; }

        /// <summary>
        /// Gets or sets title of the link
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets Url of the link
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets Target of the link
        /// </summary>
        string Target { get; set; }

        /// <summary>
        /// Gets or sets id attribute for the link
        /// </summary>
        string ElementId { get; set; }

        /// <summary>
        /// Gets or sets CSS class of the link
        /// </summary>
        string CssClass { get; set; }

        /// <summary>
        /// Is this a link to a media item?
        /// </summary>
        bool IsMedia { get; set; }

        /// <summary>
        /// If it is a media item, this is the media info
        /// </summary>
        IMediaFile MediaFile { get; set; }
    }


    /// <summary>
    /// Represents a hyperlink
    /// </summary>
    public class Link : ILink
    {
        #region ILink Members

        /// <summary>
        /// Gets or sets the Umbraco Content Id
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the content node.
        /// </summary>
        public IPublishedContent ContentNode { get; set; }

        /// <summary>
        /// Gets or sets the content type alias.
        /// </summary>
        public string ContentTypeAlias { get; set; }

        /// <summary>
        /// Gets or sets the link title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the link Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the link target
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the link element id
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// Gets or sets the link's CSS class
        /// </summary>
        public string CssClass { get; set; }

        public bool IsMedia { get; set; }

        public IMediaFile MediaFile { get; set; }

        #endregion

        public Link()
        {
            this.ContentId = 0;
            this.ContentNode = null;
            this.ContentTypeAlias = "";
            this.CssClass = "";
            this.ElementId = "";
            this.Target = "_self";
            this.Title = "";
            this.Url = "";
        }

        /// <summary>
        /// Gets the full url, including domain.
        /// </summary>
        /// <param name="AlternateDomain">If blank, will use the current hostname.</param>
        /// <returns></returns>
        public string AbsoluteUrl(string AlternateDomain = "")
        {
            var domain = AlternateDomain != "" ? AlternateDomain : HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            string absUrl = string.Format("{0}{1}", domain, this.Url);

            return absUrl;
        }
    }
}
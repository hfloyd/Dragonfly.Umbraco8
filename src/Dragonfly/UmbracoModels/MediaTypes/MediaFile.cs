namespace Dragonfly.UmbracoModels
{
    using Dragonfly.UmbracoHelpers;
    using System.Web;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;

    /// <summary>
    /// The MediaFile interface.
    /// </summary>
    public interface IMediaFile
    {
        /// <summary>
        /// Gets or sets the <see cref="IPublishedContent"/> that this image represents
        /// </summary>
        IPublishedContent Content { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        int Bytes { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// Gets or sets the full url, including domain.
        /// </summary>
        //string AbsoluteUrl { get; set; }
    }

    /// <summary>
    /// Represents a file.
    /// </summary>
    public class MediaFile : IMediaFile
    {
        /// <summary>
        /// Gets or sets the <see cref="IPublishedContent"/> that this file represents
        /// </summary>
        public IPublishedContent Content { get; set; }

        /// <summary>
        /// Gets or sets the full url, including domain.
        /// </summary>
        //public string AbsoluteUrl { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        public int Bytes { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        public string Extension { get; set; }

        public MediaFile()
        {
            this.Id = 0;
            this.Bytes = 0;
            this.Content = null;
            this.Extension = "";
            this.Name = "";
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

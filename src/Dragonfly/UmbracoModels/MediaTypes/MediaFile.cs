namespace Dragonfly.UmbracoModels
{
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

        string MediaTypeAlias { get; set; }

        //ImageCropperValue CropData { get; set; }
    }

    /// <summary>
    /// Represents a file.
    /// </summary>
    public class MediaFile : IMediaFile
    {
        #region Implementation of IMediaFile

        public IPublishedContent Content { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Bytes { get; set; }
        public string Extension { get; set; }

        public string MediaTypeAlias { get; set; }
        //public ImageCropperValue CropData { get; set; }

        #endregion

        public MediaFile()
        {
            this.Id = 0;
            this.Bytes = 0;
            this.Content = null;
            this.Extension = "";
            this.Name = "";
            this.Url = "";
            this.MediaTypeAlias = "";
           // this.CropData = null;
        }

    }
}

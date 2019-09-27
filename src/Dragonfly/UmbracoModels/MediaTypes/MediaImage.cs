namespace Dragonfly.UmbracoModels
{
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors.ValueConverters;

    /// <summary>
    /// Interface for a Media Image.
    /// </summary>
    public interface IMediaImage : IMediaFile
    {
        ImageCropperValue CropData { get; set; }

        int OriginalPixelWidth { get; set; }
        int OriginalPixelHeight { get; set; }

    }

    /// <summary>
    /// Represents an image from the Media section of Umbraco
    /// </summary>
    public class MediaImage : IMediaImage
    {

        #region Implementation of IMediaFile

        public IPublishedContent Content { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Bytes { get; set; }
        public string Extension { get; set; }

        #endregion

        #region Implementation of IMediaImage

        public ImageCropperValue CropData { get; set; }
        public int OriginalPixelWidth { get; set; }
        public int OriginalPixelHeight { get; set; }

        #endregion

        public MediaImage()
        {
            this.Id = 0;
            this.Bytes = 0;
            this.Content = null;
            this.Extension = "";
            this.Name = "";
            this.Url = "";
            this.OriginalPixelHeight = 0;
            this.OriginalPixelWidth = 0;
        }


    }


}
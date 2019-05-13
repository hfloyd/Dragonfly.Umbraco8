namespace Dragonfly.UmbracoModels
{
    using System.Collections.Generic;
    using System.Web;
    using Newtonsoft.Json.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;

    /// <summary>
    /// Marker interface for a media image.
    /// </summary>
    public interface IMediaImage : IMediaFile
    {
        /// <summary>
        /// Gets or sets the Alt Text.
        /// Default uses the MediaType Property "ImageAltText" or "ImageAltDictionaryKey"
        /// </summary>
        string ImageAltText { get; set; }

        /// <summary>
        /// Gets or sets the Alt Text.
        /// </summary>
        string ImageAltDictionaryKey { get; set; }

        //bool HasFocalPoint { get; set; }

        JObject JsonCropData { get; set; }

        bool HasFocalPoint { get; set; }
        double FocalPointLeft { get; set; }
        double FocalPointTop { get; set; }

        int OriginalPixelWidth { get; set; }
        int OriginalPixelHeight { get; set; }

    }

    /// <summary>
    /// Represents an image from the Media section of Umbraco
    /// </summary>
    public class MediaImage : IMediaImage
    {
        /// <summary>
        /// Gets or sets the <see cref="IPublishedContent"/> that this image represents
        /// </summary>
        public IPublishedContent Content { get; set; }

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
        
        public string ImageAltText { get; set; }

        public string ImageAltDictionaryKey { get; set; }

        public JObject JsonCropData { get; set; }

        public bool HasFocalPoint { get; set; }

        public double FocalPointLeft { get; set; }

        public double FocalPointTop { get; set; }

        public int OriginalPixelWidth { get; set; }
        public int OriginalPixelHeight { get; set; }

        public MediaImage()
        {
            this.Id = 0;
            this.Bytes = 0;
            this.Content = null;
            this.Extension = "";
            this.Name = "";
            this.Url = "";
            this.ImageAltText = "";
            this.ImageAltDictionaryKey = "";
            this.HasFocalPoint = false;
            this.FocalPointTop = 0;
            this.FocalPointLeft = 0;
            this.OriginalPixelHeight = 0;
            this.OriginalPixelWidth = 0;
        }

        /// <summary>
        /// Returns a localized alt text, if a Dictionary Key and/or Alt Text have been specified
        /// </summary>
        /// <param name="PreferDictionary">Using the Dictionary is preferred, so if the dictionary value for the language is missing, return data about that. If you'd rather get the default Alt text from the media item (regardless of language) set this to FALSE</param>
        /// <returns></returns>
        public string GetLocalizedAltText(UmbracoHelper umbraco, bool PreferDictionary = true)
        {
            if (this.ImageAltDictionaryKey == "")
            {
                return this.ImageAltText;
            }
            else
            {
                var dictVal = umbraco.GetDictionaryValue(this.ImageAltDictionaryKey);

                if (dictVal != "")
                {
                    return dictVal;
                }
                else
                {
                    if (PreferDictionary)
                    {
                        return string.Format("[{0}]", this.ImageAltDictionaryKey);
                    }
                    else
                    {
                        return this.ImageAltText;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a Url to crop the image
        /// </summary>
        /// <param name="Width">Width in pixels</param>
        /// <param name="Height">Height in pixels</param>
        /// <returns></returns>
        public string GetCropUrl(int Width, int Height)
        {
            var url = this.Url;

            url += "?width=" + Width;
            url += "&height=" + Height;

            if (this.HasFocalPoint)
            {
                url += "&center=" + this.FocalPointTop + "," + this.FocalPointLeft;
                url += "&mode=crop";
            }

            return url;
        }

        /// <summary>
        /// Returns a url for a resized version
        /// </summary>
        /// <param name="Height">Pixel Height (use zero to exclude value)</param>
        /// <param name="Width">Pixel Width (use zero to exclude value)</param>
        /// <param name="BgColorForPadding">Hex code for color used to fill background, since there is no up-sizing. Example: "#FFFFFF"</param>
        /// <param name="AdditionalParameters">List of Key/Value Pairs of additional ImageProcessor options</param>
        /// <returns></returns>
        public string GetResizeUrl(int Width, int Height, string BgColorForPadding, IEnumerable<KeyValuePair<string, string>> AdditionalParameters = null)
        {
            var baseUrl = this.Url;
            var bgColor = BgColorForPadding.Replace("#", "");

            var dimensions = "";
            if (Width > 0 & Height > 0)
            {
                dimensions = $"width={Width}&height={Height}";
            }
            else if (Width == 0 & Height > 0)
            {
                dimensions = $"height={Height}";
            }
            else if (Width > 0 & Height == 0)
            {
                dimensions = $"width={Width}";
            }

            var additionalParams = "";
            if (AdditionalParameters != null)
            {
                foreach (var kv in AdditionalParameters)
                {
                    var param = $"&{kv.Key}={kv.Value}";
                    additionalParams = additionalParams + param;
                }
            }

            var url = $"{baseUrl}?{dimensions}&upscale=false&bgcolor={bgColor}{additionalParams}";

            return url;
        }

        /// <summary>
        /// Returns a url for a resized version
        /// </summary>
        /// <param name="Height">Pixel Height (use zero to exclude value)</param>
        /// <param name="Width">Pixel Width (use zero to exclude value)</param>
        /// <param name="BgColorForPadding">Hex code for color used to fill background, since there is no up-sizing. Example: "#FFFFFF"</param>
        /// <returns></returns>
        public string GetResizeUrl(int Width, int Height, string BgColorForPadding)
        {
            return GetResizeUrl(Width, Height, BgColorForPadding, AdditionalParameters: null);
        }

        /// <summary>
        /// Returns a url for a resized version
        /// </summary>
        /// <param name="Height">Pixel Height (use zero to exclude value)</param>
        /// <param name="Width">Pixel Width (use zero to exclude value)</param>
        /// <param name="BgColorForPadding">Hex code for color used to fill background, since there is no up-sizing. Example: "#FFFFFF"</param>
        /// <param name="AdditionalParametersAsString">Separated string of Key/Value Pairs of additional ImageProcessor options</param>
        /// <returns></returns>
        public string GetResizeUrl(int Width, int Height, string BgColorForPadding, string AdditionalParametersAsString)
        {
            if (AdditionalParametersAsString == "")
            {
                return GetResizeUrl(Width, Height, BgColorForPadding, AdditionalParameters: null);
            }

            var kvParams = Dragonfly.NetHelpers.Strings.ParseStringToKvPairs(AdditionalParametersAsString);

            return GetResizeUrl(Width, Height, BgColorForPadding, AdditionalParameters: kvParams);
        }
        
        /// <summary>
        /// Gets the full url, including domain.
        /// </summary>
        /// <param name="AlternateDomain">If blank, will use the current hostname.</param>
        /// <returns></returns>
        public string AbsoluteUrl(string AlternateDomain = "")
        {
            var domain = AlternateDomain != "" ?  AlternateDomain : HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            var protocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
            string absUrl = string.Format("{0}{1}{2}", protocol, domain, this.Url);

            return absUrl;
        }
    }


}
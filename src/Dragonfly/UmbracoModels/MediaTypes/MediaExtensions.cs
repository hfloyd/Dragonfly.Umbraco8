namespace Dragonfly.UmbracoModels
{
    using Dragonfly.UmbracoHelpers;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors.ValueConverters;
    using Umbraco.Web;


    public static class MediaExtensions
    {
        private const string ThisClassName = "Dragonfly.UmbracoModels.MediaExtensions";

        #region IPublishedContent to IMediaFile

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="MediaFile"/>
        /// </summary>
        /// <param name="MediaContent"></param>
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// The <see cref="MediaFile"/>.
        /// </returns>
        public static IMediaFile ToIMediaFile(this IPublishedContent MediaContent)
        {
            return new MediaFile()
            {
                Content = MediaContent,
                Id = MediaContent.Id,
                Bytes = MediaContent.GetSafeInt("umbracoBytes", 0),
                Extension = MediaContent.GetSafeString("umbracoExtension"),
                Url = MediaContent.GetSafeString("umbracoFile"), //MediaContent.Url,
                Name = MediaContent.Name
            };
        }

        /// <summary>
        /// Creates a collection of <see cref="MediaFile"/> from a list of <see cref="IPublishedContent"/> (media)
        /// </summary>
        /// <param name="Contents">
        /// The collection of <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// The collection of <see cref="MediaFile"/>.
        /// </returns>
        public static IEnumerable<IMediaFile> ToIMediaFiles(this IEnumerable<IPublishedContent> Contents)
        {
            var iMedia = Contents.ToList().Select<IPublishedContent, IMediaFile>(X => X.ToIMediaFile());
            return iMedia;
        }


        #endregion

        #region IPublishedContent to IMediaImage

        /// <summary>
        /// Creates a collection of <see cref="MediaImage"/> from a list of <see cref="IPublishedContent"/> (media)
        /// </summary>
        /// <param name="Contents">
        /// The collection of <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// The collection of <see cref="MediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> ToIMediaImages(this IEnumerable<IPublishedContent> Contents)
        {
            var images = Contents.ToList().Select(X => X.ToIMediaImage());
            return images;
        }

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="MediaImage"/>
        /// </summary>
        /// <returns>
        /// The <see cref="MediaImage"/>.
        /// </returns>
        public static IMediaImage ToIMediaImage(this IPublishedContent MediaContent)
        {
            var img = new MediaImage()
            {
                Content = MediaContent,
                Id = MediaContent.Id,
                Bytes = MediaContent.GetSafeInt("umbracoBytes", 0),
                Extension = MediaContent.GetSafeString("umbracoExtension"),
                Name = MediaContent.Name,
                //ImageAltText = altText,
                //ImageAltDictionaryKey = AltDictionary != "" ? AltDictionary : MediaContent.GetSafeString("ImageAltDictionaryKey"),
                OriginalPixelWidth = MediaContent.GetSafeInt("umbracoWidth", 0),
                OriginalPixelHeight = MediaContent.GetSafeInt("umbracoHeight", 0)
            };

            var urlDataObj = MediaContent.Value("umbracoFile");
            var urlDataType = urlDataObj.GetType().ToString();

            if (urlDataType.Contains("ImageCropperValue"))
            {
                var urlDataCropSet = urlDataObj as ImageCropperValue;
                img.Url = urlDataCropSet.Src;
                img.CropData = urlDataCropSet;

                //if (urlDataCropSet.HasFocalPoint())
                //{
                //    img.HasFocalPoint = true;
                //    img.FocalPointLeft = Convert.ToDouble(urlDataCropSet.FocalPoint.Left);
                //    img.FocalPointTop = Convert.ToDouble(urlDataCropSet.FocalPoint.Top);
                //}
                //else
                //{
                //    img.HasFocalPoint = false;
                //    img.FocalPointLeft = 0;
                //    img.FocalPointTop = 0;
                //}
            }

            else
            {
                img.Url = urlDataObj.ToString();
                //img.JsonCropData = null;
                //img.FocalPointLeft = 0;
                //img.FocalPointTop = 0;
            }

            return img;
        }

        #endregion

        #region Building MediaImage from string data

        /// <summary>
        /// Create a <see cref="MediaImage"/> from an Image URL
        /// </summary>
        /// <returns>
        /// The <see cref="MediaImage"/>.
        /// </returns>
        public static MediaImage ImgUrlToImage(string ImageSrcUrl, string ImageName = "")
        {
            var img = new MediaImage()
            {
                Name = ImageName,
                Content = null,
                Id = 0,
                Url = ImageSrcUrl
            };

            if (ImageSrcUrl != "")
            {
                try
                {
                    img = AddFileInfoFromServer(img);
                }
                catch (Exception e)
                {
                    var msg =
                        $"{ThisClassName}.ImgUrlToImage() ERROR [ImageSrcUrl:{ImageSrcUrl}] [ImageName: {ImageName}]";
                    //LogHelper.Error<IMediaImage>(msg, e);
                }
            }

            return img;
        }

        private static MediaImage AddFileInfoFromServer(MediaImage MediaImage)
        {
            if (MediaImage.Url != "")
            {
                //Get image info
                var serverPath = System.Web.HttpContext.Current.Server.MapPath(MediaImage.Url);

                try
                {
                    var imgFileInfo = new System.IO.FileInfo(serverPath);
                    MediaImage.Bytes = Convert.ToInt32(imgFileInfo.Length);
                    MediaImage.Extension = imgFileInfo.Extension;

                    var imgDimensions = System.Drawing.Image.FromFile(serverPath).Size;
                    MediaImage.OriginalPixelWidth = imgDimensions.Width;
                    MediaImage.OriginalPixelHeight = imgDimensions.Height;
                }
                catch (System.IO.FileNotFoundException fnfEx)
                {
                    var msg =
                        $"{ThisClassName}.AddFileInfoFromServer() File Not Found on Disk: '{serverPath}'. Unable to get file details for IMediaImage [MediaImage: {MediaImage.Name}]";
                    //LogHelper.Error<FileInfo>(msg, fnfEx);
                }
            }
            else
            {
                var msg =
                    $"{ThisClassName}.AddFileInfoFromServer() - No Image URL present for IMediaImage '{MediaImage.Name}'";
                //LogHelper.Info<MediaImage>(msg);
            }

            return MediaImage;
        }

        #endregion

        #region GetSafeMediaFile

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultImage">
        /// The default image.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaFile"/>.
        /// </returns>
        public static IMediaFile GetSafeMediaFile(this IPublishedContent Content, UmbracoHelper Umbraco, string PropertyAlias)
        {
            //Check for property
            if (!Content.HasPropertyWithValue(PropertyAlias))
            {
                //return empty file
                return new MediaFile();
            }

            //check for property value
            var propValue = Content.Value(PropertyAlias);

            var type = propValue.GetType().ToString();
            if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
            {
                //this IS an Umbraco media item
                dynamic umbMedia = propValue;
                if (umbMedia.Id != 0)
                {
                    var mediaNode = Umbraco.Media(umbMedia.Id) as IPublishedContent;
                    var mFile = mediaNode.ToIMediaFile();

                    return mFile;
                }
            }
            else
            {
                var mediaContent = Content.GetSafeMultiContent(PropertyAlias, Umbraco).ToArray();

                if (mediaContent.Any())
                {
                    return mediaContent.Where(X => X != null).Select(X => X.ToIMediaFile()).FirstOrDefault();
                }
                else
                {
                    var allFiles = new List<IMediaFile>();

                    var emptyFile = new MediaFile();
                    allFiles.Add(emptyFile);

                    return allFiles.FirstOrDefault();
                }
            }

            //return empty file
            return new MediaFile();
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>s
        /// </summary>
        /// <param name="Content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="defaultFile">
        /// The default File.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaFile"/>.
        /// </returns>
        public static IEnumerable<IMediaFile> GetSafeMediaFiles(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco)
        {
            var mediaContent = Content.GetSafeMultiContent(PropertyAlias, Umbraco).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(X => X != null).Select(X => X.ToIMediaFile());
            }
            else
            {
                return new List<MediaFile>();
            }
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>s
        /// </summary>
        /// <param name="Content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="DefaultFile">
        /// The default File.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaFile"/>.
        /// </returns>
        public static IEnumerable<IMediaFile> GetSafeMediaFiles(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco, IMediaFile DefaultFile)
        {
            var mediaContent = Content.GetSafeMultiContent(PropertyAlias, Umbraco).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(X => X != null).Select(X => X.ToIMediaFile());
            }
            else
            {
                return new List<MediaFile>() { DefaultFile as MediaFile };
            }

        }

        #endregion

        #region GetSafeImage

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage GetSafeImage(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco)
        {
            return Content.GetSafeImage(PropertyAlias, Umbraco, null);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>
        /// </summary>
        /// <param name="Content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="DefaultImage">
        /// The default image.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage GetSafeImage(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco, IMediaImage DefaultImage)
        {
            //Check for property
            if (!Content.HasPropertyWithValue(PropertyAlias))
            {
                //return default or empty image
                return DefaultImage != null ? DefaultImage : new MediaImage();
            }

            //check for property value
            var propValue = Content.Value(PropertyAlias);
            if (propValue != null)
            {
                var type = propValue.GetType().ToString();
                if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
                {
                    //this IS an Umbraco media item
                    dynamic umbMedia = propValue;
                    if (umbMedia.Id != 0)
                    {
                        var mediaNode = Umbraco.Media(umbMedia.Id) as IPublishedContent;
                        var mImage = mediaNode.ToIMediaImage();

                        return mImage;
                    }
                }
                //if (type == "Umbraco.Web.Models.ImageCropDataSet")
                //{
                //    var cropData = propValue as Umbraco.Web.Models.ImageCropDataSet;
                //    if (cropData.HasImage())
                //    {
                //        var mImage = cropData.ToImage();

                //        return mImage;
                //    }
                //}
                else
                {
                    var mediaContent = Content.GetSafeMultiContent(PropertyAlias, Umbraco).ToArray();

                    if (mediaContent.Any())
                    {
                        return mediaContent.Where(X => X != null).Select(X => X.ToIMediaImage()).FirstOrDefault();
                    }
                    else
                    {
                        var allImages = new List<IMediaImage>();

                        if (DefaultImage != null)
                        {
                            allImages.Add(DefaultImage);
                        }
                        else
                        {
                            var emptyImg = new MediaImage();
                            allImages.Add(emptyImg);
                        }

                        return allImages.FirstOrDefault();
                    }
                }
            }

            //return default or empty image
            return DefaultImage != null ? DefaultImage : new MediaImage();
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>s
        /// </summary>
        /// <param name="Content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="DefaultImage">
        /// A default image to return if there are no results
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> GetSafeImages(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco, IMediaImage DefaultImage)
        {
            var mediaContent = Content.GetSafeMultiContent(PropertyAlias, Umbraco).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(X => X != null).Select(X => X.ToIMediaImage());
            }
            else
            {
                if (DefaultImage != null)
                {
                    return new List<IMediaImage>() { DefaultImage };
                }
                else
                {
                    var emptyImg = new MediaImage();
                    return new List<IMediaImage>() { emptyImg };
                }
            }
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>s
        /// </summary>
        /// <param name="Content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="PropertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="defaultImage">
        /// A default image to return if there are no results
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> GetSafeImages(this IPublishedContent Content, string PropertyAlias, UmbracoHelper Umbraco)
        {
            var mediaContent = Content.GetSafeMultiContent(PropertyAlias, Umbraco).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(X => X != null).Select(X => X.ToIMediaImage());
            }
            else
            {
                return new List<MediaImage>();
            }
        }


        #endregion

        #region IMediaFile Extensions

        /// <summary>
        /// Gets the full url, including domain.
        /// </summary>
        /// <param name="AlternateDomain">If blank, will use the current hostname.</param>
        /// <returns></returns>
        public static string AbsoluteUrl(this IMediaImage media, string AlternateDomain = "")
        {
            if (media != null && media.Content != null)
            {
                var domain = AlternateDomain != ""
                    ? AlternateDomain
                    : HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
                var protocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
                string absUrl = string.Format("{0}{1}{2}", protocol, domain, media.Url);

                return absUrl;
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region IMediaImage Extensions

        /// <summary>
        /// Returns a Url to crop the image
        /// </summary>
        /// <param name="Width">Width in pixels</param>
        /// <param name="Height">Height in pixels</param>
        /// <returns></returns>
        public static string GetCropUrl(this IMediaImage media, int Width, int Height)
        {
            if (media != null && media.Content != null)
            {
                var url = media.Url;

                url += "?width=" + Width;
                url += "&height=" + Height;

                if (media.CropData != null)
                {
                    if (media.CropData.HasFocalPoint())
                    {
                        url += "&center=" + media.CropData.FocalPoint.Top + "," + media.CropData.FocalPoint.Left;
                        url += "&mode=crop";
                    }
                }

                return url;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a url for a resized version
        /// </summary>
        /// <param name="Height">Pixel Height (use zero to exclude value)</param>
        /// <param name="Width">Pixel Width (use zero to exclude value)</param>
        /// <param name="BgColorForPadding">Hex code for color used to fill background, since there is no up-sizing. Example: "#FFFFFF"</param>
        /// <param name="AdditionalParameters">List of Key/Value Pairs of additional ImageProcessor options</param>
        /// <returns></returns>
        public static string GetResizeUrl(this IMediaImage media, int Width, int Height, string BgColorForPadding, IEnumerable<KeyValuePair<string, string>> AdditionalParameters = null)
        {
            if (media != null && media.Content != null)
            {
                var baseUrl = media.Url;
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
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a url for a resized version
        /// </summary>
        /// <param name="Height">Pixel Height (use zero to exclude value)</param>
        /// <param name="Width">Pixel Width (use zero to exclude value)</param>
        /// <param name="BgColorForPadding">Hex code for color used to fill background, since there is no up-sizing. Example: "#FFFFFF"</param>
        /// <returns></returns>
        public static string GetResizeUrl(this IMediaImage media, int Width, int Height, string BgColorForPadding)
        {
            return GetResizeUrl(media, Width, Height, BgColorForPadding, AdditionalParameters: null);
        }

        /// <summary>
        /// Returns a url for a resized version
        /// </summary>
        /// <param name="Height">Pixel Height (use zero to exclude value)</param>
        /// <param name="Width">Pixel Width (use zero to exclude value)</param>
        /// <param name="BgColorForPadding">Hex code for color used to fill background, since there is no up-sizing. Example: "#FFFFFF"</param>
        /// <param name="AdditionalParametersAsString">Separated string of Key/Value Pairs of additional ImageProcessor options</param>
        /// <returns></returns>
        public static string GetResizeUrl(this IMediaImage media, int Width, int Height, string BgColorForPadding, string AdditionalParametersAsString)
        {
            if (AdditionalParametersAsString == "")
            {
                return GetResizeUrl(media, Width, Height, BgColorForPadding, AdditionalParameters: null);
            }

            var kvParams = Dragonfly.NetHelpers.Strings.ParseStringToKvPairs(AdditionalParametersAsString);

            return GetResizeUrl(media, Width, Height, BgColorForPadding, AdditionalParameters: kvParams);
        }



        #endregion

    }
}

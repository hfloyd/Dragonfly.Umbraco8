namespace Dragonfly.UmbracoModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DataTypes;
    using Newtonsoft.Json.Linq;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;



    public static class MediaExtensions
    {
        private const string ThisClassName = "Dragonfly.UmbracoModels.MediaExtensions";

        #region To IEnum<MediaFile> extensions

        public static IEnumerable<MediaFile> ToMediaFiles(this IEnumerable<IMediaFile> IMedias)
        {
            var all = new List<MediaFile>();

            foreach (var iMedia in IMedias)
            {
                if (iMedia != null)
                {
                    all.Add(iMedia as MediaFile);
                }
            }

            return all;
        }

        /// <summary>
        /// Creates a collection of <see cref="IMediaFile"/> from a list of <see cref="IPublishedContent"/> (media)
        /// </summary>
        /// <param name="contents">
        /// The collection of <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// The collection of <see cref="IMediaFile"/>.
        /// </returns>
        public static IEnumerable<MediaFile> ToMediaFiles(this IEnumerable<IPublishedContent> contents)
        {
            var iMedia = contents.ToList().Select<IPublishedContent, IMediaFile>(x => x.ToMediaFile());
            return iMedia.ToMediaFiles();
        }

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="IMediaFile"/>
        /// </summary>W
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// The <see cref="IMediaFile"/>.
        /// </returns>
        public static MediaFile ToMediaFile(this IPublishedContent MediaContent)
        {
            return new MediaFile()
            {
                Content = MediaContent,
                Id = MediaContent.Id,
                Bytes = MediaContent.GetSafeInt("umbracoBytes", 0),
                Extension = MediaContent.GetSafeString("umbracoExtension"),
                Url = MediaContent.Url,
                Name = MediaContent.Name
            };
        }

        #endregion

        #region To IEnum<MediaImage> extensions

        public static IEnumerable<MediaImage> ToMediaImages(this IEnumerable<IMediaImage> IMedias)
        {
            var all = new List<MediaImage>();

            foreach (var iMedia in IMedias)
            {
                if (iMedia != null)
                {
                    all.Add(iMedia as MediaImage);
                }
            }

            return all;
        }

        /// <summary>
        /// Creates a collection of <see cref="IMediaImage"/> from a list of <see cref="IPublishedContent"/> (media)
        /// </summary>
        /// <param name="contents">
        /// The collection of <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// The collection of <see cref="IMediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> ToImages(this IEnumerable<IPublishedContent> contents)
        {
            return contents.ToList().Select(x => x.ToImage());
        }


        #endregion

        #region To IMediaImage extensions

        //TODO: Cleanup and consolidate all these so they work the same...

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="IMediaImage"/>
        /// </summary>W
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> of the Media node
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        [Obsolete("Use 'ToMediaImage()'")]
        public static IMediaImage ToImage(this IPublishedContent content)
        {
            return content.ToMediaImage();
        }

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="IMediaImage"/>
        /// </summary>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage ToImage(this ImageCropDataSet CropData)
        {
            var img = new MediaImage()
            {
                Content = null,
                Id = 0,
                Bytes = 0,
                Url = CropData.Src,
                Extension = "", //TODO: extract extension from filename (Src)
                Name = "", //TODO: extract name from filename (Src)
                ImageAltText = "",
                ImageAltDictionaryKey = "",
                OriginalPixelWidth = 0,
                OriginalPixelHeight = 0
            };

            if (CropData.HasFocalPoint())
            {
                img.HasFocalPoint = true;
                img.FocalPointLeft = Convert.ToDouble(CropData.FocalPoint.Left);
                img.FocalPointTop = Convert.ToDouble(CropData.FocalPoint.Top);
            }
            else
            {
                img.HasFocalPoint = false;
                img.FocalPointLeft = 0;
                img.FocalPointTop = 0;
            }

            return img;
        }

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="IMediaImage"/>
        /// </summary>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage ToMediaImage(this IPublishedContent MediaContent, string ImageName = "", string AltText = "", string AltDictionary = "")
        {
            var name = ImageName != "" ? ImageName : MediaContent.GetSafeString("Name");
            if (name == "")
            {
                name = MediaContent.Name;
            }

            var altText = AltText != "" ? AltText : MediaContent.GetSafeString("ImageAltText");
            if (altText == "")
            {
                altText = MediaContent.Name;
            }

            var img = new MediaImage()
            {
                Content = MediaContent,
                Id = MediaContent.Id,
                Bytes = MediaContent.GetSafeInt("umbracoBytes", 0),
                Extension = MediaContent.GetSafeString("umbracoExtension"),
                Name = name,
                ImageAltText = altText,
                ImageAltDictionaryKey = AltDictionary != "" ? AltDictionary : MediaContent.GetSafeString("ImageAltDictionaryKey"),
                OriginalPixelWidth = MediaContent.GetSafeInt("umbracoWidth", 0),
                OriginalPixelHeight = MediaContent.GetSafeInt("umbracoHeight", 0)
            };

            var urlDataObj = MediaContent.GetPropertyValue("umbracoFile");
            var urlDataType = urlDataObj.GetType().ToString();

            if (urlDataType == "Umbraco.Web.Models.ImageCropDataSet")
            {
                var urlDataCropSet = urlDataObj as ImageCropDataSet;
                img.Url = urlDataCropSet.Src;

                if (urlDataCropSet.HasFocalPoint())
                {
                    img.HasFocalPoint = true;
                    img.FocalPointLeft = Convert.ToDouble(urlDataCropSet.FocalPoint.Left);
                    img.FocalPointTop = Convert.ToDouble(urlDataCropSet.FocalPoint.Top);
                }
                else
                {
                    img.HasFocalPoint = false;
                    img.FocalPointLeft = 0;
                    img.FocalPointTop = 0;
                }
            }
            else if (urlDataType == "System.String")
            {
                var urlDataString = urlDataObj as string;

                if (urlDataString.StartsWith("{"))
                {
                    JObject jsonCrop = JObject.Parse(urlDataString);

                    img.JsonCropData = jsonCrop;

                    JToken src;
                    var srcFound = jsonCrop.TryGetValue("src", out src);
                    if (srcFound)
                    {
                        img.Url = src.ToString();
                    }
                    else
                    {
                        img.Url = "";
                    }

                    JToken focalPoint;
                    var focalPointFound = jsonCrop.TryGetValue("focalPoint", out focalPoint);
                    if (focalPointFound)
                    {
                        img.HasFocalPoint = true;

                        var left = focalPoint["left"].ToString();
                        img.FocalPointLeft = Convert.ToDouble(left);

                        var top = focalPoint["top"].ToString();
                        img.FocalPointTop = Convert.ToDouble(top);
                    }
                    else
                    {
                        img.HasFocalPoint = false;
                        img.FocalPointLeft = 0;
                        img.FocalPointTop = 0;
                    }
                }
                else
                {
                    img.Url = urlDataString.ToString();
                    img.JsonCropData = null;
                    img.FocalPointLeft = 0;
                    img.FocalPointTop = 0;
                }
            }
            return img;
        }


        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> crop to an <see cref="IMediaImage"/>
        /// </summary>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage CropperPropertyToImage(string ImageCropperProperty, string ImageName = "", string AltText = "", string AltDictionary = "")
        {
            try
            {
                var jsonProp = new JObject(ImageCropperProperty);
                return CropperPropertyToImage(jsonProp, ImageName, AltText, AltDictionary);
            }
            catch (ArgumentException exSystemArgumentException)
            {
                //most likely because only a file path is present, no crop data
                return ImgUrlToImage(ImageCropperProperty, ImageName, AltText, AltDictionary);
            }

        }

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="IMediaImage"/>
        /// </summary>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage CropperPropertyToImage(JObject ImageCropperProperty, string ImageName = "", string AltText = "", string AltDictionary = "")
        {
            var img = new MediaImage()
            {
                Name = ImageName,
                ImageAltText = AltText,
                ImageAltDictionaryKey = AltDictionary,
                Content = null,
                Id = 0
            };

            if (ImageCropperProperty != null)
            {
                try
                {
                    //var jsonCrop = JObject.Parse(ImageCropperProperty);
                    var jsonCrop = ImageCropperProperty;
                    img.JsonCropData = jsonCrop;

                    JToken src;
                    var srcFound = jsonCrop.TryGetValue("src", out src);
                    if (srcFound)
                    {
                        img.Url = src.ToString();
                    }
                    else
                    {
                        img.Url = "";
                    }

                    JToken focalPoint;
                    var focalPointFound = jsonCrop.TryGetValue("focalPoint", out focalPoint);
                    if (focalPointFound)
                    {
                        img.HasFocalPoint = true;

                        var left = focalPoint["left"].ToString();
                        img.FocalPointLeft = Convert.ToDouble(left);

                        var top = focalPoint["top"].ToString();
                        img.FocalPointTop = Convert.ToDouble(top);
                    }
                    else
                    {
                        img.HasFocalPoint = false;
                        img.FocalPointLeft = 0;
                        img.FocalPointTop = 0;
                    }

                    //Get image info
                    img = AddFileInfoFromServer(img);
                }
                catch (Exception e)
                {
                    var msg =
                        $"{ThisClassName}.CropperPropertyToImage() ERROR [ImageCropperProperty:{ImageCropperProperty}] [ImageName: {ImageName}]";
                    LogHelper.Error<IMediaImage>(msg, e);
                }
            }

            return img;
        }

        /// <summary>
        /// Utility extension to convert <see cref="IPublishedContent"/> to an <see cref="IMediaImage"/>
        /// </summary>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage ImgUrlToImage(string ImageSrcUrl, string ImageName = "", string AltText = "", string AltDictionary = "")
        {
            var img = new MediaImage()
            {
                Name = ImageName,
                ImageAltText = AltText,
                ImageAltDictionaryKey = AltDictionary,
                Content = null,
                Id = 0,
                Url = ImageSrcUrl,
                HasFocalPoint = false,
                FocalPointLeft = 0,
                FocalPointTop = 0
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
                    LogHelper.Error<IMediaImage>(msg, e);
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
                    LogHelper.Error<FileInfo>(msg, fnfEx);
                }
            }
            else
            {
                var msg =
                    $"{ThisClassName}.AddFileInfoFromServer() - No Image URL present for IMediaImage '{MediaImage.Name}'";
                LogHelper.Info<MediaImage>(msg);
            }

            return MediaImage;
        }

        #endregion

        #region GetSafeMediaFile

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaFile"/>.
        /// </returns>
        public static IMediaFile GetSafeMediaFile(this RenderModel model, UmbracoHelper umbraco, string propertyAlias)
        {
            return model.Content.GetSafeMediaFile(umbraco, propertyAlias);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaFile"/>.
        /// </returns>
        //public static IMediaFile GetSafeMediaFile(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        //{
        //    return content.GetSafeMediaFile(umbraco, propertyAlias, null);
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultImage">
        /// The default image.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaFile"/>.
        /// </returns>
        public static IMediaFile GetSafeMediaFile(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            //Check for property
            if (!content.HasPropertyWithValue(propertyAlias))
            {
                //return empty file
                return new MediaFile();
            }

            //check for property value
            var propValue = content.GetPropertyValue(propertyAlias);

            var type = propValue.GetType().ToString();
            if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
            {
                //this IS an Umbraco media item
                dynamic umbMedia = propValue;
                if (umbMedia.Id != 0)
                {
                    var mediaNode = umbraco.TypedMedia(umbMedia.Id) as IPublishedContent;
                    var mFile = mediaNode.ToMediaFile();

                    return mFile;
                }
            }
            else
            {
                var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

                if (mediaContent.Any())
                {
                    return mediaContent.Where(x => x != null).Select(x => x.ToMediaFile()).FirstOrDefault();
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
        /// <param name="model">
        /// The <see cref="RenderModel"/> which has the media picker property
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias of the media picker
        /// </param>        
        /// <returns>
        /// A collection of <see cref="IMediaFile"/>.
        /// </returns>
        public static IEnumerable<IMediaFile> GetSafeMediaFiles(this RenderModel model, UmbracoHelper umbraco, string propertyAlias)
        {
            return model.Content.GetSafeMediaFiles(umbraco, propertyAlias, null);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaFile"/> representation
        /// of the property or the default <see cref="IMediaFile"/>s
        /// </summary>
        /// <param name="content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="defaultFile">
        /// The default File.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaFile"/>.
        /// </returns>
        public static IEnumerable<MediaFile> GetSafeMediaFiles(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(x => x != null).Select(x => x.ToMediaFile());
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
        /// <param name="content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="defaultFile">
        /// The default File.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaFile"/>.
        /// </returns>
        public static IEnumerable<MediaFile> GetSafeMediaFiles(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias, IMediaFile defaultFile)
        {
            var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(x => x != null).Select(x => x.ToMediaFile());
            }
            else
            {
                return new List<MediaFile>() { defaultFile as MediaFile };
            }


        }

        #endregion

        #region GetSafeImage

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage GetSafeImage(this RenderModel model, UmbracoHelper umbraco, string propertyAlias)
        {
            return model.Content.GetSafeImage(umbraco, propertyAlias);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage GetSafeImage(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            return content.GetSafeImage(umbraco, propertyAlias, null);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias.
        /// </param>
        /// <param name="defaultImage">
        /// The default image.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage GetSafeImage(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias, IMediaImage defaultImage)
        {
            //Check for property
            if (!content.HasPropertyWithValue(propertyAlias))
            {
                //return default or empty image
                return defaultImage != null ? defaultImage : new MediaImage();
            }

            //check for property value
            var propValue = content.GetPropertyValue(propertyAlias);
            if (propValue != null)
            {
                var type = propValue.GetType().ToString();
                if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
                {
                    //this IS an Umbraco media item
                    dynamic umbMedia = propValue;
                    if (umbMedia.Id != 0)
                    {
                        var mediaNode = umbraco.TypedMedia(umbMedia.Id) as IPublishedContent;
                        var mImage = mediaNode.ToImage();

                        return mImage;
                    }
                }
                if (type == "Umbraco.Web.Models.ImageCropDataSet")
                {
                    var cropData = propValue as Umbraco.Web.Models.ImageCropDataSet;
                    if (cropData.HasImage())
                    {
                        var mImage = cropData.ToImage();

                        return mImage;
                    }
                }
                else
                {
                    var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

                    if (mediaContent.Any())
                    {
                        return mediaContent.Where(x => x != null).Select(x => x.ToImage()).FirstOrDefault();
                    }
                    else
                    {
                        var allImages = new List<IMediaImage>();

                        if (defaultImage != null)
                        {
                            allImages.Add(defaultImage);
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
            return defaultImage != null ? defaultImage : new MediaImage();
        }

        ///// <summary>
        ///// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        ///// of the property or the default <see cref="IMediaImage"/>
        ///// </summary>
        ///// <param name="propValue">
        ///// The Property Value
        ///// </param>
        ///// <param name="umbraco">
        ///// The <see cref="UmbracoHelper"/>
        ///// </param>
        ///// <param name="propertyAlias">
        ///// The Umbraco property alias.
        ///// </param>
        ///// <param name="defaultImage">
        ///// The default image.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IMediaImage"/>.
        ///// </returns>
        //public static IMediaImage PropertyValueToSafeImage(this object propValue, UmbracoHelper umbraco, string propertyAlias, IMediaImage defaultImage)
        //{
        //    var type = propValue.GetType().ToString();
        //    if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
        //    {
        //        //this IS an Umbraco media item
        //        dynamic umbMedia = propValue;
        //        var mImage = new MediaImage()
        //        {
        //            Id = umbMedia.Id,
        //            Name = umbMedia.Name,
        //            Content = umbMedia.Id != 0 ? umbraco.TypedMedia(umbMedia.Id) : null
        //        };

        //        if (mImage.Content != null)
        //        {
        //            var imgCont = mImage.Content as IPublishedContent;

        //            mImage.Bytes = imgCont.GetSafeInt("umbracoBytes", 0);
        //            mImage.Extension = imgCont.GetSafeString("umbracoExtension");
        //            mImage.Url = imgCont.GetSafeString("umbracoFile");
        //            //TODO: Add support for this
        //            //mImage.AbsoluteUrl
        //        }

        //        return mImage;
        //    }
        //    else
        //    {
        //        //var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

        //        //if (mediaContent.Any())
        //        //{
        //        //    return mediaContent.Where(x => x != null).Select(x => x.ToImage()).FirstOrDefault();
        //        //}
        //        //else
        //        //{
        //        //    var allImages = new List<IMediaImage>();

        //        //    if (defaultImage != null)
        //        //    {
        //        //        allImages.Add(defaultImage);
        //        //    }
        //        //    else
        //        //    {
        //        //        var emptyImg = new MediaImage();
        //        //        allImages.Add(emptyImg);
        //        //    }

        //        //    return allImages.FirstOrDefault();
        //        //}

        //        //return default or empty image
        //        return defaultImage != null ? defaultImage : new MediaImage();
        //    }
        //}

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>s
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/> which has the media picker property
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias of the media picker
        /// </param>        
        /// <returns>
        /// A collection of <see cref="IMediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> GetSafeImages(this RenderModel model, UmbracoHelper umbraco, string propertyAlias)
        {
            return model.Content.GetSafeImages(umbraco, propertyAlias, null);
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IMediaImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>s
        /// </summary>
        /// <param name="content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="defaultImage">
        /// A default image to return if there are no results
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> GetSafeImages(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias, IMediaImage defaultImage)
        {
            var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(x => x != null).Select(x => x.ToImage());
            }
            else
            {
                if (defaultImage != null)
                {
                    return new List<IMediaImage>() { defaultImage };
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
        /// <param name="content">
        /// The content which has the media picker property
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="propertyAlias">
        /// The property alias of the media picker
        /// </param>
        /// <param name="defaultImage">
        /// A default image to return if there are no results
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMediaImage"/>.
        /// </returns>
        public static IEnumerable<IMediaImage> GetSafeImages(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(x => x != null).Select(x => x.ToImage());
            }
            else
            {
                return new List<IMediaImage>();
            }
        }


        #endregion
    }
}

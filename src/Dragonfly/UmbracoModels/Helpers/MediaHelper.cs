namespace Dragonfly.UmbracoModels.Helpers
{
    using DataTypes;
    using Dragonfly.UmbracoHelpers;
    using Dragonfly.UmbracoModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;

    public static class MediaHelper
    {
        public static IMediaImage MediaIdToSafeImage(int? MediaId, UmbracoHelper UmbHelper, IMediaImage DefaultImage = null)
        {
            if (MediaId == null || MediaId == 0)
            {
                return DefaultImage != null ? DefaultImage : new MediaImage();
            }
            else
            {
                var mediaNode = UmbHelper.Media(MediaId);
                if (mediaNode != null)
                {
                    var mImage = mediaNode.ToMediaImage();
                    return mImage;
                }
                else
                {
                    //return default or empty image
                    return DefaultImage != null ? DefaultImage : new MediaImage();
                }
            }
        }

        public static IMediaImage MediaIdToSafeImage(Guid? MediaGuid, UmbracoHelper Umbraco, IMediaImage DefaultImage = null)
        {
            if (MediaGuid == null)
            {
                return DefaultImage != null ? DefaultImage : new MediaImage();
            }
            else
            {
                var mediaNode = Umbraco.Media(MediaGuid);
                if (mediaNode != null)
                {
                    var mImage = mediaNode.ToMediaImage();
                    return mImage;
                }
                else
                {
                    //return default or empty image
                    return DefaultImage != null ? DefaultImage : new MediaImage();
                }
            }
        }


        /// <summary>
        /// Checks if the model has a property and a value for the property and returns either the <see cref="IImage"/> representation
        /// of the property or the default <see cref="IMediaImage"/>
        /// </summary>
        /// <param name="PropValue">
        /// The Property Value
        /// </param>
        /// <param name="Umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="DefaultImage">
        /// The default image.
        /// </param>
        /// <returns>
        /// The <see cref="IMediaImage"/>.
        /// </returns>
        public static IMediaImage PropertyValueToSafeImage(object PropValue, UmbracoHelper Umbraco, IMediaImage DefaultImage = null)
        {
            if (PropValue == null)
            {
                return DefaultImage != null ? DefaultImage : new MediaImage();
            }

            var type = PropValue.GetType().ToString();
            if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
            {
                //this IS an Umbraco media item
                dynamic umbMedia = PropValue;
                if (umbMedia.Id != 0)
                {
                    var mediaNode = Umbraco.Media(umbMedia.Id) as IPublishedContent;
                    var mImage = mediaNode.ToMediaImage();

                    return mImage;
                }
                else
                {
                    return DefaultImage != null ? DefaultImage : new MediaImage();
                }
            }
            else if (type == "System.String")
            {
                //see if this is a media id...
                dynamic umbMedia = Umbraco.Media(PropValue);
                var mediaNode = Umbraco.Media(umbMedia.Id) as IPublishedContent;
                var mImage = mediaNode.ToMediaImage();

                return mImage;
            }
            else
            {
                //var mediaContent = content.GetSafeMntpContent(umbraco, propertyAlias, true).ToArray();

                //if (mediaContent.Any())
                //{
                //    return mediaContent.Where(x => x != null).Select(x => x.ToImage()).FirstOrDefault();
                //}
                //else
                //{
                //    var allImages = new List<IMediaImage>();

                //    if (defaultImage != null)
                //    {
                //        allImages.Add(defaultImage);
                //    }
                //    else
                //    {
                //        var emptyImg = new MediaImage();
                //        allImages.Add(emptyImg);
                //    }

                //    return allImages.FirstOrDefault();
                //}

                //return default or empty image
                return DefaultImage != null ? DefaultImage : new MediaImage();
            }
        }

        public static IEnumerable<MediaImage> GatherImages(IPublishedContent TopMediaItem)
        {
            var images = new List<MediaImage>();

            if (TopMediaItem.ContentType.Alias != "Folder")
            {
                var mediaImage = TopMediaItem.ToMediaImage();
                images.Add(mediaImage as MediaImage);
            }
            else
            {
                foreach (var media in TopMediaItem.Children)
                {
                    if (media.ContentType.Alias == "Folder")
                    {
                        foreach (var child in media.Children)
                        {
                            images.AddRange(GatherImages(child));
                        }
                    }
                    else
                    {
                        images.Add(media.ToMediaImage() as MediaImage);
                    }
                }
            }

            return images;
        }

        public static IMediaImage GetMediaImage(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            //Check for property
            if (!content.HasPropertyWithValue(propertyAlias))
            {
                //return default or empty image
                return new MediaImage();
            }

            //check for property value
            var propValue = content.Value(propertyAlias);

            var type = propValue.GetType().ToString();
            if (type == "Umbraco.Web.PublishedCache.XmlPublishedCache.PublishedMediaCache+DictionaryPublishedContent")
            {
                //this IS an Umbraco media item
                dynamic umbMedia = propValue;
                if (umbMedia.Id != 0)
                {
                    var mediaNode = umbraco.Media(umbMedia.Id) as IPublishedContent;
                    var mImage = mediaNode.ToMediaImage();

                    return mImage;
                }
            }
            else
            {
                var mediaContent = content.GetMntpContent(propertyAlias).ToArray();

                if (mediaContent.Any())
                {
                    return mediaContent.Where(x => x != null).Select(x => x.ToMediaImage()).FirstOrDefault();
                }
                else
                {
                    var allImages = new List<IMediaImage>();

                    //if (defaultImage != null)
                    //{
                    //    allImages.Add(defaultImage);
                    //}
                    //else
                    //{
                    var emptyImg = new MediaImage();
                    allImages.Add(emptyImg);
                    //}

                    return allImages.FirstOrDefault();
                }
            }

            //return default or empty image
            return new MediaImage();


            //var mediaHelper = new MediaHelper();
            // return mediaHelper.PropertyValueToSafeImage()
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
        public static IEnumerable<IMediaImage> GetMediaImages(this IPublishedContent content, UmbracoHelper umbraco, string propertyAlias)
        {
            var mediaContent = content.GetMntpContent(propertyAlias).ToArray();

            if (mediaContent.Any())
            {
                return mediaContent.Where(x => x != null).Select(x => x.ToMediaImage());
            }
            else
            {
                return new List<IMediaImage>();
            }
        }

    }

}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentExtensions.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The content extensions.
//   From: https://gist.github.com/jbreuer/dde3605035179c34b7287850c45cb8c9
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dragonfly.UmbracoHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Core.Strings;
    
    public static class IContentExtensions
    {

        /// <summary>
        /// Convert an IContent to an IPublishedContent.
        /// </summary>
        /// <param name="Content">
        /// The content.
        /// </param>
        /// <param name="IsPreview">
        /// The is preview.
        /// </param>
        /// <returns>
        /// The <see cref="IPublishedContent"/>.
        /// </returns>
        public static IPublishedContent ToPublishedContent(this IContent Content, bool IsPreview = false)
        {
            return new PublishedContent(Content, IsPreview);
        }
    }

    /// <summary>
    /// The published content.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class PublishedContent : PublishedContentWithKeyBase, IPublishedContent
    {
        private readonly PublishedContentType _contentType;

        private readonly IContent _inner;

        private readonly bool _isPreviewing;

        private readonly Lazy<string> _lazyCreatorName;

        private readonly Lazy<string> _lazyUrlName;

        private readonly Lazy<string> _lazyWriterName;

        private readonly IPublishedProperty[] _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedContent"/> class.
        /// </summary>
        /// <param name="Inner">
        /// The inner.
        /// </param>
        /// <param name="IsPreviewing">
        /// The is previewing.
        /// </param>
        public PublishedContent(IContent Inner, bool IsPreviewing)
        {
            if (Inner == null)
            {
                throw new NullReferenceException("inner");
            }

            this._inner = Inner;
            this._isPreviewing = IsPreviewing;

            this._lazyUrlName = new Lazy<string>(() => this._inner.GetUrlSegment().ToLower());
#pragma warning disable 618
            this._lazyCreatorName = new Lazy<string>(() => this._inner.GetCreatorProfile().Name);
#pragma warning restore 618
#pragma warning disable 618
            this._lazyWriterName = new Lazy<string>(() => this._inner.GetWriterProfile().Name);
#pragma warning restore 618

            this._contentType = PublishedContentType.Get(PublishedItemType.Content, this._inner.ContentType.Alias);

            this._properties =
                MapProperties(
                    this._contentType.PropertyTypes,
                    this._inner.Properties,
                    (T, V) => new PublishedProperty(T, V, this._isPreviewing)).ToArray();
        }

        ///// <summary>
        ///// Gets the id.
        ///// </summary>
        //public override int Id
        //{
        //    get
        //    {
        //        return this.inner.Id;
        //    }
        //}

        ///// <summary>
        ///// Gets the key.
        ///// </summary>
        //public override Guid Key
        //{
        //    get
        //    {
        //        return this.inner.Key;
        //    }
        //}

        ///// <summary>
        ///// Gets the document type id.
        ///// </summary>
        //public override int DocumentTypeId
        //{
        //    get
        //    {
        //        return this.inner.ContentTypeId;
        //    }
        //}

        ///// <summary>
        ///// Gets the document type alias.
        ///// </summary>
        //public override string DocumentTypeAlias
        //{
        //    get
        //    {
        //        return this.inner.ContentType.Alias;
        //    }
        //}

        /// <summary>
        /// Gets the item type.
        /// </summary>
        public override PublishedItemType ItemType
        {
            get
            {
                return PublishedItemType.Content;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return this._inner.Name;
            }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        public override int Level
        {
            get
            {
                return this._inner.Level;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public override string Path
        {
            get
            {
                return this._inner.Path;
            }
        }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        public override int SortOrder
        {
            get
            {
                return this._inner.SortOrder;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public override Guid Version
        {
            get
            {
                return this._inner.Version;
            }
        }

        /// <summary>
        /// Gets the template id.
        /// </summary>
        public override int TemplateId
        {
            get
            {
                return this._inner.Template == null ? 0 : this._inner.Template.Id;
            }
        }

        /// <summary>
        /// Gets the url name.
        /// </summary>
        public override string UrlName
        {
            get
            {
                return this._lazyUrlName.Value;
            }
        }

        /// <summary>
        /// Gets the create date.
        /// </summary>
        public override DateTime CreateDate
        {
            get
            {
                return this._inner.CreateDate;
            }
        }

        /// <summary>
        /// Gets the update date.
        /// </summary>
        public override DateTime UpdateDate
        {
            get
            {
                return this._inner.UpdateDate;
            }
        }

        /// <summary>
        /// Gets the creator id.
        /// </summary>
        public override int CreatorId
        {
            get
            {
                return this._inner.CreatorId;
            }
        }

        /// <summary>
        /// Gets the creator name.
        /// </summary>
        public override string CreatorName
        {
            get
            {
                return this._lazyCreatorName.Value;
            }
        }

        /// <summary>
        /// Gets the writer id.
        /// </summary>
        public override int WriterId
        {
            get
            {
                return this._inner.WriterId;
            }
        }

        /// <summary>
        /// Gets the writer name.
        /// </summary>
        public override string WriterName
        {
            get
            {
                return this._lazyWriterName.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is draft.
        /// </summary>
        public override bool IsDraft
        {
            get
            {
                return this._inner.Published == false;
            }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public override IPublishedContent Parent
        {
            get
            {
                var parent = this._inner.Parent();
                if (parent != null)
                {
                    return parent.ToPublishedContent(this._isPreviewing);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public override IEnumerable<IPublishedContent> Children
        {
            get
            {
                var children = this._inner.Children().ToList();

                return
                    children.Select(X => X.ToPublishedContent(this._isPreviewing))
                        .Where(X => X != null)
                        .OrderBy(X => X.SortOrder);
            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public override ICollection<IPublishedProperty> Properties
        {
            get
            {
                return this._properties;
            }
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public override PublishedContentType ContentType
        {
            get
            {
                return this._contentType;
            }
        }

        ///// <summary>
        ///// The get property.
        ///// </summary>
        ///// <param name="alias">
        ///// The alias.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IPublishedProperty"/>.
        ///// </returns>
        //public override IPublishedProperty GetProperty(string alias)
        //{
        //    return this.properties.FirstOrDefault(x => x.PropertyTypeAlias.InvariantEquals(alias));
        //}

        /// <summary>
        /// The map properties.
        /// </summary>
        /// <param name="PropertyTypes">
        /// The property types.
        /// </param>
        /// <param name="Properties">
        /// The properties.
        /// </param>
        /// <param name="Map">
        /// The map.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        internal static IEnumerable<IPublishedProperty> MapProperties(
            IEnumerable<PublishedPropertyType> PropertyTypes,
            IEnumerable<Property> Properties,
            Func<PublishedPropertyType, object, IPublishedProperty> Map)
        {
            var propertyEditorResolver = PropertyEditorResolver.Current;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            return PropertyTypes.Select(
                X =>
                {
                    var p = Properties.SingleOrDefault(Xx => Xx.Alias == X.PropertyTypeAlias);
                    var v = p == null || p.Value == null ? null : p.Value;
                    if (v != null)
                    {
                        var e = propertyEditorResolver.GetByAlias(X.PropertyEditorAlias);

                        // We are converting to string, even for database values which are integer or
                        // DateTime, which is not optimum. Doing differently would require that we have a way to tell
                        // whether the conversion to XML string changes something or not... which we don't, and we
                        // don't want to implement it as PropertyValueEditor.ConvertDbToXml/String should die anyway.

                        // Don't think about improving the situation here: this is a corner case and the real
                        // thing to do is to get rig of PropertyValueEditor.ConvertDbToXml/String.

                        // Use ConvertDbToString to keep it simple, although everywhere we use ConvertDbToXml and
                        // nothing ensures that the two methods are consistent.
                        if (e != null)
                        {
                            v = e.ValueEditor.ConvertDbToString(p, p.PropertyType, dataTypeService);
                        }
                    }

                    return Map(X, v);
                });
        }
    }

    internal static class ContentBaseExtensions
    {
        /// <summary>
        /// Gets the url segment providers.
        /// </summary>
        /// <remarks>This is so that unit tests that do not initialize the resolver do not
        /// fail and fall back to defaults. When running the whole Umbraco, CoreBootManager
        /// does initialise the resolver.</remarks>
        private static IEnumerable<IUrlSegmentProvider> UrlSegmentProviders
        {
            get
            {
                return UrlSegmentProviderResolver.HasCurrent
                           ? UrlSegmentProviderResolver.Current.Providers
                           : new IUrlSegmentProvider[] { new DefaultUrlSegmentProvider() };
            }
        }

        /// <summary>
        /// Gets the default url segment for a specified content.
        /// </summary>
        /// <param name="Content">
        /// The content.
        /// </param>
        /// <returns>
        /// The url segment.
        /// </returns>
        public static string GetUrlSegment(this IContentBase Content)
        {
            var url = UrlSegmentProviders.Select(P => P.GetUrlSegment(Content)).First(U => U != null);
            url = url ?? new DefaultUrlSegmentProvider().GetUrlSegment(Content); // be safe
            return url;
        }

        /// <summary>
        /// Gets the url segment for a specified content and culture.
        /// </summary>
        /// <param name="Content">
        /// The content.
        /// </param>
        /// <param name="Culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The url segment.
        /// </returns>
        public static string GetUrlSegment(this IContentBase Content, CultureInfo Culture)
        {
            var url = UrlSegmentProviders.Select(P => P.GetUrlSegment(Content, Culture)).First(U => U != null);
            url = url ?? new DefaultUrlSegmentProvider().GetUrlSegment(Content, Culture); // be safe
            return url;
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://umbraco.org/webservices/")]
    internal class PublishedProperty : PublishedPropertyBase
    {
        private readonly object _dataValue;

        private readonly bool _isPreviewing;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedProperty"/> class.
        /// </summary>
        /// <param name="PropertyType">
        /// The property type.
        /// </param>
        /// <param name="DataValue">
        /// The data value.
        /// </param>
        /// <param name="IsPreviewing">
        /// The is previewing.
        /// </param>
        public PublishedProperty(PublishedPropertyType PropertyType, object DataValue, bool IsPreviewing)
            : base(PropertyType)
        {
            this._dataValue = DataValue;
            this._isPreviewing = IsPreviewing;
        }

        /// <summary>
        /// Gets a value indicating whether has value.
        /// </summary>
        public override bool HasValue
        {
            get
            {
                return this._dataValue != null
                       && ((this._dataValue is string) == false
                           || string.IsNullOrWhiteSpace((string)this._dataValue) == false);
            }
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        public override object DataValue
        {
            get
            {
                return this._dataValue;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public override object Value
        {
            get
            {
                var source = this.PropertyType.ConvertDataToSource(this._dataValue, this._isPreviewing);
                return this.PropertyType.ConvertSourceToObject(source, this._isPreviewing);
            }
        }

        /// <summary>
        /// Gets the x path value.
        /// </summary>
        public override object XPathValue
        {
            get
            {
                var source = this.PropertyType.ConvertDataToSource(this._dataValue, this._isPreviewing);
                return this.PropertyType.ConvertSourceToXPath(source, this._isPreviewing);
            }
        }
    }

    internal abstract class PublishedPropertyBase : IPublishedProperty
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public readonly PublishedPropertyType PropertyType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedPropertyBase"/> class.
        /// </summary>
        /// <param name="PropertyType">
        /// The property type.
        /// </param>
        protected PublishedPropertyBase(PublishedPropertyType PropertyType)
        {
            if (PropertyType == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            this.PropertyType = PropertyType;
        }

        /// <summary>
        /// Gets the property type alias.
        /// </summary>
        public string PropertyTypeAlias
        {
            get
            {
                return this.PropertyType.PropertyTypeAlias;
            }
        }

        /// <summary>
        /// Gets a value indicating whether has value.
        /// </summary>
        public abstract bool HasValue { get; }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        public abstract object DataValue { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public abstract object Value { get; }

        /// <summary>
        /// Gets the x path value.
        /// </summary>
        public abstract object XPathValue { get; }
    }
}
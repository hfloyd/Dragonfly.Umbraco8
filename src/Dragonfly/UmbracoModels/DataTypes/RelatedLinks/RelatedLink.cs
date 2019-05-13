namespace Dragonfly.UmbracoModels.DataTypes
{
    using System;
    using Newtonsoft.Json.Linq;
    using Umbraco.Core.Models.PublishedContent;

    public class RelatedLink
    {
        #region Private Vars

        private readonly JToken _linkItem;

        private string _caption;

        private bool? _newWindow;

        private bool? _isInternal;

        private string _link;

        private bool? _linkDeleted;

        private RelatedLinkType _type;

        private int? _nodeId;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedLink"/> class.
        /// </summary>
        /// <param name="linkItem">
        /// The link item.
        /// </param>
        public RelatedLink(JToken linkItem)
        {
            this._linkItem = linkItem;

            // get the current Link to set the _linkDeleted is a internal link
            var currentLink = this.Link;
        }

        #endregion

        #region Properties & Methods

        /// <summary>
        /// Gets the caption.
        /// </summary>
        public string Caption
        {
            get
            {
                if (string.IsNullOrEmpty(this._caption))
                {
                    this._caption = this._linkItem.Value<string>("caption");
                }

                return this._caption;
            }
        }

        /// <summary>
        /// Gets a value indicating whether new window.
        /// </summary>
        public bool NewWindow
        {
            get
            {
                if (this._newWindow == null)
                {
                    this._newWindow = this._linkItem.Value<bool>("newWindow");
                }

                return this._newWindow.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the link is internal.
        /// </summary>
        public bool IsInternal
        {
            get
            {
                if (this._isInternal == null)
                {
                    this._isInternal = this._linkItem.Value<bool>("isInternal");
                }

                return this._isInternal.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public RelatedLinkType? Type
        {
            get
            {
                if (Enum.TryParse(this._linkItem.Value<string>("type"), true, out this._type))
                {
                    return this._type;    
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the link.
        /// </summary>
        public string Link
        {
            get
            {
                if (string.IsNullOrEmpty(this._link))
                {
                    if (this.IsInternal)
                    {
                        if (UmbracoContext.Current == null)
                        {
                            return null;
                        }

                        this._link = UmbracoContext.Current.UrlProvider.GetUrl(this._linkItem.Value<int>("internal"));
                        if (this._link.Equals("#"))
                        {
                            this._linkDeleted = true;
                            this._link = this._linkItem.Value<string>("internal");
                        }
                        else
                        {
                            this._linkDeleted = false;
                        }                        
                    }
                    else
                    {
                        this._link = this._linkItem.Value<string>("link");
                    }
                }

                return this._link;
            }
        }

        /// <summary>
        /// Gets the related Node Id.
        /// </summary>
        public int? NodeId
        {
            get
            {
                if (this.IsInternal)
                {
                    this._nodeId = this._linkItem.Value<int>("internal");
                }
                else
                {
                    this._nodeId = 0;
                }
                
                return this._nodeId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether deleted.
        /// </summary>
        internal bool InternalLinkDeleted
        {
            get
            {
                var linkDeleted = this._linkDeleted;
                return linkDeleted != null && (bool)linkDeleted;
            }
        }

        #endregion
    }
}

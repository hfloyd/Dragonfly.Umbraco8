namespace Dragonfly.UmbracoModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a link tier
    /// </summary>
    public interface ILinkTier : ILink
    {
        /// <summary>
        /// Gets or sets the children of the current tier
        /// </summary>
        List<ILinkTier> Children { get; set; }
    }

    /// <summary>
    /// Represents a link tier
    /// </summary>
    public class LinkTier : Link, ILinkTier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkTier"/> class.
        /// </summary>
        public LinkTier()
        {
            this.Children = new List<ILinkTier>();
        }

        /// <summary>
        /// Gets or sets the children <see cref="ILinkTier"/>
        /// </summary>
        public List<ILinkTier> Children { get; set; }
    }
}
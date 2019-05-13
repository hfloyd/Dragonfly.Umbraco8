namespace Dragonfly.UmbracoHelpers
{
    using System.Collections.Generic;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;

    public class PublishedContentComparer : IEqualityComparer<IPublishedContent>
        {
            public bool Equals(IPublishedContent X, IPublishedContent Y)
            {
                if (ReferenceEquals(X, Y)) return true;

                if (ReferenceEquals(X, null) || ReferenceEquals(Y, null)) return false;

                return X.Id == Y.Id;
            }

        public int GetHashCode(IPublishedContent Obj)
            {
                if (ReferenceEquals(Obj, null)) return 0;

                return Obj.Id.GetHashCode();
            }

    }

}

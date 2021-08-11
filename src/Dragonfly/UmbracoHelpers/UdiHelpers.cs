namespace Dragonfly.UmbracoHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Umbraco.Core;

    public static class UdiHelpers
    {
        public static IEnumerable<Udi> ToUdis(this IEnumerable<string> UdiStrings)
        {
            var udis = new List<Udi>();

            foreach (var s in UdiStrings)
            {
                Udi newUdi;
                var isUdi = Udi.TryParse(s, out newUdi);
                if (isUdi)
                { udis.Add(newUdi); }
            }

            return udis;
        }
    }
}

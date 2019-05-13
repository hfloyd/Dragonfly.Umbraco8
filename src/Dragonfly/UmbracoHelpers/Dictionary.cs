namespace Dragonfly.UmbracoHelpers
{
    using Umbraco.Core.Models.PublishedContent;

    public static class Dictionary
    {
        private const string ThisClassName = "Dragonfly.Umbraco7Helpers.Dictionary";

        private static UmbracoHelper umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

        //TODO: Add 'create' functionality options?

        /// <summary>
        /// Return a dictionary value or a placeholder representing the dictionary value which needs to be added
        /// </summary>
        /// <param name="DictionaryKey">The dictionary key to lookup</param>
        /// <param name="DefaultValue">A default value to return instead of the placeholder if the key is not found (optional)</param>
        /// <returns>Either the display text, or a placeholder surrounded by [square brackets]</returns>
        public static string GetDictionaryOrPlaceholder(string DictionaryKey, string DefaultValue = "")
        {
            var dictValue = umbracoHelper.GetDictionaryValue(DictionaryKey);
            if (dictValue == string.Empty)
            {
                if (DefaultValue != "")
                {
                    return DefaultValue;
                }
                else
                {
                    //Placeholder
                    return $"[{DictionaryKey}]";
                }
            }
            else
            {
                return dictValue;
            }
        }


    }
}

namespace Dragonfly.UmbracoHelpers
{
    public static class UmbracoForms
    {
        private const string ThisClassName = "Dragonfly.UmbracoHelpers.UmbracoForms";

        public static string GetFieldCodeName(string FieldCssClass)
        {
            var fieldCodeName = FieldCssClass.Replace("contourField", "");
            fieldCodeName = fieldCodeName.Replace("alternating", "");

            fieldCodeName = fieldCodeName.Replace("textfield", "");
            fieldCodeName = fieldCodeName.Replace("textarea", "");
            fieldCodeName = fieldCodeName.Replace("dropdownlist", "");
            fieldCodeName = fieldCodeName.Replace("checkbox", "");
            fieldCodeName = fieldCodeName.Replace("checkboxlist", "");
            fieldCodeName = fieldCodeName.Replace("datepicker", "");
            fieldCodeName = fieldCodeName.Replace("dropdownlist", "");
            fieldCodeName = fieldCodeName.Replace("fileupload", "");
            fieldCodeName = fieldCodeName.Replace("hiddenfield", "");
            fieldCodeName = fieldCodeName.Replace("passwordfield", "");
            fieldCodeName = fieldCodeName.Replace("radiobuttonlist", "");
            fieldCodeName = fieldCodeName.Replace("text", "");
            //fieldCodeName = fieldCodeName.Replace("x", "");

            fieldCodeName = fieldCodeName.Trim(' ');

            return fieldCodeName;
        }

    }
}

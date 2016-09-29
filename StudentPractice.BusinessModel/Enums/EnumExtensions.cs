using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.BusinessModel.Resources;

namespace StudentPractice.BusinessModel
{
    public static class EnumExtensions
    {
        private static System.Resources.ResourceManager GetResourceManager()
        {
            switch (LanguageService.GetUserLanguage())
            {
                case enLanguage.English:
                    return LabelsEN.ResourceManager;
                case enLanguage.Greek:
                default:
                    return Labels.ResourceManager;
            }
        }

        public static string GetLabel(this Enum enumeration)
        {
            var resourceKey = enumeration.GetType().Name + "_" + enumeration.ToString();
            var label = GetResourceManager().GetString(resourceKey);

            return string.IsNullOrEmpty(label) ? enumeration.ToString() : label;
        }

        public static string GetAcronym(this Enum enumeration)
        {
            var resourceKey = enumeration.GetType().Name + "_" + enumeration.ToString() + "_Acronym";
            var label = GetResourceManager().GetString(resourceKey);

            return string.IsNullOrEmpty(label) ? enumeration.ToString() : label;
        }

        public static string GetIcon(this Enum enumeration)
        {
            var resourceKey = enumeration.GetType().Name + "_" + enumeration.ToString() + "_Icon";
            var label = GetResourceManager().GetString(resourceKey);

            return string.IsNullOrEmpty(label) ? enumeration.ToString() : label;
        }

        public static int GetValue(this Enum enumeration)
        {
            return Convert.ToInt32(enumeration);
        }
    }
}

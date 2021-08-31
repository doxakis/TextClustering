using System;

namespace TextClustering
{
    public enum Language
    {
        Arabic,
        Bulgarian,
        Catalan,
        Czech,
        Danish,
        Dutch,
        English,
        Finnish,
        French,
        German,
        Gujarati,
        Hebrew,
        Hindi,
        Hungarian,
        Indonesian,
        Malaysian,
        Italian,
        Norwegian,
        Polish,
        Portuguese,
        Romanian,
        Russian,
        Slovak,
        Spanish,
        Swedish,
        Turkish,
        Ukrainian,
        Vietnamese
    }
    
    internal static class LanguageExtensions
    {
        internal static string GetShortCode(this Language language)
        {
            switch (language)
            {
                case Language.Arabic:
                    return "ar";
                case Language.Bulgarian:
                    return "bg";
                case Language.Catalan:
                    return "ca";
                case Language.Czech:
                    return "cs";
                case Language.Danish:
                    return "da";
                case Language.Dutch:
                    return "nl";
                case Language.English:
                    return "en";
                case Language.Finnish:
                    return "fi";
                case Language.French:
                    return "fr";
                case Language.German:
                    return "de";
                case Language.Gujarati:
                    return "gu";
                case Language.Hebrew:
                    return "he";
                case Language.Hindi:
                    return "hi";
                case Language.Hungarian:
                    return "hu";
                case Language.Indonesian:
                    return "id";
                case Language.Malaysian:
                    return "ms";
                case Language.Italian:
                    return "it";
                case Language.Norwegian:
                    return "nb";
                case Language.Polish:
                    return "pl";
                case Language.Portuguese:
                    return "pt";
                case Language.Romanian:
                    return "ro";
                case Language.Russian:
                    return "ru";
                case Language.Slovak:
                    return "sk";
                case Language.Spanish:
                    return "es";
                case Language.Swedish:
                    return "sv";
                case Language.Turkish:
                    return "tr";
                case Language.Ukrainian:
                    return "uk";
                case Language.Vietnamese:
                    return "vi";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}
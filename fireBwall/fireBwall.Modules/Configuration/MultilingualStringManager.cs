using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fireBwall.Configuration
{
    public enum Language
    {
        ENGLISH,
        SPANISH,
        GERMAN,
        CHINESE,
        RUSSIAN,
        PORTUGUESE,
        JAPANESE,
        ITALIAN,
        FRENCH,
        HEBREW,
        DUTCH
    }

    public class MultilingualStringManager
    {
        Dictionary<string, Dictionary<string, string>> strings = new Dictionary<string, Dictionary<string, string>>();

        public void SetString(Language language, string name, string value)
        {
            switch (language)
            {
                case Language.CHINESE:
                    SetString("zh", name, value);
                    break;
                case Language.ENGLISH:
                    SetString("en", name, value);
                    break;
                case Language.GERMAN:
                    SetString("de", name, value);
                    break;
                case Language.PORTUGUESE:
                    SetString("pt", name, value);
                    break;
                case Language.RUSSIAN:
                    SetString("ru", name, value);
                    break;
                case Language.SPANISH:
                    SetString("es", name, value);
                    break;
                case Language.JAPANESE:
                    SetString("ja", name, value);
                    break; 
                case Language.ITALIAN:
                    SetString("it", name, value);
                    break;
                case Language.FRENCH:
                    SetString("fr", name, value);
                    break;
                case Language.HEBREW:
                    SetString("he", name, value);
                    break;
                case Language.DUTCH:
                    SetString("nl", name, value);
                    break;
            }
        }

        public void SetString(string language, string name, string value)
        {
            if (!strings.ContainsKey(language))
            {
                strings[language] = new Dictionary<string, string>();
            }
            strings[language][name] = value;
        }

        public string GetString(string name)
        {
            string lang = "en";
            if (strings.ContainsKey(GeneralConfiguration.Instance.PreferredLanguage))
                lang = GeneralConfiguration.Instance.PreferredLanguage;
            if(strings[lang].ContainsKey(name))
                return strings[lang][name];
            return strings["en"][name];
        }
    }
}

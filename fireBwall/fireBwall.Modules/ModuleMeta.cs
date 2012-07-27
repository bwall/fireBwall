using System;
using System.Collections.Generic;
using fireBwall.Configuration;
using System.Text;

namespace fireBwall.Modules
{
    public class ModuleMeta
    {
        public struct Meta
        {
            public string Name;
            public string Version;
            public string Author;
            public string Description;
            public string Help;
            public string Contact;
        }

        Dictionary<string, Meta> multiLingualMetas = new Dictionary<string, Meta>();

        public ModuleMeta(Meta englishMeta)
        {
            AddMeta("en", englishMeta);
        }

        public void AddMeta(string Language, Meta meta)
        {
            multiLingualMetas[Language] = meta;
        }

        public Meta GetMeta()
        {
            string lang = "en";
            if (multiLingualMetas.ContainsKey(GeneralConfiguration.Instance.PreferredLanguage))
            {
                lang = GeneralConfiguration.Instance.PreferredLanguage;
            }
            return multiLingualMetas[lang];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using R2API;

namespace LostInTransit
{
    public static class LITUtil
    {
        public static void AddCooldownBuff(this CharacterBody body, BuffDef buffDef, float seconds)
        {
            for(int i = 0; i <= seconds; i++)
            {
                body.AddTimedBuff(buffDef, i);
            }
        }

        public static void AddTokenToLanguage(string key, string value, LangEnum language)
        {
            switch(language)
            {
                case LangEnum.de:
                    LanguageAPI.Add(key, value, "de");
                    break;
                case LangEnum.en:
                    LanguageAPI.Add(key, value, "en");
                    break;
                case LangEnum.es:
                    LanguageAPI.Add(key, value, "es-419");
                    break;
                case LangEnum.fr:
                    LanguageAPI.Add(key, value, "FR");
                    break;
                case LangEnum.it:
                    LanguageAPI.Add(key, value, "IT");
                    break;
                case LangEnum.ja:
                    LanguageAPI.Add(key, value, "ja");
                    break;
                case LangEnum.ko:
                    LanguageAPI.Add(key, value, "ko");
                    break;
                case LangEnum.br:
                    LanguageAPI.Add(key, value, "pt-BR");
                    break;
                case LangEnum.ru:
                    LanguageAPI.Add(key, value, "RU");
                    break;
                case LangEnum.tr:
                    LanguageAPI.Add(key, value, "tr");
                    break;
                case LangEnum.cn:
                    LanguageAPI.Add(key, value, "zh-CN");
                    break;
            }
        }
    }
    public enum LangEnum
    {
        de,
        en,
        es,
        fr,
        it,
        ja,
        ko,
        br,
        ru,
        tr,
        cn,
    }
}

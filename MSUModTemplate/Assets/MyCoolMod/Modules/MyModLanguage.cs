using RoR2;
using Moonstorm.Loaders;

namespace MyMod.Modules
{
    public class MyModLanguage : LanguageLoader<MyModLanguage>
    {
        public override string AssemblyDir => MyModAssets.Instance.AssemblyDir;

        public override string LanguagesFolderName => "LITLang";

        internal void Init()
        {
            LoadLanguages();
        }
    }
}

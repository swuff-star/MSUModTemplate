using RoR2;
using Moonstorm.Loaders;

namespace LostInTransit.Modules
{
    public class LITLanguage : LanguageLoader<LITLanguage>
    {
        public override string AssemblyDir => LITAssets.Instance.AssemblyDir;

        public override string LanguagesFolderName => "LITLang";

        internal void Init()
        {
            LoadLanguages();
        }
    }
}

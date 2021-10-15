using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostInTransit.Utils;
using R2API;

namespace LostInTransit.Modules
{
    public static class LITLanguage
    {
        public static string languageFileName = "LITLanguage.language";

        public static void Initialize()
        {
            LITLogger.LogI("Initializing Language...");
            var path = Path.Combine(Assets.AssemblyDir, languageFileName);
            LanguageAPI.AddPath(path);
        }
    }
}

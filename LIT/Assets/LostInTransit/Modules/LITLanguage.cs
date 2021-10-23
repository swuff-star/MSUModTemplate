using RoR2;
using System.Collections.Generic;
using Zio;
using Zio.FileSystems;

namespace LostInTransit.Modules
{
    public static class LITLanguage
    {
        public static FileSystem FileSystem { get; private set; }

        public static void Initialize()
        {
            PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem();
            FileSystem = new SubFileSystem(physicalFileSystem, physicalFileSystem.ConvertPathFromInternal(Assets.AssemblyDir), true);

            if(FileSystem.DirectoryExists("/Languages/"))
            {
                Language.collectLanguageRootFolders += delegate (List<DirectoryEntry> list)
                {
                    LITLogger.LogI($"Initializing Language");
                    list.Add(FileSystem.GetDirectoryEntry("/Languages/"));
                };
            }
        }
    }
}

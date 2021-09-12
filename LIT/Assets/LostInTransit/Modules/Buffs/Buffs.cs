using LostInTransit.Utils;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moonstorm;
using RoR2.ContentManagement;

namespace LostInTransit.Buffs
{
    public class Buffs : BuffModuleBase
    {
        public static Buffs Instance { get; set; }
        public static BuffDef[] LoadedLITBuffs { get => LITContent.serializableContentPack.buffDefs; }
        public override SerializableContentPack ContentPack { get; set; } = LITContent.serializableContentPack;


        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Buffs...");
            InitializeBuffs();
        }

        public override IEnumerable<BuffBase> InitializeBuffs()
        {
            base.InitializeBuffs()
                .ToList()
                .ForEach(buff => AddBuff(buff, ContentPack));
            return null;
        }

    }
}

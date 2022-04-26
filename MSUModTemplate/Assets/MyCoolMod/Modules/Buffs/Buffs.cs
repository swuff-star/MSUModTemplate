using Moonstorm;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Linq;

namespace MyMod.Buffs
{
    public sealed class Buffs : BuffModuleBase
    {
        public static Buffs Instance { get; set; }
        public static BuffDef[] LoadedLITBuffs { get => MyModContent.Instance.SerializableContentPack.buffDefs; }
        public override R2APISerializableContentPack SerializableContentPack => MyModContent.Instance.SerializableContentPack;


        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            MyModLogger.LogI($"Initializing Buffs...");
            GetBuffBases();
        }

        protected override IEnumerable<BuffBase> GetBuffBases()
        {
            base.GetBuffBases()
                .ToList()
                .ForEach(buff => AddBuff(buff));
            return null;
        }

    }
}

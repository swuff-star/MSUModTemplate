using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LostInTransit.Utils;
using Moonstorm;
using static R2API.DamageAPI;

namespace LostInTransit.DamageTypes
{
    public class DamageTypes : DamageTypeModuleBase
    {
        public static DamageTypes Instance { get; set; }
        public override Assembly Assembly { get; set; } = typeof(DamageTypes).Assembly;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Damage Types.");
            InitializeDamageTypes();
        }

        public override IEnumerable<DamageTypeBase> InitializeDamageTypes()
        {
            base.InitializeDamageTypes()
                .ToList()
                .ForEach(dType => AddDamageType(dType));
            return null;
        }
    }
}

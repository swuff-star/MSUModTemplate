using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostInTransit.Utils;
using static R2API.DamageAPI;
using DamageType = LostInTransit.DamageTypes.DamageTypeBase;

namespace LostInTransit.DamageTypes
{
    public static class DamageTypes
    {
        public static ModdedDamageType[] moddedDamageTypes
        {
            get
            {
                return moddedDamageTypes;
            }
        }

        public static Dictionary<ModdedDamageType, DamageType> damageTypes = new Dictionary<ModdedDamageType, DamageType>();

        public static void Initialize()
        {
            LITLogger.LogI("Initializing DamageTypes.");

            typeof(DamageTypes).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(DamageType)))
                .Select(type => (DamageType)Activator.CreateInstance(type))
                .ToList()
                .ForEach(damageType =>
                {
                    damageType.Initialize();
                    var moddedDamageType = damageType.GetDamageType();
                    damageTypes.Add(moddedDamageType, damageType);
                    LITLogger.LogD($"Added damage type {damageType.GetType().Name}");
                });
        }
    }
}

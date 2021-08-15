using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTransit
{
    public static class LITConfig
    {
        internal static ConfigEntry<bool> EnableItems;

        internal static ConfigEntry<bool> EnableEquipments;

        internal static void Initialize(ConfigFile config)
        {
            EnableItems = config.Bind<bool>("Lost in Transit Pickups", "Enable Items", true, "Wether or not Lost in Transit's items will be enabled.");
            EnableEquipments = config.Bind<bool>("Lost in Transit Pickups", "Enable Equipments", true, "Wether or not Lost in Transit's equipments will be enabled.");
        }
    }
}

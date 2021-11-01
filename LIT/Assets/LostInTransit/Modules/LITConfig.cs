using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System.Linq;
using R2API;

namespace LostInTransit
{
    public static class LITConfig
    {
        internal static ConfigFile ConfigFileBlightCost;
        internal static ConfigEntry<bool> EnableItems;
        internal static ConfigEntry<bool> EnableEquipments;
        internal static ConfigEntry<KeyCode> FrenziedBlink;
        internal static ConfigEntry<int> TPBlightCost;

        internal static void Initialize(ConfigFile config)
        {
            ConfigFileBlightCost = new ConfigFile($"{Paths.ConfigPath}\\{LITMain.GUID}.BlightCosts.cfg", true);
            EnableItems = config.Bind<bool>("Lost in Transit :: Pickups", "Enable Items", true, "Wether or not Lost in Transit's items will be enabled.");
            EnableEquipments = config.Bind<bool>("Lost in Transit :: Pickups", "Enable Equipments", true, "Wether or not Lost in Transit's equipments will be enabled.");
            FrenziedBlink = config.Bind<KeyCode>("Lost in Transit :: Keybinds", "AffixFrenzied Blink Key", KeyCode.F, "The key a player must press to use the blinking passive of the AffixFrenzied buff.");
            TPBlightCost = ConfigFileBlightCost.Bind<int>($"Teleboss Cost Multiplier", "Teleboss Cost Multiplier", 2, "Cost multiplied appled to the final body cost if the body is part of the teleporter boss, set this to a negative value to disable teleport bosses altogether.");
        }

        internal static int BindBlightCost(GameObject bodyPrefab, SpawnCard card)
        {
            return ConfigFileBlightCost.Bind<int>("Blight Costs",
                                                 $"{bodyPrefab.name} Blight Cost",
                                                 Mathf.RoundToInt(card.directorCreditCost / 10),
                                                 $"The cost of turning {bodyPrefab.name} into a blighted elite.\nSet this value to -1 to blacklist the body.").Value;
        }
    }
}

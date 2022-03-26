using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System.Linq;
using R2API;
using Moonstorm.Loaders;

namespace LostInTransit
{
    public class LITConfig : ConfigLoader<LITConfig>
    {
        public const string items = "LIT.Items";
        public const string equips = "LIT.Equips";
        public const string blight = "LIT.BlightCosts";

        public override BaseUnityPlugin MainClass { get; } = LITMain.instance;

        public override bool CreateSubFolder => true;

        public static ConfigFile itemConfig;
        public static ConfigFile equipsConfig;
        public static ConfigFile blightCost;

        internal static ConfigEntry<bool> enableItems;
        internal static ConfigEntry<bool> enableEquipments;
        internal static ConfigEntry<KeyCode> frenziedBlink;
        internal static ConfigEntry<int> tpBlightCost;

        public void Init()
        {
            itemConfig = CreateConfigFile(items, true);
            equipsConfig = CreateConfigFile(equips, true);
            blightCost = CreateConfigFile(blight, true);

            SetConfigs();
        }

        internal static void SetConfigs()
        {
            frenziedBlink = LITMain.config.Bind<KeyCode>("Lost in Transit :: Keybinds", "AffixFrenzied Blink Key", KeyCode.F, "The key a player must press to use the blinking passive of the AffixFrenzied buff.");
            tpBlightCost = blightCost.Bind<int>($"Teleboss Cost Multiplier", "Teleboss Cost Multiplier", 2, "Cost multiplied appled to the final body cost if the body is part of the teleporter boss, set this to a negative value to disable teleport bosses altogether.");
        }

        internal static int BindBlightCost(GameObject bodyPrefab, SpawnCard card)
        {
            return blightCost.Bind<int>("Blight Costs",
                                                 $"{bodyPrefab.name} Blight Cost",
                                                 Mathf.RoundToInt(card.directorCreditCost / 2),
                                                 $"The cost of turning {bodyPrefab.name} into a blighted elite.\nSet this value to -1 to blacklist the body.").Value;
        }
    }
}

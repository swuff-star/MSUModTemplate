using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LostInTransit
{
    public static class LITConfig
    {
        internal static ConfigEntry<bool> EnableItems;

        internal static ConfigEntry<bool> EnableEquipments;

        internal static ConfigEntry<KeyCode> FrenziedBlink;

        internal static ConfigEntry<string> BlightBlacklist;

        internal static void Initialize(ConfigFile config)
        {
            EnableItems = config.Bind<bool>("Lost in Transit :: Pickups", "Enable Items", true, "Wether or not Lost in Transit's items will be enabled.");
            EnableEquipments = config.Bind<bool>("Lost in Transit :: Pickups", "Enable Equipments", true, "Wether or not Lost in Transit's equipments will be enabled.");
            FrenziedBlink = config.Bind<KeyCode>("Lost in Transit :: Keybinds", "AffixFrenzied Blink Key", KeyCode.F, "The key a player must press to use the blinking passive of the AffixFrenzied buff.");
            BlightBlacklist = config.Bind<string>("Lost in Transit :: Blighted Elites", "Blighted Elite Blacklist", "BrotherBody, BrotherHurtBody, SuperRoboBallBossBody", "A List of CharacterBody names that can never be blighted elites.");
        }
    }
}

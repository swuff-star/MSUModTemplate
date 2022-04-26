using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System.Linq;
using R2API;
using Moonstorm.Loaders;

namespace MyMod
{
    public class MyModConfig : ConfigLoader<MyModConfig>
    {
        public const string items = "MyMod.Items";
        public const string equips = "MyMod.Equips";

        public override BaseUnityPlugin MainClass { get; } = MyModMain.instance;

        public override bool CreateSubFolder => true;

        public static ConfigFile itemConfig;
        public static ConfigFile equipsConfig;

        internal static ConfigEntry<bool> enableItems;
        internal static ConfigEntry<bool> enableEquipments;

        public void Init()
        {
            itemConfig = CreateConfigFile(items, true);
            equipsConfig = CreateConfigFile(equips, true);
        }
    }
}

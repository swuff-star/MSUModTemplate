using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.Cil;
using static LostInTransit.LostInTransitMain;
using LostInTransit.Equipment;

namespace LostInTransit.Items
{
    public class Amethyst : EquipmentBase

    {
        public override string EquipmentName => "Gigantic Amethyst";

        public override string EquipmentLangTokenName => "Gigantic_Amethyst";

        public override string EquipmentPickupDesc => "Reset cooldowns on use.";

        public override string EquipmentFullDescription => "Reset cooldowns on use.";

        public override string EquipmentLore => "[3:27 PM] the antichrist: i had a dream that i screamed the n word in front of a bunch of black people then i broke a cyanide capsule in my mouth and killed myself";

        public override GameObject EquipmentModel => MainAssets.LoadAsset<GameObject>("amethyst.prefab");

        public static float amethystCooldown;

        public override Sprite EquipmentIcon => MainAssets.LoadAsset<Sprite>("amethyst.png");
        public override bool AppearsInSinglePlayer { get; } = true;
        public override bool AppearsInMultiPlayer { get; } = true;
        public override bool CanDrop { get; } = true;
        
        public override float Cooldown => amethystCooldown;
        public override bool EnigmaCompatible { get; } = true;
        public override bool IsBoss { get; } = false;
        public override bool IsLunar { get; } = false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            amethystCooldown = config.Bind<float>("Equipment: " + EquipmentName, "Cooldown", 18f, "Cooldown between uses").Value;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            var sloc = slot.characterBody?.skillLocator;
            if (!sloc) return false;
            sloc.ApplyAmmoPack();
            return true;
        }
    }
}

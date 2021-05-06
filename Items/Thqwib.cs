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

/*namespace LostInTransit.Items
{
    public class Thqwib : EquipmentBase
    {
        public override string EquipmentName => "Thqwib";

        public override string EquipmentLangTokenName => "THQWIB";

        public override string EquipmentPickupDesc => "Release a bloom of thqwibs, detonating on impact";

        public override string EquipmentFullDescription => "Release a bloom of thqwibs, detonating on impact";

        public override string EquipmentLore => "Lore";

        public override GameObject EquipmentModel => MainAssets.LoadAsset<GameObject>("BeckoningCat.prefab");

        public override Sprite EquipmentIcon => MainAssets.LoadAsset<Sprite>("BeckoningCat.png");

        public float Thqwib1;
        public float Thqwib2;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            throw new NotImplementedException();
        }

        public override void Hooks()
        {
            throw new NotImplementedException();
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
            Thqwib1 = 1;
            Thqwib2 = 1;
        }

        public static GameObject ThqwibProjectile;

        public void CreateProjectile()
        {
            ThqwibProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/FMJ"), "ThqwibProjectile", true);

            var damage = ThqwibProjectile.GetComponent<RoR2.Projectile.ProjectileDamage>();
            damage.damageType = DamageType.Generic;
            damage.damage = 0;

            if (ThqwibProjectile) PrefabAPI.RegisterNetworkPrefab(ThqwibProjectile);

            ProjectileAPI.Add(ThqwibProjectile);

        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            throw new NotImplementedException();
        }


    }
}*/

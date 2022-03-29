using Moonstorm;
using R2API;
using RoR2.Items;
using On.RoR2;
using RoR2;
using System;
using UnityEngine;

namespace LostInTransit.Items
{
    public class RapidMitosis : ItemBase
    {
        private const string token = "LIT_ITEM_RAPIDMITOSIS_DESC";
        public override RoR2.ItemDef ItemDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<RoR2.ItemDef>("RapidMitosis");

        [ConfigurableField(ConfigName = "CDR Amount", ConfigDesc = "Extra Cooldown Per Rapid Mitosis.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float mitosisCD = 0.25f;


        public class RapidMitosisBehavior : BaseItemBodyBehavior
        {
            [ItemDefAssociation(useOnClient = true, useOnServer = true)]
            public static RoR2.ItemDef GetItemDef() => LITContent.Items.RapidMitosis;

            //★ please work
            public void Start()
            {
                On.RoR2.Inventory.CalculateEquipmentCooldownScale += (orig, self) =>
                {
                    float num = orig.Invoke(self);
                    num *= (1 - MSUtil.InverseHyperbolicScaling(mitosisCD, mitosisCD, 0.7f, stack));
                    return num;
                };
            }

            

            public void OnDestroy()
            {
                On.RoR2.Inventory.CalculateEquipmentCooldownScale -= (orig, self) =>
                {
                    float num = orig.Invoke(self);
                    num *= (1 - MSUtil.InverseHyperbolicScaling(mitosisCD, mitosisCD, 0.7f, stack));
                    return num;
                };
            }
        }
    }
}

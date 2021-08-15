using BepInEx.Configuration;
using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LostInTransit.LostInTransitMain;


namespace LostInTransit.Elites
{
    public abstract class EliteBase
    {
        public EliteDef eliteDef;
        public abstract string EliteName { get; }
        public abstract EquipmentDef AffixEquip { get; }
        public abstract Color32 EliteColor { get; }
        public abstract string EliteToken { get; }
        public virtual Sprite affixIconSprite { get; } = null;
        public virtual int desiredTierIndex { get; }

        public EquipmentDef EquipmentDef;

        public BuffDef eliteBuffDef;

        public abstract void Init(ConfigFile config);

        public virtual void Hooks()
        { }

    }
}

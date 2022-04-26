using RoR2.ContentManagement;
using R2API.ScriptableObjects;
using Moonstorm.Loaders;
using System;
using MyMod.Modules;
using System.Linq;
using RoR2;
using UnityEngine;

namespace MyMod
{
    public class MyModContent : ContentLoader<MyModContent>
    {
        public static class Buffs
        {
        }

        public static class Elites
        {
        }

        public static class Equipments
        {
        }

        public static class Items
        {
        }
        public override string identifier => MyModMain.GUID;

        public override R2APISerializableContentPack SerializableContentPack { get; protected set; } = MyModAssets.Instance.MainAssetBundle.LoadAsset<R2APISerializableContentPack>("ContentPack");
        public override Action[] LoadDispatchers { get; protected set; }
        public override Action[] PopulateFieldsDispatchers { get; protected set; }

        public override void Init()
        {
            base.Init();
            LoadDispatchers = new Action[]
            {
                delegate
                {
                    new MyMod.Buffs.Buffs().Initialize();
                },
                delegate
                {
                    new Modules.Equipments().Initialize();
                },
                delegate
                {
                    new Modules.Items().Initialize();
                },
                delegate
                {
                    new Modules.Elites().Initialize();
                },
                delegate
                {
                    new ItemDisplays().Initialize();
                },
                delegate
                {
                    SerializableContentPack.entityStateTypes = typeof(MyModContent).Assembly.GetTypes()
                        .Where(type => typeof(EntityStates.EntityState).IsAssignableFrom(type))
                        .Select(type => new EntityStates.SerializableEntityStateType(type))
                        .ToArray();
                },
                delegate
                {
                    SerializableContentPack.effectPrefabs = MyModAssets.LoadAllAssetsOfType<GameObject>().Where(go => go.GetComponent<EffectComponent>()).ToArray();
                },
                delegate
                {
                    MyModAssets.Instance.SwapMaterialShaders();
                }
            };

            PopulateFieldsDispatchers = new Action[]
            {
                delegate
                {
                    PopulateTypeFields(typeof(Buffs), ContentPack.buffDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Elites), ContentPack.eliteDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Equipments), ContentPack.equipmentDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Items), ContentPack.itemDefs);
                }
            };
        }
    }
}

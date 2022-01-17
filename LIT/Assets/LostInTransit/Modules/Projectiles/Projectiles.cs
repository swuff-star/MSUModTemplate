using Moonstorm;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LostInTransit.Modules
{
    public class Projectiles : ProjectileModuleBase
    {
        public static Projectiles Instance { get; set; }
        public override SerializableContentPack ContentPack { get; set; } = LITContent.Instance.SerializableContentPack;

        public override AssetBundle AssetBundle { get; set; } = LITAssets.Instance.MainAssetBundle;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Projectiles...");
            InitializeProjectiles();
        }

        public override IEnumerable<ProjectileBase> InitializeProjectiles()
        {
            base.InitializeProjectiles()
                .ToList()
                .ForEach(proj => AddProjectile(proj, ContentPack));
            return null;
        }
    }
}

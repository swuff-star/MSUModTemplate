using Moonstorm;
using R2API;
using RoR2.Projectile;
using UnityEngine;

namespace LostInTransit.Projectiles
{
    public class ThqwibProjectile : ProjectileBase
    {
        public override GameObject ProjectilePrefab { get; set; } = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/scavsackprojectile"), "ThqwibProjectile", false);

        public static GameObject ThqwibProj;

        public override void Initialize()
        {
            var go = Resources.Load<GameObject>("prefabs/characterbodies/scavsackprojectile");
            if (ProjectilePrefab)
            {
                var onKillComponent = ProjectilePrefab.GetComponent<ProjectileGrantOnKillOnDestroy>();
                if (onKillComponent)
                    GameObject.Destroy(onKillComponent);
                ProjectilePrefab.AddComponent<ProjectileChanceForOnKillOnDestroy>();
                ThqwibProj = ProjectilePrefab;
            }
        }
    }
}
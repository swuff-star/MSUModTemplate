using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using R2API;
using RoR2.Projectile;

namespace LostInTransit.Projectiles
{
    public class ThqwibProjectile : ProjectileBase
    {
        public override GameObject ProjectilePrefab { get; set; } = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/scavsackprojectile"), "ThqwibProjectile", false);

        public static GameObject ThqwibProj;

        public override void Initialize()
        {
            var go = Resources.Load<GameObject>("prefabs/characterbodies/scavsackprojectile");
            Debug.Log(go);
            if (ProjectilePrefab)
            {
                var onKillComponent = ProjectilePrefab.GetComponent<ProjectileGrantOnKillOnDestroy>();
                if (onKillComponent)
                    GameObject.Destroy(onKillComponent);
                ThqwibProj = ProjectilePrefab;
            }
        }
    }
}
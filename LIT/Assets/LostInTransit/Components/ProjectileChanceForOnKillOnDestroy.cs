using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using RoR2.Projectile;
using UnityEngine.Networking;

[RequireComponent(typeof(HealthComponent), typeof(ProjectileDamage), typeof(ProjectileController))]
public class ProjectileChanceForOnKillOnDestroy : MonoBehaviour
{
    public float chance;

    private ProjectileController projectileController;
    private ProjectileDamage projectileDamage;
    private HealthComponent healthComponent;

    private void OnDestroy()
    {
		healthComponent = GetComponent<HealthComponent>();
		projectileController = GetComponent<ProjectileController>();
		projectileDamage = GetComponent<ProjectileDamage>();
		if (NetworkServer.active && (bool)projectileController.owner)
		{
			if(Util.CheckRoll(chance, (float)projectileController.owner.GetComponent<CharacterBody>()?.master?.luck))
            {
				DamageInfo damageInfo = new DamageInfo
				{
					attacker = projectileController.owner,
					crit = projectileDamage.crit,
					damage = projectileDamage.damage,
					position = base.transform.position,
					procCoefficient = projectileController.procCoefficient,
					damageType = projectileDamage.damageType,
					damageColorIndex = projectileDamage.damageColorIndex
				};
				HealthComponent victim = healthComponent;
				DamageReport damageReport = new DamageReport(damageInfo, victim, damageInfo.damage, healthComponent.combinedHealth);
				GlobalEventManager.instance.OnCharacterDeath(damageReport);
			}
		}
	}
}

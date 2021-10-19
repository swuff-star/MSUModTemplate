using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonstorm;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LostInTransit.Equipments
{
	[DisabledContent]
    public class Thqwib : LITEquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("Thqwib");
		public static float damage;
		public static int thqwibAmount;
		public static float chance;

        public override void Initialize()
        {
			Config();
			DescriptionToken();
        }

        public override void Config()
        {
			var section = "Equipment: " + EquipmentDef.name;
			damage = LITMain.config.Bind<float>(section, "Damage per Projectile", 200, "Amount of %damage done by each projectile").Value;
			thqwibAmount = LITMain.config.Bind<int>(section, "Amount of Thqwibs", 30, "Amount of thqwibs to throw.").Value;
			var component = Projectiles.ThqwibProjectile.ThqwibProj.GetComponent<ProjectileChanceForOnKillOnDestroy>();
			if (component)
			{
				chance = LITMain.config.Bind<float>(section, "Chance for On Kill", 10f, "Chance for each Thqwib to trigger an on kill effect.").Value; ;
				component.chance = chance;
			}
		}

        public override void DescriptionToken()
		{
			LITUtil.AddTokenToLanguage(EquipmentDef.descriptionToken,
				$"Release a bloom of <style=cIsDamage>{thqwibAmount} thqwibs</style>, detonating on impact for <style=cIsDamage>{damage}%</style> damage. Each thqwib has a <style=cIsDamage>{chance}%</style> chance to trigger <style=cIsDamage>On-Kill</style> effects.",
				LangEnum.en);
		}
		public override bool FireAction(EquipmentSlot slot)
        {
			InputBankTest inputBank = slot.inputBank;
			CharacterBody charBody = slot.characterBody;
            if(inputBank && charBody)
            {
				Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
				Ray ray = aimRay;
				Ray ray2 = aimRay;
				Vector3 point = aimRay.GetPoint(10);
				bool flag = false;
				if (Util.CharacterRaycast(slot.gameObject, ray, out var hitInfo, 500f, (int)LayerIndex.world.mask | (int)LayerIndex.entityPrecise.mask, QueryTriggerInteraction.Ignore))
				{
					point = hitInfo.point;
					flag = true;
				}
				float magnitude = 40;
				if (flag)
				{
					Vector3 vector = point - ray2.origin;
					Vector2 vector2 = new Vector2(vector.x, vector.z);
					float magnitude2 = vector2.magnitude;
					Vector2 vector3 = vector2 / magnitude2;
					if (magnitude2 < 10)
					{
						magnitude2 = 10;
					}
					float y = Trajectory.CalculateInitialYSpeed(1, vector.y);
					float num = magnitude2 / 1;
					Vector3 direction = new Vector3(vector3.x * num, y, vector3.y * num);
					magnitude = direction.magnitude;
					ray2.direction = direction;
				}
				for (int i = 0; i < thqwibAmount; i++)
				{
					Quaternion rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(ray2.direction, 0, 25, 1f, 1f));
					ProjectileManager.instance.FireProjectile(Projectiles.ThqwibProjectile.ThqwibProj, ray2.origin, rotation, slot.gameObject, charBody.damage * (damage / 100), 0f, Util.CheckRoll(charBody.crit, charBody?.master), DamageColorIndex.Default, null, magnitude);
				}
				return true;
			}
            return false;
        }
    }
}

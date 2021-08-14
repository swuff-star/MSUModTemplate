using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using static LostInTransit.LostInTransitMain;
using UnityEngine;

namespace LostInTransit.Items
{
    class TelescopicSight : ItemBase
    {
        public override string ItemName => "Telescopic Sight";

        public override string ItemLangTokenName => "CRIT_SCOPE";

        public override string ItemPickupDesc => "Crits have a chance to crit";

        public override string ItemFullDescription => $"Gain <style=cIsUtility>{TelescopicSight.addCrit}% critical chance.</style> <style=cIsDamage>{TelescopicSight.procChance}%</style> <style=cStack>(+{TelescopicSight.stackChance}% per stack)</style> chance for critical strikes to do <style=cIsDamage>{TelescopicSight.dmgMultiplier}x damage</style>";

        public override string ItemLore => "<style=cStack>[A lone RV is driving down a desert road. camera cuts to the dashboard, where a bobblehead of the former Civillian sits. A hand comes from offscreen and flicks the head.]</style>\n\n<style=cDeath>Sniper</style>: \"Boom. Headshot.\"\n\n<style=cStack>[Meet the Sniper text appears. Cuts to the Sniper driving his RV as Valve's edited version of Magnum Force plays in the background.]</style>\n\n<style=cDeath>Sniper</style>: \"Snipin's a good job, mate! <style=cStack>[He pauses to make a right turn]</style> It's challengin' work, outta doors. I guarantee you'll not go hungry-\"\n\n<style=cStack>[Cuts to a shot of the Sniper brushing his teeth. There are three photographs on the right of his mirror of a BLU Engineer, Heavy, and Scout - the Engineer and the Scout are crossed off, but the Heavy is not.]</style>\n\n<style=cDeath>Sniper</style>: \"-cause at the end of the day, long as there's two people left on the planet, <style=cIsDamage>someone</style> is gonna want <style=cIsDamage>someone</style> dead.\"\n\n<style=cStack>[Scene cuts to view inside the Sniper's scope. The Sniper headshots the Heavy from the previous scene; the bullet shatters the Bottle of the Demoman behind him, causing the top half of the Bottle to embed itself in his remaining eye. The Demoman flails around, takes out his Grenade Launcher, fires three grenades wildly in the air and falls over a ledge, with his stray grenades igniting a cluster of explosive barrels below and causing a chain explosion.]</style>\n\n<style=cDeath>Sniper</style>: \"Ooh.\"\n\n<style=cStack>[Cuts to the Sniper talking on a pay phone.]</style>\n\n<style=cDeath>Sniper</style>: \"Dad? Dad, I'm a- Ye- <style=cIsDamage>Not</style> a \"crazed gunman\", dad, I'm an assassin! ...Well, the difference bein' one is a job and the other's a mental sickness!\"\n\n<style=cStack>[Back to Sniper in the RV]</style>\n\n<style=cDeath>Sniper</style>: \"I'll be honest with ya: my parents do <style=cIsDamage>not</style> care for it.\"\n\n<style=cStack>[Cuts to Sniper climbing the tall tower in Gold Rush Stage 3, Cap 1. The Sniper is now waiting at the top of the tower for a shot.]</style>\n\n<style=cDeath>Snpier</style>: <style=cStack>[Glances away from his scope briefly to address the viewer]</style> \"I think his mate saw me.\"\n\n<style=cStack>[A bullet ricochets off the ledge under the Sniper.]</style>\n\n<style=cDeath>Sniper</style>: \"Yes, yes he did!\"\n\n<style=cStack>[The Sniper takes cover as return fire ricochets off the tower.]\n\n[Cuts to a time-lapse image of the Sniper waiting for a shot. Several jars of urine fill up to his side, as the Sniper drinks coffee and waits. As the sun starts to set, the Sniper smiles and finally takes his shot.]\n\n[The Sniper backstabs a Spy through the chest with his Kukri on Gold Rush Stage 3, then slides him off the Knife with a satisfied expression.]</style>\n\n<style=cDeath>Sniper</style>: \"Feelins'? Look mate, you know who has a lot of feelings?\"\n\n<style=cStack>[Cut back to the RV]</style>\n\n<style=cDeath>Sniper</style>: \"Blokes that bludgeon to death with a golf trophy. Professionals have <style=cIsDamage>standards</style>.\"\n\n<style=cStack>[Sniper takes off his hat and puts it on his chest, standing over the dead Spy.]</style>\n\n<style=cDeath>Sniper</style>: \"Be polite.\"\n\n<style=cStack>[Sniper headshots a Medic, a Soldier, and a Pyro, blowing the last's head clean off.]</style>\n\n<style=cDeath>Sniper</style>: \"Be efficient.\"\n\n<style=cStack>[Slow-motion shot of the Sniper reloading his rifle.]</style>\n\n<style=cDeath>Sniper</style>: \"Have a plan to kill everyone you meet.\"\n\n<style=cStack>[The Sniper fires directly at the camera. The screen blacks out.]\n\n[Team Fortress 2 ending flourish music plays.]\n\n[Cut back to the pay phone.]</style>\n\n<style=cDeath>Sniper</style>: \"Dad... Dad p-, yeah - put Mum on the phone!\"";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("TelescopicSight.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("TelescopicSight.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public static float procChance;
        public static float stackChance;
        public static float dmgMultiplier;
        public static float addCrit;

        public void CreateConfig(ConfigFile config)
        {
            procChance = config.Bind<float>("Item: " + ItemName, "Base Proc Chance", 10f, "Base chance of double critting.").Value;
            stackChance = config.Bind<float>("Item: " + ItemName, "Stacking Proc Chance", 10f, "Added chance to double crit per stack.").Value;
            dmgMultiplier = config.Bind<float>("Item: " + ItemName, "Damage Multiplier", 5f, "How much damage is multiplied by.").Value;
            addCrit = config.Bind<float>("Item: " + ItemName, "Added Crit Chance", 10f, "How much regular crit chance is given.").Value;
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            GameObject attacker = damageInfo.attacker;
            if (self && attacker)
            {
                var attackerBody = attacker.GetComponent<CharacterBody>();
                int scopeCount = GetCount(attackerBody);
                if (scopeCount > 0)
                {
                    if (damageInfo.crit)
                    {
                        //Debug.Log("Pre-scope damage: " + damageInfo.damage);
                        if (Util.CheckRoll((procChance + (stackChance * (scopeCount - 1)))))
                        {
                            //This is not the ideal but I am left with no other options.
                            DamageInfo newDamageInfo = damageInfo;
                            newDamageInfo.damage = damageInfo.damage * (dmgMultiplier - 1);
                            victim.GetComponent<HealthComponent>().TakeDamage(newDamageInfo);
                            //Debug.Log("Scope Triggered for total damage of: " + damageInfo.damage);

                        }
                    }
                }
            }
            orig(self, damageInfo, victim);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            self.crit += addCrit;
        }
    }
}

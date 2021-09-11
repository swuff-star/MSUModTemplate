using RoR2;
using System;
using LostInTransit.Buffs;
using UnityEngine.Networking;
using Moonstorm;

namespace LostInTransit.Items
{
    public class GoldenGun : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("GoldenGun");
        public override void Initialize()
        {
            Stage.onStageStartGlobal += Stage_onServerStageBegin;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<GoldenGunBehavior>(stack);
        }

        public static float gunCap; //Gets calculated by taking the scaled cost of the gold cap.
        public static float goldCap = 600f; //Amount of gold to reach the cap, Stage 1

        private void Stage_onServerStageBegin(Stage obj)
        {
            if (!NetworkServer.active) return;
            gunCap = Run.instance.GetDifficultyScaledCost((int)goldCap); //If this isn't done here, scaling will update on the minute instead of with stages cleared. Feels inconsistent as fuck like that.
        }

        public class GoldenGunBehavior : CharacterBody.ItemBehavior
        {
            private void FixedUpdate()
            {
                //This all works, but the math is the tiniest bit inconsistent. This is due to the fact that 40 does not divide evenly into 700. Fuck Hopoo.
                //"Fixed" the above by buffing it to a 600 gold cap instead of 700.
                //Debug.Write("Current cap for Golden Gun: " + gunCap);
                float goldPerBuff = (gunCap / 40f); //To-do: Change the 40f out for a configurable value. This is hard to explain in words how the math works in a short desc.
                //Debug.Write("Amount of gold needed for one Golden Gun buff: " + goldPerBuff);
                float buffToGive = (body.master.money / (int)goldPerBuff);
                Math.Floor(buffToGive);
                //Debug.WriteLine("Amount of earned Golden Gun buffs: " + buffToGive);
                float currentBuffs = body.GetBuffCount(GoldenGunBuff.buff);
                if (currentBuffs < (40f + ((40f * 0.5f) * (stack - 1))) && buffToGive > currentBuffs)
                    { body.AddBuff(GoldenGunBuff.buff); }
                if (buffToGive < currentBuffs)
                    { body.RemoveBuff(GoldenGunBuff.buff); }
                //body.SetBuffCount(GoldenGunBuff.buffGGIndex, (int)buffToGive); This seemed like a much less hacky implementation, but for some reason gave the Leeching buff rather than Golden Gun buff.
                //To-do: Find cleaner implementation?
            }
        }
    }
}

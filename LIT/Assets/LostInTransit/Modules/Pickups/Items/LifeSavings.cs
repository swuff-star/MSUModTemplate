using LostInTransit.Modules;
using RoR2;
using System.Diagnostics;
using UnityEngine.Networking;

namespace LostInTransit.Items
{
    public class LifeSavings : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("LifeSavings");
        private static bool checkMoney = false;
        private static bool grantMoney = true;

        //★ (godzilla 1998 main character voice)
        //★ that's a lotta Debug.WriteLine()
        public override void Initialize()
        {
            SceneExitController.onBeginExit += checkGoldAtEndOfStage;
            Stage.onStageStartGlobal += Stage_onServerStageBegin;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<LifeSavingsBehavior>(stack);
        }
        private void checkGoldAtEndOfStage(SceneExitController obj)
        {
            if (!NetworkServer.active) return;
            checkMoney = true;
            //Debug.WriteLine("Stage ended! Right now, the bools are lookin' like:");
            //Debug.WriteLine("checkMoney: " + checkMoney);
            //Debug.WriteLine("grantMoney: " + grantMoney);
        }
        private void Stage_onServerStageBegin(Stage obj)
        {
            if (!NetworkServer.active) return;
            checkMoney = false;
            //Debug.WriteLine("Stage started! Right now, the bools are lookin' like:");
            //Debug.WriteLine("checkMoney: " + checkMoney);
            //Debug.WriteLine("grantMoney: " + grantMoney);
        }
        public class LifeSavingsBehavior : CharacterBody.ItemBehavior
        {
            private static float moneyToGrant = 0f;
            public void FixedUpdate()
            {
                float currentMoney = body.master.money;
                
                if (checkMoney && grantMoney && stack >= 1)
                {
                    //Debug.WriteLine("Tallying money...");
                    //Debug.WriteLine("Current gold: " + currentMoney);
                    moneyToGrant = currentMoney * (stack * 2f) * 0.01f;
                    //Debug.WriteLine("Money to grant: " + moneyToGrant);
                    grantMoney = false;
                    //Debug.WriteLine("checkMoney: " + checkMoney);
                    //Debug.WriteLine("grantMoney: " + grantMoney);
                }
                if (!checkMoney && !grantMoney && stack >= 1)
                {
                    //Debug.WriteLine("Giving money...");
                    body.master.GiveMoney((uint)moneyToGrant);
                    //Debug.WriteLine("Money given: " + moneyToGrant);
                    moneyToGrant = 0f;
                    //Debug.WriteLine("moneyToGrant set to: " + moneyToGrant);
                    //Debug.WriteLine("checkMoney: " + checkMoney);
                    //Debug.WriteLine("grantMoney: " + grantMoney);
                    grantMoney = true;
                }
            }


        }
    }
}

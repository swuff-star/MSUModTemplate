using Moonstorm;
using RoR2;
using RoR2.Items;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceLuck : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceLuck");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }


        //Todo: Have this as a hook on CharacterMaster.OnInventoryChanged (otherwise the "luck" stat gets rewritten every time the inventory changes)
        public class DiceLuckBehavior : BaseItemBodyBehavior
        {
            private CharacterMaster master;
            public void Start()
            {
                master = body.master;
                if(master)
                {
                    master.luck += Items.BlessedDice.luckAmount;
                }
            }
            public void OnDestroy()
            {
                if(master)
                {
                    master.luck -= Items.BlessedDice.luckAmount;
                }
            }
        }
    }
}
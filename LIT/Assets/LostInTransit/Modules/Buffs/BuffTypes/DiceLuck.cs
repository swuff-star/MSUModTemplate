using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceLuck : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceLuck");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceLuckBehavior>(stack);
        }

        public class DiceLuckBehavior : CharacterBody.ItemBehavior
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